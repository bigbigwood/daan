<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillOut.aspx.cs" Inherits="daan.web.admin.bill.BillOut"
    EnableViewStateMac="false" Title="已出账账单" %>

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
                                Height="90px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:ContentPanel ID="ContentPanel1" runat="server" ShowHeader="false" ShowBorder="false"
                                        BodyPadding="1px">
                    <table cellpadding="5" cellspacing="5" style="width: 100%;">
                        <tr>
                            <td style="text-align: right; width:60px">
                                <ext:Label runat="server" Text="分点："  ID="lblLab" Width="60px"> </ext:Label>
                            </td>
                            <td colspan="2">
                                <ext:DropDownList runat="server" Width="205px" ID="dropLab" Resizable="True" AutoPostBack="true" OnSelectedIndexChanged="dropLab_SelectedIndexChanged"
                                    EnableEdit="true">
                                </ext:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width:60px; height: 30;">
                                <ext:Label runat="server" Text="选择单位：" ID="lblCustomer" Width="60px">
                                </ext:Label>
                            </td>
                            <td>
                                <ext:DropDownList runat="server"  Width="130px" ID="dropCustomer" AutoPostBack="false" Resizable="True"
                                    EnableEdit="true">
                                    <ext:ListItem Selected="true" Text="请选择" Value="-1" />
                                </ext:DropDownList>
                            </td>
                            <td style="text-align: left">
                                <ext:CheckBox runat="server" Text="外包账单" Width="90px" CssClass="inline" ID="chkSendOut"
                                    AutoPostBack="true" OnCheckedChanged="chkSendOut_CheckedChanged">
                                </ext:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width:60px;">
                                <ext:Label runat="server"  Width="60px" Text="账号：" ID="Label1">
                                </ext:Label>
                            </td>
                            <td>
                            <ext:numberbox runat="server"  id="tbxBillheadid" cssclass="tbxwidth100" />
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
                                        Title="表格" ShowHeader="False" Width="300px" Height="350" IsDatabasePaging="true" EnableTextSelection="true"
                                        ID="gvList" OnRowClick="gvList_RowClick" OnPageIndexChange="gvList_PageIndexChange">
                                        <Toolbars>
                                            <ext:Toolbar ID="Toolbar1" runat="server">
                                                <Items>
                                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                                    </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="接收" Icon="TableGo" CssClass="inlineButton" ID="btnReceive"
                                                        OnClick="btnReceive_Click">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="作废" Icon="Cancel" CssClass="inlineButton" ID="btnInvalid"
                                                        OnClick="btnInvalid_Click">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="导出账单头信息" Icon="PageExcel" CssClass="inlineButton" ID="btnExcel"
                                                        EnableAjax="false" OnClick="btnExcel_Click" DisableControlBeforePostBack="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Toolbars>
                                        <Columns>
                                            <ext:BoundField DataField="Billheadid" HeaderText="账单号" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Customername" HeaderText="客户名称"></ext:BoundField>
                                            <ext:BoundField DataField="Createdate" HeaderText="出账时间" Width="80"></ext:BoundField>
                                            <ext:BoundField DataField="Totalstandardprice" HeaderText="标准收费" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Totalfinalprice" HeaderText="实际收费" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Salename" HeaderText="业务员" Width="60"></ext:BoundField>
                                            <ext:BoundField DataField="Dictcheckbillid" HeaderText="财务清单核对人" Hidden="true"></ext:BoundField>
                                            <ext:BoundField DataField="Begindate" HeaderText="开始时间" Hidden="true"></ext:BoundField>
                                            <ext:BoundField DataField="Enddate" HeaderText="结束时间" Hidden="true"></ext:BoundField>
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
                ShowBorder="false" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="region4" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center"
                                runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid runat="server" AllowPaging="True" PageSize="20" EnableRowNumber="True" EnableTextSelection="true"
                                        IsDatabasePaging="true" Title="表格" ShowHeader="False" AutoHeight="True" Width="680px"
                                        Height="525px" OnPageIndexChange="gvDetail_PageIndexChange" ID="gvDetail">
                                        <Toolbars>
                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                <Items>
                                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                                    </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="调整价钱" Icon="MoneyYen" CssClass="inlineButton" ID="btnAdjustPrice"
                                                        OnClick="btnAdjustPrice_Click">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="打印明细" Icon="Printer" CssClass="inlineButton" ID="btnPrint"
                                                        OnClick="btnPrint_Click" OnClientClick="test()">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="删除标本" Icon="Delete" CssClass="inlineButton" ID="btnDeleteSample"
                                                        OnClick="btnDeleteSample_Click">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="导出账单详细信息" Icon="PageExcel" CssClass="inlineButton" ID="btnExcels"
                                                        OnClick="btnExcels_Click" EnableAjax="false" DisableControlBeforePostBack="false">
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server">
                                                     </ext:ToolbarSeparator>
                                                    <ext:Button runat="server" Text="备注" Icon="Book" CssClass="inlineButton" ID="btnRemark"
                                                        OnClick="btnRemark_Click">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Toolbars>
                                        <Columns>
                                            <ext:BoundField DataField="Status" HeaderText="状态" Width="50px"></ext:BoundField>
                                            <ext:BoundField DataField="Enterdate" HeaderText="登记日期" Width="80px"></ext:BoundField>
                                            <ext:BoundField DataField="Ordernum" HeaderText="体检号" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Realname" HeaderText="病人姓名" Width="60" />
                                            <ext:BoundField DataField="Testname" HeaderText="体检项目" Width="150px"></ext:BoundField>
                                            <ext:BoundField DataField="Testcount" HeaderText="项目数量" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Standardprice" HeaderText="标准收费" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Groupprice" HeaderText="分点收费" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Contractprice" HeaderText="应收金额" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Finalprice" HeaderText="实收金额" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="Selfremark" HeaderText="财务说明" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Remark" HeaderText="财务备注" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="Billheadid" Hidden="True" HeaderText="账单号"></ext:BoundField>
                                            <ext:BoundField DataField="Ids" Hidden="true"></ext:BoundField>
                                            <ext:BoundField DataField="Testitemids" Hidden="true"></ext:BoundField>
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="region5" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Bottom"
                                Height="25px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:TextBox runat="server" Readonly="True" Width="750" ID="tbxMsg">
                                    </ext:TextBox>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385" OnClose="WinBillRemark_Close">
    </ext:Window>
    <!--获取mac地址取该用户的本地打印机IP等信息-->
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
