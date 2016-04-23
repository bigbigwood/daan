<%@ Page Title="检查项目维护" Language="C#" AutoEventWireup="true" CodeBehind="DictTestItems.aspx.cs"
    Inherits="daan.web.admin.dict.DictTestItems" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
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
                                Position="Top" Height="80px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        Title="Form" ShowBorder="false" ShowHeader="false" LabelWidth="50" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlgoupLibrary" runat="server" AutoPostBack="true" Label="科室"
                                                        Resizable="True" EnableEdit="true" OnSelectedIndexChanged="ddlgoupLibrary_SelectedIndexChanged">
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
                                    <ext:Grid ID="gdTestItem" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        AutoPostBack="true" Title="Grid" ShowHeader="false" DataKeyNames="Dicttestitemid,Testname"
                                        Width="250" EnableRowClick="true" Height="430" EnableMultiSelect="false" OnRowClick="gdTestItem_RowClick"
                                        IsDatabasePaging="true" AllowPaging="true" PageSize="20" OnPageIndexChange="gdTestItem_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="Dicttestitemid" HeaderText="编号"
                                                Hidden="true" />
                                            <ext:BoundField DataField="Uniqueid" HeaderText="全国统一码" DataToolTipField="Uniqueid" Width="100px" />
                                            <ext:BoundField DataField="Testname" HeaderText="项目名称" DataToolTipField="Testname" Width="200px" />  
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
                    <ext:TabStrip ID="TabStrip1" ShowBorder="false" ActiveTabIndex="0" runat="server">
                        <Tabs>
                            <ext:Tab ID="Tab1" Title="项目详细信息" EnableBackgroundColor="true" BodyPadding="0px"
                                Layout="Fit" runat="server">
                                <Toolbars>
                                    <ext:Toolbar ID="Toolbar5" runat="server">
                                        <Items>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                            </ext:ToolbarFill>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" OnClick="btnAdd_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="Form1"
                                                OnClick="btnSave_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnDel" runat="server" Text="删除" Icon="Delete" OnClick="btnDel_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnExport" runat="server" Text="导出项目列表" Icon="PageExcel" EnableAjax="false"
                                                OnClick="btnExport_Click" DisableControlBeforePostBack="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Toolbars>
                                <Items>
                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="0px"
                                        EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtTestName" runat="server" Label="项目名称" ShowRedStar="true" Required="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtEngName" runat="server" Label="英文缩写" ShowRedStar="true" Required="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtUniQueid" runat="server" Label="全国统一码" Required="true" ShowRedStar="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtEngLongName" runat="server" Label="英文全称" ShowRedStar="true" Required="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtItemCode" runat="server" Label="项目代码" ShowRedStar="true" Required="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:NumberBox ID="txtPrintOrder" runat="server" Label="打印次序" ShowRedStar="true"
                                                        Required="true">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtUnit" runat="server" Label="单位" Text="">
                                                    </ext:TextBox>
                                                    <ext:DropDownList ID="ddlforsex" runat="server" Label="测试项性别" Resizable="True">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtFastCode" runat="server" Label="助记符" ShowRedStar="true" Required="true"
                                                        Text="">
                                                    </ext:TextBox>
                                                    <ext:DropDownList ID="ddlContainerType" runat="server" EnableEdit="true" Label="试管类型"
                                                        Resizable="True" Required="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlSpecimenType" runat="server" ShowRedStar="true" Resizable="True"
                                                        EnableEdit="true" Label="标本类型" Required="true">
                                                    </ext:DropDownList>
                                                    <ext:NumberBox ID="txtLabelNumber" runat="server" ShowRedStar="true" Label="标签份数"
                                                        Required="true">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlPhysicalGourp" runat="server" ShowRedStar="true" Resizable="True"
                                                        EnableEdit="true" Label="科室" Required="true">
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="ddlTubeGroup" runat="server" ShowRedStar="true" Resizable="True"
                                                        EnableEdit="true" Label="分管原则" Required="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox ID="txtPrecision" runat="server" Label="小数位">
                                                    </ext:NumberBox>
                                                    <ext:NumberBox ID="txtStandardPrice" runat="server" Label="标准价格" ShowRedStar="true"
                                                        Required="true">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlResultType" runat="server" ShowRedStar="true" Resizable="True"
                                                        Label="结果类型" Required="true" EnableEdit="true">
                                                    </ext:DropDownList>
                                                    <ext:TextBox ID="txtDefaultResult" runat="server" Label="默认结果" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlRefmethod" runat="server" Label="参考值方式" ShowRedStar="true"
                                                        Resizable="True" Required="true">
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="ddlReportTemplate" runat="server" Label="报告模板" ShowRedStar="true"
                                                        Resizable="True" Required="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtLimitHight" runat="server" Label="限制高值" Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtLimiLow" runat="server" Label="限制低值" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:CheckBox ID="chbReport" runat="server" Text="打印报告" ShowLabel="false">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbBillable" runat="server" Text="计费" ShowLabel="false">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbActive" runat="server" Text="可用" ShowLabel="false">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbImportanttest" runat="server" Text="重要项目" ShowLabel="false">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbIsonlyForBill" runat="server" Text="是否只是收费项" ShowLabel="false">
                                                    </ext:CheckBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtDictlocusid" runat="server" Label="基因座ID" Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtGenotype" runat="server" Label="基因座类型" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtTestalias" runat="server" Label="项目别名" Text="">
                                                    </ext:TextBox>
                                                    <ext:CheckBox ID="txtImageneed" runat="server" Text="基因座是否出图" ShowLabel="false">
                                                    </ext:CheckBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtRiskdenominator" runat="server" Label="风险运算字母" Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtAveragerisk" runat="server" Label="平均风险" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtMaxriskMultiple" runat="server" Label="最大风险倍数" Text="">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="txtMinriskMultiple" runat="server" Label="最小风险倍数" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="txtOperationRemark" runat="server" Label="操作指引说明" Text="">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab2" Title="项目结果" EnableBackgroundColor="true" BodyPadding="0px" Layout="Fit"
                                runat="server">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ShowHeader="false" ShowBorder="false" ID="region5" Layout="Fit">
                                                <Items>
                                                    <ext:Grid ID="gdTestItemResult" runat="server" ShowBorder="false" EnableCheckBoxSelect="true"
                                                        EnableRowNumber="true" Title="Grid" ShowHeader="false" AutoHeight="true" AllowPaging="true"
                                                        PageSize="20" DataKeyNames="Dicttestitemresultid" OnPageIndexChange="gdTestItemResult_PageIndexChange">
                                                        <Toolbars>
                                                            <ext:Toolbar ID="Toolbar1" runat="server">
                                                                <Items>
                                                                    <ext:ToolbarFill ID="ToolbarFill3" runat="server">
                                                                    </ext:ToolbarFill>
                                                                    <ext:ToolbarSeparator ID="ToolbarSeparator10" runat="server">
                                                                    </ext:ToolbarSeparator>
                                                                    <ext:Button ID="btnAddTestResult" Text="新增" Icon="Add" runat="server" OnClick="btnAddTestResult_Click">
                                                                    </ext:Button>
                                                                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                                                    </ext:ToolbarSeparator>
                                                                    <ext:Button ID="btnDelAll" Text="删除" Icon="Delete" runat="server" OnClick="btnDelAll_Click">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Toolbars>
                                                        <Columns>
                                                            <ext:BoundField Width="159px" DataField="Dicttestitemresultid" DataFormatString="{0}"
                                                                HeaderText="" Hidden="true" />
                                                            <ext:BoundField Width="159px" DataField="Result" DataFormatString="{0}" HeaderText="结果" />
                                                            <ext:TemplateField Width="100px" HeaderText="诊断">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# GetDiagnosis(Eval("Isexception")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </ext:TemplateField>
                                                            <ext:BoundField Width="100px" DataField="Displayorder" DataFormatString="{0}" HeaderText="顺序" />
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
    <ext:Window ID="WinTestResultAdd" runat="server" Height="350px" IsModal="true" Popup="false"
        Title="添加" Width="500px" EnableConfirmOnClose="true" CloseAction="HidePostBack"
        IFrameUrl="about:blank" EnableIFrame="true" Target="Top">
    </ext:Window>
    </form>
</body>
</html>
