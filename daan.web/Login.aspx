<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="daan.web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>体检系统登录</title>
    <link href="style/styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function handleEnter(field, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode == 13) {
                var i;
                for (i = 0; i < field.form.elements.length; i++)
                    if (field == field.form.elements[i])
                        break;
                i = (i + 1) % field.form.elements.length;
                field.form.elements[i].focus();
                return false;
            }
            else
                return true;
        }     

</script>
</head>
<body>
    <form id="loginform" runat="server">
    <div class="warp">
        <div class="login">
            <p>
                <label>
                    帐号：</label><br />
                <asp:TextBox ID="tbUserName" runat="server" Text="" CssClass="txt" TabIndex="1" onkeypress="return handleEnter(this, event)" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAcc" runat="server" ControlToValidate="tbUserName"
                    ErrorMessage="请输入帐号" Display="Dynamic" ValidationGroup="check" />
            </p>
            <p>
                <label>
                    密码：</label><br />
                <asp:TextBox ID="tbPassWord" runat="server" Text="123" TextMode="Password" CssClass="pwd" TabIndex="2" onkeypress="return handleEnter(this, event)" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPassWord"
                    ErrorMessage="请输入密码" Display="Dynamic" ValidationGroup="check" />
            </p>
            <p>
                <label>
                    验证码：</label>
                <span style="vertical-align: baseline; font-size: 14px;">
                    <asp:Image ID="Captcha" runat="server" ToolTip="验证码" ImageUrl="~/code/Captcha/createImg.aspx" onclick="this.src='/code/Captcha/createImg.aspx?t='+(new Date().getTime());return false;"/>&nbsp;&nbsp;&nbsp;
                    <a href="#" style="color: Red; vertical-align: text-top; text-decoration: none;"
                        onclick="javascript:document.getElementById('Captcha').src='/code/Captcha/createImg.aspx?t='+(new Date().getTime());return false;">
                        看不清换一张</a></span>
                <br />
                <asp:TextBox ID="tbCaptcha" runat="server" TabIndex="3" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbCaptcha"
                    ErrorMessage="请输入验证码" Display="Dynamic" ValidationGroup="check" />
            </p>
            <p>
                <span style="margin-left: 40px;">
                    <asp:Button ID="btLogin" runat="server" CssClass="btn_log" OnClick="btLogin_Click"
                        ValidationGroup="check" TabIndex="4" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btReset" runat="server" CssClass="btn_clear" OnClick="btReset_Click" TabIndex="5"/>
                </span>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
