<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="EditPassword.aspx.cs" Inherits="daan.web.EditPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt
        {
            background: url(../images/btn_bg1.gif) no-repeat;
            border: none;
            width: 78px;
            height: 25px;
            color: #fff;
            margin-right: 5px;
            font-size: 14px;
            padding: 5px 0 5px 0px;
        }
    </style>
    <link href="style/global.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table width="500" bgcolor="dimgray" cellpadding="15" cellspacing="1" style=" margin:15px 0px 0px 15px; border-color:Gray;">
            <tr>
                <td colspan="2" class="t3">
                   <p>提示：将同步更新CRM系统、IT服务管理系统、体检系统的密码，在登录以上系统时请使用更新后的密码。</p><p>密码由6-20位的数字和字母组成。</p>
                </td>                
            </tr>
            <tr>
                <td class="t3" nowrap style=" width:100px;">
                    <asp:Label ID="Label1" runat="server" Text="旧密码："></asp:Label>
                </td>
                <td class="t1">
                    <asp:TextBox ID="tbOldPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="请输入旧密码" ControlToValidate="tbOldPassword" Display="dynamic"  ValidationGroup="check" />
                </td>
            </tr>
            <tr>
                <td class="t3" nowrap>
                    <asp:Label ID="Label2" runat="server" Text="新密码："></asp:Label>
                </td>
                <td class="t1">
                    <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
       <asp:RequiredFieldValidator ID="reqPwd" runat="server" ErrorMessage="请输入新密码" ControlToValidate="tbNewPassword" Display="dynamic"  ValidationGroup="check" />
       <asp:RegularExpressionValidator ID="regPwd" runat="server" ErrorMessage="密码由6-20位的数字和字母组成" ControlToValidate="tbNewPassword" Display="Dynamic" ValidationGroup="check" ValidationExpression="(^.*?[a-zA-Z]+.*?\d+.*?$)|(^.*?\d+.*?[a-zA-Z]+.*?$)" />
                </td>
            </tr>
              <tr>
                <td class="t3" nowrap>
                    <asp:Label ID="Label3" runat="server" Text="确认新密码："></asp:Label>
                </td>
                <td class="t1">
                    <asp:TextBox ID="tbRepeatPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="reqConfrimPwd" runat="server" ErrorMessage="请输入确认密码" ControlToValidate="tbRepeatPassword" Display="dynamic"  ValidationGroup="check" />
      <asp:CompareValidator ID="comPwd" runat="server" ErrorMessage="确认密码与新密码要一致" ControlToCompare="tbNewPassword" ControlToValidate="tbRepeatPassword" Display="Dynamic" ValidationGroup="check" />
                </td>
            </tr>
            <tr>
               
                <td class="t1" colspan="2">
                <span style="margin:20px  0px 0px 120px">
                    <asp:Button ID="btnConfirm" runat="server" Text="确认"  CssClass="txt" OnClick="btnConfirm_Click" ValidationGroup="check" />
               </span> </td>
            </tr>
        </table>
    </div>
</asp:Content>
