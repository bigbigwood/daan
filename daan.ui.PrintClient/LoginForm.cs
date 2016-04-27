using System;
using System.Configuration;
using System.Windows.Forms;
using daan.webservice.phyReportSystem.Contract.Messages;
using log4net;

namespace daan.ui.PrinterApplication
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
                string url = ConfigurationManager.AppSettings.Get("UserServiceUrl");
                var userService = ServiceFactory.GetUserService(url);
                var authenticateResponse = userService.Authenticate(new AuthenticateRequest()
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text
                });

                if (authenticateResponse.ResultType == ResultTypes.Ok)
                {
                    Log.Info("Authenticate OK");

                    //PrinterApp.UserInfo = 
                    
                    //show main form
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.ShowDialog();
                }
                else
                {
                    Log.Info("Authenticate fail.");

                    // Do something, for example load user data in the context

                }
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
