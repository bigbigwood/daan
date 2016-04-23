using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtAspNet;
using Daan.Authority.Handler;
using daan.domain;
using daan.service.dict;
using System.Collections;
using daan.service.report;
using System.Text.RegularExpressions;

namespace daan.web
{
    public partial class Login : System.Web.UI.Page
    {
        public string hostMac = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            // 创建一个 6 位的随机数并保存在 Session 对象中
            Session["CaptchaImageText"] = GenerateRandomCode();
        }
        /// <summary>
        /// 创建一个 6 位的随机数
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomCode()
        {
            string s = String.Empty;
            try
            {

                Random random = new Random();
                for (int i = 0; i < 6; i++)
                {
                    s += random.Next(10).ToString();
                }
                return s;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"" + ex.Message + "\")</script>");
                return s;
            }
        }


        private Initsyssetting GetSysSetting()
        {
            try
            {
                InitsyssettingService service = new InitsyssettingService();
                IList<Initsyssetting> result = service.GetInitSysSetting();
                if (result.Count > 0)
                    return result.ToList<Initsyssetting>().First();
                else
                    return null;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"" + ex.Message + "\")</script>");
                return null;
            }
        }






        protected void btLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["check"] == null)
                {
                    this.tbCaptcha.Text = string.Empty;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"验证码失效！\")</script>");
                    return;
                }
                if (tbCaptcha.Text != Session["check"].ToString())
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"验证码错误！\")</script>");
                    this.tbCaptcha.Text = string.Empty;
                    return;
                }
                string usercode = Server.HtmlEncode(tbUserName.Text);
                string passWord = tbPassWord.Text;
                //进行用户登录，security.LoginResult为null或者security.LoginResult.IsPassed && security.LoginResult.AuthorizationCode != ""
                //都是登录失败
                SecurityHandler security = SecurityHandler.Login(usercode, passWord);
                if (security.LoginResult.SystemCode == null) //判断存不存在
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"用户名或密码错误！\")</script>");
                    this.tbUserName.Text = string.Empty;
                    this.tbPassWord.Text = string.Empty;
                    this.tbCaptcha.Text = string.Empty;
                    return;
                }
                var aa = SecurityHandler.LoginOn(security.LoginResult.AuthorizationCode).GetCurrentUserInfo();
                Dictuser user = new Dictuser();
                user.Usercode = aa.USERNAME;
                user = new DictuserService().GetDictuserInfoByUserCode(user);
                if (user != null)
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.AuthorizationCode = security.LoginResult.AuthorizationCode;
                    userInfo.userCode = user.Usercode;
                    userInfo.userName = user.Username;
                    userInfo.userId = Convert.ToInt32(user.Dictuserid);
                    userInfo.loginTime = DateTime.Now;
                    userInfo.joinLabidstr = user.Joinlabid;
                    userInfo.dictlabid = user.Dictlabid;
                    userInfo.joinDeptstr = user.Joindeptid;
                    userInfo.dictlabdeptid = user.Dictlabdeptid;
                    userInfo.sysSetting = GetSysSetting();
                    Session["UserInfo"] = userInfo;
                }

                if (security.LoginResult.IsPassed && security.LoginResult.AuthorizationCode != "")
                {
                    //这里的Cookie名字不能更改
                    HttpCookie cookie = new HttpCookie("authorizationcode");
                    cookie.Value = security.LoginResult.AuthorizationCode;
                    TimeSpan ts = new TimeSpan(1, 0, 0, 0);
                    cookie.Expires = DateTime.Now.Add(ts);//添加作用时间
                    Response.AppendCookie(cookie);

                    if (!RegexPassWordSecurity(passWord))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectToChangePassword",
                                                              "<script> alert('您的密码安全性较弱，请重新修改密码'); window.location.href='EditPassword.aspx';</script>");

                        return;
                    }

                    Response.Redirect("Main.aspx", false);
                    //////PageContext.RegisterStartupScript("top.location.href = 'Main.aspx';");
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">top.location.href = 'Main.aspx';</script>");
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"用户名或密码错误！\")</script>");
                    this.tbUserName.Text = string.Empty;
                    this.tbPassWord.Text = string.Empty;
                    this.tbCaptcha.Text = string.Empty;
                    return;
                }
            }
            catch (Exception ex)
            {                            
                Alert.ShowInTop(ex.Message, "体检系统");                
            }
        }

        protected void btReset_Click(object sender, EventArgs e)
        {
            this.tbUserName.Text = string.Empty;
            this.tbPassWord.Text = string.Empty;
            this.tbCaptcha.Text = string.Empty;
        }

        /// <summary>
        /// 验证密码安全性
        /// (密码必须6-20位，不能为纯数字或字母)
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true: 安全 false：不安全</returns>
        private bool RegexPassWordSecurity(string password)
        {
            if (Regex.IsMatch(password, @"(^.*?[a-zA-Z]+.*?\d+.*?$)|(^.*?\d+.*?[a-zA-Z]+.*?$)"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}