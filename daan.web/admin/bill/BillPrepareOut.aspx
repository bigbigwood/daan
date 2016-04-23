<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillPrepareOut.aspx.cs"
    EnableViewStateMac="false" Inherits="daan.web.admin.bill.BillPrepareOut" Title="预出账" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
</head>
<body>
    <form id="form7" runat="server">
    <ext:PageManager ID="PageManager2" AutoSizePanelID="RegionPanel1" runat="server"
       HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                      <ext:Button runat="server" Text="查询" ID="btnSearch"  Icon="Magnifier" OnClick="btnSearch_Click"></ext:Button>
                      <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPreInvoice" runat="server" Icon="TableGear" Text="预出账" CssClass="inlineButton"
                        OnClick="btnPreInvoice_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <%--<ext:Button ID="btnAdjustPrice" runat="server" Icon="MoneyYen" Text="调整价钱" CssClass="inlineButton"
                        OnClick="btnAdjustPrice_Click">
                    </ext:Button>--%>
                 <%--   <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>--%>
                    <ext:Button ID="btnDeleteSample" runat="server" Icon="Delete" Text="删除标本" CssClass="inlineButton"
                        OnClick="btnDeleteSample_Click1" Hidden="true">
                    </ext:Button>
                <%--    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>--%>
                    <ext:Button ID="btnRemark" runat="server" Icon="Book" Text="备注" CssClass="inlineButton"
                        OnClick="btnRemark_Click" Hidden="true">
                    </ext:Button>
          <%--          <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>--%>
                    <ext:Button ID="btnOperationLog" Icon="SystemNew" runat="server" Text="操作记录" CssClass="inlineButton"
                        OnClick="btnOperationLog_Click" Hidden="true">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="regiontop" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="40px" runat="server" ShowBorder="false">
                <Items>
                    <ext:ContentPanel ID="ContentPanel1" runat="server" ShowHeader="false" ShowBorder="false"
                        BodyPadding="1px">
                <table width="800px" >
                <tr style="height:35px;">
                    <td style="width: 36px; text-align: left; ">
                          <ext:Label runat="server" Text="分点：" ID="lblLab" Width="36"> </ext:Label>
                     </td>
                      <td style="width: 170px; text-align: left;">
                      <ext:DropDownList runat="server" Width="130px" ID="dropLab" AutoPostBack="true" Resizable="True"
                           OnSelectedIndexChanged="dropLab_SelectedIndexChanged" EnableEdit="true"> </ext:DropDownList>
                      </td>
                      <td style="width: 150px; text-align: center;">
                         <ext:CheckBox runat="server" Text="外包帐单" ID="chkSendOut" AutoPostBack="true"
                                        OnCheckedChanged="chkSendOut_CheckedChanged">
                                    </ext:CheckBox>
                      </td>
                      <td style="width: 80px; text-align: right;">
                         <ext:Label runat="server" Text="选择单位：" ID="Label1" Width="60"> </ext:Label>
                      </td>
                      <td style="width: 140px; text-align: left;">
                        <ext:DropDownList runat="server"  Width="140px" ID="dropCustomer" EnableEdit="false" Resizable="True" AutoPostBack="true">
                                    </ext:DropDownList>
                      </td>
                      <td style="width:100px; text-align: right;">
                         <ext:Label runat="server" Text="登记日期：" ID="Label3" Width="100"> </ext:Label>
                      </td>
                      <td style="width: 130px; text-align: left;">
                        <ext:DatePicker runat="server" Width="90px" ID="dtpStart">
                                    </ext:DatePicker>
                      </td>
                      <td style="width: 36px; text-align: right;">
                         <ext:Label runat="server" Text="到：" ID="Label2" Width="26"> </ext:Label>
                      </td>
                      <td style="width: 130px; text-align: left;">
                         <ext:DatePicker runat="server"  Width="90px" ID="dtpEnd">
                                    </ext:DatePicker>
                      </td>
                </tr>
                </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="Regioncenter" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Center" runat="server" ShowBorder="false">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false"
                        AutoWidth="true" EnableRowNumber="True" AutoHeight="true" AllowPaging="true" EnableTextSelection="true"
                        IsDatabasePaging="true" runat="server" Height="530" EnableMultiSelect="false" 
                        OnPageIndexChange="gvList_PageIndexChange">
                        <Columns>
                            <ext:BoundField Width="50px" DataField="Status" HeaderText="状态" />
                            <ext:BoundField Width="80px" DataField="Enterdate" HeaderText="登记日期" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField Width="100px" DataField="Ordernum" HeaderText="体检号" />
                            <ext:BoundField Width="60" DataField="Realname" HeaderText="病人姓名" />
                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="体检项目" />
                            <ext:BoundField Width="60px" DataField="Testcount" HeaderText="项目数量" />
                            <ext:BoundField Width="60px" DataField="Standardprice" HeaderText="标准收费" />
                            <ext:BoundField Width="60px" DataField="Groupprice" HeaderText="分点收费" />
                            <ext:BoundField Width="60px" DataField="Contractprice" HeaderText="应收金额" />
                            <ext:BoundField Width="60px" DataField="Finalprice" HeaderText="实收金额" />
                            <ext:BoundField Width="140px" DataField="Selfremark" HeaderText="财务说明" />
                            <ext:BoundField Width="140px" DataField="Remark" HeaderText="财务备注" />
                            <ext:BoundField Hidden="true" DataField="Billheadid" HeaderText="账单号" />
                            <ext:BoundField DataField="Ids" Hidden="true"></ext:BoundField>
                            <ext:BoundField DataField="Testitemids" Hidden="true"></ext:BoundField>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
            <ext:Region ID="Regionbottom" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Bottom"
                runat="server" ShowBorder="false" Height="25px">
                <Items>
                    <ext:TextBox ID="tbxMsg" runat="server" Readonly="true" Width="1070">
                    </ext:TextBox>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="Hide"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        OnClose="WinBillRemark_Close" Width="580" Height="385">
    </ext:Window>
    </form>
</body>
</html>
