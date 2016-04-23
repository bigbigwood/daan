<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerInfoAudit.aspx.cs" Inherits="daan.web.admin.dict.DictCustomerInfoAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>体检单位信息审核</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="Button1" runat="server" EnablePostBack="false" Text="单位信息审核" Icon="UserOrange" CssClass="inline">
                        <Menu ID="Menu1" runat="server">
                            <ext:MenuButton runat="server" ID="btnAudit" Icon="UserGreen" Text="审核" ConfirmText="是否确定审核选中的单位信息" ConfirmIcon="Question" ConfirmTitle="审核单位" ConfirmTarget="Self" OnClick="btnAudit_Click"></ext:MenuButton>
                            <ext:MenuButton runat="server" ID="btnNaudit" Icon="UserRed" Text="取消审核" ConfirmText="是否确定取消审核选中的单位信息" ConfirmIcon="Question" ConfirmTitle="取消审核单位" ConfirmTarget="Self" OnClick="btnNaudit_Click"></ext:MenuButton>
                        </Menu>
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                    <ext:Button runat="server" ID="btnProductAudit" Text="单位套餐价格审核" Icon="UserOrange" OnClick="btnProductAudit_Click"></ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                    <ext:Button runat="server" ID="btnExport" Text="导出到EXCEL" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnExport_Click"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region1" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="330px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region3" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="110px" runat="server" ShowBorder="false">
                                <Items>
                                   <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        LabelWidth="70px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                                        <Rows>
                                            <ext:FormRow runat="server" ID="Row1">
                                                <Items>
                                                    <ext:DropDownList runat="server" ID="dropDictlab" Label="分点" Resizable="True" EnableEdit="true"></ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow runat="server" ID="ROW2">
                                                <Items>
                                                    <ext:RadioButtonList Label="类型" runat="server" ID="raoType">
                                                        <ext:RadioItem Text="单位" Value="0" Selected="true" />
                                                        <ext:RadioItem Text="套餐" Value="1" />
                                                    </ext:RadioButtonList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow runat="server" ID="FormRow1">
                                                <Items>
                                                    <ext:DropDownList Label="审核状态" ID="dropStatus" runat="server">
                                                        <ext:ListItem Text="全部" Value="-1"/>
                                                        <ext:ListItem Text="已审核" Value="1" />
                                                        <ext:ListItem Text="未审核" Value="0" Selected="true" />
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow runat="server" ID="FormRow2">
                                                <Items>
                                                    <ext:TriggerBox runat="server" ID="txtStrKey" Label="关键字" ShowTrigger="false" OnTriggerClick="txtStrKey_TriggerClick"></ext:TriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region4" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid runat="server" ID="gvList" Title="体检单位" DataKeyNames="DICTCUSTOMERID,CUSTOMERNAME,CUSTOMERCODE,Active"
                                    AutoScroll="true" EnableCheckBoxSelect="true" ShowHeader="false" PageSize="20"
                                    IsDatabasePaging="true" EnableTextSelection="true" AllowPaging="true" EnableRowClick="true"
                                    AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnRowClick="gvList_RowClick" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="audittext" HeaderText="审核状态" Width="60px" />
                                            <ext:BoundField DataField="CUSTOMERNAME" DataToolTipField="CUSTOMERNAME" Width="250px" HeaderText="单位名称" />
                                            <ext:BoundField DataField="CUSTOMERCODE" HeaderText="客户代码" Width="80px" />
                                            <%--<ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="分点实验室" Width="200px" />
                                            <ext:BoundField DataField="ADDRESS" DataToolTipField="ADDRESS" HeaderText="单位地址" Width="150px" />
                                            <ext:BoundField DataField="CONTACTMAN" HeaderText="联系人" Width="60px" />
                                            <ext:BoundField DataField="CONTACTPHONE" HeaderText="联系人电话" Width="100px" />
                                            <ext:BoundField DataField="saleman" HeaderText="销售人员" Width="70px" />
                                            <ext:BoundField DataField="checkbillnam" HeaderText="财务人员" Width="70px" />
                                            <ext:BoundField DataField="TELEPHONE" HeaderText="联系电话" Width="100px"/>
                                            <ext:BoundField DataField="FAX" HeaderText="传真" Width="100px" />
                                            <ext:BoundField DataField="EMAIL" HeaderText="邮箱" DataToolTipField="EMAIL" Width="100px" />
                                            <ext:BoundField DataField="POSTCODE" HeaderText="邮编" Width="80px" />
                                            <ext:BoundField DataField="CUSTOMERNAME2" DataToolTipField="CUSTOMERNAME2" HeaderText="单位别名" Width="150px" />
                                            <ext:BoundField DataField="REMARK" DataToolTipField="REMARK" HeaderText="备注" />--%>
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
                    <ext:Form ID="Form3" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                        Title="单位详细信息" LabelWidth="90px" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="tbxCoustomerName" Label="单位名称" Enabled="false" runat="server">
                                    </ext:TextBox>
                                    <ext:TextBox ID="tbxAddress" Label="单位地址" Enabled="false" runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Label="客户代码" Enabled="false" ID="tbxCoustomerCode">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Label="联系人" Enabled="false" ID="tbxContactman">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:NumberBox ID="tbxDisplayOrder" Enabled="false" Label="排序" runat="server">
                                    </ext:NumberBox>
                                    <ext:TextBox runat="server" Enabled="false" Label="电话" ID="tbxTelePhone">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" Enabled="false" ID="dropDictsalemanId" Label="销售人员">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" Enabled="false" ID="dropDictcheckBillId" Label="财务人员">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" Enabled="false" ID="dropDictlab2" Label="分点实验室">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" Enabled="false" ID="DropDownList2" Label="客户状态" Resizable="True">
                                        <ext:ListItem Text="合作客户" Value="1" Selected="true"></ext:ListItem>
                                        <ext:ListItem Text="意向客户" Value="2"></ext:ListItem>
                                        <ext:ListItem Text="终止客户" Value="3"></ext:ListItem>
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" ID="tbxFastCode" Label="助记符" MaxLength="10">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="30" Label="联系电话" ID="tbxContactPhone">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" ID="tbxFax" Label="传真" MaxLength="40">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" ID="tbxEmail" Label="邮箱地址" MaxLength="60">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="50" ID="tbxEnErpCode" Label="ERP客户代号">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" ID="tbxErpName" MaxLength="100" Label="ERP客户名称">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="50" Label="英文名称" ID="tbxEnName">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="250" Label="英文地址" ID="tbxEnAddress">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="50" ID="tbxDocumentType" Label="证件类型">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" MaxLength="100" ID="tbxDocumentCode" Label="证件代号">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:RadioButtonList runat="server" Enabled="false" Label="客户类型" ID="radlCustomerType">
                                        <ext:RadioItem Selected="True" Text="一般客户" Value="0"></ext:RadioItem>
                                        <ext:RadioItem Text="外包客户" Value="1"></ext:RadioItem>
                                    </ext:RadioButtonList>
                                    <ext:CheckBox ID="chkIssms" Enabled="false" runat="server" Checked="true" Label="是否短信提醒" ShowLabel="true">
                                    </ext:CheckBox>
                                    <ext:RadioButtonList runat="server" Enabled="false" ID="radlActive" Label="审核状态">
                                        <ext:RadioItem Selected="True" Text="未审核" Value="0"></ext:RadioItem>
                                        <ext:RadioItem Text="已审核" Value="1"></ext:RadioItem>
                                    </ext:RadioButtonList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxReporTitle" Enabled="false" MaxLength="100" Label="报告单抬头">
                                    </ext:TextBox>
                                    <ext:TextBox runat="server" ID="tbxPostCode" Enabled="false" MaxLength="10" Label="邮编">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxCoustomerName2" Enabled="false" MaxLength="100" Label="单位名称别名">
                                    </ext:TextBox>
                                    <ext:RadioButtonList runat="server" ID="radIsPub" Enabled="false" Label="是否公用单位">
                                        <ext:RadioItem Text="是" Value="1" />
                                        <ext:RadioItem Text="否" Value="0" Selected="true" />
                                    </ext:RadioButtonList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea runat="server" AutoGrowHeight="True" Enabled="false" Label="备注" AutoGrowHeightMax="100px"
                                        AutoGrowHeightMin="100px" EmptyText="" Height="100px" ID="TxaRemark"
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
        Target="Top" IsModal="True" Width="900px" Height="500px">
    </ext:Window>
    </form>
</body>
</html>
