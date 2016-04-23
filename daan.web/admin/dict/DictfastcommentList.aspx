<%@ Page Title="快速模板录入维护" Language="C#" AutoEventWireup="true" CodeBehind="DictfastcommentList.aspx.cs"
    Inherits="daan.web.admin.dict.DictfastcommentList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
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
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="SimpleForm3"
                        CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDelAll_Click">
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" BodyPadding="3px"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="Right">
                                        <Items>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                EmptyText="请输入模块名称" OnTrigger2Click="btSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true" EnableTextSelection="true"
                                        Title="Grid" ShowHeader="false" DataKeyNames="Dictfastcommentid,Fastcode,Hotkey,Keymask,Modulename,Commentdesc,Dictlabdeptid" Width="380"
                                        EnableRowClick="true" Height="425" EnableMultiSelect="false" OnRowClick="gvList_RowClick"
                                        AllowPaging="true" PageSize="20" IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="110px" DataField="Modulename" DataFormatString="{0}" HeaderText="模块名称"
                                                DataToolTipField="Modulename" />
                                            <ext:BoundField Width="110px" DataField="Fastcode" DataFormatString="{0}" HeaderText="助记码"
                                                DataToolTipField="Fastcode" />
                                          <%--  <ext:BoundField Width="100px" DataField="Commentdesc" DataFormatString="{0}" HeaderText="显示内容"
                                                DataToolTipField="Commentdesc" />--%>
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
                    <ext:Form ID="SimpleForm3" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                        LabelWidth="70px" EnableBackgroundColor="false" AutoWidth="true" Title="当前状态-新增" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxModulename" Label="模块名称" FocusOnPageLoad="true" Required="true"
                                        ShowRedStar="true" MaxLength="20" runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxFastcode" Label="助记码" Required="true" ShowRedStar="true" MaxLength="100"
                                        runat="server">
                                    </ext:TextBox>
                                    <ext:TextBox ID="tbxKeymask" Label="使用键" MaxLength="10" runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxHotkey" Label="热键" MaxLength="10" runat="server">
                                    </ext:TextBox>
                                    <ext:DropDownList runat="server" Label="科室" ID="Drop_DictLabDepTid" ShowRedStar="true" EnableEdit="true"
                                        CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual"
                                        AutoPostBack="true">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea runat="server" Label="显示内容" ID="Txa" EmptyText="文本框的高度会自动扩展" Height="100"
                                        AutoGrowHeight="true" AutoGrowHeightMin="100" AutoGrowHeightMax="100" MaxLength="1000">
                                    </ext:TextArea>                                 
                                </Items> 
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
