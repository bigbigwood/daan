<%@ Page Language="C#" Title="LIS退单取消审核处理中心" AutoEventWireup="true" CodeBehind="FrmLisExceptional.aspx.cs"
    Inherits="daan.web.admin.exceptional.FrmLisExceptional" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LIS退意取消审核管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="70px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:DatePicker ID="datebegin" Label="登记时间" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:DatePicker ID="dateend" Label="至" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:DropDownList ID="DropState" Label="处理状态" runat="server">
                                        <ext:ListItem Text="全部" Value="-1" Selected="true" />
                                        <ext:ListItem Text="未处理" Value="0" />
                                        <ext:ListItem Text="已处理" Value="1" />
                                        <ext:ListItem Text="已处理" Value="1" />
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:TextBox runat="server" ID="txtOrderNum" Label="体检号" EmptyText="此条件忽略其他条件" />
                                    <ext:TextBox runat="server" ID="txtBarCode" Label="条码号" EmptyText="此条件忽略其他条件" />
                                    <ext:TextBox runat="server" ID="txtName" Label="姓名" />
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridOrders" Title="订单列表" DataKeyNames="Orderexceptionid" AutoScroll="true"
                        ShowHeader="false" PageSize="20" IsDatabasePaging="true"
                        EnableTextSelection="true" AllowPaging="true" runat="server" AutoWidth="true"
                        AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="GridOrders_PageIndexChange">
                        <Columns>
                            <ext:WindowField ColumnID="myWindowField" Width="60px" WindowID="WindowFrame" HeaderText="处理意见"
                                Icon="Pencil" DataTextFormatString="{0}" DataIFrameUrlFields="Orderexceptionid"
                                DataIFrameUrlFormatString="FrmLisExceptionEdit.aspx?id={0}" DataWindowTitleField="ordernum"
                                DataWindowTitleFormatString="体检号 - {0}" />
                            <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="100px" />
                            <ext:BoundField DataField="barcode" HeaderText="条码号" Width="100px" />
                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="80px" />
                            <ext:BoundField DataField="EXCEPTIONTYPE" HeaderText="异常类别" Width="80px" />
                            <ext:BoundField DataField="APPLYBY" HeaderText="LIS申请人" Width="80px" />
                            <ext:BoundField DataField="APPLYDATE" HeaderText="申请时间" DataFormatString="{0:yyyy-MM-dd}"
                                Width="80px" />
                            <ext:BoundField DataField="REMARK" HeaderText="理由" DataToolTipField="REMARK" Width="200px" />
                            <ext:BoundField DataField="Approveby" HeaderText="LIS审批人" Width="80px" />
                            <ext:BoundField DataField="Approvedate" HeaderText="审批时间" DataFormatString="{0:yyyy-MM-dd}"
                                Width="80px" />
                            <ext:BoundField DataField="STATUS" HeaderText="LIS状态" Width="80px" />
                            <ext:BoundField DataField="DISPOSEBY" HeaderText="处理人" Width="80px" />
                            <ext:BoundField DataField="DISPOSEDATE" HeaderText="处理日期" Width="80px" />
                            <ext:BoundField DataField="Disposestate" HeaderText="处理状态" Width="80px" />
                            <ext:BoundField DataField="Suggestion" HeaderText="处理意见" Width="80px" />
                            <ext:BoundField DataField="createdate" HeaderText="登记时间" Width="80px" DataFormatString="{0:yyyy-MM-dd}" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WindowFrame" Hidden="true" EnableIFrame="true" runat="server" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="900" Height="500">
    </ext:Window>
    </form>
</body>
</html>
