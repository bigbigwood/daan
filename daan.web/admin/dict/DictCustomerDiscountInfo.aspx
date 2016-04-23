<%@ Page Title="外包单位价格维护" Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerDiscountInfo.aspx.cs"  EnableViewStateMac="false"
 Inherits="daan.web.admin.dict.DictCustomerDiscountInfo" %>

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
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>  
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" Text="新增" runat="server" Icon="Add" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="保存" runat="server" ValidateForms="SimpleForm3"
                        Icon="SystemSaveNew" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" Text="删除" Icon="Delete" runat="server" OnClick="btnDelAll_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="300px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false" Height="100px">
                        <Regions>
                            <ext:Region ID="Region1" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="5 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="65px" LabelAlign="Right">
                                       <Items>
                                         <ext:Panel BodyPadding="0px" Title="中区" EnableBackgroundColor="false"
                                                ShowHeader="false" ShowBorder="false" runat="server" Layout="Column" ID="panelTitle">
                                                <Items>
                                                <ext:Label ID="Label2" Text="起始时间：" runat="server" Width="60px">
                                                    </ext:Label>
                                                <ext:DatePicker runat="server"  AutoPostBack="true" Label="开始日期" EmptyText="请选择日期" ID="Dp_Bingin" Width="90"></ext:DatePicker>
                                                <ext:DatePicker runat="server" Readonly="false" EmptyText="请选择日期" DateFormatString="yyyy-MM-dd" Label="结束日期" ID="DatePicker3" Width="90"></ext:DatePicker>
                                                <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="查询" ValidateForms="SimpleForm4" OnClick="btnSearch_Click"></ext:Button>
                                                 </Items>
                                                </ext:Panel>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true" EnableTextSelection="true"
                                        AutoHeight="true" Title="Grid" ShowBorder="true" ShowHeader="false" DataKeyNames="Dictcustomerdiscountid"
                                        AllowPaging="true" EnableRowClick="true" Height="275" EnableMultiSelect="false"
                                        OnRowClick="gvList_RowClick" IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="110px" DataField="Testname" DataFormatString="{0}" HeaderText="测试项"
                                                DataToolTipField="Testname" />
                                            <ext:BoundField Width="50px" DataField="Finalprice" DataFormatString="{0}" HeaderText="最终价格"
                                                DataToolTipField="Finalprice" />
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
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:Panel ID="SimpleFormEdit" ColumnWidth="100%" Layout="Fit" BodyPadding="5px"
                        AutoHeight="true" Title="当前状态-新增" EnableBackgroundColor="false" runat="server"
                        ShowHeader="true" Height="390px">
                        <Items>
                            <ext:SimpleForm ID="SimpleForm3" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false"
                                runat="server" EnableCollapse="True"  LabelWidth="100px" LabelAlign="Right">
                                <Items>
                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        LabelWidth="70px" EnableBackgroundColor="false" AutoWidth="true" Height="350px">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList runat="server"  Resizable="True"  FocusOnPageLoad="true" Label="测试项" ID="Drop_DictTestItemId" ShowRedStar="true" CompareValue="-1"
                                                        EnableEdit="true" CompareType="String"  CompareOperator="NotEqual" >
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox ID="tbxFinalprice" runat="server" Label="最终价格" Required="true" 
                                                        MaxLength="8" MaxValue="1000000" MinValue="0" ShowRedStar="true" NoNegative="True"
                                                        CompareMessage="必填项,且输入数字类型">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DatePicker runat="server" Required="true" AutoPostBack="true" OnTextChanged="DatePicker1_TextChanged"
                                                        Label="开始日期" EmptyText="请选择开始日期" ID="DatePicker1" ShowRedStar="True" >
                                                    </ext:DatePicker>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DatePicker ID="DatePicker2" Required="true" Readonly="false" CompareControl="DatePicker1"
                                                        EmptyText="请选择结束日期" DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThanEqual"
                                                        CompareMessage="结束日期应该大于开始日期" Label="结束日期" runat="server" ShowRedStar="True"
                                                        >
                                                    </ext:DatePicker>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:SimpleForm>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
