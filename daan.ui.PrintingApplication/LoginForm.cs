using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;
using daan.ui.PrintingApplication.Helper;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Contract.Models.User;
using log4net;

namespace daan.ui.PrintingApplication
{
    public partial class LoginForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginForm()
        {
            InitializeComponent();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("用户名或者密码不能为空");
                    return;
                }

                var userService = ServiceFactory.GetClientApplicationService();
                var authenticateResponse = userService.Authenticate(new AuthenticateRequest()
                {
                    Username = username,
                    Password = password,
                    HostMac = LocalMachineInfomationProvider.GetMac(),
                });

                if (authenticateResponse.ResultType != ResultTypes.Ok)
                {
                    Log.Info("Authenticate fail");
                    MessageBox.Show("用户名或者密码不正确");
                    return;
                }
                Log.Info("Authenticate OK");
                PrintingApp.CurrentUserInfo = authenticateResponse.UserInfo;
                PrintingApp.UserCredential = new UserCredential() { UserName = username, Password = password };

                var authorizeResponse = userService.Authorize(new AuthorizeRequest()
                {
                    Username = username,
                    Password = password,
                });
                if (authorizeResponse.ResultType != ResultTypes.Ok)
                {
                    Log.Info("Authorize fail");
                    MessageBox.Show("授权失败");
                    return;
                }
                Log.Info("Authorize OK");
                PrintingApp.LabAssociations = authorizeResponse.LabAssociations.ToList();
                PrintingApp.OrganizationAssociations = authorizeResponse.OrganizationAssociations.ToList();
                PrintingApp.ReportTemplates = authorizeResponse.ReportTemplates.ToList();

                ApplicationUpdater updater = new ApplicationUpdater();
                updater.Initialize();

                ReportTemplateFileProvider.Init(updater.ReportTemplateVersion);

                //show main form
                this.Hide();
                var mainForm = new MainForm();
                mainForm.ShowDialog();

            }
            catch (Exception ex)
            {
                Log.Error("Login exception.", ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
