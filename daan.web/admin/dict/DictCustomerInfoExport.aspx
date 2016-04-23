<%@ Page Title="客户资料导出" Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerInfoExport.aspx.cs"
    Inherits="daan.web.admin.dict.DictCustomerInfoExport" %>

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
                            <ext:Label Text="单位：" runat="server" CssStyle="margin-left:10px"></ext:Label>
                            <ext:DropDownList ID="DropCustomer" EnableEdit="true" Resizable="True" runat="server"></ext:DropDownList>
                            <ext:Label Text="姓名：" runat="server" CssStyle="margin-left:20px"></ext:Label>
                            <ext:TextBox runat="server" Width="110px" ID="tbxName"/>
                            <ext:Label runat="server" Text="身份证号：" CssStyle="margin-left:20px"></ext:Label>
                            <ext:TextBox runat="server" ID="tbxIDNumber" />
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnExport" runat="server" Text="导出" DisableControlBeforePostBack="false" Icon="PageExcel" EnableAjax="false"
                                OnClick="btnExport_Click">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="GridInfos" runat="server" DataKeyNames="" AutoScroll="true" EnableCheckBoxSelect="false"
                        ShowHeader="false" PageSize="20" IsDatabasePaging="true" EnableTextSelection="true"
                        AllowPaging="true" AutoWidth="true" AutoHeight="true" EnableRowNumber="true"
                        OnPageIndexChange="GridInfos_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="Realname" HeaderText="姓  名" Width="80px" />
                            <ext:BoundField DataField="Sex" HeaderText="性别" Width="50px" />
                            <ext:BoundField DataField="Birthday" HeaderText="出生日期" Width="100px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="Mobile" HeaderText="手机" Width="100px" />
                            <ext:BoundField DataField="Idnumber" HeaderText="身份证" Width="110px" />
                            <ext:BoundField DataField="Addres" HeaderText="住址" ExpandUnusedSpace="true"/>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
