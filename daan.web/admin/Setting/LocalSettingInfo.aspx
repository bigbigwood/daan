<%@ Page Title="本地参数设置" Language="C#" AutoEventWireup="true" CodeBehind="LocalSettingInfo.aspx.cs"
    Inherits="daan.web.admin.Setting.LocalSettingInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
</head>
<body onload="test()">
    <form id="form2" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSaveRefresh" Text="保存" ValidateForms="SimpleForm1" CssClass="inline"
                        runat="server" Icon="SystemSaveNew" OnClick="btnSaveRefresh_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Position="Center" runat="server">
                <Items>
                    <ext:Form ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" EnableBackgroundColor="false"
                        runat="server" LabelWidth="100px" Title="打印机配置" LabelAlign="Right" BodyPadding="5px">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxPrint" runat="server" Hidden="true">
                                    </ext:TextBox>
                                    <ext:TextBox ID="tbxMac" runat="server" Hidden="true">
                                    </ext:TextBox>
                                    <ext:TextBox ID="tbxName" runat="server" Hidden="true">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:Label runat="server" ID="lbHostMac" Label="MAC码">
                                    </ext:Label>
                                    <ext:Label runat="server" ID="lbHostName" Label="主机名">
                                    </ext:Label>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" FocusOnPageLoad="true" ID="Drop_A4PRINTER" ShowRedStar="true"
                                        CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual"
                                        AutoPostBack="true" Label="A4报告打印机">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="Drop_A5PRINTER" ShowRedStar="true" CompareType="String"
                                        Resizable="True" CompareValue="-1" CompareOperator="NotEqual" AutoPostBack="true"
                                        Label="A5报告打印机">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="Drop_BARCODEPRINTER" ShowRedStar="true" CompareType="String"
                                        Resizable="True" CompareValue="-1" CompareOperator="NotEqual" AutoPostBack="true"
                                        Label="条码打印机">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="Drop_PDFPRINTER" ShowRedStar="true" CompareType="String"
                                        Resizable="True" CompareValue="-1" CompareOperator="NotEqual" AutoPostBack="true"
                                        Label="PDF打印机">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
    <script type="text/javascript">
        function GetPrinter() {
            var hostMAC = Ext.getCmp('<%=lbHostMac.ClientID%>');
            hostMAC.setValue(document.ActiveXPrint.GetMAC());

            var tbMac = Ext.getCmp('<%=tbxMac.ClientID%>');
            tbMac.setValue(document.ActiveXPrint.GetMAC());


            var hostName = Ext.getCmp('<%=lbHostName.ClientID%>');
            hostName.setValue(document.ActiveXPrint.GetHostName());

            var tbName = Ext.getCmp('<%=tbxName.ClientID%>');
            tbName.setValue(document.ActiveXPrint.GetHostName());

            var hiddenField = Ext.getCmp('<%=tbxPrint.ClientID%>');
            hiddenField.setValue(document.ActiveXPrint.GetPrinter());

            //第一个参数是控件的ID,第二个参数是此控件所带的值
            __doPostBack('<%= tbxPrint.ClientID %>', 'specialkey');
        }
    </script>
</body>
</html>
