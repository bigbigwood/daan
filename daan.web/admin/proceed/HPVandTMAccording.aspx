<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HPVandTMAccording.aspx.cs" Inherits="daan.web.admin.proceed.HPVandTMAccording" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Regions>
            <ext:Region runat="server" Layout="Fit" Height="35px" ShowBorder="false" ShowHeader="false" Split="false">
                <Toolbars>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Label Text="统计类别" runat="server" CssStyle="margin-left:10px"></ext:Label>
                            <ext:DropDownList ID="DropAccordingType" Resizable="True" runat="server">
                                <ext:ListItem Text="全部" Value="0" />
                                <ext:ListItem Text="TM检查" Value="1" />
                                <ext:ListItem Text="HPV检查" Value="2" />
                            </ext:DropDownList>

                            <ext:Label Text="开始日期：" runat="server" CssStyle="margin-left:20px"></ext:Label>
                            <ext:DatePicker runat="server" AutoPostBack="true" ID="Dp_BeginDate"></ext:DatePicker>
                            <ext:Label Text="结束日期：" runat="server" CssStyle="margin-left:20px"></ext:Label>
                            <ext:DatePicker runat="server" AutoPostBack="true" ID="Dp_EndDate"></ext:DatePicker>

                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="GridAcconding" runat="server" AutoScroll="true" ShowHeader="false" PageSize="20" IsDatabasePaging="true" AllowPaging="true"
                     AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="GridAcconding_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="realname" DataToolTipField="realname" HeaderText="姓名" Width="80px" />
                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="50px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="180px" />
                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="100px" />
                            <ext:BoundField DataField="productname" DataToolTipField="productname" HeaderText="所做套餐" Width="200px" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
