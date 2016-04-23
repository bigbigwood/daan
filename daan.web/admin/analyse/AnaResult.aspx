<%@ Page Title="体检结果录入" Language="C#" AutoEventWireup="true" CodeBehind="AnaResult.aspx.cs"
    Inherits="daan.web.admin.analyse.AnaResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .highlight
        {
            background-color:lightgreen;
        }
        .highlight .x-grid3-col
        {
            background-image:lightgreen;
        }
        
        .x-grid3-row-selected .highlight
        {
            background-color: yellow;
        }
        .x-grid3-row-selected .highlight .x-grid3-col
        {
            background-image: none;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" Position="Top" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAutoSummary" Text="自动小结" runat="server" Icon="CogGo" ConfirmTitle="警告"
                        ConfirmIcon="Warning" ConfirmText="如果您已自动小结过,再次小结会覆盖掉先前结果，确定要执行此项操作吗？" OnClick="btnAutoSummary_Click">
                    </ext:Button>
                    <%--<ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAudit" Text="审核通过" runat="server" Icon="UserGreen" OnClick="btnAudit_Click">
                    </ext:Button>--%>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelDiagnosis" Text="删除诊断" Icon="Delete" runat="server" OnClick="btnDelDiagnosis_Click"
                        ConfirmTitle="警告" ConfirmIcon="Warning" ConfirmText="确定要执行此项操作吗？">
                    </ext:Button>       
                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnLog" runat="server" Text="操作记录" Icon="SystemNew" OnClick="btnLog_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="客户列表" CollapseMode="Mini" Width="260px" ShowHeader="false"
                Icon="Outline" Margins="0 0 0 0" Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" CollapseMode="Mini" ShowHeader="false" Layout="Fit" Position="Top"
                                Height="105px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        BodyPadding="3px" EnableBackgroundColor="true" LabelWidth="60px" LabelAlign="Right">
                                        <Items>
                                            <ext:DropDownList ID="ddlDictlab" runat="server" AutoPostBack="False" Label="分点"
                                                Resizable="True" ShowLabel="true" Width="175px">
                                            </ext:DropDownList>
                                        </Items>
                                        <Items>
                                            <ext:DropDownList ID="ddlStatus" runat="server" AutoPostBack="False" Label="状态" Width="175px"
                                                Resizable="True">
                                                <ext:ListItem Text="已登记" Value="5" />
                                                <ext:ListItem Selected="true" Text="条码已打印" Value="10" />
                                                <ext:ListItem Text="全部" Value="-1" />
                                            </ext:DropDownList>
                                        </Items>
                                        <Items>
                                            <ext:Panel ID="Panel2" BodyPadding="0px" Title="中区" EnableBackgroundColor="true"
                                                ShowHeader="false" ShowBorder="false" runat="server" Layout="Column">
                                                <Items>
                                                    <ext:Label ID="Label1" Text="起始时间：" runat="server" Width="60px">
                                                    </ext:Label>
                                                    <ext:DatePicker ID="dpStart" Width="90px" runat="server" Required="true">
                                                    </ext:DatePicker>
                                                    <ext:DatePicker ID="dpEnd" Width="90px" runat="server" Required="true">
                                                    </ext:DatePicker>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                        <Items>
                                            <ext:TwinTriggerBox ID="txtSearch" Trigger1Icon="Clear" ShowTrigger1="False" EmptyText="请输入关键字搜索"
                                                Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true" OnTrigger1Click="txtSearch_Trigger1Click"
                                                OnTrigger2Click="txtSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" CollapseMode="Mini" Width="200px" ShowHeader="false" Icon="Outline"
                                Layout="Fit" Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvOrderUser" ShowBorder="false" ShowHeader="false" Title="客户信息" runat="server"
                                        EnableCheckBoxSelect="false" PageSize="20" IsDatabasePaging="true" AllowPaging="true"
                                        DataKeyNames="Ordernum,Realname,SexName,Age,Idnumber,Customername,Caculatedage,Status,Sex,Ismarried"
                                        OnRowClick="gvOrderUser_RowClick" OnPageIndexChange="gvOrderUser_PageIndexChange"
                                        EnableTextSelection="true" EnableRowClick="true" AutoScroll="true" AutoWidth="true"
                                        AutoHeight="true">
                                        <Columns>
                                            <ext:BoundField DataField="Ordernum" HeaderText="流水号" />
                                            <ext:BoundField DataField="Realname" HeaderText="姓名" />
                                            <ext:BoundField DataField="StatusName" HeaderText="总检状态" />
                                            <ext:BoundField DataField="SexName" HeaderText="性别" />
                                            <ext:BoundField DataField="Labname" HeaderText="分点" />
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
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:Label ID="labInfo" runat="server" EncodeText="true" Text="选中列表中的项可查看用户资料" />
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:TabStrip ID="TabStrip1" ShowBorder="false" ActiveTabIndex="0" runat="server">
                        <Tabs>
                            <ext:Tab ID="Tab1" Title="结果录入" EnableBackgroundColor="true" Layout="Fit" runat="server">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region6" runat="server" Position="Center" ShowHeader="false" ShowBorder="false"
                                                Layout="Fit" Title="Left Region">
                                                <Items>
                                                    <ext:Grid ID="gvOrderTest" ShowBorder="false" ShowHeader="false" runat="server" OnRowClick="gvOrderTest_RowClick"
                                                        DataKeyNames="Ordertestid,Ordernum,Testname,Testresult,Dicttestitemid,Status,Dictlabdeptid,IsexceptionToBool"
                                                        EnableRowClick="true" Height="160px" OnRowDataBound="gvOrderTest_RowDataBound">
                                                        <Columns>
                                                            <ext:BoundField Width="100px"  DataField="Labdeptname" HeaderText="科室" />
                                                            <ext:BoundField Width="100px"  DataField="Testname" HeaderText="项目"
                                                                DataToolTipField="Testname" />
                                                            <ext:BoundField Width="100px"  DataField="IsexceptionToBool"
                                                                HeaderText="异常" />
                                                            <ext:BoundField Width="100px"  DataField="Testresult" HeaderText="检验结果" />
                                                            <ext:BoundField Width="100px"  DataField="Unit" HeaderText="单位" />
                                                            <ext:BoundField Width="100px"  DataField="Hlhint" HeaderText="高低提示" />
                                                            <ext:BoundField Width="100px"  DataField="Textshow" HeaderText="参考范围" />
                                                            <ext:BoundField Width="100px"  DataField="Lastdate" HeaderText="上次时间" />
                                                            <ext:BoundField Width="100px"  DataField="Lastresult" HeaderText="上次结果" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region9" runat="server" Position="Bottom" Layout="Fit" ShowHeader="false"
                                                Title="Center Region" Height="150px">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar5" runat="server">
                                                        <Items>
                                                            <ext:Button ID="btnTestResult" Text="保存" IconAlign="Left" runat="server" Icon="SystemSaveNew"
                                                                OnClick="btnTestResult_Click">
                                                            </ext:Button>
                                                            <ext:Button ID="btnResultSelect" Text="结果模板" IconAlign="Left" runat="server" Icon="Accept">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:Form ID="FormSave" BodyPadding="0px" LabelAlign="Right" Title="表单" ShowHeader="false"
                                                        ShowBorder="false" runat="server">
                                                        <Rows>
                                                            <ext:FormRow>
                                                                <Items>
                                                                    <ext:CheckBox ID="chkIsexception" runat="server" Label="是否异常" ShowLabel="true">
                                                                    </ext:CheckBox>
                                                                </Items>
                                                            </ext:FormRow>
                                                        </Rows>
                                                        <Rows>
                                                            <ext:FormRow>
                                                                <Items>
                                                                    <ext:TextArea runat="server" ID="txtTestResult" AutoGrowHeight="true" AutoGrowHeightMin="90"
                                                                        Label="检查结果" AutoGrowHeightMax="90" Width="400px">
                                                                    </ext:TextArea>
                                                                </Items>
                                                            </ext:FormRow>
                                                        </Rows>
                                                    </ext:Form>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab2" Title="科室小结" EnableBackgroundColor="true" Layout="Fit" runat="server" >
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region4" runat="server" Position="Center" ShowHeader="false" ShowBorder="false"
                                                Layout="Fit" Title="Left Region">
                                                <Items>
                                                    <ext:Grid ID="gvOrderLabdeptResult" ShowBorder="false" ShowHeader="false" runat="server"
                                                        OnRowClick="gvOrderLabdeptResult_RowClick" EnableRowClick="true" DataKeyNames="Ordertlabdeptresultid,Ordernum,Labdeptresult">
                                                        <Columns>
                                                            <ext:BoundField Width="100px" DataField="Labdeptname" HeaderText="科室" />
                                                            <ext:BoundField Width="390px" DataField="Labdeptresult" HeaderText="科室小结" />
                                                            <ext:TemplateField Width="100px" HeaderText="小结状态">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# GetStatus(Eval("Status"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </ext:TemplateField>
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region5" runat="server" Position="Bottom" Layout="Fit" ShowHeader="false"
                                                Title="Center Region" Height="110px">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar4" runat="server">
                                                        <Items>
                                                            <ext:Button ID="btnSaveOrderLabdeptResult" Text="保存" IconAlign="Left" runat="server"
                                                                Icon="SystemSaveNew" OnClick="btnSaveOrderLabdeptResult_Click">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:TextArea runat="server" ID="txtOrderLabdeptResult" AutoGrowHeight="true" AutoGrowHeightMin="100"
                                                        AutoGrowHeightMax="100" Width="400px">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab3" Title="诊断建议" EnableBackgroundColor="true" Layout="Fit" runat="server">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region7" runat="server" Position="Center" ShowHeader="false" ShowBorder="false"
                                                Layout="Fit" Title="Left Region">
                                                <Items>
                                                    <ext:Grid ID="gvSuggestion" ShowBorder="false" ShowHeader="false" runat="server"
                                                        EnableCheckBoxSelect="true" OnRowClick="gvSuggestion_RowClick" EnableMultiSelect="false"
                                                        EnableRowClick="true" DataKeyNames="Orderdiagnosisid,Ordernum,Suggestion">
                                                        <Columns>
                                                            <ext:BoundField Width="100px" DataField="Labdeptname" HeaderText="科室名称" />
                                                            <ext:BoundField Width="100px" DataField="Diagnosisname" HeaderText="诊断" />
                                                            <ext:BoundField Width="390px" DataField="Suggestion" HeaderText="建议" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region8" runat="server" Position="Bottom" Layout="Fit" ShowHeader="false"
                                                Title="Center Region" Height="110px">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar3" runat="server">
                                                        <Items>
                                                            <ext:Button ID="btnSaveOrderdiagnosis" Text="保存" IconAlign="Left" runat="server"
                                                                Icon="Accept" OnClick="btnSaveOrderdiagnosis_Click">
                                                            </ext:Button>
                                                            <ext:Button ID="btnOrderdiagnosisModel" Text="添加建议" IconAlign="Left" runat="server"
                                                                Icon="Accept">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:TextArea runat="server" ID="txtEditSuggestion" AutoGrowHeight="true" AutoGrowHeightMin="100"
                                                        AutoGrowHeightMax="100" Width="400px">
                                                    </ext:TextArea>
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
    <ext:Window ID="WinResultEdit" Title="编辑" Popup="false" EnableIFrame="true" runat="server"
        CloseAction="Hide" EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top"
        IsModal="True" Width="750px" Height="450px">
    </ext:Window>
    <ext:Window ID="WinSuggestion" Title="诊断建议" Popup="false" EnableIFrame="true" runat="server"
        CloseAction="Hide" EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top"
        OnClose="WinSuggestion_Close" IsModal="True" Width="750px" Height="450px">
    </ext:Window>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385">
    </ext:Window>
     <ext:HiddenField ID="highlightRows" runat="server">
    </ext:HiddenField>
    </form>
    <script type="text/javascript">
        var highlightRowsClientID = '<%= highlightRows.ClientID %>';
        var gridClientID = '<%= gvOrderTest.ClientID %>';

        function highlightRows() {
            var highlightRows = X(highlightRowsClientID);
            var grid = X(gridClientID);

            grid.el.select('.x-grid3-row table.highlight').removeClass('highlight');

            Ext.each(highlightRows.getValue().split(','), function (item, index) {
               
                if (item !== '') {
                    var row = grid.getView().getRow(parseInt(item, 10));
                    Ext.get(row).first().addClass('highlight');
                }
            });

        }

        // 页面第一个加载完毕后执行的函数
        function onReady() {
            var grid = X(gridClientID);
            grid.addListener('viewready', function () {
                highlightRows();
            });
        }

        // 页面AJAX回发后执行的函数
        function onAjaxReady() {
            highlightRows();
        }
    </script>

</body>
</html>
