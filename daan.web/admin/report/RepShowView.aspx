<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepShowView.aspx.cs" Inherits="daan.web.admin.report.RepShowView" %>

<%@ Register Assembly="FastReport.Web, Version=1.7.1.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c"
    Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="JavaScript" type="text/javascript" src="../../js/ActiveXPrint.js"></script>
    <title>报告显示</title>
</head>
<body style="text-align: center;">
    <form id="form1" runat="server">    
     <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"  AjaxLoadingType=Mask/>  
    <cc1:WebReport ID="reportShowView" runat="server" OnStartReport="reportShowView_StartReport"
        ShowExports="False" ShowPrint="true" ShowWord2007Export="true" Width="780px"
        Height="100%" Layers="True" />
    </form>
</body>
</html>
