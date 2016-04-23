<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillOperationLog.aspx.cs"
    Inherits="daan.web.admin.bill.BillOperationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>操作日志记录</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" />
    <div>
        <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false" EnableTextSelection="true"
            AutoHeight="true" AllowPaging="true" runat="server" Height="350" Width="565" EnableRowNumber="true"
            OnPageIndexChange="gvList_PageIndexChange" IsDatabasePaging="true">
            <Columns>
                <ext:BoundField DataField="Ordernum" DataToolTipField="Ordernum" Width="120" HeaderText="体检号" />
                <ext:BoundField DataField="Modulename" DataToolTipField="Modulename" HeaderText="模块名称" Width="100" />
                <ext:BoundField DataField="Operatername" DataToolTipField="Operatername" HeaderText="操作员" Width="60" />
                <ext:BoundField DataField="Content" DataToolTipField="Content" HeaderText="操作内容" Width="200" />
                <ext:BoundField DataField="Operationtype" DataToolTipField="Operationtype" HeaderText="操作类型" Width="60" />
                <ext:BoundField DataField="Createdate" DataToolTipField="Createdate" HeaderText="创建时间" Width="80" />
                <ext:BoundField DataField="Remark" DataToolTipField="Remark" HeaderText="备注内容" />
            </Columns>
        </ext:Grid>
    </div>
    </form>
</body>
</html>
