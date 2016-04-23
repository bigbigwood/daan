<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillCashReceive.aspx.cs"
    Inherits="daan.web.admin.bill.BillCashReceive" EnableViewStateMac="false" Title="现金接收" %>

<%@ Register Assembly="FastReport.Web, Version=1.7.1.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c"
    Namespace="FastReport.Web" TagPrefix="cc1" %>
<%@ Register Src="../../usercontrol/DropInitBasic.ascx" TagName="DropInitBasic" TagPrefix="uc2" %>
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
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="查询" ID="btnSearch" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="修改价格" ID="btnAdjustPrice" Icon="Magnifier" OnClick="btnAdjustPrice_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrintIndividual" runat="server" Icon="Printer" Text="收据重打" CssClass="inlineButton"
                        OnClick="btnPrintIndividual_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrint" runat="server" Icon="Printer" Text="打印清单" CssClass="inlineButton"
                        OnClick="btnPrint_Click" OnClientClick="test()">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnReceive" runat="server" Icon="TableGo" Text="接收" ConfirmTitle="提示"
                        OnClick="btnReceive_Click" ConfirmTarget="Top">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnRefundment" runat="server" Icon="ArrowUndo" Text="退款" CssClass="inlineButton"
                        EnableAjax="false" OnClick="btnRefundment_Click"
                        ConfirmText="是否对选中订单退款？"
                        ConfirmTitle="体检系统" ConfirmIcon="Question" ConfirmTarget="Top">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExcel" runat="server" Icon="PageExcel" Text="导出" CssClass="inlineButton"
                        DisableControlBeforePostBack="false" OnClick="btnExcel_Click" EnableAjax="false">
                    </ext:Button>  
                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnRemark" runat="server" Icon="Book" Text="备注" CssClass="inlineButton"
                        OnClick="btnRemark_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnOperationLog" runat="server" Icon="SystemNew" Text="操作记录" CssClass="inlineButton"
                        OnClick="btnOperationLog_Click">
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
                        <table width="800px">
                            <tr style="height:35px;">
                                <td style="width: 36px; text-align:right;">
                                    <ext:Label runat="server" Text="分点：" ID="lblLab" Width="36">
                                    </ext:Label>
                                </td>
                                <td style="text-align: left;">
                                   <ext:DropDownList runat="server" Width="120px" ID="dropLab" Resizable="True" EnableEdit="true"></ext:DropDownList>
                                </td>
                                <td style="width: 40px; text-align:right;">
                                    <ext:Label runat="server" Text="状态：" ID="LabStatus" Width="40">
                                    </ext:Label>
                                </td>
                                <td style="text-align:left;">
                                    <uc2:DropInitBasic runat="server" ID="dropStatus"  BasicType="BILLHEADSTATUS" Type="1" Width="90">
                                    </uc2:DropInitBasic>
                                </td>
                                <td style="width: 60px;text-align:right;">
                                    <ext:Label runat="server" Text="收费日期：" ID="lblStart" Width="80">
                                    </ext:Label>
                                </td>
                                <td style="text-align:left;">
                                    <ext:DatePicker runat="server" Width="90px" ID="dtpStart">
                                    </ext:DatePicker>
                                </td>
                                <td style="width: 20px; text-align:right;">
                                    <ext:Label runat="server" Text="到：" ID="lblEnd" Width="26">
                                    </ext:Label>
                                </td>
                                <td style="text-align:left;">
                                    <ext:DatePicker runat="server" Width="90px" ID="dtpEnd">
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
                        EnableTextSelection="true" IsDatabasePaging="true" AutoWidth="true" EnableCheckBoxSelect="true"
                        EnableRowNumber="true" AllowPaging="true" runat="server" Height="530" OnPageIndexChange="gvList_PageIndexChange"
                       >
                        <Columns>
                            <ext:BoundField DataField="Billheadid" HeaderText="账单号" Width="60" />
                            <ext:BoundField DataField="Realname" HeaderText="姓名" Width="60" />
                            <ext:BoundField DataField="Duedate" HeaderText="收费日期" Width="80" />
                            <ext:BoundField DataField="Ordernum" HeaderText="体检号" />
                            <ext:BoundField DataField="Testname" HeaderText="体检项目" Width="150" />
                            <ext:BoundField DataField="Testcount" HeaderText="项目数量" Width="60" />
                            <ext:BoundField DataField="Totalstandardprice" HeaderText="标准收费" Width="60" />
                            <ext:BoundField DataField="Totalgrouprprice" HeaderText="分点收费" Width="60" />
                            <ext:BoundField DataField="Totalcontractprice" HeaderText="应收金额" Width="60" />
                            <ext:BoundField DataField="Totalfinalprice" HeaderText="实收金额" Width="60" />

                            <ext:TemplateField  HeaderText="账单状态" Width="60px">
                                <ItemTemplate><%-- =="1"?"已接收":"预出账"--%>
                                    <asp:Label ID="Label1"  runat="server" Text='<%# Eval("Status").ToString() %> '></asp:Label>
                                </ItemTemplate>
                            </ext:TemplateField>
                            <ext:BoundField DataField="Selfremark" HeaderText="财务说明" Width="120" />
                            <ext:BoundField DataField="Remark" HeaderText="财务备注" Width="120" />  
                            
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
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385" OnClose="WinBillRemark_Close">
    </ext:Window>
    <ext:Window ID="WindowFrame" Hidden="true" EnableIFrame="true" runat="server" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="850" Height="518" OnClose="WindowFrame_Close">
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
