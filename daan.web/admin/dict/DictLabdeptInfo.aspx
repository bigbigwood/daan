<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="DictLabdeptInfo.aspx.cs"
    Inherits="daan.web.admin.dict.DictLabdeptInfo" %>

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
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="Form1"
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" Margins="5 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="Right">
                                        <Items>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                EmptyText="请输入科室名称" Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                OnTrigger2Click="btSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true" EnableTextSelection="true"
                                        AutoHeight="true" Title="Grid" ShowBorder="true" ShowHeader="false" DataKeyNames="Dictlabdeptid,Labdeptname,Labdepttype"
                                        Width="380" AllowPaging="true" EnableRowClick="true" Height="425" EnableMultiSelect="false"
                                        IsDatabasePaging="true" OnRowClick="gvList_RowClick" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="130px" DataField="Labdeptname" DataFormatString="{0}" HeaderText="科室名称"
                                                DataToolTipField="Labdeptname" />
                                            <ext:BoundField Width="100px" DataField="Basicname" DataFormatString="{0}" HeaderText="类型"
                                                DataToolTipField="Basicname" />
                                            <ext:BoundField Width="100px" DataField="Createdate" DataFormatString="{0}" HeaderText="创建时间"
                                                DataToolTipField="Createdate" />
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
                    <ext:Panel ID="SimpleFormEdit" ColumnWidth="100%" Layout="Fit" BodyPadding="5px"
                        AutoHeight="true" Title="当前状态-新增" EnableBackgroundColor="false" runat="server"
                        ShowHeader="true" Height="460px">
                        <Items>
                            <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                LabelWidth="70px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                <Rows>
                                    <ext:FormRow>
                                        <Items>
                                            <ext:TextBox ID="tbxLabdeptname" Label="科室名称" FocusOnPageLoad="true" Required="true"
                                                ShowRedStar="true" MaxLength="25" runat="server">
                                            </ext:TextBox>
                                            <ext:DropDownList runat="server" Label="类型" ID="Drop_LabdeptTyped" ShowRedStar="true"
                                                CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual"
                                                AutoPostBack="true">
                                            </ext:DropDownList>
                                        </Items>
                                    </ext:FormRow>                                    
                                </Rows>
                            </ext:Form>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
