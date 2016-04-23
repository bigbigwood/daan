<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProUploadPDFToSheQu.aspx.cs"
    Inherits="daan.web.admin.proceed.ProUploadPdfToSheQu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>社区上传</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form4" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server">
    </ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Regions>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                EnableBackgroundColor="false" Title="Center Region">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:ToolbarText ID="ToolbarText1" Text="分点：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList ID="dropDictLab" Resizable="True" runat="server" Label="分点" EnableEdit="true" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="dropDictLab_SelectedIndexChanged">
                            </ext:DropDownList>
                            <ext:DropDownList ID="dropDictcustomer" runat="server" Label="体检单位" Resizable="True"
                                EnableEdit="true">
                            </ext:DropDownList>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="录单时间：" runat="server">
                            </ext:ToolbarText>
                            <ext:DatePicker runat="server" Label="录单时间从" ID="dpFrom" Width="96px">
                            </ext:DatePicker>
                            <ext:DatePicker runat="server" Required="True" CompareControl="dpFrom" CompareOperator="GreaterThanEqual"
                                CompareMessage="结束日期应该大于开始日期" Label="到" ID="dpTo" Width="96px">
                            </ext:DatePicker>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="状态：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList runat="server" Label="上传状态" Resizable="True" ID="dropTransed" Width="80px">
                                <ext:ListItem Selected="True" Text="上传失败" Value="2"></ext:ListItem>
                                <ext:ListItem Text="未上传" Value="0"></ext:ListItem>
                                <ext:ListItem Text="已上传" Value="1"></ext:ListItem>
                                <ext:ListItem Text="全部" Value="-1"></ext:ListItem>
                            </ext:DropDownList>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText runat="server" Text="关键字：">
                            </ext:ToolbarText>
                            <ext:TextBox runat="server" Label="关键字" Width="110px" ID="tbStrKey" EmptyText="体检流水号,姓名"
                                ShowLabel="false">
                            </ext:TextBox>
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:Button runat="server" Text="查询" Icon="Magnifier" ID="btnSearch" OnClick="btnSearch_Click">
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnSave" Text="重新上传" Icon="Disk" runat="server" OnClick="btnSave_Click">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="gdUploadPdfToSheQu" EnableCheckBoxSelect="True" EnableRowNumber="True" EnableTextSelection="true"
                        runat="server" ShowHeader="false" AllowPaging="true" PageSize="25" IsDatabasePaging="true" 
                        DataKeyNames="ORDERNUM,FaileTRANSED" OnPageIndexChange="gdUploadPdfToSheQu_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="ORDERNUM" HeaderText="体检流水号" Width="105px" />
                            <ext:BoundField DataField="REALNAME" HeaderText="姓名" />
                            <ext:BoundField DataField="SEX" HeaderText="姓别" />
                            <ext:BoundField DataField="FaileTRANSED" HeaderText="上传状态" />
                            <ext:BoundField DataField="ENTERDATE" HeaderText="录单时间" Width="130px" />
                            <ext:BoundField DataField="REMARKS" HeaderText="备注" ExpandUnusedSpace="true" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
