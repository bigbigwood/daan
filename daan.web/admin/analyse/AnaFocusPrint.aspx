﻿<%@ Page Title="集中打印" Language="C#" AutoEventWireup="true" CodeBehind="AnaFocusPrint.aspx.cs"
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
                Position="Top" Height="110px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="90px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="false">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DropDownList ID="dropDictLab" runat="server" Label="分点" Resizable="True" AutoPostBack="true"
                                        OnSelectedIndexChanged="dropDictLab_SelectedIndexChanged">
                                    </ext:DropDownList>
                                    <ext:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                                        <table width="100%">
                                            <tr>
                                                <td align="right" width="90px">
                                                    <ext:Label runat="server" Text="体检单位："></ext:Label>
                                                </td>
                                                <td align="left" width="170px">
                                                    <ext:DropDownList ID="dropDictcustomer" runat="server" Width="170" Resizable="True" EnableEdit="true">
                                                    </ext:DropDownList>
                                                </td>
                                                <td align="right">
                                                    <ext:TextBox runat="server" CssClass="inline" ID="tbxmember" EmptyText="输入后回车" Width="68px">
                                                    </ext:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ext:ContentPanel>
                                    <ext:DatePicker ID="dpFrom" runat="server" Label="登记时间">
                                    </ext:DatePicker>
                                    <ext:DatePicker ID="dpTo" runat="server" Label="到" CompareMessage="结束日期应该大于开始日期"
                                        ShowLabel="true" CompareControl="dpFrom" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow4" runat="server">
                                <Items>
                                    <ext:DropDownList ID="dropAuditStatus" runat="server" Label="财务状态">
                                        <ext:ListItem Text="全部" Value="-1" Selected="true"  />
                                        <ext:ListItem Text="未审核" Value="0"/>
                                        <ext:ListItem Text="已审核" Value="1" />
                                    </ext:DropDownList>
                                    <ext:TextBox runat="server" ID="tbxBatchNumber" Label="场次号" />
                                    <ext:DatePicker runat="server" Label="财务审核日期" ID="dpFFrom">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" Label="到" ID="dpFTo" CompareControl="dpFFrom"
                                        CompareMessage="结束日期应该大于开始日期" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:DropDownList ID="dropStatus" runat="server" Label="标本状态">
                                    </ext:DropDownList>
                                    <ext:TextBox runat="server" ID="tbxOrderNum" Label="体检号/条码号" EmptyText="体检号/条码号" />
                                    <ext:DatePicker runat="server" Label="采样日期" ID="dpSFrom">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" Label="到" ID="dpSTo" CompareControl="dpSFrom"
                                        CompareMessage="结束日期应该大于开始日期" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxSection" Label="部门机构" EmptyText="个险部、银保部等" />
                                    <ext:TextBox runat="server" ID="tbxName" Label="关键词" EmptyText="姓名、套餐、营业区、收件人" />
                                    <ext:DropDownList ID="dropReportStatus" Enabled="false" runat="server" Label="报告状态">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" Label="省份" Resizable="true" EnableEdit="true" ID="dpProvince"></ext:DropDownList>
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
                        EnableRowNumber="true" Title="Grid" ShowHeader="false" DataKeyNames="ORDERNUM,STATUS,REALNAME,IDNUMBER,dictreporttemplateid,AUDITSTATUS">
                        <Columns>
                            <ext:BoundField DataField="ordernum" DataToolTipField="ordernum" HeaderText="体检号" Width="110px" />
                            <ext:BoundField DataField="STATUSNAME" DataToolTipField="STATUSNAME" HeaderText="标本状态" Width="70px" />
                            <ext:BoundField DataField="AUDITSTATUSTEXT" HeaderText="财务状态" Width="60px" />
                            <ext:BoundField DataField="AUDITTIME" HeaderText="审核日期" Width="125px" />
                            <ext:BoundField DataField="BATCHNUMBER" HeaderText="场次号" Width="100px" />
                            <ext:BoundField DataField="realname" DataToolTipField="realname" HeaderText="姓名" Width="60px" />
                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="50px" />
                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField HeaderText="联系方式" DataToolTipField="MOBILE" DataField="MOBILE" Width="90" />
                            <ext:BoundField DataField="createdate" DataToolTipField="createdate" HeaderText="登记时间" Width="85px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="分点" Width="120px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="体检单位" Width="150px" />
                            <ext:BoundField DataField="section" DataToolTipField="section" HeaderText="部门机构" Width="60" />
                            <ext:BoundField DataField="area" DataToolTipField="area" HeaderText="营业区" Width="80px" />
                            <ext:BoundField HeaderText="套餐名称" DataToolTipField="ORDERTESTLST" DataField="ORDERTESTLST" Width="200" />
                            <ext:BoundField DataField="samplingdate" HeaderText="采样日期" Width="85px" DataFormatString="{0:yyyy-MM-dd}"  />
                            <ext:BoundField HeaderText="邮寄地址" DataField="POSTADDRESS" DataToolTipField="POSTADDRESS" Width="250" />
                            <ext:BoundField HeaderText="收件人" DataField="RECIPIENT" DataToolTipField="RECIPIENT" Width="60" />
                            <ext:BoundField HeaderText="联系电话" DataField="CONTACTNUMBER" DataToolTipField="CONTACTNUMBER" ExpandUnusedSpace="true"  />
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
        Target="Top" IsModal="True" Width="960" Height="600">
    </ext:Window>
    <script type="text/javascript">
        function onReady() {
            var tbxmemberID = '<%= tbxmember.ClientID %>';
            var tbxmember = Ext.getCmp(tbxmemberID);
            tbxmember.on("specialkey", function (box, e) {
//                if (e.getKey() == e.ENTER) {
//                    if (tbxmember.getValue() == "") {
//                        top.X.util.alert("请输入要搜索的内容！");
//                        return false
//                    } else {
//                        __doPostBack(tbxmemberID, 'specialkey');
//                    }
                //                }
                __doPostBack(tbxmemberID, 'specialkey');
            });
        }
    </script>
    </form>
</body>
</html>
