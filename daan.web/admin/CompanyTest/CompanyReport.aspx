<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyReport.aspx.cs"
    Inherits="daan.web.admin.CompanyTest.CompanyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
    <ext:pagemanager id="PageManager1" runat="server" autosizepanelid="RegionPanel1" />
    <ext:regionpanel id="RegionPanel1" runat="server" showborder="false">
        <regions>
            <ext:Region ID="Region1" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="510px"
                ShowBorder="false" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                <ext:RegionPanel runat="server" Width="510px" Height="500">
                    <Regions>
                        <ext:Region  Layout="Fit" Position="Left" Width="240px" showborder=false ShowHeader="false">
                            <Items>
                                <ext:RegionPanel ID="RegionPanel51" runat="server" ShowBorder="false">
                                     <Regions>
                                        <ext:Region ID="Region22" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                            Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                            Position="Top" Height="50px" runat="server" ShowBorder="false" Title="Center Region">
                                            <Items>  
                                            <ext:Form ID="Form22" runat="server" BodyPadding="2px" EnableBackgroundColor="false"
                                                    ShowBorder="false" ShowHeader="false" Title="Form">
                                                    <Rows>
                                                      <ext:FormRow ID="FormRow31a" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panel3a1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    Layout="Column" ShowBorder="false" ShowHeader="false" Title="Panel">
                                                                    <Items>
                                                                        <ext:Label ID="Label31a" runat="server" Label="Label" Text="选择分点:" ShowLabel="false">
                                                                        </ext:Label>
                                                                        <ext:DropDownList ID="DropDictLab" runat="server" EnableEdit="true" Resizable="True"
                                                                            Width="182px" ShowLabel="false" autopostback="true"   onselectedindexchanged="DropDictLab_SelectedIndexChanged"> >
                                                                        </ext:DropDownList>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                         <ext:FormRow ID="FormRow31b" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panelb1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    Layout="Column" ShowBorder="false" ShowHeader="false" Title="Panel">
                                                                    <Items>
                                                                        <ext:Label ID="Label31b" runat="server" Label="Label" Text="体检单位:" ShowLabel="false">
                                                                        </ext:Label>
                                                                         <ext:DropDownList ID="dropCommetDictcustomer" runat="server" EnableEdit="true" Width="182px"    autopostback="true" Resizable="True"
                                                        onselectedindexchanged="dropCommetDictcustomer_SelectedIndexChanged"> </ext:DropDownList>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                        </Rows>
                                                        </ext:Form>                                      
                                            </Items>
                                        </ext:Region>
                                        <ext:Region ID="Region3" runat="server" Position="Center" ShowHeader="false" Layout="Fit">
                                            <Items>
                                                <ext:Grid ID="gdCustomervaliddiagnosis" runat="server"  ShowBorder="false"
                                                    EnableRowNumber="true" DataKeyNames="customervaliddiagnosisid,Laststartdate,Lastenddate,Thisstartdate,Thisenddate" ShowHeader="false"  EnableCheckBoxSelect="false" EnableMultiSelect="false"
                                                     OnRowDoubleClick="gdCustomervaliddiagnosis_RowClick" EnableRowDoubleClick="true">
                                                    <Columns>
                                                        <ext:BoundField DataField="Ordersyear" HeaderText="诊断年份"  width="60"/>
                                                        <ext:BoundField DataField="Commentdate" HeaderText="评价时间" />
                                                        <ext:BoundField DataField="Laststartdate" HeaderText="上次开始时间" />
                                                        <ext:BoundField DataField="Lastenddate" HeaderText="上次结束时间" />
                                                        <ext:BoundField DataField="Thisstartdate" HeaderText="本次开始时间" />
                                                        <ext:BoundField DataField="Thisenddate" HeaderText="本次结束时间" />
                                                        <ext:BoundField DataField="Resultcomment" HeaderText="结果建议" width="150" />
                                                    </Columns>
                                                </ext:Grid>
                                            </Items>
                                       </ext:Region>
                                    </Regions>
                              </ext:RegionPanel>
                            </Items>
                        </ext:Region>
                        <ext:Region Layout="Fit" Position="Center" ShowHeader="false">
                            <Items>
                               <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false"  >
                                    <Regions >
                                        <ext:Region Split="false" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                            ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                            Height="100px" runat="server" ShowBorder="false" Title="Center Region" >
                                            <Items>
                                                <ext:Form ID="Form2" runat="server" BodyPadding="2px" EnableBackgroundColor="false"
                                                    ShowBorder="false" ShowHeader="false" Title="Form">
                                                    <Rows>
                                                        <ext:FormRow ID="FormRow1" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panel1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    ShowBorder="false" ShowHeader="false" Layout="Column">
                                                                    <Items>
                                                                        <ext:Label ID="Label1" runat="server" Label="Label" Text="上年登记时间:" ShowLabel="false">
                                                                        </ext:Label>
                                                                        <ext:DatePicker ID="dplastyearFrom" runat="server" Label="上年登记时间" ShowLabel="false"
                                                                            Width="90px">
                                                                        </ext:DatePicker>
                                                                        <ext:DatePicker ID="dplastyearTo" runat="server" ShowLabel="false" Width="90px" CompareControl="dplastyearFrom"
                                                                            CompareOperator="GreaterThan">
                                                                        </ext:DatePicker>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                        <ext:FormRow ID="FormRow2" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panel2" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    Layout="Column" ShowBorder="false" ShowHeader="false" Title="Panel">
                                                                    <Items>
                                                                        <ext:Label ID="Label2" runat="server" Label="Label" Text="本年登记时间:" ShowLabel="false">
                                                                        </ext:Label>
                                                                        <ext:DatePicker ID="dpThisyearFrom" runat="server" ShowLabel="false" Width="90px"
                                                                            CompareControl="dplastyearFrom" CompareOperator="GreaterThan">
                                                                        </ext:DatePicker>
                                                                        <ext:DatePicker ID="dpThisyearTo" ShowLabel="false" CompareControl="dpThisyearFrom"
                                                                            Width="90px" CompareOperator="GreaterThan" runat="server" Label="到">
                                                                        </ext:DatePicker>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                        <ext:FormRow ID="FormRow3" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panel3" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    Layout="Column" ShowBorder="false" ShowHeader="false" Title="Panel">
                                                                    <Items>
                                                                        <ext:Label ID="Label3" runat="server" Label="Label" Text="体检单位选择:" ShowLabel="false">
                                                                        </ext:Label>
                                                                        <ext:DropDownList ID="dropDictcustomer" runat="server" Label="体检单位" EnableEdit="true" Resizable="True"
                                                                            Width="179px" ShowLabel="false">
                                                                        </ext:DropDownList>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                        <ext:FormRow ID="FormRow4" runat="server">
                                                            <Items>
                                                                <ext:Panel ID="Panel4" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                    Layout="Column" ShowBorder="false" ShowHeader="false" Title="Panel">
                                                                    <Items>
                                                                        <ext:CheckBox ID="chkShowSum" runat="server" Label="显示汇总结果" ShowLabel="false" Text="显示汇总结果">
                                                                        </ext:CheckBox>
                                                                        <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click"/>
                                                                        <ext:Button ID="btnPreview" runat="server" Text="预览" Icon="Magnifier" EnablePostBack="true" OnClick="btnReportView_Click" />
                                                                        <ext:Button ID="btnPrint" runat="server" Text="打印" Icon="Printer" OnClick="btnPrint_Click" OnClientClick="test()"/>
                                                                    </Items>
                                                                </ext:Panel>
                                                            </Items>
                                                        </ext:FormRow>
                                                    </Rows>
                                                </ext:Form>
                                            </Items>
                                        </ext:Region>
                                        <ext:Region ID="Region4" runat="server" Position="Center" ShowHeader="false" 
                                            Layout="Fit">
                                            <Items>
                                                <ext:Grid ID="gdOrderdiagnosis" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" EnableRowNumber="true"
                                                    DataKeyNames="dictdiagnosisid,checkstatus" ShowHeader="false" Title="Grid">
                                                    <Columns>
                                                        <ext:BoundField DataField="diagnosisname" HeaderText="异常名称" />
                                                        <ext:BoundField DataField="numpeople" HeaderText="人数" />
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
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" ShowBorder="false"
                Layout="Fit" Title="Center Region">
                <Items>
                    <ext:TabStrip ID="TabStrip1" runat="server" ActiveTabIndex="0" ShowBorder="True">
                        <Tabs>
                            <ext:Tab ID="Tab1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                Layout="Fit" Title="基本">
                                <Toolbars>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                         <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                                    </ext:ToolbarFill>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                                    </ext:ToolbarSeparator>
                                            <ext:Button ID="btnGenerate" runat="server" Text="生成异常建议" Icon="DatabaseRefresh"
                                                OnClick="btnGenerate_Click">
                                            </ext:Button>
                                             <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                                    </ext:ToolbarSeparator>
                                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" OnClick="btnSave_Click">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Toolbars>
                                <Items>
                                    <ext:TextArea ID="taContent" runat="server" ShowLabel="false">
                                    </ext:TextArea>
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
                                                            <ext:Region ID="Region11" runat="server" Position="Top" ShowHeader="false" Split="true"
                                                                EnableSplitTip="true" Title="Left Region" Height="52px" Layout="Fit" ShowBorder="false">
                                                                <Items>
                                                                    <ext:Form ID="Form5" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                                        ShowHeader="false" Title="Form" LabelAlign="Right" LabelWidth="60px">
                                                                        <Rows>
                                                                            <ext:FormRow ID="FormRow7" runat="server">
                                                                                <Items>
                                                                                    <ext:TwinTriggerBox ID="tbStrKey" runat="server" Trigger1Icon="Clear" Label="关键字"
                                                                                        ShowTrigger1="false" OnTrigger1Click="tbStrKey_Trigger1Click" OnTrigger2Click="tbStrKey_Trigger2Click"
                                                                                        Trigger2Icon="Search">
                                                                                    </ext:TwinTriggerBox>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow ID="FormRow8" runat="server">
                                                                                <Items>
                                                                                    <ext:DatePicker ID="dpNextRERUNDATE" runat="server" Required="true" Label="复查时间"
                                                                                        RequiredMessage="请输入复查时间">
                                                                                    </ext:DatePicker>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        </Rows>
                                                                    </ext:Form>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region12" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                                                                ShowBorder="false" Title="Center Region">
                                                                <Items>
                                                                    <ext:Grid ID="gdDicttestitem" runat="server" EnableRowNumber="true" DataKeyNames="Dicttestitemid,Engname,Fastcode,Testcode,Testname"
                                                                        OnPageIndexChange="gdDicttestitem_PageIndexChange" AllowPaging="true" IsDatabasePaging="true"
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
                                                            </ext:Region>
                                                        </Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region10" runat="server" Position="Center" ShowHeader="false" ShowBorder="false"
                                                Layout="Fit" Title="Center Region">
                                                <Items>
                                                    <ext:Grid ID="gdOrdernexttest" ShowBorder="true" Title="已选项目" EnableRowNumber="true"
                                                        ShowHeader="false" EnableCheckBoxSelect="true" runat="server" DataKeyNames="CUSTOMERNEXTTESTID,TESTNAME">
                                                        <Toolbars>
                                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                                <Items>
                                                                    <ext:ToolbarText ID="ToolbarText2" runat="server" Text="已选项目">
                                                                    </ext:ToolbarText>
                                                                    <ext:ToolbarFill ID="ToolbarFill3" runat="server">
                                                                    </ext:ToolbarFill>
                                                                    <ext:Button ID="btnDelgdOrdernexttest" Text="删除" runat="server" Icon="Delete"
                                                                        ConfirmText="确定要删除吗?" OnClick="btnDelgdOrdernexttest_Click">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Toolbars>
                                                        <Columns>
                                                            <ext:BoundField DataField="TESTNAME" HeaderText="项目名称" />
                                                            <ext:BoundField DataField="RERUNDATE" HeaderText="预约复查时间" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                          <%--  <ext:Tab ID="Tab2" EnableIFrame="true" BodyPadding="5px" IFrameUrl="about:blank"  Title="预览报告2" runat="server"> </ext:Tab>
--%>
                            <%--<ext:Tab ID="Tab2" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                Layout="Fit" Title="预览报告">

                                <Items>
                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region EnableIFrame="true" runat="server" Position="Center" ID="tabReport" ShowHeader="false"
                                                Title="Center Region" Layout="Fit">
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>--%>
                        </Tabs>
                    </ext:TabStrip>
                </Items>
            </ext:Region>
        </regions>
    </ext:regionpanel>
     <ext:Window ID="WinReportView" Hidden="true" EnableIFrame="true" Title="报告预览" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="825" Height="550" >
    </ext:Window>
    <ext:hiddenfield id="hdMac" runat="server">
    </ext:hiddenfield>
    <script type="text/javascript">
        function onReady() {
            //获取mac地址取该用户的本地打印机IP等信息
            document.getElementById("<%=hdMac.ClientID %>").value = document.ActiveXPrint.GetMAC();
        }
    </script>
    </form>
</body>
</html>
