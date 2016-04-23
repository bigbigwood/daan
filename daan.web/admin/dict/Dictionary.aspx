<%@ Page Title="基础字典分类维护" Language="C#" AutoEventWireup="true" CodeBehind="Dictionary.aspx.cs"
    Inherits="daan.web.admin.dict.Dictionary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">       
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="260px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" Margins="5 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="60px" LabelAlign="Right">
                                        <Items>
                                            <ext:TwinTriggerBox ID="btSearch" Trigger1Icon="Clear" ShowTrigger1="False" EmptyText="请输入关键字搜索"
                                                Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true" OnTrigger1Click="btSearch_Trigger1Click"
                                                OnTrigger2Click="btSearch_Trigger2Click" Width="170px">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false" >
                                <Items>
                                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false"
                                        EnableCheckBoxSelect="false" Height="425px" AutoWidth="true" AllowPaging="true"
                                        EnableRowClick="true" runat="server" AutoScroll="true" DataKeyNames="Dictlibraryid,Librarycode,Libraryname"
                                        IsDatabasePaging="true" EnableRowNumber="true" OnPageIndexChange="gvList_PageIndexChange"
                                        OnRowClick="gvList_RowClick">
                                        <Columns>
                                            <ext:BoundField Width="35px" DataField="Dictlibraryid" DataFormatString="{0}" HeaderText="编号" />
                                            <ext:BoundField Width="100px" DataField="Librarycode" DataFormatString="{0}" HeaderText="代码" />
                                            <ext:BoundField Width="100px" DataField="Libraryname" DataFormatString="{0}" HeaderText="名称" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:SimpleForm ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" EnableBackgroundColor="false"
                        runat="server" LabelWidth="60px" Title="详细信息" LabelAlign="Right">
                        <Items>
                            <ext:TextBox ID="txtLibraryCode" Label="代码" Required="true" ShowLabel="true" ShowRedStar="true"
                                runat="server">
                            </ext:TextBox>
                            <ext:TextBox ID="txtLibraryName" Label="名称" Required="true" ShowLabel="true" ShowRedStar="true"
                                runat="server">
                            </ext:TextBox>
                        </Items>
                    </ext:SimpleForm>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
