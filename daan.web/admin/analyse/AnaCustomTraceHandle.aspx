<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AnaCustomTraceHandle.aspx.cs"
    Inherits="daan.web.admin.analyse.AnaCustomTraceHandle" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form4" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Regions>
            <ext:Region ID="Region1" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="250px"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region6" runat="server" Position="Center" Layout="Fit" ShowBorder="false"
                                ShowHeader="false" Title="Center Region">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel6" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region9" runat="server" Position="Top" ShowHeader="false" Split="true"
                                                Layout="Fit" ShowBorder="false" Title="Left Region" Height="85px">
                                                <Items>
                                                    <ext:Form ID="Form1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                                                        ShowHeader="false" ShowBorder="false" Title="Form">
                                                        <Rows>
                                                            <ext:FormRow ID="FormRow1" runat="server">
                                                                <Items>
                                                                    <ext:Panel ID="Panel1" runat="server" BodyPadding="2px" EnableBackgroundColor="false"
                                                                        ShowBorder="false" ShowHeader="false" Title="Panel" Layout="Column">
                                                                        <Items>
                                                                            <ext:Label ID="Label1" runat="server" Label="Label" Text="复查时间：">
                                                                            </ext:Label>
                                                                            <ext:DatePicker ID="dpFrom" runat="server" Label="复查时间" Width="90px" ShowLabel="false">
                                                                            </ext:DatePicker>
                                                                            <ext:DatePicker ID="dpTo" runat="server" CompareMessage="结束日期应该大于开始日期" ShowLabel="false"
                                                                                Width="90px" CompareControl="dpFrom" CompareOperator="GreaterThanEqual">
                                                                            </ext:DatePicker>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                </Items>
                                                            </ext:FormRow>
                                                            <ext:FormRow>
                                                                <Items>
                                                                    <ext:Panel ID="Panel2" runat="server" BodyPadding="2px" EnableBackgroundColor="false"
                                                                        ShowBorder="false" ShowHeader="false" Title="Panel" Layout="Column">
                                                                        <Items>
                                                                            <ext:Label ID="Label2" runat="server" Label="Label" Text="分点选择：">
                                                                            </ext:Label>
                                                                            <ext:DropDownList ID="dropDictLab" runat="server" Label="分点" Resizable="True" Width="180px" ShowLabel="false">
                                                                            </ext:DropDownList>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                </Items>
                                                            </ext:FormRow>
                                                            <ext:FormRow>
                                                                <Items>
                                                                    <ext:Panel ID="Panel3" runat="server" BodyPadding="2px" EnableBackgroundColor="false"
                                                                        ShowBorder="false" ShowHeader="false" Title="Panel" Layout="Column">
                                                                        <Items>
                                                                            <ext:Label ID="Label3" runat="server" Label="Label" Text="状态选择：">
                                                                            </ext:Label>
                                                                            <ext:DropDownList ID="dropStatus" runat="server" Label="状态选择" Resizable="True" ShowLabel="false" Width="120px">
                                                                                <ext:ListItem Text="全部" Value="-1" />
                                                                                <ext:ListItem Text="跟进中" Value="0" Selected="true" />
                                                                                <ext:ListItem Text="跟进完成" Value="1" />
                                                                            </ext:DropDownList>
                                                                            <ext:Button ID="btnSearch" runat="server" Text="查询"  OnClick="btnSearch_Click" Icon="Magnifier">
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                </Items>
                                                            </ext:FormRow>
                                                        </Rows>
                                                    </ext:Form>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region10" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                                                ShowBorder="false" Title="Center Region">
                                                <Items>
                                                    <ext:Grid ID="gdOrders" runat="server" IsDatabasePaging="true" EnableRowClick="true"
                                                        AllowPaging="true" EnableRowNumber="true" DataKeyNames="ORDERNUM,REVIEWSTATE"
                                                        EnableCheckBoxSelect="true" EnableMultiSelect="false" ShowHeader="false" OnPageIndexChange="gdOrders_PageIndexChange"
                                                        OnRowClick="gdOrders_RowClick">
                                                        <Columns>
                                                            <ext:BoundField HeaderText="会员姓名" DataField="REALNAME" />
                                                            <ext:BoundField HeaderText="状态" DataField="BASICNAME" />
                                                            <ext:BoundField HeaderText="体检号" DataField="ORDERNUM" />
                                                            <ext:BoundField HeaderText="联系方式" DataField="PHONE" />
                                                            <ext:BoundField HeaderText="登记时间" DataField="ENTERDATE" />
                                                            <ext:BoundField HeaderText="预约复查时间" DataField="RERUNDATE" />
                                                            <ext:BoundField HeaderText="跟进状态" DataField="CNREVIEWSTATE" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Title="Center Region" ShowBorder="false"
                Layout="Fit">
                <Items>
                    <ext:TabStrip ID="TabStrip1" ShowBorder="false" ActiveTabIndex="0" runat="server" EnableTitleBackgroundColor="true"
                        BodyPadding="0">
                        <Tabs>
                            <ext:Tab ID="Tab1" Title="跟进记录" EnableBackgroundColor="false" BodyPadding="0px" runat="server"
                                Layout="Fit">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region4" runat="server" Position="Center" ShowHeader="false" Split="true"
                                                EnableSplitTip="true" EnableCollapse="true" CollapseMode="Mini" ShowBorder="false"
                                                Layout="Fit">
                                                <Items>
                                                    <ext:Grid ID="gdOrderserviceinfo" runat="server" EnableRowNumber="true" DataKeyNames="ORDERSERVICEINFOID,ORDERNUM"
                                                        ShowHeader="false">
                                                        <Toolbars>
                                                            <ext:Toolbar ID="Toolbar1" runat="server">
                                                                <Items>
                                                                  <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                                                     </ext:ToolbarFill>
                                                                     <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                                        </ext:ToolbarSeparator>
                                                                    <ext:Button ID="btnFinish" Text="完成跟进" Icon="Accept" EnablePostBack="true" runat="server"
                                                                        ConfirmText="确定要完成跟进吗?" OnClick="btnFinish_Click">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Toolbars>
                                                        <Columns>
                                                            <ext:BoundField HeaderText="体检号" DataField="ORDERNUM" />
                                                            <ext:BoundField HeaderText="跟进人" DataField="USERNAME" />
                                                            <ext:BoundField HeaderText="跟进日" DataField="CREATEDATE" />
                                                            <ext:BoundField HeaderText="跟进内容" DataField="SERVICECONTENT" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region><ext:Region ID="Region5" runat="server" Position="Right" ShowHeader="false"
                                                Width="200px" ShowBorder="false" Title="Center Region" Layout="Fit">
                                                <Toolbars>
                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                        <Items>
                                                        <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                                        </ext:ToolbarFill>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                        </ext:ToolbarSeparator>
                                                            <ext:Button ID="btnSave" Text="保存跟进内容" Icon="Add" EnablePostBack="true" runat="server"
                                                                OnClick="btnSave_Click">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Toolbars>
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region7" runat="server" Position="Top" ShowHeader="true" Split="true"
                                                                Layout="Fit" ShowBorder="false" Title="预约复查时间" Height="50px">
                                                                <Items>
                                                                    <ext:DatePicker ID="dpRerundate" runat="server" Label="预约复查时间" ShowLabel="false">
                                                                    </ext:DatePicker>
                                                                </Items>
                                                            </ext:Region><ext:Region ID="Region8" runat="server" Position="Center" ShowHeader="true"
                                                                Layout="Fit" ShowBorder="false" Title="跟进内容">
                                                                <Items>
                                                                    <ext:TextArea runat="server" Label="跟进内容" ShowLabel="false" ID="tbServicecontent"
                                                                        ShowRedStar="true">
                                                                    </ext:TextArea>
                                                                </Items>
                                                            </ext:Region></Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab Title="报告预览" BodyPadding="0px" Layout="Fit" EnableBackgroundColor="false"
                                runat="server">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region EnableIFrame="true" runat="server" Position="Center" ID="tabReport" ShowHeader="false"
                                                Title="Center Region" Layout="Fit">
                                            </ext:Region></Regions>
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
