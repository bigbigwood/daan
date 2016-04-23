<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnaResultSun_AddRemark.aspx.cs" Inherits="daan.web.admin.analyse.AnaResultSun_AddRemark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel2" runat="server"/>
    <ext:RegionPanel runat="server" ID="RegionPanel2" ShowBorder="false">
        <Regions>
            <ext:Region runat="server" ShowHeader="false" ShowBorder="false" Position="Center" Layout="Fit">
                <Items>
                    <ext:RegionPanel ID="RegionPanel1" runat="server" BodyPadding="10" >
                        <Toolbars>
                            <ext:Toolbar ID="Toolbar9" runat="server" Position="Top" >
                                <Items>
                                    <ext:ToolbarFill ID="ToolbarFill6" runat="server">
                                    </ext:ToolbarFill>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator14" runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:HiddenField ID="hidOrdernum" runat="server"></ext:HiddenField>
                                    <ext:HiddenField runat="server" ID="hidOldRemarks"></ext:HiddenField>
                                    <ext:Button ID="btnSaveRemark" runat="server" Text="保存" Icon="SystemSaveNew"
                                     OnClick="btnSaveRemark_Click"></ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Toolbars>
                        <Regions>
                            <ext:Region ID="Region1" runat="server" ShowHeader="false" ShowBorder="false">
                                <Items>
                                    <ext:TextArea Width="400" Height="150" ID="txaRemark" runat="server">
                                    </ext:TextArea>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
