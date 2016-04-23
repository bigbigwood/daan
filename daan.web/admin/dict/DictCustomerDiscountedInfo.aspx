<%@ Page Title="体检单位总体价格维护" Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerDiscountedInfo.aspx.cs"
EnableViewStateMac="false" EnableEventValidation="false"     Inherits="daan.web.admin.dict.DictCustomerDiscountedInfo" %>

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
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" Text="关闭" runat="server" OnClick="btnClose_Click" Icon="BulletCross">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="600px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
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
                                                    <ext:Label ID="Label2" Text="起始时间：" runat="server" Width="60px"></ext:Label>
                                                    <ext:DatePicker runat="server"  AutoPostBack="true" Label="开始日期" EmptyText="请选择日期" ID="Dp_Bingin" Width="90"></ext:DatePicker>
                                                    <ext:DatePicker runat="server" Readonly="false" EmptyText="请选择日期" DateFormatString="yyyy-MM-dd" Label="结束日期" ID="DatePicker3" Width="90"></ext:DatePicker>
                                                    <ext:Button ID="btnSearch" runat="server" Icon="Magnifier" Text="查询" ValidateForms="SimpleForm4" OnClick="btnSearch_Click"></ext:Button>
                                                 </Items>
                                                </ext:Panel>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="400px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true" EnableTextSelection="true"
                                        AutoHeight="true" Title="Grid" ShowBorder="true" ShowHeader="false" DataKeyNames="Dictcustomerdiscountid"
                                        Width="480" AllowPaging="true" EnableRowClick="true" Height="275" EnableMultiSelect="false"
                                        OnRowClick="gvList_RowClick" IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="AuditText" HeaderText="审核状态" Width="60px" />
                                            <ext:BoundField DataField="testcode" DataToolTipField="testcode" Width="80px" HeaderText="套餐代码" />
                                            <ext:BoundField DataField="testname" DataToolTipField="testname" Width="150px" HeaderText="套餐" />
                                            <ext:BoundField DataField="price" HeaderText="成交价格" Width="60px" />
                                            <ext:BoundField Width="80px" DataField="Begindate" DataFormatString="{0:yyyy-MM-dd}"
                                                HeaderText="开始日期" DataToolTipField="Begindate" />
                                            <ext:BoundField Width="80px" DataField="Enddate" DataFormatString="{0:yyyy-MM-dd}"
                                                HeaderText="结束日期" DataToolTipField="Enddate" />
                                            <ext:BoundField DataField="subcompanyname" DataToolTipField="subcompanyname" HeaderText="签约子公司" Width="120px"/>
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
                                        LabelWidth="90px" EnableBackgroundColor="false" AutoWidth="true" Height="250px">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox ID="tbxDiscounted"  EmptyText="请输入0～1之间的数字"  FocusOnPageLoad="true" runat="server" Label="折扣率" Required="false" 
                                                        MaxLength="8"  MinValue="0" MaxValue="1"  ShowRedStar="false" NoNegative="True">
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
                                                        CompareMessage="结束日期应该大于开始日期" Label="结束日期" runat="server" ShowRedStar="True">
                                                    </ext:DatePicker>
                                                </Items>
                                            </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList runat="server" AutoPostBack="true" Required="true" CompareType="String" CompareValue="-1" CompareOperator="NotEqual"
                                                     ID="dropProducts" Label="套餐" ShowRedStar="true" Resizable="true"
                                                    EnableEdit="true" OnSelectedIndexChanged="dropProducts_SelectedIndexChanged"></ext:DropDownList>
                                                </Items>
                                           </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox runat="server" ID="txtTestCode" Label="套餐代码" EmptyText="0" Enabled="false"></ext:TextBox>
                                                </Items>
                                           </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox runat="server" ID="txtProductPrice" Label="标准价格" EmptyText="0" Enabled="false"></ext:TextBox>
                                                </Items>
                                           </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox runat="server" ShowRedStar="true" ID="txtPrice" Label="成交价格" Required="true" RequiredMessage="标准价格不能为空"></ext:NumberBox>
                                                </Items>
                                           </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList runat="server" ID="dropSubCompany" Label="签约子公司" ShowRedStar="true" Resizable="true" EnableEdit="true"
                                                     Required="true" CompareType="String" CompareValue="-1" CompareOperator="NotEqual"></ext:DropDownList>
                                                </Items>
                                           </ext:FormRow>
                                           <ext:FormRow>
                                                <Items>
                                                    <ext:Label runat="server" ID="lblAuditStatus" Label="审核状态"></ext:Label>
                                                </Items>
                                           </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                    <ext:Form ID="Form2" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        LabelWidth="70px" EnableBackgroundColor="false" AutoWidth="true" Height="60px">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox runat="server" ID="txtUpdateBy" Enabled="false" Label="操作人"></ext:TextBox>
                                                    <ext:TextBox runat="server" ID="txtUpdateDate" Enabled="false" Label="操作时间"></ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox runat="server" ID="txtAuditBy" Enabled="false" Label="审核人"></ext:TextBox>
                                                    <ext:TextBox runat="server" ID="txtAuditDate" Enabled="false" Label="审核时间"></ext:TextBox>
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
