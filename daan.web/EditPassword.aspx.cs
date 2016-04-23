using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Daan.Authority.Handler;
using daan.service.dict;
using System.Collections;

namespace daan.web
{
    public partial class EditPassword : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }



        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!this.tbOldPassword.Text.Trim().Equals("") && !this.tbNewPassword.Text.Trim().Equals("") && !this.tbRepeatPassword.Text.Trim().Equals(""))
            {
                bool flag = true;
                string message = "";
                string oldpassword = this.tbOldPassword.Text.Trim(); //旧密码
                string newpassword = this.tbNewPassword.Text.Trim(); //新密码
                string repeatpassword = this.tbRepeatPassword.Text.Trim();//确认密码
                string AuthorizationCode = Userinfo.AuthorizationCode;
                if (!AuthorizationCode.Equals(""))
                {
                    if (newpassword.Equals(repeatpassword))
                    {
                        flag = SecurityHandler.ChangePassword(AuthorizationCode, oldpassword, newpassword);
                        if (flag == false)
                        {
                            message = "您输入的原始密码有误，修改失败！";
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"您输入的确认密码有误！\")</script>");
                    }
                }
                else
                {
                    flag = false;
                    message = "授权编码有误，请重新登陆后修改密码！";
                }

                if (flag)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"密码修改成功，请重新登录！\");window.parent.location='Login.aspx';</script>");
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"" + message + "\")</script>");
                }

            }
            else
            {
                if (this.tbOldPassword.Text.Trim().Equals(""))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"请输入旧密码！\")</script>");
                }
                else if (this.tbNewPassword.Text.Trim().Equals(""))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language=\"javascript\">alert(\"请输入新密码！\")</script>");
                }
            }
        }


    }
}