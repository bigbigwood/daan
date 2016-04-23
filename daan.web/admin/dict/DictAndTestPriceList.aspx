<%@ Page Title="分点项目价格维护" Language="C#" AutoEventWireup="true" CodeBehind="DictAndTestPriceList.aspx.cs"
    Inherits="daan.web.admin.dict.DictAndTestPriceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .inline
        {
            margin-right: 5px;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form3" runat="server">
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="75px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="60px" LabelAlign="Right">
                                        <Items>
                                            <ext:DropDownList ID="DropDictLab" runat="server" Label="分点" Width="170" AutoPostBack="true"
                                                Resizable="True" EnableEdit="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
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
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        EnableTextSelection="true" AutoHeight="true" Title="Grid" ShowBorder="true" ShowHeader="false"
                                        DataKeyNames="Dictlabandtestid,Dicttestitemid,Dictlabid" AllowPaging="true" EnableRowClick="true"
                                        Height="425" EnableMultiSelect="false" IsDatabasePaging="true" OnRowClick="gvList_RowClick"
                                        OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="100px" DataField="Uniqueid" DataFormatString="{0}" HeaderText="全国统一码"
                                                DataToolTipField="Uniqueid" />
                                            <ext:BoundField Width="150px" DataField="Testname" DataFormatString="{0}" HeaderText="检测项名称"
                                                DataToolTipField="Testname" />
                                            <ext:TemplateField HeaderText="类别" Width="60px">
                                                <ItemTemplate>
                                                    <%-- =="1"?"组合":"单项"--%>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# GetType(Eval("Testtype").ToString()) %> '></asp:Label>
                                                </ItemTemplate>
                                            </ext:TemplateField>
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
                        AutoHeight="true" Title="当前状态：编辑" EnableBackgroundColor="false" runat="server"
                        ShowHeader="false" Height="460px">
                        <Items>
                            <ext:TabStrip ID="TabStrip1" runat="server" ActiveTabIndex="0" ShowBorder="True"
                                Height="460px" ColumnWidth="100%" AutoPostBack="true" Width="700px">
                                <Tabs>
                                    <ext:Tab ID="Tab1" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                        ColumnWidth="100%" Layout="Fit" Title="检测项价格维护">
                                        <Items>
                                            <ext:RegionPanel ID="RegionPanel3" ShowBorder="false" runat="server">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                                        <Items>
                                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                                            </ext:ToolbarFill>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnAdd" Text="新增" runat="server" CssClass="inline" Icon="Add" OnClick="btnAdd_Click">
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnSave" Text="保存" runat="server" CssClass="inline" ValidateForms="SimpleForm5"
                                                                Icon="SystemSaveNew" OnClick="btnSave_Click">
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnDelAll" Text="价格删除" Icon="Delete" CssClass="inline" runat="server"
                                                                OnClick="btnDelAll_Click">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Regions>
                                                    <ext:Region ID="Region4" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                                                        Width="300px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                                                        Layout="Fit" Position="Left" runat="server">
                                                        <Items>
                                                            <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                                                <Regions>
                                                                    <ext:Region ID="Region5" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="5 0 0 0"
                                                                        ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                                                        Height="30px" runat="server" ShowBorder="false">
                                                                        <Items>
                                                                            <ext:SimpleForm ID="SimpleForm2" ShowBorder="false" ShowHeader="false" runat="server"
                                                                                EnableCollapse="True" LabelWidth="65px" LabelAlign="left">
                                                                                <Items>
                                                                                    <ext:Panel BodyPadding="0px" Title="中区" EnableBackgroundColor="false" ShowHeader="false"
                                                                                        ShowBorder="false" runat="server" Layout="Column" ID="panelTitle">
                                                                                        <Items>
                                                                                            <ext:Label ID="Label2" Text="起始时间：" runat="server" Width="60px">
                                                                                            </ext:Label>
                                                                                            <ext:DatePicker runat="server" AutoPostBack="true" Label="开始日期" EmptyText="请选择日期"
                                                                                                ID="Dp_Bingin" Width="90">
                                                                                            </ext:DatePicker>
                                                                                            <ext:DatePicker runat="server" Readonly="false" EmptyText="请选择日期" DateFormatString="yyyy-MM-dd"
                                                                                                Label="结束日期" ID="DatePicker3" Width="90">
                                                                                            </ext:DatePicker>
                                                                                            <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="查询" ValidateForms="SimpleForm2"
                                                                                                OnClick="btnSearch_Click">
                                                                                            </ext:Button>
                                                                                        </Items>
                                                                                    </ext:Panel>
                                                                                </Items>
                                                                            </ext:SimpleForm>
                                                                        </Items>
                                                                    </ext:Region>
                                                                    <ext:Region ID="Region6" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                                                        Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                                                        Position="Center" runat="server" ShowBorder="false">
                                                                        <Items>
                                                                            <ext:Grid ID="GridList" PageSize="20" runat="server" EnableCheckBoxSelect="false"
                                                                                EnableTextSelection="true" EnableRowNumber="true" AutoHeight="true" Title="Grid"
                                                                                ShowBorder="true" ShowHeader="false" DataKeyNames="Dictlabandtestpriceid" AllowPaging="true"
                                                                                EnableRowClick="true" Height="325" EnableMultiSelect="false" OnRowClick="GridList_RowClick"
                                                                                IsDatabasePaging="true" OnPageIndexChange="GridList_PageIndexChange">
                                                                                <Columns>
                                                                                    <ext:BoundField Width="100px" DataField="Testname" DataFormatString="{0}" HeaderText="检测项"
                                                                                        DataToolTipField="Testname" />
                                                                                    <ext:BoundField Width="50px" DataField="Price" DataFormatString="{0}" HeaderText="价格"
                                                                                        DataToolTipField="Price" />
                                                                                    <ext:BoundField Width="100px" DataField="Begindate" DataFormatString="{0:yyyy-MM-dd}"
                                                                                        HeaderText="开始日期" DataToolTipField="Begindate" />
                                                                                    <ext:BoundField Width="100px" DataField="Enddate" DataFormatString="{0:yyyy-MM-dd}"
                                                                                        HeaderText="结束日期" DataToolTipField="Enddate" />
                                                                                </Columns>
                                                                            </ext:Grid>
                                                                        </Items>
                                                                    </ext:Region>
                                                                </Regions>
                                                            </ext:RegionPanel>
                                                        </Items>
                                                    </ext:Region>
                                                    <ext:Region ID="Region7" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                                                        runat="server">
                                                        <Items>
                                                            <ext:SimpleForm ID="SimpleForm5" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false"
                                                                runat="server" EnableCollapse="True" LabelWidth="100px" LabelAlign="Right">
                                                                <Items>
                                                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                                                                        Title="当前状态-新增" EnableBackgroundColor="false" AutoWidth="true" Height="350px">
                                                                        <Rows>
                                                                            <ext:FormRow>
                                                                                <Items>
                                                                                    <ext:NumberBox ID="tbxFinalprice" runat="server" Label="价格" Required="true" MaxLength="6"
                                                                                        ShowRedStar="true" NoNegative="True" CompareMessage="必填项,且输入数字类型">
                                                                                    </ext:NumberBox>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow>
                                                                                <Items>
                                                                                    <ext:DatePicker runat="server" Required="true" AutoPostBack="true" Label="开始日期" EmptyText="请选择开始日期"
                                                                                        OnTextChanged="DatePicker1_TextChanged" ID="DatePicker1" ShowRedStar="True">
                                                                                    </ext:DatePicker>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow>
                                                                                <Items>
                                                                                    <ext:DatePicker ID="DatePicker2" Required="true" Readonly="false" CompareControl="DatePicker1"
                                                                                        DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThanEqual" CompareMessage="结束日期应该大于开始日期"
                                                                                        EmptyText="请选择结束日期" Label="结束日期" runat="server" ShowRedStar="True">
                                                                                    </ext:DatePicker>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        </Rows>
                                                                    </ext:Form>
                                                                </Items>
                                                            </ext:SimpleForm>
                                                        </Items>
                                                    </ext:Region>
                                                </Regions>
                                            </ext:RegionPanel>
                                        </Items>
                                    </ext:Tab>
                                </Tabs>
                            </ext:TabStrip>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
