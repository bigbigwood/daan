<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinishOrdersList.aspx.cs" Inherits="daan.web.admin.analyse.FinishOrdersList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager runat="server" AutoSizePanelID="rp1" />
    <ext:RegionPanel runat="server" ID="rp1" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:ToolbarText runat="server" Text="状态："></ext:ToolbarText>
                    <ext:DropDownList runat="server" ID="dpStatus" Width="80px">
                        <ext:ListItem Text="初步总检" Value="1" />
                        <ext:ListItem Text="完成总检" Value="2" />
                    </ext:DropDownList>
                    <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                    <ext:ToolbarText runat="server" Text="总检医生："></ext:ToolbarText>
                    <ext:TextBox runat="server" ID="txtName" Width="80px"></ext:TextBox>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                    <ext:ToolbarText runat="server" Text="时间："></ext:ToolbarText>
                    <ext:DatePicker runat="server" ID="TimeStart"></ext:DatePicker>
                    <ext:ToolbarText runat="server" Text="到"></ext:ToolbarText>
                    <ext:DatePicker runat="server" ID="TimeEnd"></ext:DatePicker>
                    <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                    <ext:Button runat="server" ID="btnQuery" OnClick="btnQuery_Click" Text="查询" Icon="Magnifier"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region runat="server" Layout="Fit" ShowBorder="false" ShowHeader="false">
                <Items>
                    <ext:Grid runat="server" ID="gridList" ShowBorder="false" ShowHeader="false" EnableRowNumber="true"
                    AutoWidth="true" AutoHeight="true" DataKeyNames="ordernum" AllowPaging="true" IsDatabasePaging="true"
                    AutoScroll="true" PageSize="20" OnPageIndexChange="gridList_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="110px" />
                            <ext:BoundField DataField="status" HeaderText="状态" Width="80px" />
                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="60px" />
                            <ext:BoundField DataField="age" HeaderText="年龄" Width="50px" />
                            <ext:BoundField DataField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField DataField="ismarried" HeaderText="婚否" Width="50px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="120px" />
                            <ext:BoundField DataField="authorizeddate" HeaderText="初步总检时间" Width="130px"/>
                            <ext:BoundField DataField="authorizedByname" HeaderText="初步总检医生" Width="85px" />
                            <ext:BoundField DataField="finishdate" HeaderText="总检时间" Width="130px" />
                            <ext:BoundField DataField="finishByname" HeaderText="总检医生" Width="60px" />
                            <ext:BoundField DataField="createdate" HeaderText="登记时间" Width="85px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="remarks" HeaderText="备注" ExpandUnusedSpace="true"/>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
