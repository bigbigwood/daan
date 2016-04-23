<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AnaResultSum.aspx.cs"
    Inherits="daan.web.admin.analyse.AnaResultSum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../../js/Resources/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="form4" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel2" runat="server">
    </ext:PageManager>
    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false" EnableBackgroundColor="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar3" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator13" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnFirstInstance" Text="初步总检" runat="server" Icon="BasketPut" OnClick="btnFirstInstance_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnFinish" Text="完成总检" runat="server" Icon="BasketGo" OnClick="btnFinish_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnCancelFinish" Text="取消完成总检" runat="server" Icon="ApplicationOsxDelete"
                        OnClick="btnCancelFinish_Click" ConfirmText="确定要取消完成总检吗?">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnBack" runat="server" Text="退回重做" Icon="BasketRemove" OnClick="btnBack_Click"
                        ConfirmText="确定要退回重做吗?">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator12" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnRecoder" runat="server" Text="档案" Icon="BookMagnify" OnClick="btnRecoder_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator11" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnReportView" runat="server" Text="报告预览" Icon="Magnifier" OnClick="btnReportView_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnLog" runat="server" Text="操作记录" Icon="SystemNew" OnClick="btnLog_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAddRemark" Text="添加备注" runat="server" Icon="Add" OnClick="btnAddRemark_Click"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region1" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="310px"
                ShowBorder="true" Margins="1 0 0 0" ShowHeader="false" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server" EnableBackgroundColor="false">
                <Items>
                    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false" EnableBackgroundColor="false">
                        <Regions>
                            <ext:Region ID="Region3" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="160px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        ShowHeader="false" Title="Form" LabelAlign="Right" LabelWidth="60px" ShowBorder="false">
                                        <Rows>
                                            <ext:FormRow ID="FormRow4" runat="server">
                                                <Items>
                                                    <ext:Panel ID="Panel1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                        Layout="Column" ShowBorder="false" ShowHeader="false" Title="体检日期">
                                                        <Items>
                                                            <ext:Label ID="Label1" runat="server" Text="体检日期：&nbsp;">
                                                            </ext:Label>
                                                            <ext:DatePicker runat="server" ID="dpFrom" Width="115px" EmptyText="开始日期">
                                                            </ext:DatePicker>
                                                            <ext:DatePicker runat="server" ID="dpTo" Width="115px" EmptyText="结束日期">
                                                            </ext:DatePicker>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow5" runat="server">
                                                <Items>
                                                    <ext:DropDownList ID="ddlDictLab" runat="server" Label="分点选择" EnableEdit="true" Resizable="True"
                                                        AutoPostBack="true" Width="230px" OnSelectedIndexChanged="ddlDictLab_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow24" runat="server">
                                                <Items>
                                                    <ext:DropDownList runat="server" Label="体检单位" Resizable="True" Width="230px" ID="dropDictcustomer"
                                                        EnableEdit="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow3" runat="server">
                                                <Items>
                                                    <ext:DropDownList ID="ddlStatus" runat="server" Label="总检状态" Width="80px">
                                                        <ext:ListItem Text="全部" Value="-1" />
                                                        <ext:ListItem Text="已登记" Value="5" />
                                                        <ext:ListItem Text="条码已打印" Value="10" />
                                                        <ext:ListItem Text="待总检" Value="15" Selected="true" />
                                                        <ext:ListItem Text="初步总检" Value="20" />
                                                        <ext:ListItem Text="完成总检" Value="25" />
                                                        <ext:ListItem Text="报告已打印" Value="30" />
                                                    </ext:DropDownList>
                                                    <ext:DropDownList runat="server" ID="dropStatus" Label="接收状态" Resizable="True" Width="80px" EnableEdit="true">
                                                        <ext:ListItem Text="全部" Value="-1" />
                                                        <ext:ListItem Selected="true" Text="接收完成" Value="2" />
                                                        <ext:ListItem Text="部分接收" Value="1" />
                                                        <ext:ListItem Text="未接收" Value="0" />
                                                        <ext:ListItem Text="接收失败" Value="3" />
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow23" runat="server">
                                                <Items>
                                                    <ext:TextBox runat="server" ID="txtDoctor" Label="总检医生"></ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow6" runat="server">
                                                <Items>
                                                    <ext:TwinTriggerBox ID="tbKeyWord" runat="server" Label="关键字" Trigger1Icon="Clear"
                                                        ShowTrigger1="false" OnTrigger1Click="tbKeyWord_Trigger1Click" OnTrigger2Click="tbKeyWord_Trigger2Click"
                                                        Width="230px" Trigger2Icon="Search">
                                                    </ext:TwinTriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region4" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                                Title="Center Region" EnableBackgroundColor="false" Layout="Fit">
                                <Items>
                                    <ext:Grid ID="gdOrders" ShowBorder="true" ShowHeader="false" Title="标本列表" runat="server"
                                        AllowSorting="true" SortColumn="repType" OnSort="gdOrders_Sort"
                                        DataKeyNames="ORDERNUM,STATUS,BARCODE,DICTMEMBERID,REALNAME,SEX,AGE,ISMARRIED,MOBILE,CUSTOMERNAME"
                                        AllowPaging="true" EnableCheckBoxSelect="true" EnableRowNumber="false" EnableTextSelection="true" IsDatabasePaging="true"
                                        OnRowClick="gdOrders_RowClick" EnableRowClick="true" OnPageIndexChange="gdOrders_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="ORDERNUM" Width="110" HeaderText="订单号" SortField="ORDERNUM" />
                                            <ext:BoundField DataField="REALNAME" DataToolTipField="REALNAME" Width="58" HeaderText="姓名" />
                                            <ext:BoundField DataField="dictreporttemplateid" Width="70px" HeaderText="报告类型" ColumnID="repType" SortField="dictreporttemplateid" />
                                            <ext:BoundField DataField="BASICNAME" Width="60" HeaderText="检查状态" />
                                            <ext:BoundField DataField="IOLIS" Width="60" HeaderText="接收状态" />
                                            <ext:BoundField DataField="lastupdatedate" DataToolTipField="lastupdatedate" SortField="lastupdatedate"
                                            Width="130px" HeaderText="进入待总检日期" />
                                            <ext:BoundField DataField="visittime" DataToolTipField="visittime" SortField="visittime" Width="130px" HeaderText="回访时间" />
                                            <%--<ext:BoundField DataField="name1" HeaderText="初步医生" Width="60px" ColumnID="chu" Hidden="true" />
                                            <ext:BoundField DataField="name2" HeaderText="完成医生" Width="60px" ColumnID="wan" Hidden="true" />--%>
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Title="Center Region"
                EnableBackgroundColor="false" ShowBorder="false" Layout="Fit">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar6" runat="server">
                        <Items>
                            <ext:TextBox ID="labInfo" Width="700px" Readonly="true" runat="server" EmptyText="选中列表中的项可查看用户资料">
                            </ext:TextBox>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:TabStrip ID="TabStrip1" runat="server" ActiveTabIndex="0" ShowBorder="True"
                        EnableTitleBackgroundColor="true">
                        <Tabs>
                            <ext:Tab ID="Tab2" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                Layout="Fit" Title="科室小结与诊断信息">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region7" runat="server" Position="Center" ShowHeader="false" Split="true"
                                                ShowBorder="false" EnableSplitTip="true" Layout="Fit">
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false" EnableBackgroundColor="false">
                                                        <Regions>
                                                            <ext:Region ID="Region6" runat="server" Position="Top" ShowHeader="false" Layout="Fit"
                                                                Height="190px" Title="Center Region">
                                                                <Items>
                                                                    <ext:Grid ID="gdOrderlabdeptResult" ShowBorder="false" ShowHeader="false" runat="server"
                                                                        DataKeyNames="ORDERNUM,ORDERTLABDEPTRESULTID">
                                                                        <Columns>
                                                                            <ext:BoundField DataField="LABDEPTNAME" HeaderText="科室" Width="120px" />
                                                                            <ext:BoundField DataField="LABDEPTRESULT" DataToolTipField="LABDEPTRESULT" HeaderText="结果小结" Width="350px" />
                                                                            <ext:BoundField DataField="USERNAME" HeaderText="小结医生" Width="80px" />
                                                                            <ext:BoundField DataField="APPRAISEDATE" HeaderText="小结日期" DataFormatString="{0:yyyy-MM-dd}" ExpandUnusedSpace="true"/>
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region21" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                                                                Title="Center Region">
                                                                <Items>
                                                                    <ext:Grid ID="gdOrderdiagnosis" ShowBorder="true" ShowHeader="false" Title="诊断信息" runat="server" 
                                                                    DataKeyNames="Orderdiagnosisid,Diagnosisname,Diseasedescription,Diseasecause,Suggestion,Diagnosistype"
                                                                        EnableCheckBoxSelect="true" EnableRowNumber="false" EnableMultiSelect="true" EnableRowClick="true" OnRowClick="gdOrderdiagnosis_RowClick">
                                                                        <Columns>
                                                                            <ext:BoundField DataField="LABDEPTNAME" HeaderText="科室" Width="120px" />
                                                                            <ext:BoundField DataField="DIAGNOSISNAME" DataToolTipField="DIAGNOSISNAME" HeaderText="诊断名" Width="150px" />
                                                                            <ext:BoundField DataField="DISEASEDESCRIPTION" DataToolTipField="DISEASEDESCRIPTION" HeaderText="医学解释" Width="150px" />
                                                                            <ext:BoundField DataField="DISEASECAUSE" DataToolTipField="DISEASECAUSE" HeaderText="常见原因" Width="150px" />
                                                                            <ext:BoundField DataField="SUGGESTION" DataToolTipField="SUGGESTION" HeaderText="本次建议" Width="150px" />
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Region>
                                                        </Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region8" runat="server" Position="Right" ShowHeader="false" Width="300px"
                                                ShowBorder="false" Layout="Fit">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                                        <Items>
                                                            <ext:ToolbarFill runat="server" ID="ToolbarFill4">
                                                            </ext:ToolbarFill>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnInsertOrderdiagnosis" Text="添加" runat="server" Icon="Add">
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnSaveOrderdiagnosis" Text="保存" runat="server" Icon="SystemSaveNew"
                                                                OnClick="btnSaveOrderdiagnosis_Click">
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator99" runat="server">
                                                            </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnDeleteOrderdiagnosis" Text="删除" runat="server" Icon="Delete" ConfirmText="是否删除选中诊断信息？"
                                                                ConfirmTitle="体检系统" ConfirmIcon="Question" ConfirmTarget="Top" OnClick="btnDeleteOrderdiagnosis_Click">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel7" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region13" runat="server" Position="Top" ShowHeader="true" Split="true"
                                                                ShowBorder="false" Title="诊断名" Height="50px" Layout="Fit">
                                                                <Items>
                                                                    <ext:TextArea ID="tbDiagnosisname" runat="server" Label="诊断名" ShowLabel="false" Enabled="false">
                                                                    </ext:TextArea>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region14" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                                                                ShowBorder="false" Title="Center Region">
                                                                <Items>
                                                                    <ext:RegionPanel ID="RegionPanel8" runat="server" ShowBorder="false">
                                                                        <Regions>
                                                                            <ext:Region ID="Region16" runat="server" Position="Top" ShowHeader="true" Split="true"
                                                                                Layout="Fit" ShowBorder="false" Title="医学解释" Height="150px">
                                                                                <Items>
                                                                                    <ext:TextArea ID="taDiseasedescription" runat="server" Label="医学解释">
                                                                                    </ext:TextArea>
                                                                                </Items>
                                                                            </ext:Region>
                                                                            <ext:Region ID="Region15" runat="server" Position="Center" ShowHeader="true" Layout="Fit"
                                                                                ShowBorder="false" Title="本次建议">
                                                                                <Items>
                                                                                    <ext:TextArea ID="taSuggestion" runat="server" Label="本次建议">
                                                                                    </ext:TextArea>
                                                                                </Items>
                                                                            </ext:Region>
                                                                            <ext:Region ID="Region17" runat="server" Position="Bottom" ShowHeader="true" ShowBorder="false"
                                                                                Layout="Fit" Height="150px" Title="常见原因">
                                                                                <Items>
                                                                                    <ext:TextArea ID="taDiseasecause" runat="server" Label="常见原因">
                                                                                    </ext:TextArea>
                                                                                </Items>
                                                                            </ext:Region>
                                                                        </Regions>
                                                                    </ext:RegionPanel>
                                                                </Items>
                                                            </ext:Region>
                                                        </Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab5" runat="server" BodyPadding="0px" Layout="Fit" Title="详细结果">
                                <Items>
                                    <ext:Grid ID="gdOrdertest" runat="server" EnableRowNumber="true" ShowBorder="false"
                                        ShowHeader="false" Title="检查结果明细" AutoScroll="true" AutoHeight="true">
                                        <Columns>
                                            <ext:BoundField Width="150px" DataField="DICTGROUPNAME" DataToolTipField="DICTGROUPNAME" HeaderText="组合名称" />
                                            <ext:BoundField Width="200px" DataField="TESTNAME" DataToolTipField="TESTNAME" HeaderText="项目名称" />
                                            <ext:BoundField Width="70px" DataField="TESTRESULT" HeaderText="检查结果" />
                                            <ext:BoundField Width="70px" DataField="LASTRESULT" HeaderText="上次结果" />
                                            <ext:BoundField Width="100px" DataField="LASTDATE" HeaderText="上次时间" />
                                            <ext:BoundField Width="50px" DataField="HLFLAG" HeaderText="提示" />
                                            <ext:BoundField Width="80px" DataField="ISEXCEPTION" HeaderText="是否异常" />
                                            <ext:BoundField Width="110px" DataField="TEXTSHOW" HeaderText="参考范围" DataToolTipField="TEXTSHOW"/>
                                            <ext:BoundField Width="70px" DataField="TRANSCOUNT" HeaderText="传输次数" />
                                            <ext:BoundField DataField="TRANSDATE"  DataToolTipField="TRANSDATE" HeaderText="传输时间" ExpandUnusedSpace="true"  />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab4" runat="server" BodyPadding="0px" Layout="Fit" Title=" 总体评价">
                                <Toolbars>
                                    <ext:Toolbar ID="Toolbar5" runat="server">
                                        <Items>
                                            <ext:TextBox ID="tbxordernum" runat="server" CssClass="diaplay_none" />
                                            <ext:ToolbarFill runat="server" ID="ToolbarFill5">
                                            </ext:ToolbarFill>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator10" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="btnSase" Text="保存" OnClick="btnSave_Click" runat="server" Icon="SystemSaveNew">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Toolbars>
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel9" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region18" EnableSplitTip="true" CollapseMode="Mini" Margins="5 5 0 5"
                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                                Height="200px" Split="true" runat="server" ShowBorder="false">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar8" runat="server">
                                                        <Items>
                                                            <ext:Label runat="server" ID="Label5" Text="结果评价"></ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:TextArea ID="tbxComment" runat="server" />
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region20" runat="server" Position="Center" Margins="0 5 5 5" ShowHeader="false"
                                                Layout="Fit" ShowBorder="false" Title="Center Region">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar7" runat="server">
                                                        <Items>
                                                            <ext:Label runat="server" ID="lbltitle" Text="评估建议"></ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:TextArea ID="tbxSuggestion" runat="server" />
                                                </Items>
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab3" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                Layout="Fit" Title="推荐项目">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region9" runat="server" Position="Left" ShowHeader="false" Split="true"
                                                ShowBorder="false" Layout="Fit" Title="Left Region" Width="250px">
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel6" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region11" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                                                Position="Top" Height="95px" runat="server" ShowBorder="false">
                                                                <Items>
                                                                    <ext:Form ID="Form5" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                        ShowHeader="false" Title="Form" ShowBorder="false" LabelAlign="Right" LabelWidth="90px">
                                                                        <Rows>
                                                                            <ext:FormRow ID="FormRow8" runat="server">
                                                                                <Items>
                                                                                    <ext:DatePicker ID="dpNextRERUNDATE" runat="server" Required="true" Label="复查开始时间"
                                                                                        RequiredMessage="请输入复查开始时间">
                                                                                    </ext:DatePicker>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        
                                                                            <ext:FormRow runat="server">
                                                                            <Items>
                                                                                <ext:DatePicker ID="dpReRunEndDate" runat="server" Required="true" Label="复查结束时间"
                                                                                        RequiredMessage="请输入复查结束时间">
                                                                                    </ext:DatePicker>
                                                                                    </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow ID="FormRow7" runat="server">
                                                                                <Items>
                                                                                    <ext:TwinTriggerBox ID="tbStrKey" runat="server" Trigger1Icon="Clear" Label="关键字"
                                                                                        ShowTrigger1="false" OnTrigger1Click="tbStrKey_Trigger1Click" OnTrigger2Click="tbStrKey_Trigger2Click"
                                                                                        Trigger2Icon="Search">
                                                                                    </ext:TwinTriggerBox>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        </Rows>
                                                                    </ext:Form>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region12" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                                                                ShowBorder="false" Title="Center Region">
                                                                <Items>
                                                                    <ext:Grid ID="gdDicttestitem" runat="server" EnableRowNumber="true" DataKeyNames="Dicttestitemid,Engname,Fastcode,Testcode,Testname,Isneededorder"
                                                                        IsDatabasePaging="true" AllowPaging="true" OnPageIndexChange="gdDicttestitem_PageIndexChange"
                                                                        ShowHeader="false" EnableCheckBoxSelect="true" Title="备选项目">
                                                                        <Toolbars>
                                                                            <ext:Toolbar ID="Toolbar4" runat="server">
                                                                                <Items>
                                                                                    <ext:ToolbarText ID="ToolbarText1" runat="server" Text="备选项目">
                                                                                    </ext:ToolbarText>
                                                                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                                                    </ext:ToolbarSeparator>
                                                                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                                                                    </ext:ToolbarFill>
                                                                                    <ext:Button ID="btnSaveDicttestitem" runat="server" Text="添加" Icon="Add" OnClick="btnSaveDicttestitem_Click">
                                                                                    </ext:Button>
                                                                                </Items>
                                                                            </ext:Toolbar>
                                                                        </Toolbars>
                                                                        <Columns>
                                                                            <ext:BoundField DataField="Testname" HeaderText="项目名称" />
                                                                            <ext:BoundField DataField="Engname" HeaderText="英文名" />
                                                                            <ext:BoundField DataField="Fastcode" HeaderText="快捷录入码" />
                                                                            <ext:BoundField DataField="Testcode" HeaderText="测试项编号" />
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Region></Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region10" runat="server" Position="Center" ShowHeader="false" ShowBorder="false" Layout="Fit" Title="Center Region">
                                                <Items>
                                                    <ext:Grid ID="gdOrdernexttest" ShowBorder="true" Title="已选项目" EnableRowNumber="true"
                                                        ShowHeader="false" EnableCheckBoxSelect="true" runat="server" DataKeyNames="ORDERNEXTTESTID,TESTNAME,dicttestitemid">
                                                        <Toolbars>
                                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                                <Items>
                                                                    <ext:ToolbarText ID="ToolbarText2" runat="server" Text="已选项目">
                                                                    </ext:ToolbarText>
                                                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                                    </ext:ToolbarSeparator>
                                                                    <ext:ToolbarFill ID="ToolbarFill3" runat="server">
                                                                    </ext:ToolbarFill>
                                                                    <ext:Button ID="btnDelgdOrdernexttest" Text="删除" runat="server" Icon="Delete" ConfirmText="确定要删除吗?"
                                                                        OnClick="btnDelgdOrdernexttest_Click">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Toolbars>
                                                        <Columns>
                                                            <ext:BoundField DataField="TESTNAME" HeaderText="项目名称" />
                                                            <ext:BoundField DataField="RERUNDATE" DataFormatString="{0:yyyy-MM-dd}" HeaderText="预约复查开始时间" />
                                                            <ext:BoundField DataField="RERUNENDDATE" DataFormatString="{0:yyyy-MM-dd}" HeaderText="预约复查结束时间" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                             Layout="Fit" Title="回访记录">
                                <Toolbars>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                                            <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                                            <ext:Button runat="server" ID="btn_Add" Icon="Add" Text="添加" OnClick="btn_Add_Click"></ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator14" runat="server"></ext:ToolbarSeparator>
                                            <ext:Button runat="server" ID="btn_Save" Icon="SystemSaveNew" Text="保存" OnClick="btn_Save_Click"></ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator15" runat="server"></ext:ToolbarSeparator>
                                            <ext:Button runat="server" ID="btn_Delete" Icon="Delete" Text="删除" ConfirmIcon="Question" ConfirmText="是否确定删除该回访记录" OnClick="btn_Delete_Click"></ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Toolbars>
                                <Items>
                                    <ext:RegionPanel runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region runat="server" Position="Top" Height="130px" Layout="Fit" Title="回访记录详情">
                                                <Items>
                                                    <ext:Form runat="server" ID="fmVisit" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false"
                                                     LabelAlign="Right" LabelWidth="100px" BodyPadding="20px">
                                                        <Rows>
                                                            <ext:FormRow runat="server" ID="rowVisit1">
                                                                <Items>
                                                                    <ext:TextArea runat="server" ID="txtVisitText" Label="回访内容" Required="true" ShowRedStar="true"></ext:TextArea>
                                                                </Items>
                                                            </ext:FormRow>
                                                            <ext:FormRow runat="server" ID="rowVisit2">
                                                                <Items>
                                                                    <ext:TextBox runat="server" ID="txtVisitDoctor" Enabled="false" Label="回访记录人"></ext:TextBox>
                                                                    <%--<ext:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                                                        回访时间：
                                                                        <asp:TextBox runat="server" ID="txtVisitdate" CssClass="Wdate" onfocus="WdatePicker({lang:'zh-cn',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                                    </ext:ContentPanel>--%>
                                                                    <ext:TextBox runat="server" ID="txtVisitdate" Label="回访时间" Enabled="false"></ext:TextBox>
                                                                </Items>
                                                            </ext:FormRow>
                                                        </Rows>
                                                    </ext:Form>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region5" runat="server" Layout="Fit" Title="历史回访记录列表">
                                                <Items>
                                                    <ext:Grid runat="server" ID="gdVisitList" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="false" EnableMultiSelect="false"
                                                     EnableRowNumber="false" EnableRowClick="true" DataKeyNames="visitid,visitor" OnRowClick="gdVisitList_RowClick">
                                                        <Columns>
                                                            <ext:BoundField DataField="rn" HeaderText="序号" Width="40px" />
                                                            <ext:BoundField DataField="Visitor" Width="80px" HeaderText="回访人" />
                                                            <ext:BoundField DataField="visitcontext" DataToolTipField="visitcontext" Width="700px" HeaderText="回访内容" />
                                                            <ext:BoundField DataField="VisitTime" HeaderText="回访时间" Width="120px"/>
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
    <ext:HiddenField runat="server" ID="hidSortKey"></ext:HiddenField>
    <ext:Window ID="winArchives" Title="档案" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" IFrameUrl="about:blank" Target="Top" IsModal="True" Width="800px"
        Height="550px">
    </ext:Window>
    <ext:Window ID="winDictdiagnosis" Title="添加诊断信息" Popup="false" EnableIFrame="true"
        WindowPosition="Center" CloseAction="Hide" runat="server" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="800px" OnClose="winDictdiagnosis_Close" Height="550px">
    </ext:Window>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385">
    </ext:Window>
    <ext:Window ID="WinReportView" Hidden="true" EnableIFrame="true" Title="报告预览" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="825" Height="550">
    </ext:Window>
    <ext:Window runat="server" ID="WinAddRemark" Hidden="true" Title="添加备注" Target="Top"
     IsModal="true" AutoScroll="true" Icon="TagBlue" Layout="Fit" Width="400" Height="200"
      EnableIFrame="true" IFrameUrl="about:blank">
    </ext:Window>
    </form>
</body>
</html>
