<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProSusManagement.aspx.cs" Inherits="daan.web.admin.proceed.ProSusManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExport" runat="server" Text="导出" Icon="PageExcel"
                    DisableControlBeforePostBack="false" EnableAjax="false" OnClick="btnExport_Click"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="65px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                         LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="选择分点"
                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" runat="server">
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="dropStatus" runat="server" Label="状态">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="DropIScancel" Label="是否作废" Resizable="True">
                                        <ext:ListItem Text="全部" Value="-1" />
                                        <ext:ListItem Text="已作废" Value="1" />
                                        <ext:ListItem Selected="true" Text="未作废" Value="0" />
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:DatePicker ID="datebegin" Label="登记时间" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:DatePicker ID="dateend" Label="至" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:NumberBox ID="tbxOrderNum" runat="server" Label="体 检 号" EmptyText="此条件忽略其他条件">
                                    </ext:NumberBox>
                                    <ext:TextBox runat="server" ID="tbxName" Label="姓　　名" />
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridOrders" Title="订单列表" DataKeyNames="ordernum,dictmemberid"
                        AutoScroll="true" EnableCheckBoxSelect="false" ShowHeader="false" PageSize="20"
                        IsDatabasePaging="true" EnableTextSelection="true" AllowPaging="true" runat="server"
                        AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="GridOrders_PageIndexChange">
                        <Columns>
                            <%--<ext:BoundField DataField="provincename" DataToolTipField="provincename" HeaderText="省份" Width="60px" />
                            <ext:BoundField DataField="cityname" DataToolTipField="cityname" HeaderText="城市" Width="60px" />
                            <ext:BoundField DataField="countyname" DataToolTipField="countyname" HeaderText="地区" Width="60px" />--%>
                            <ext:BoundField DataField="barcode" HeaderText="条码号" Width="100px" />
                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="80px" />
                            <ext:BoundField DataField="sex" HeaderText="性别" Width="50px" />
                            <ext:BoundField DataField="birthday" HeaderText="出生年月" Width="100px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="mobile" HeaderText="手机号码" Width="100px" />
                            <ext:BoundField DataField="idnumber" HeaderText="身份证号" Width="150px" />
                            <ext:BoundField DataField="testcode" HeaderText="项目1编号" Width="100px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="客户（门店名称)" Width="200px" />
                            <ext:BoundField DataField="addres" DataToolTipField="addres" HeaderText="客户(门店)详细地址" ExpandUnusedSpace="true" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
