<%@ Page Title="集中打印" Language="C#" AutoEventWireup="true" CodeBehind="AnaFocusPrint.aspx.cs"
    EnableViewStateMac="false" EnableEventValidation="false" Inherits="daan.web.admin.analyse.AnaFocusPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script src="../../js/ActiveXPrint.js" type="text/javascript"></script>
</head>
<body>
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        AjaxLoadingType="Mask" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar3" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPreview" runat="server" Text="预览" Icon="Magnifier" EnablePostBack="true"
                        OnClick="btnPreview_Click" />
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrint" runat="server" Text="打印" ConfirmText="确定要打印吗?" Icon="Printer"
                        OnClick="btnPrint_Click" OnClientClick="GetIP()" EnablePostBack="true">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrintNew" runat="server" Text="新版打印" ConfirmText="确定要打印吗?" Icon="Printer"
                        OnClick="btnPrintNew_Click" OnClientClick="GetIP()" EnablePostBack="true" Visible="true">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDownload" runat="server" Text="下载" Icon="Printer" OnClick="btnDownload_Click"
                        DisableControlBeforePostBack="false">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnLog" runat="server" Text="操作记录" Icon="SystemNew" OnClick="btnLog_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExport" runat="server" Text="导出" Icon="PageExcel" EnableAjax="false"
                        DisableControlBeforePostBack="false" OnClick="btnExport_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="90px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="90px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="false">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DropDownList ID="dropDictLab" runat="server" Label="分点" Resizable="True" AutoPostBack="true"
                                        OnSelectedIndexChanged="dropDictLab_SelectedIndexChanged">
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="dropDictcustomer" runat="server" Label="体检单位" Resizable="True"
                                        EnableEdit="true">
                                    </ext:DropDownList>
                                    <ext:DatePicker ID="dpFrom" runat="server" Label="登记时间" Width="163">
                                    </ext:DatePicker>
                                    <ext:DatePicker ID="dpTo" runat="server" Label="到" Width="163" CompareMessage="结束日期应该大于开始日期"
                                        ShowLabel="true" CompareControl="dpFrom" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:DropDownList ID="dropStatus" runat="server" Label="状态">
                                    </ext:DropDownList>
                                    <ext:TextBox runat="server" ID="tbxOrderNum" Label="体检号/条码号" EmptyText="体检号/条码号" />
                                    <ext:DatePicker runat="server" Label="采样日期" Width="163" ID="dpSFrom">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" Label="到" Width="163" ID="dpSTo" CompareControl="dpSFrom"
                                        CompareMessage="结束日期应该大于开始日期" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxName" Label="关键词" />
                                    <ext:DropDownList ID="dropReportStatus" Enabled="false" runat="server" Label="报告状态">
                                    </ext:DropDownList>
                                    <ext:Label runat="server" Text="" ShowLabel="False" ID="label2" />
                                    <ext:Label runat="server" Text="" ShowLabel="False" ID="label3" />
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                ShowBorder="false" Title="Center Region">
                <Items>
                    <ext:Grid ID="gdOrders" runat="server" EnableCheckBoxSelect="true" EnableTextSelection="true"
                        PageSize="50" IsDatabasePaging="true" AllowPaging="true" OnPageIndexChange="gdOrders_PageIndexChange"
                        EnableRowNumber="true" Title="Grid" ShowHeader="false" DataKeyNames="ORDERNUM,STATUS,REALNAME,IDNUMBER,dictreporttemplateid">
                        <Columns>
                            <ext:BoundField HeaderText="订单状态" DataField="STATUSNAME" Width="80" />
                            <ext:BoundField HeaderText="订单号" DataField="ORDERNUM" Width="110" />
                            <ext:BoundField HeaderText="姓名" DataField="REALNAME" Width="60" />
                            <ext:BoundField HeaderText="性别" DataField="SEX" Width="50" />
                            <ext:BoundField HeaderText="年龄" DataField="AGE" Width="50" />
                            <ext:BoundField HeaderText="联系方式" DataToolTipField="MOBILE" DataField="MOBILE" Width="90" />
                            <ext:BoundField HeaderText="登记时间" DataField="createdate" Width="80" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField HeaderText="体检单位" DataToolTipField="CUSTOMERNAME" DataField="CUSTOMERNAME" Width="250" />
                            <ext:BoundField HeaderText="套餐名称" DataToolTipField="ORDERTESTLST" DataField="ORDERTESTLST" Width="250" />
                            <ext:BoundField HeaderText="部门[地区]" DataField="section" Width="100" />
                            <ext:BoundField HeaderText="采样时间" DataField="samplingdate" DataFormatString="{0:yyyy-MM-dd}" Width="80" />
                            <ext:BoundField HeaderText="邮寄地址" DataField="POSTADDRESS" DataToolTipField="POSTADDRESS" Width="250" />
                            <ext:BoundField HeaderText="收件人" DataField="RECIPIENT" Width="60" />
                            <ext:BoundField HeaderText="联系电话" DataField="CONTACTNUMBER" Width="100" />
                            <%--<ext:BoundField HeaderText="报告状态" DataField="REPORTSTATUS" Width="70" />--%>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <!--获取mac地址取该用户的本地打印机IP等信息-->
    <ext:HiddenField ID="hdMac" runat="server">
    </ext:HiddenField>
    <script type="text/javascript">
        function GetIP() {
            //获取mac地址取该用户的本地打印机IP等信息
            document.getElementById("<%=hdMac.ClientID %>").value = document.ActiveXPrint.GetMAC();
        }
    </script>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385">
    </ext:Window>
    <ext:Window ID="WinReportView" Hidden="true" EnableIFrame="true" Title="报告预览" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="960" Height="600"> <%--Width="825" Height="550"--%>
    </ext:Window>
    </form>
</body>
</html>
