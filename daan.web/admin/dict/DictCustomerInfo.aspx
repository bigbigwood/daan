<%@ Page Title="体检单位资料维护" Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerInfo.aspx.cs"
    EnableViewStateMac="false" EnableEventValidation="false" Inherits="daan.web.admin.dict.DictCustomerInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
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
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="Form1"
                        CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDelAll_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrice" Text="设置单位价格"  Icon="BasketAdd" runat="server"  OnClick="btnPrice_Click" >
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="80px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="Right">
                                        <Items>
                                            <ext:DropDownList runat="server" ID="dropLabSearch" Label="分点" Resizable="True" EnableEdit="true">
                                            </ext:DropDownList>
                                            <ext:CheckBox ID="chkActive" runat="server" Checked="true" Label="已审核" ShowLabel="true">
                                            </ext:CheckBox>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                EmptyText="请输入单位名称" Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                OnTrigger2Click="btSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" PageSize="20" runat="server" EnableCheckBoxSelect="true" EnableRowNumber="true"
                                        EnableTextSelection="true" AutoHeight="true" Title="Grid" ShowBorder="true" ShowHeader="false"
                                        DataKeyNames="Dictcustomerid,Customername,Customertype,Active" Width="250" AllowPaging="true"
                                        EnableRowClick="true" Height="425" EnableMultiSelect="true" IsDatabasePaging="true" OnRowClick="gvList_RowClick" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="80px" DataField="Customercode" HeaderText="客户代码"/>
                                            <ext:BoundField Width="200px" DataField="Customername" HeaderText="单位名称"/>       
                                            <ext:BoundField Width="200px" DataField="Dictcustomerid" HeaderText="编号"/>                                                                                    
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center" runat="server">
                <Items>
                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                        Title="当前状态-新增" LabelWidth="90px" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxCoustomerName" MaxLength="50" Required="true" FocusOnPageLoad="true"
                                        Label="单位名称" ShowRedStar="true" Text="" runat="server">
                                    </ext:TextBox>
                                    <ext:TextBox ID="tbxAddress" MaxLength="250" Required="true" Label="单位地址" ShowRedStar="true"
                                        runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Required="true" MaxLength="10" Label="客户代码" ShowRedStar="true"
                                        ID="tbxCoustomerCode">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" MaxLength="30" Label="联系人" Required="true" ShowRedStar="true"
                                        ID="tbxContactman">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:NumberBox ID="tbxDisplayOrder" Required="true" Label="排序" MinValue="0" MaxLength="3"
                                        NoNegative="True" RegexPattern="NUMBER" ShowRedStar="true" runat="server">
                                    </ext:NumberBox>
                                    <ext:TextBox runat="server" Required="true" Label="电话" MaxLength="30" ShowRedStar="true"
                                        ID="tbxTelePhone">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropDictsalemanId" ShowRedStar="true" Label="销售人员"
                                        Required="true" CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual"
                                        EnableEdit="true">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="dropDictcheckBillId" ShowRedStar="true" Label="财务人员"
                                        Required="true" CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual"
                                        AutoPostBack="true" EnableEdit="true">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropDictLab" ShowRedStar="true" Label="分点实验室"
                                        Required="true" CompareType="String" Resizable="True" CompareValue="-1" CompareOperator="NotEqual">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="dropStatus" Label="客户状态" Resizable="True">
                                        <ext:ListItem Text="合作客户" Value="1" Selected="true"></ext:ListItem>
                                        <ext:ListItem Text="意向客户" Value="2"></ext:ListItem>
                                        <ext:ListItem Text="终止客户" Value="3"></ext:ListItem>
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxFastCode" Label="助记符" MaxLength="10">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" MaxLength="30" Label="联系电话" ID="tbxContactPhone">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxFax" Label="传真" MaxLength="40">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" ID="tbxEmail" Label="邮箱地址" MaxLength="60">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" MaxLength="50" ID="tbxEnErpCode" Label="ERP客户代号">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" ID="tbxErpName" MaxLength="100" Label="ERP客户名称">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" MaxLength="50" Label="英文名称" ID="tbxEnName">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" MaxLength="250" Label="英文地址" ID="tbxEnAddress">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" MaxLength="50" ID="tbxDocumentType" Label="证件类型">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" MaxLength="100" ID="tbxDocumentCode" Label="证件代号">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:RadioButtonList runat="server" Label="客户类型" ID="radlCustomerType">
                                        <ext:RadioItem Selected="True" Text="一般客户" Value="0"></ext:RadioItem>
                                        <ext:RadioItem Text="外包客户" Value="1"></ext:RadioItem>
                                    </ext:RadioButtonList>
                                    <ext:CheckBox ID="chkIssms" runat="server" Checked="true" Label="是否短信提醒" ShowLabel="true">
                                    </ext:CheckBox>
                                    <ext:RadioButtonList runat="server" Enabled="false" ID="radlActive" Label="审核状态">
                                        <ext:RadioItem Selected="True" Text="未审核" Value="0"></ext:RadioItem>
                                        <ext:RadioItem Text="已审核" Value="1"></ext:RadioItem>
                                    </ext:RadioButtonList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxReporTitle" MaxLength="100" Label="报告单抬头" Required="true"
                                        ShowRedStar="true">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" ID="tbxPostCode" MaxLength="10" Label="邮编">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxCoustomerName2" MaxLength="100" Label="单位名称别名">
                                    </ext:TextBox>
                                    <ext:RadioButtonList runat="server" ID="radIsPub" Label="是否公用单位">
                                        <ext:RadioItem Text="是" Value="1" />
                                        <ext:RadioItem Text="否" Value="0" Selected="true" />
                                    </ext:RadioButtonList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea runat="server" AutoGrowHeight="True" Label="备注" AutoGrowHeightMax="100px"
                                        AutoGrowHeightMin="100px" EmptyText="文本框的高度会自动扩展" Height="100px" ID="TxaRemark"
                                        MaxLength="250">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinLibraryEdit" Title="编辑" Popup="false" EnableIFrame="true" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="1000px" Height="500px">
    </ext:Window>
    </form>
</body>
</html>
