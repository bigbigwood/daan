using System;
using System.Configuration;
using System.IO;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;
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


                string url = ConfigurationManager.AppSettings.Get("ClientApplicationServiceUrl");
                var userService = ServiceFactory.GetClientApplicationService(url);
                var authenticateResponse = userService.Authenticate(new AuthenticateRequest()
                {
                    Username = username,
                    Password = password,
                });

                if (authenticateResponse.ResultType != ResultTypes.Ok)
                {
                    Log.Info("Authenticate fail.");
                    MessageBox.Show("用户名或者密码不正确");
                    return;
                }

                Log.Info("Authenticate OK");
                var userCredential = new UserCredential() { UserName = username, Password = password };
                PrintingApp.CurrentUserInfo = authenticateResponse.UserInfo;
                PrintingApp.CurrentUserInfo.UserCredential = userCredential;

                ApplicationUpdater updater = new ApplicationUpdater();
                updater.Initialize();



                //show main form
                this.Hide();
                var mainForm = new MainForm();
                mainForm.ShowDialog();

            }
            catch (Exception ex)
            {
                Log.Error("Authenticaition exception.", ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
