<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictTestResult_Window.aspx.cs"
    Inherits="daan.web.admin.dict.DictTestResult_Window" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    <ext:Button ID="btnSaveRefresh" Text="保存" runat="server" Icon="SystemSaveNew"
                        OnClick="btnSaveRefresh_Click" ValidateForms="SimpleForm1">
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Items>
            <%--  <ext:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>--%>
            <ext:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="true"
                AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                <Items>
                    <ext:TextBox ID="txtResult" Label="结果" Required="true" ShowRedStar="true" runat="server">
                    </ext:TextBox>
                    <ext:RadioButtonList ID="rdobtnException" runat="server" Label="诊断">
                        <ext:RadioItem Text="正常" Value="value1" Selected="true" />
                        <ext:RadioItem Text="异常" Value="value2" />
                    </ext:RadioButtonList>
                <%--    <ext:TextBox ID="txtIsexception" Label="诊断" Required="true" ShowRedStar="true" runat="server">
                    </ext:TextBox>--%>
                    <ext:NumberBox ID="txtDisplayorder" runat="server" Label="顺序" Required="true" ShowRedStar="true">
                    </ext:NumberBox>
                 
                </Items>
            </ext:SimpleForm>
            <%-- </Items>
            </ext:Panel>--%>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
