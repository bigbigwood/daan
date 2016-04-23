<%@ Page Title="用户资源设置" Language="C#" AutoEventWireup="true" CodeBehind="DictUserInfoList.aspx.cs"  EnableViewStateMac="false" EnableEventValidation="false"
    Inherits="daan.web.admin.dict.DictUserInfoList" %>

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
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSynchronous" runat="server" Icon="Disk" Text="同步用户" OnClick="btnSynchronous_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
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
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="left">
                                        <Items>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                EmptyText="请输入用户名或工号" OnTrigger2Click="btSearch_Trigger2Click" Width="150px">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" runat="server" EnableRowNumber="true" Title="Grid" ShowHeader="false" EnableTextSelection="true"
                                        DataKeyNames="Dictuserid,Username" Width="380" EnableRowClick="true" Height="425"  
                                        OnRowClick="gvList_RowClick" AllowPaging="true" PageSize="20" IsDatabasePaging="true"
                                        OnRowCommand="gvList_RowCommand" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="80px" DataField="Username" DataFormatString="{0}" HeaderText="用户姓名"
                                                DataToolTipField="Username" />
                                            <ext:BoundField Width="80px" DataField="Usercode"  DataFormatString="{0}" HeaderText="用户工号"
                                                DataToolTipField="Usercode" />
                                            <ext:LinkButtonField Icon="TableEdit" CommandArgument="Dictuserid" CommandName="edit"
                                                HeaderText="默认权限配置" />
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
                    <ext:TabStrip ID="TabStrip1" ShowBorder="false" ActiveTabIndex="0" runat="server" >
                        <Tabs>
                            <ext:Tab ID="Tab1" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                Layout="Fit" Title="用户对分点设置">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                        <Toolbars>
                                            <ext:Toolbar runat="server">
                                                <Items>
                                                <ext:ToolbarFill ID="ToolbarFill3" runat="server">
                                                    </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnSave" Text="保存" runat="server" Icon="SystemSaveNew" OnClick="btnSave_Click">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Toolbars>
                                        <Regions>
                                            <ext:Region ID="Region6" runat="server" Position="Center" ShowHeader="false" Split="true"
                                                Layout="Fit" Title="Left Region" ShowBorder="false">
                                                <Items>
                                                    <ext:Grid ID="gdLabItem" runat="server" EnableCheckBoxSelect="True" Width="350px" EnableTextSelection="true"
                                                        AutoHeight="true" ShowHeader="false" Title="Grid" EnableRowNumber="True" DataKeyNames="Dictlabid">
                                                        <Columns>
                                                            <ext:BoundField ColumnID="ct0" DataFormatString="{0}" HeaderText="编号" DataField="Dictlabid"
                                                                DataToolTipField="Dictlabid" Width="50px"></ext:BoundField>
                                                            <ext:BoundField ColumnID="ct1" DataFormatString="{0}" HeaderText="分点名称" DataField="Labname"
                                                                DataToolTipField="Labname" Width="200px"></ext:BoundField>
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab2" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                Layout="Fit" Title="用户对科室设置">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                        <Toolbars>
                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                <Items>
                                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                                     </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnDepSave" Text="保存" runat="server" Icon="SystemSaveNew" OnClick="btnDepSave_Click">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Toolbars>
                                        <Regions>
                                            <ext:Region ID="Region4" runat="server" Position="Center" ShowHeader="false" Split="true"
                                                Layout="Fit" Title="Left Region" ShowBorder="false">
                                                <Items>
                                                    <ext:Grid ID="gdDep" runat="server" EnableCheckBoxSelect="True" Width="350px" AutoHeight="true" EnableTextSelection="true"
                                                        ShowHeader="false" Title="Grid" EnableRowNumber="True" DataKeyNames="Dictlabdeptid">
                                                        <Columns>
                                                            <ext:BoundField ColumnID="ct0" DataFormatString="{0}" HeaderText="编号" DataField="Dictlabdeptid"
                                                                DataToolTipField="Dictlabdeptid" Width="50px"></ext:BoundField>
                                                            <ext:BoundField ColumnID="ct1" DataFormatString="{0}" HeaderText="科室名称" DataField="Labdeptname"
                                                                DataToolTipField="Labdeptname" Width="200px"></ext:BoundField>
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                        </Tabs>
                    </ext:TabStrip>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinLibraryEdit" Title="编辑" Popup="false" EnableIFrame="true" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="700px" Height="500px">
    </ext:Window>
    </form>
</body>
</html>
