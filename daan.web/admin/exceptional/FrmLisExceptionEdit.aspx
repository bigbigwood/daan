<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLisExceptionEdit.aspx.cs"
    Inherits="daan.web.admin.exceptional.FrmLisExceptionEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <ext:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px" EnableBackgroundColor="true">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSaveRefresh" Text="保存" runat="server" Icon="SystemSaveNew" OnClick="btnSaveRefresh_Click"
                        ValidateForms="SimpleForm1">
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Items>
            <ext:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="true"
                AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                <Items>
                    <ext:TextArea runat="server" AutoGrowHeight="True" Label="LIS处理意见" AutoGrowHeightMax="150px" Enabled="false"
                        AutoGrowHeightMin="150px" EmptyText="请录入处理意见，保存后状态将更改为已处理。" Height="150px" ID="txtRemark"
                        MaxLength="300">
                    </ext:TextArea>
                    <ext:TextArea runat="server" AutoGrowHeight="True" Label="处理意见" AutoGrowHeightMax="100px"
                        AutoGrowHeightMin="300px" EmptyText="请录入处理意见，保存后状态将更改为已处理。" Height="300px" ID="txtResult"
                        MaxLength="300">
                    </ext:TextArea>
                </Items>
            </ext:SimpleForm>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
