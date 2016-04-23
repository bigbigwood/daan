<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictCustomerProductPriceAudit.aspx.cs" Inherits="daan.web.admin.dict.DictCustomerProductPriceAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>体检单位套餐价格审核</title>
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
                    <ext:Button ID="Button1" runat="server" EnablePostBack="false" Text="套餐价格审核" Icon="UserOrange" CssClass="inline">
                        <Menu ID="Menu1" runat="server">
                            <ext:MenuButton runat="server" ID="btnAudit" Icon="UserGreen" Text="审核" ConfirmText="是否确定审核选中的信息" ConfirmIcon="Question" ConfirmTitle="审核单位" ConfirmTarget="Self" OnClick="btnAudit_Click"></ext:MenuButton>
                            <ext:MenuButton runat="server" ID="btnNaudit" Icon="UserRed" Text="取消审核" ConfirmText="是否确定取消审核选中的信息" ConfirmIcon="Question" ConfirmTitle="取消审核单位" ConfirmTarget="Self" OnClick="btnNaudit_Click"></ext:MenuButton>
                        </Menu>
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" Text="关闭" runat="server" OnClick="btnClose_Click" Icon="BulletCross">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region1" runat="server" Layout="Fit" Height="60px" Position="Top" ShowHeader="False" ShowBorder="False" Split="true">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow runat="server" ID="Row1">
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropProducts" Label="套餐" Resizable="True" EnableEdit="true"></ext:DropDownList>
                                    <ext:DropDownList Label="审核状态" ID="dropStatus" runat="server">
                                        <ext:ListItem Text="全部" Value="-1" Selected="true"/>
                                        <ext:ListItem Text="已审核" Value="1" />
                                        <ext:ListItem Text="未审核" Value="0" />
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="dropSubCompany" Label="签约子公司" Resizable="true" EnableEdit="true"></ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DatePicker runat="server"  AutoPostBack="true" Label="开始日期" EmptyText="请选择日期" ID="Dp_Bingin"></ext:DatePicker>
                                    <ext:DatePicker runat="server" Readonly="false" EmptyText="请选择日期" DateFormatString="yyyy-MM-dd" Label="结束日期" ID="DatePicker3"></ext:DatePicker>
                                    <ext:TriggerBox runat="server" Hidden="true" ID="txtStrKey" Label="关键字" ShowTrigger="false" OnTriggerClick="txtStrKey_TriggerClick"></ext:TriggerBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Region2" Layout="Fit" runat="server" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid runat="server" ID="gvList" Title="体检单位" DataKeyNames="Dictcustomerdiscountid,Active"
                    AutoScroll="true" EnableCheckBoxSelect="true" ShowHeader="false" PageSize="20"
                    IsDatabasePaging="true" EnableTextSelection="true" AllowPaging="true"
                    AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="gvList_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="audittext" HeaderText="审核状态" Width="60px" />
                            <ext:BoundField DataField="testcode" DataToolTipField="testcode" HeaderText="套餐代码" Width="80px" />
                            <ext:BoundField DataField="testname" DataToolTipField="testname" HeaderText="套餐" Width="200px" />
                            <ext:BoundField DataField="price" HeaderText="成交价格" Width="60px" />
                            <ext:BoundField DataField="nprice" HeaderText="标准价格" Width="60px" />
                            <ext:BoundField DataField="Begindate" HeaderText="开始日期" Width="80px" DataFormatString="{0:yyyy-MM-dd}"/>
                            <ext:BoundField DataField="Enddate" HeaderText="结束日期" Width="80px" DataFormatString="{0:yyyy-MM-dd}"/>
                            <ext:BoundField DataField="subcompanyname" DataToolTipField="subcompanyname" Width="150px" HeaderText="签约子公司" />
                            <ext:BoundField DataField="UpdatebyName" HeaderText="操作人" Width="80px" />
                            <ext:BoundField DataField="Updatedate" HeaderText="操作时间" DataToolTipField="Updatedate" Width="120px" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
