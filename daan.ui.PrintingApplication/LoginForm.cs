using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;
using daan.ui.PrintingApplication.Helper;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models.User;
using log4net;

namespace daan.ui.PrintingApplication
{
    public enum MessageType
    {
        Infomation,
        Warning,
    }

    public partial class LoginForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MainForm mainForm = new MainForm();

        public LoginForm()
        {
            InitializeComponent();

            lbl_message.Text = string.Empty;
            AcceptButton = btnLogin;
            CancelButton = btnExit;

            //txtUsername.Text = "lishapeng";
            //txtPassword.Text = "lishapeng";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ControlButtons(bool btnStatus)
        {
            btnLogin.Enabled = btnStatus;
            btnExit.Enabled = btnStatus;
        }

        private void ShowMessage(string message, MessageType type)
        {
            lbl_message.ForeColor = (type == MessageType.Infomation) ? Color.Aqua : Color.Red;
            lbl_message.Text = message;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowMessage("用户名或者密码不能为空", MessageType.Warning);
                    return;
                }

                ControlButtons(false);

                var beginInvokeThread = new Thread(new ThreadStart(DoWorkInBackgroud));
                beginInvokeThread.Start();
            }
            catch (Exception ex)
            {
                Log.Error("Login exception.", ex);
            }
        }

        private void ShowMainForm()
        {
            this.Hide();
            var mainForm = new MainForm();
            mainForm.ShowDialog();
        }


        private void DoWorkInBackgroud()
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                BeginInvoke(new Action<String, MessageType>(ShowMessage), "用户认证中...", MessageType.Infomation);
                var userService = ServiceFactory.GetClientApplicationService();
                var authenticateResponse = userService.Authenticate(new AuthenticateRequest()
                {
                    Username = username,
                    Password = password,
                    HostMac = LocalMachineInfomationProvider.GetMac(),
                });
                if (authenticateResponse.ResultType != ResultTypes.Ok)
                {
                    Log.Info("Authenticate fail.");
                    Invoke(new Action<String, MessageType>(ShowMessage), "用户认证失败...", MessageType.Warning);
                    Invoke(new Action<bool>(ControlButtons), true);
                    return;
                }
                Log.Info("Authenticate OK.");
                PrintingApp.CurrentUserInfo = authenticateResponse.UserInfo;
                PrintingApp.UserCredential = new UserCredential() { UserName = username, Password = password };


                BeginInvoke(new Action<String, MessageType>(ShowMessage), "用户授权中...", MessageType.Infomation);
                var authorizeResponse = userService.Authorize(new AuthorizeRequest()
                {
                    Username = username,
                    Password = password,
                });
                if (authorizeResponse.ResultType != ResultTypes.Ok)
                {
                    Log.Info("Authorize fail.");
                    Invoke(new Action<String, MessageType>(ShowMessage), "用户授权失败...", MessageType.Warning);
                    Invoke(new Action<bool>(ControlButtons), true);
                    return;
                }
                Log.Info("Authorize OK.");
                PrintingApp.LabAssociations = authorizeResponse.LabAssociations.ToList();
                PrintingApp.OrganizationAssociations = authorizeResponse.OrganizationAssociations.ToList();
                PrintingApp.ReportTemplates = authorizeResponse.ReportTemplates.ToList();

                Log.Info("Loading system data.");
                BeginInvoke(new Action<String, MessageType>(ShowMessage), "加载系统配置中...", MessageType.Infomation);
                ApplicationUpdater updater = new ApplicationUpdater();
                updater.Initialize();

                ReportTemplateFileProvider.Init(updater.ReportTemplateVersion);

                BeginInvoke(new Action<String, MessageType>(ShowMessage), "登陆成功...", MessageType.Infomation);
                BeginInvoke(new Action(ShowMainForm));
            }
            catch (Exception ex)
            {
                Invoke(new Action<String, MessageType>(ShowMessage), "发生登陆异常，请联系管理员...", MessageType.Warning);
                Invoke(new Action<bool>(ControlButtons), true);
                Log.Error("Login fail.", ex);
            }
            
        }
    }
}
