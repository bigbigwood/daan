<%@ Page Title="分点检测项目维护" Language="C#" AutoEventWireup="true" CodeBehind="DictAndTestInfo.aspx.cs"
    Inherits="daan.web.admin.dict.DictAndTestInfo" %>

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
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="导入所有检查项" Icon="Add" ValidateForms="Form1"
                        CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSaveRefresh" Text="保存" runat="server" ValidateForms="SimpleForm3"
                        Icon="SystemSaveNew" OnClick="btnSaveRefresh_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDelAll_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExcel" Text="导出Excel" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false"
                        runat="server" OnClick="btnExcel_Click">
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="75px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="60px" LabelAlign="Right">
                                        <Items>
                                            <ext:DropDownList ID="DropDictLab" runat="server" Label="分点" Width="170" Resizable="True"
                                                EnableEdit="true" FocusOnPageLoad="true">
                                            </ext:DropDownList>
                                            <ext:CheckBox ID="chkActive" runat="server" Checked="true" Label="是否可用" ShowLabel="true">
                                            </ext:CheckBox>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                Width="170" EmptyText="请输入测试项名称" OnTrigger2Click="btSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableRowNumber="true"
                                        EnableTextSelection="true" AutoHeight="true" ShowBorder="true" ShowHeader="false"
                                        DataKeyNames="Dictlabandtestid" AllowPaging="true" EnableRowClick="true" Height="425"
                                        EnableMultiSelect="false" IsDatabasePaging="true" OnRowClick="gvList_RowClick"
                                        OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>       
                                            <ext:BoundField Width="100px" DataField="Uniqueid" HeaderText="全国统一码"/>                                     
                                            <ext:BoundField Width="150px" DataField="Testname" HeaderText="检测项名称"/>                                            
                                            <ext:TemplateField HeaderText="类别" Width="60px">
                                                <ItemTemplate>
                                                    <%-- =="1"?"组合":"单项"--%>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# GetType(Eval("Testtype").ToString()) %> '></asp:Label>
                                                </ItemTemplate>
                                            </ext:TemplateField>
                                            <ext:BoundField DataField="Dicttestitemid" Hidden="true" HeaderText="检测项ID" />
                                            <ext:BoundField DataField="Issendouttest" Hidden="true" HeaderText="是否外包项目" />
                                            <ext:BoundField DataField="Isactive" Hidden="true" HeaderText="是否可用" />
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
                    <ext:SimpleForm ID="SimpleForm3" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false"
                        runat="server" Width="400px" LabelWidth="100px" LabelAlign="Right" BodyPadding="5px">
                        <Items>
                            <ext:TextBox ID="txtTestItemName" runat="server" Label="检测项目" ShowLabel="true" Enabled="false">
                            </ext:TextBox>
                            <ext:RadioButtonList runat="server" Label="是否外包项目" Width="150px" ID="radlIssendouttest">
                                <ext:RadioItem Text="是" Value="1"></ext:RadioItem>
                                <ext:RadioItem Text="否" Selected="True" Value="0"></ext:RadioItem>
                            </ext:RadioButtonList>
                            <ext:RadioButtonList runat="server" Label="是否可用" Width="150px" AutoPostBack="true"
                                ID="radlIsactive" OnSelectedIndexChanged="radlIsactive_SelectedIndexChanged">
                                <ext:RadioItem Selected="True" Text="可用" Value="1"></ext:RadioItem>
                                <ext:RadioItem Text="不可用" Value="0"></ext:RadioItem>
                            </ext:RadioButtonList>
                        </Items>
                    </ext:SimpleForm>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
