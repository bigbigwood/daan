<%@ Page Title="诊断建议字典维护" Language="C#" AutoEventWireup="true" CodeBehind="DictDiagnosis.aspx.cs"
    Inherits="daan.web.admin.dict.DictDiagnosis" %>

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
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" CssClass="inline"
                        OnClick="btnSave_Click" ValidateForms="SimpleFormEdit">
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
                            <ext:Region ID="Region1" CollapseMode="Mini" Margins="5 0 0 0" ShowHeader="false"
                                Split="false" Icon="Outline" EnableCollapse="false" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="Right">
                                        <Items>
                                            <ext:TwinTriggerBox ID="ttbSearch" runat="server" Label="关键字" Trigger1Icon="Clear"
                                                EmptyText="输入诊断名称" Trigger2Icon="Search" ShowTrigger1="False" OnTrigger2Click="ttbSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center"
                                runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gdDiagnosis" runat="server" ShowHeader="false" EnableCheckBoxSelect="false" EnableTextSelection="true"
                                        EnableRowNumber="true" Title="Grid" CssClass="inline" DataKeyNames="Dictdiagnosisid"
                                        OnRowClick="gdDiagnosis_RowClick" AutoPostBack="true" EnableMultiSelect="false"
                                        AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="gdDiagnosis_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="200px" DataField="Diagnosisname" DataFormatString="{0}" HeaderText="诊断名称" />
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
                    <ext:TabStrip ID="TabStrip1" ShowBorder="false" ActiveTabIndex="0" runat="server">
                        <Tabs>
                            <ext:Tab ID="Tab1" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                Layout="Fit" Title="异常建议字典维护">
                                <Items>
                                    <ext:Form ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" EnableBackgroundColor="false"
                                        runat="server" LabelWidth="100px" Title="当前状态：新增" LabelAlign="Right" BodyPadding="5px">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtdiagnosisname" runat="server" Label="诊断名称" Required="true" ShowRedStar="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:DropDownList ID="ddlgoupLibrary" runat="server" Label="科室" Resizable="True"
                                                        EnableEdit="true">
                                                    </ext:DropDownList>
                                                    <ext:TextBox ID="txtdiagnosiscode" runat="server" Label="疾病代码"                                                        Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddldiagnosistype" runat="server" Required="true" Resizable="True"
                                                        Label="疾病分类" ShowRedStar="true">
                                                    </ext:DropDownList>
                                                    <ext:TextBox ID="txtdisplayorder" runat="server" Label="打印顺序" Text="1" Width="30">
                                                    </ext:TextBox>
                                                    <ext:CheckBox ID="chbisdisease" runat="server" Label="是否疾病" ShowLabel="true">
                                                    </ext:CheckBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="tasuggestion" runat="server" Height="50px" Label="建议" Required="true"
                                                        ShowRedStar="true" Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="tadiseasedescription" runat="server" Height="50px" AutoGrowHeightMin="50"
                                                        AutoGrowHeightMax="150" Label="医学解释" Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="tadiseasecause" runat="server" Height="50px" AutoGrowHeightMin="50"
                                                        AutoGrowHeightMax="150" Label="常见原因" Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="taengdiseasedescription" runat="server" Height="50px" Label="医学解释(英文)"
                                                        Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="taengdiseasecause" runat="server" Height="50px" Label="常见原因(英文)"
                                                        Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea ID="taengsuggestion" runat="server" Height="50px" Label="建议(英文)" Text="">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab2" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                Layout="Fit" Title="互斥异常建议维护">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region6" runat="server" Position="Left" ShowHeader="true" Split="true" Icon="Outline"
                                                Layout="Fit" Title="已选互斥异常建议" ShowBorder="false" Width="410px">
                                                <Items>
                                                    <ext:Grid ID="gvSelect" runat="server" EnableTextSelection="true"
                                                        OnRowCommand="gvSelect_RowCommand" AutoHeight="true" ShowHeader="false" Title="Grid"
                                                        EnableRowNumber="True" DataKeyNames="Dictdiagnosesmutexid,Dictdiagnosisid,Dictmutexdiagnosisid,Createdate,Diagnosisname">
                                                        <Columns>                                                            
                                                            <ext:BoundField ColumnID="ct1" HeaderText="编号" DataToolTipField="Dictmutexdiagnosisid" DataField="Dictmutexdiagnosisid"
                                                                Width="50px"></ext:BoundField>                                                            
                                                            <ext:BoundField ColumnID="ct2" HeaderText="异常建议名称" DataField="Diagnosisname" DataToolTipField="Diagnosisname"
                                                                Width="250px"></ext:BoundField>
                                                            <ext:LinkButtonField ConfirmText="你确定要删除此项？" Icon="Delete" ConfirmTarget="Top" ColumnID="lbfAction2"
                                                                HeaderText="操作" Width="50px" CommandName="ActionDel" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region4" Margins="0 0 0 0" ShowHeader="false"
                                                Icon="Outline" Layout="Fit" Position="Center" Title="" runat="server"
                                                ShowBorder="false">
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region5" CollapseMode="Mini" Margins="5 0 0 0" ShowHeader="true" Position="Top"
                                                                Icon="Outline" Layout="Fit" Title="待选互斥异常建议" runat="server" Height="30px"
                                                                ShowBorder="false">
                                                                <Items>
                                                                    <ext:TwinTriggerBox ID="ttbMutexSearch" Trigger1Icon="Clear" ShowTrigger1="true"
                                                                        EmptyText="请输入异常建议模糊搜索" Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true"
                                                                        OnTrigger1Click="ttbMutexSearch_Trigger1Click" OnTrigger2Click="ttbMutexSearch_Trigger2Click">
                                                                    </ext:TwinTriggerBox>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region7" CollapseMode="Mini" Margins="0 0 0 0" ShowHeader="false"
                                                                Icon="Outline" Layout="Fit" Position="Center" Title="待选互斥异常建议" runat="server"
                                                                ShowBorder="false">
                                                                <Items>
                                                                    <ext:Grid ID="gvNoSelect" runat="server" EnableTextSelection="true"
                                                                        OnRowCommand="gvNoSelect_RowCommand" AutoHeight="true" ShowHeader="false" Title="Grid"
                                                                        EnableRowNumber="True" DataKeyNames="Dictdiagnosisid,Diagnosisname">
                                                                        <Columns>
                                                                            <ext:BoundField ColumnID="ct3" HeaderText="编号" DataToolTipField="Dictdiagnosisid"
                                                                                DataField="Dictdiagnosisid" Width="50px"></ext:BoundField>
                                                                            <ext:BoundField ColumnID="ct4" HeaderText="异常建议名称" DataField="Diagnosisname" DataToolTipField="Diagnosisname"
                                                                                Width="250px"></ext:BoundField>
                                                                            <ext:LinkButtonField Icon="Add" ConfirmTarget="Top" ColumnID="lbfAction2" HeaderText="操作"
                                                                                Width="50px" CommandName="ActionAdd" />
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
                            </ext:Tab>
                        </Tabs>
                    </ext:TabStrip>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
