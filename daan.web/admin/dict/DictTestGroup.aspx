<%@ Page Title="检查组合维护" Language="C#" AutoEventWireup="true" CodeBehind="DictTestGroup.aspx.cs"
    Inherits="daan.web.admin.DictTestGroup" %>

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
    <form id="form5" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="FormEdit"
                        CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDel" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExport" runat="server" Text="导出Excel" Icon="PageExcel" CssClass="inline"
                        EnableAjax="false" OnClick="btnExport_Click" DisableControlBeforePostBack="false">
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
                                Height="80px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form3" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        Title="Form" ShowBorder="false" ShowHeader="false" LabelWidth="50" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>                                  
                                                    <ext:DropDownList ID="ddlgoupLibrary" runat="server" Label="科室" AutoPostBack="true" Resizable="True" EnableEdit="true"
                                                        OnSelectedIndexChanged="ddlgoupLibrary_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:CheckBox ID="chkActive" runat="server" Checked="true" Label="可用" ShowLabel="true">
                                                    </ext:CheckBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TwinTriggerBox ID="ttbSearch" runat="server" Label="关键字" Trigger1Icon="Clear"
                                                        Trigger2Icon="Search" ShowTrigger1="False" OnTrigger2Click="ttbSearch_Trigger2Click">
                                                    </ext:TwinTriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gdGroupTestItem" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        Title="Grid" ShowHeader="false" Width="250" Height="450" DataKeyNames="Dicttestitemid,Testname"
                                        OnRowClick="gdGroupTestItem_RowClick" AutoPostBack="true" EnableMultiSelect="false"
                                        AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="gdGroupTestItem_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="Dicttestitemid" DataFormatString="{0}" HeaderText="编号"
                                                Hidden="true" />
                                             <ext:BoundField Width="100px" DataField="Uniqueid" HeaderText="全国统一码"
                                                DataToolTipField="Uniqueid" />       
                                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="组合名称"
                                                DataToolTipField="Testname" />    
                                            <ext:BoundField DataField="Dicttestitemid" HeaderText="编号"/>                                                                                  
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
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region4" EnableSplitTip="false" CollapseMode="Default" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="false" Layout="Fit" Position="Top"
                                Height="185px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="FormEdit" Title="当前状态：新增" runat="server" ShowBorder="false" ShowHeader="true"
                                        BodyPadding="5px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtGroupTestCode" runat="server" Label="组合代码" ShowRedStar="true"
                                                        Text="" Required="true">
                                                    </ext:TextBox> 
                                                    <ext:TextBox ID="txtGroupTestName" runat="server" Label="组合名称" ShowRedStar="true"
                                                        Text="" Required="true">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtFastCode" runat="server" Required="true" Label="助记符" ShowRedStar="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:DropDownList ID="ddlPhysicalGourp" runat="server" Label="科室" ShowRedStar="true" Resizable="True" EnableEdit="true"
                                                        Required="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlSpecimenType" runat="server" Label="标本类型" ShowRedStar="true" Resizable="True" EnableEdit="true"
                                                        Required="true">
                                                    </ext:DropDownList>
                                                    <ext:NumberBox ID="nbbStandardPrice" runat="server" Required="true" Text="0" Label="标准价格"
                                                        ShowRedStar="true">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtUniQueid" runat="server" Required="true" Label="全国统一码" ShowRedStar="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:DropDownList ID="ddlTubeGroup" runat="server" Label="分管原则" ShowRedStar="true" EnableEdit="true" Resizable="True"
                                                        Required="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox ID="txtDisplayOrder" runat="server" Label="打印顺序" Text="" Required="true"  ShowRedStar="true">
                                                    </ext:NumberBox>
                                                    <ext:TextBox ID="txtOperationRemark" runat="server" Label="操作指引说明" Text="">
                                                    </ext:TextBox>
                                                    <ext:CheckBox ID="chbActive" runat="server" Label="可用" ShowLabel="true" Checked="true">
                                                    </ext:CheckBox>
                                                     <ext:CheckBox ID="chbIsonlyForBill" runat="server"  Text="是否只是收费项" ShowLabel="false">
                                                    </ext:CheckBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region5" Split="true" EnableSplitTip="true" CollapseMode="Mini" ShowHeader="false"
                                Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center" runat="server"
                                BodyPadding="5px" ShowBorder="false">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region6" Split="true" CollapseMode="Mini" Width="400px" Margins="0 0 0 0"
                                                ShowHeader="true" Icon="Outline" Layout="Fit" Title="已包含项目" Position="Left" runat="server"
                                                ShowBorder="false">
                                                <Items>
                                                    <ext:Grid runat="server" EnableRowNumber="True" EnableCheckBoxSelect="True" DataKeyNames="Dicttestitemid,Testname"
                                                        Title="Grid" ShowHeader="False" Width="190px" ID="gdGroupIncludeTestItem" EnableTextSelection="true">
                                                        <Columns>
                                                            <ext:BoundField DataField="Dicttestitemid" DataFormatString="{0}" Hidden="True" ColumnID="ct0"
                                                                HeaderText="编号"></ext:BoundField>
                                                            <ext:BoundField DataField="Uniqueid" DataFormatString="{0}" ColumnID="ct1" HeaderText="全国统一码"
                                                                Width="100px"></ext:BoundField>       
                                                            <ext:BoundField DataField="Testname" DataFormatString="{0}" ColumnID="ct3" HeaderText="项目名称"
                                                                Width="200px" DataToolTipField="Testname"></ext:BoundField>                                            
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region8" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center"
                                                runat="server" ShowBorder="false">
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region9" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="30px"
                                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left"
                                                                runat="server" ShowBorder="false">
                                                                <Items>
                                                                    <ext:Form runat="server" Title="操作" ShowHeader="False" ShowBorder="False" Width="35px"
                                                                        ID="Form4">
                                                                        <Rows>
                                                                            <ext:FormRow ID="FormRow1" runat="server">
                                                                                <Items>
                                                                                    <ext:Panel runat="server" Title="Panel" ShowHeader="False" ShowBorder="False" Height="35px"
                                                                                        ID="Panel6">
                                                                                    </ext:Panel>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow ID="FormRow2" runat="server">
                                                                                <Items>
                                                                                    <ext:Button runat="server" Size="Large" Text="＞" ID="btnRemove" OnClick="btnRemove_Click">
                                                                                    </ext:Button>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow ID="FormRow3" runat="server">
                                                                                <Items>
                                                                                    <ext:Panel runat="server" Title="Panel" ShowHeader="False" ShowBorder="False" Height="35px"
                                                                                        ID="Panel7">
                                                                                    </ext:Panel>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow ID="FormRow4" runat="server">
                                                                                <Items>
                                                                                    <ext:Button runat="server" Size="Large" Text="＜" ID="btnAppend" OnClick="btnAppend_Click">
                                                                                    </ext:Button>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        </Rows>
                                                                    </ext:Form>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region7" Split="true" CollapseMode="Mini" Margins="0 0 0 0" ShowHeader="true"
                                                                Icon="Outline" Layout="Fit" Position="Center" Title="未包含项目" runat="server" ShowBorder="false">
                                                                <Toolbars>
                                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                                        <Items>
                                                                            <ext:TwinTriggerBox ID="btnSearchNoIn" Trigger1Icon="Clear" ShowTrigger1="False"
                                                                                EmptyText="请输入关键字搜索" Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true"
                                                                                OnTrigger1Click="btnSearchNoIn_Trigger1Click" OnTrigger2Click="btnSearchNoIn_Trigger2Click">
                                                                            </ext:TwinTriggerBox>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </Toolbars>
                                                                <Items>
                                                                    <ext:Grid runat="server" EnableRowNumber="True" EnableCheckBoxSelect="True" Title="Grid" EnableTextSelection="true"
                                                                        ShowHeader="False" ID="gdGroupNotIncludeTestItem" AllowPaging="true" PageSize="20"
                                                                        IsDatabasePaging="true" DataKeyNames="Dicttestitemid,Testname" OnPageIndexChange="gdGroupNotIncludeTestItem_PageIndexChange">
                                                                        <Columns>
                                                                            <ext:BoundField DataField="Dicttestitemid" DataFormatString="{0}" HeaderText="编号"
                                                                                Hidden="true" />
                                                                            <ext:BoundField Width="100px" DataField="Uniqueid" DataFormatString="{0}" HeaderText="全国统一码"/>
                                                                            <ext:BoundField Width="200px" DataField="Testname" DataFormatString="{0}" HeaderText="项目名称"
                                                                                DataToolTipField="Testname" />                                                                       
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Region>
                                                        </Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
