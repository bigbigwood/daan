<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteOrder.aspx.cs" Inherits="daan.web.admin.exceptional.DeleteOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>异常订单删除</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Label Text="体检号：" runat="server"></ext:Label>
                    <ext:TextBox runat="server" ID="txtOrderNum" EmptyText="体检号、条码号"></ext:TextBox>
                    <ext:Button runat="server" ID="btnQuery" Text="查询" Icon="Magnifier" OnClick="btnQuery_Click" CssStyle="padding-left:10px"></ext:Button>
                    <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                    <ext:Button runat="server" ID="btnDelete" Text="删除" Icon="Delete" ConfirmIcon="Question" ConfirmText="确认要删除选中的订单吗？" OnClick="btnDelete_Click"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region runat="server" Layout="Fit" ShowBorder="false" ShowHeader="false">
                <Items>
                    <ext:Grid ID="grigList" runat="server" EnableCheckBoxSelect="false" EnableMultiSelect="false" Title="待删除订单"
                     AutoWidth="true" AutoHeight="true" EnableRowNumber="true" ShowBorder="false" ShowHeader="false" DataKeyNames="ordernum">
                        <Columns>
                            <ext:BoundField DataField="ordernum" DataToolTipField="ordernum" HeaderText="体检号" Width="110px" />
                            <ext:BoundField DataField="barcode" DataToolTipField="barcode" HeaderText="条码号" Width="110px" />
                            <ext:BoundField DataField="realname" DataToolTipField="realname" HeaderText="姓名" Width="80px" />
                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="50px" />
                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField DataField="ismarried" DataToolTipField="ismarried" HeaderText="婚否" Width="50px" />
                            <ext:BoundField DataField="status" DataToolTipField="status" HeaderText="状态" Width="80px" />
                            <ext:BoundField DataField="iscancel" DataToolTipField="iscancel" HeaderText="是否作废" Width="60px" />
                            <ext:BoundField DataField="createdate" DataToolTipField="createdate" HeaderText="登记时间" Width="85px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="120px" />
                            <ext:BoundField DataField="enterby" DataToolTipField="enterby" HeaderText="录单人" Width="80px" />
                            <ext:BoundField DataField="remarks" DataToolTipField="remarks" HeaderText="备注" ExpandUnusedSpace="true"/>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
   
    </form>
</body>
</html>
