<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProUploadToLIS.aspx.cs" Inherits="daan.web.admin.proceed.ProUploadToLIS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>体检信息上传至LIS</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form4" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server">
    </ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarText Text="显示行数：">
                    </ext:ToolbarText>
                        <ext:DropDownList runat="server" ID="ddlRowCount" EnableEdit="true" ForceSelection="false" Width="70" Regex="^\d+$" >
                        <ext:ListItem Text="50" Value="50" Selected="true" />
                        <ext:ListItem Text="100" Value="100" />
                        <ext:ListItem Text="150" Value="150" />
                        <ext:ListItem Text="200" Value="200" />
                        <ext:ListItem Text="250" Value="250" />
                        <ext:ListItem Text="300" Value="300" />
                    </ext:DropDownList>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                     <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button runat="server" Text="查询" Icon="Magnifier" ID="btnSearch" OnClick="btnSearch_Click">
                            </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="重新上传" Icon="Disk" runat="server" OnClick="btnSave_Click">
                    </ext:Button>
                    
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Layout="Fit" EnableBackgroundColor="false"
                Title="Center Region">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:ToolbarText ID="ToolbarText1" Text="分点：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList ID="dropDictLab"  Resizable="True" runat="server" Label="分点" EnableEdit="true" Width="200px">
                            </ext:DropDownList>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="状态：">
                            </ext:ToolbarText>
                            <ext:DropDownList runat="server" Label="上传状态" Resizable="True" ID="dropTransed" Width="80px">
                                <ext:ListItem Selected="True" Text="上传失败" Value="2"></ext:ListItem>
                                <ext:ListItem Text="未上传" Value="0"></ext:ListItem>
                                <ext:ListItem Text="已上传" Value="1"></ext:ListItem>
                                <ext:ListItem Text="全部" Value="-1"></ext:ListItem>
                            </ext:DropDownList>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="关键字：">
                            </ext:ToolbarText>
                            <ext:TextBox runat="server" Label="关键字" Width="110px" ID="tbStrKey" EmptyText="体检流水号,条码号,姓名"
                                ShowLabel="false">
                            </ext:TextBox>
                           
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="gdUploadToLIS" EnableCheckBoxSelect="True" EnableRowNumber="True" runat="server" EnableTextSelection="true" ShowHeader="false" DataKeyNames="ORDERNUM,FaileTRANSED">
                        <Columns>
                         <ext:BoundField DataField="ORDERNUM" HeaderText="体检流水号" Width="105px" />
                            <ext:BoundField DataField="BARCODE" HeaderText="条码号" />
                            <ext:BoundField DataField="REALNAME" HeaderText="姓名" />
                            <ext:BoundField DataField="FaileTRANSED" HeaderText="上传状态" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
