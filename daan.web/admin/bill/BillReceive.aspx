<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillReceive.aspx.cs" Inherits="daan.web.admin.bill.BillReceive"
    Title="已接收账单" EnableViewStateMac="false" %>

<%@ Register Assembly="FastReport.Web, Version=1.7.1.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c"
    Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <ext:Region ID="RegionLeft" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="280px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="115px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:ContentPanel ID="ContentPanel1" runat="server" ShowHeader="false" ShowBorder="false"
                                        BodyPadding="1px">
                        <table cellpadding="5" cellspacing="5" style="width: 100%;" >
                    <tr>
                        <td style="text-align:left; height: 30; width:65px;">
                                <ext:Label runat="server" Text="出账时间：" ID="lblStart" Width="60px">
                                </ext:Label>
                            </td>
                            <td style="text-align:left; width:100px;">
                                <ext:DatePicker runat="server" Width="90px" ID="dtpStart">
                                </ext:DatePicker>
                            </td>
                            <td style="text-align:right; width: 10px;">
                                <ext:Label runat="server" Text="~" ID="lblEnd" Width="10px">
                                </ext:Label>
                            </td>
                            <td style="text-align:left; width:100px;">
                                <ext:DatePicker runat="server" Width="85px" ID="dtpEnd">
                                </ext:DatePicker>
                            </td>
                    </tr>
                    <tr>
                         <td style="text-align: right;width:65px;">
                                <ext:Label runat="server" Text="分点：" ID="lblLab" Width="60px"> </ext:Label>
                            </td>
                            <td colspan="3">
                                <ext:DropDownList runat="server" EnableEdit="True" AutoPostBack="True" Width="205px" Resizable="True"
                                    ID="dropLab" OnSelectedIndexChanged="dropLab_SelectedIndexChanged">
                                </ext:DropDownList>
                            </td>
                    </tr>
                        <tr>
                            <td style="text-align: right; width:65px; height: 30;">
                                <ext:Label runat="server" Text="客户名称：" ID="lblCustomer" Width="60px">
                                </ext:Label>
                            </td>
                            <td colspan="2">
                                <ext:DropDownList runat="server" EnableEdit="True" Width="115px" ID="dropCustomer" Resizable="True">
                                    <ext:ListItem Selected="True" Text="请选择" Value="-1"></ext:ListItem>
                                </ext:DropDownList>
                            </td>
                            <td style="text-align:left;">
                                <ext:CheckBox runat="server" Text="外包账单" AutoPostBack="True" Width="90px" CssClass="inline"
                                    ID="chkSendOut" OnCheckedChanged="chkSendOut_CheckedChanged">
                                </ext:CheckBox>
                            </td>
                        </tr>
                        <tr>
                           
                            <td style="text-align: right; width:65px;">
                                <ext:Label runat="server" Text="账号：" ID="Label1" Width="60px">
                                </ext:Label>
                            </td>
                            <td colspan="2">
                            <ext:numberbox runat="server"  id="tbxBillheadid" cssclass="tbxwidth100" Width="115" />
                               <%-- <ext:TextBox runat="server" ID="tbxBillheadid">
                                </ext:TextBox>--%>
                            </td>
                            <td style="text-align:left;">
                                <ext:Button runat="server" Text="查询" ID="btnSearch" Icon="Magnifier" OnClick="btnSearch_Click">
                                </ext:Button>
                            </td>
                        </tr>
                    </table>
                                    </ext:ContentPanel>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="330px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid runat="server" AllowPaging="True" PageSize="20" AutoPostBack="True" EnableCheckBoxSelect="True"
                                        Title="表格" ShowHeader="False" AutoHeight="True" Width="335px" Height="475px" EnableTextSelection="true"
                                        IsDatabasePaging="true" ID="gvList" OnRowClick="gvList_RowClick" OnPageIndexChange="gvList_PageIndexChange">
                                        <Toolbars>
                                            <ext:Toolbar ID="Toolbar1" runat="server">
                                                <Items>
                                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                                    </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="反结账" Icon="TableRefresh" CssClass="inlineButton"
                                                        ID="btnUnReceive" OnClick="btnUnReceive_Click">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Icon="PageExcel" Text="导出" CssClass="inlineButton" ID="btnExcel"
                                                        EnableAjax="False" OnClick="btnExcel_Click" DisableControlBeforePostBack="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Toolbars>
                                        <Columns>
                                            <ext:BoundField DataField="Billheadid" HeaderText="账单号" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Customername" HeaderText="客户名称"></ext:BoundField>
                                            <ext:BoundField DataField="Totalstandardprice" HeaderText="标准收费" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Totalfinalprice" HeaderText="实际收费" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Createdate" DataFormatString="{0:yyyy/MM/dd}" Width="80"
                                                HeaderText="生成账单日期"></ext:BoundField>
                                            <ext:BoundField DataField="Duedate" DataFormatString="{0:yyyy/MM/dd}" Width="80"
                                                HeaderText="预计结账日期"></ext:BoundField>
                                            <ext:BoundField DataField="Totalcontractprice" HeaderText="应收价钱" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Totalgrouprprice" HeaderText="区域价钱" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Salename" HeaderText="销售员" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Dictcheckbillid" HeaderText="财务清单核对人" Hidden="true"></ext:BoundField>
                                            <ext:BoundField DataField="Begindate" HeaderText="开始时间" Hidden="true" DataFormatString="{0:yyyy/MM/dd}">
                                            </ext:BoundField>
                                            <ext:BoundField DataField="Enddate" HeaderText="结束时间" Hidden="true" DataFormatString="{0:yyyy/MM/dd}">
                                            </ext:BoundField>
                                            <ext:BoundField DataField="Customertype" HeaderText="客户类型" Hidden="true"></ext:BoundField>
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="RegionRight" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Toolbars>
                            <ext:Toolbar runat="server" ID="Toolbar2">
                                <Items>
                                    <ext:ToolbarFill runat="server" ID="ToolbarFill1">
                                    </ext:ToolbarFill>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" Icon="Printer" Text="打印明细" CssClass="inlineButton" ID="btnPrintDetail"
                                        OnClick="btnPrintDetail_Click" OnClientClick="test()">
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                     </ext:ToolbarSeparator>
                                    <ext:Button runat="server" Icon="PageExcel" Text="导出文件" CssClass="inlineButton" ID="btnExcels" DisableControlBeforePostBack="false"
                                        EnableAjax="False" OnClick="btnExcels_Click">
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Toolbars>
                        <Regions>
                            <ext:Region ID="region4" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form runat="server" LabelWidth="55px" ShowHeader="False" BodyPadding="5px" ShowBorder="False"
                                        ID="Form11">
                                        <Rows>
                                            <ext:FormRow ID="FormRow1" runat="server">
                                                <Items>
                                                    <ext:TwinTriggerBox runat="server" ShowTrigger1="False" Trigger2Icon="Search" EmptyText="输入体检流水号或姓名"
                                                        Label="关键字" ID="ttbxSearch" OnTrigger2Click="ttbxSearch_Trigger2Click" Width="350px">
                                                    </ext:TwinTriggerBox>
                                                </Items>
                                                <Items>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="region5" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center"
                                runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid runat="server" AllowPaging="True" PageSize="20" AutoPostBack="True" EnableRowNumber="True"
                                        IsDatabasePaging="true" Title="表格" ShowHeader="False" AutoWidth="True" Height="535px" EnableTextSelection="true"
                                        ID="gvDetail" OnPageIndexChange="gvDetail_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="Status" ColumnID="ct0" HeaderText="状态" Width="50px"></ext:BoundField>
                                            <ext:BoundField DataField="Enterdate" DataFormatString="{0:yyyy/MM/dd}" ColumnID="ct1"
                                                HeaderText="接收日期" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Ordernum" HeaderText="体检号" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Realname" HeaderText="病人姓名" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Testcount" HeaderText="项目数量" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Testname" HeaderText="项目清单" Width="150px" DataToolTipField="Testname"></ext:BoundField>
                                            <ext:BoundField DataField="Standardprice" HeaderText="标准收费" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Groupprice" HeaderText="分点收费" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Contractprice" HeaderText="应收金额" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Finalprice" HeaderText="实收金额" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Selfremark" HeaderText="财务说明" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Remark" HeaderText="财务备注" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Billheadid" Hidden="True" HeaderText="账单号"></ext:BoundField>
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="region6" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Bottom"
                                Height="25px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:TextBox runat="server" Readonly="True" Width="750px" ID="tbxMsg">
                                    </ext:TextBox>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="Hide"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        OnClose="WinBillRemark_Close" Width="580" Height="385">
    </ext:Window>
    <ext:HiddenField ID="hdMac" runat="server">
    </ext:HiddenField>
    <script type="text/javascript">
        function onReady() {
            //获取mac地址取该用户的本地打印机IP等信息
            document.getElementById("<%=hdMac.ClientID %>").value = document.ActiveXPrint.GetMAC();
        }
    </script>
    </form>
</body>
</html>
