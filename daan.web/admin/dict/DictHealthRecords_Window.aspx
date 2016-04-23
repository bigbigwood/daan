<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictHealthRecords_Window.aspx.cs" Inherits="daan.web.admin.dict.DictHealthRecords_Window" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color:Blue">
    <form id="form1" runat="server">
    <ext:PageManager runat="server" ID="Pagemanager1" AutoSizePanelID="Panel1" />
    <ext:Panel runat="server" ID="Panel1" Layout="Fit" ShowBorder="false" ShowHeader="false"
    EnableBackgroundColor="true" BodyPadding="5px">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="保存" runat="server" Icon="SystemSaveNew"
                        OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" Text="关闭" runat="server" Icon="BulletCross"
                     OnClick="btnClose_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Items>
            <ext:ContentPanel runat="server" ID="ContentPanel1" ShowBorder="false" ShowHeader="false">
                <table border="0" width="100%">
                    <tr height="30px">
                        <td align="right" width="15%">类型：</td>
                        <td align="left">
                            <ext:DropDownList runat="server" Width="100px" ID="dpType"></ext:DropDownList>
                        </td>
                    </tr>
                    <tr height="200px">
                        <td align="right" valign="top">详细情况：</td>
                        <td align="left" valign="top">
                            <ext:TextArea runat="server" Width="400px" Height="180px" ID="txtHealthRecord"></ext:TextArea>
                        </td>
                    </tr>
                </table>
            </ext:ContentPanel>
        </Items>
    </ext:Panel>
    <ext:HiddenField runat="server" ID="hidMemberID"></ext:HiddenField>
    </form>
</body>
</html>
