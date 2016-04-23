<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmStat.aspx.cs" Inherits="daan.web.admin.bill.FrmStat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>体检统计</title>
</head>
<body>
    <form id="form4" runat="server">
     <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server">
    </ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>    
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="导出" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false"
                        ID="btnExcel" OnClick="btnExcel_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Center" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region4" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="90px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        LabelWidth="90px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow ID="FormRow2" runat="server">
                                                <Items>
                                                    <ext:DropDownList ID="ddlStatus"  runat="server" Label="查询统计" Resizable="True" EnableEdit="true">
                                                        <ext:ListItem Text="所有检测数据统计" Value="0" />
                                                        <ext:ListItem Text="护美类产品返回公司体检量统计" Value="1"/>
                                                        <ext:ListItem Text="查询太平TM15检测数据" Value="2" Selected="true"  />
                                                        <ext:ListItem Text="分点查询所有检测项目，并组合项目及价格求和" EnableSelect="false" Value="3" />
                                                        <ext:ListItem Text="HPV+TM检查统计" Value="4" />
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="dropDictLab" Resizable="True" runat="server" EnableEdit="true" Label="分点"  AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" runat="server">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow3" runat="server">
                                                <Items>
                                                    <ext:DatePicker ID="Dp_BeginDate" runat="server" Label="登记时间" DateFormatString="yyyy-MM-dd" />
                                                    <ext:DatePicker ID="Dp_EndDate" runat="server" DateFormatString="yyyy-MM-dd" Label="至" />   
                                                    <ext:DropDownList runat="server" ID="dropProvice" Label="省份"></ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow4" runat="server">
                                                <Items>
                                                    <ext:TextBox ID="txtSection" runat="server" Label="关键字" EmptyText="区域、备注、单位名称"></ext:TextBox>
                                                    <ext:Label runat="server" Hidden="true"></ext:Label>
                                                    <ext:Label ID="Label1" runat="server" Hidden="true"></ext:Label>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region5" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>  
                                    <ext:Grid ID="GridAcconding" runat="server" AutoScroll="true" ShowHeader="false" PageSize="20" IsDatabasePaging="true" AllowPaging="true"
                                     AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="GridAcconding_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="110px" />
                                            <ext:BoundField DataField="barcode" HeaderText="条码号" Width="100px" />
                                            <ext:BoundField DataField="realname" DataToolTipField="realname" HeaderText="姓名" Width="60px" />
                                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="50px" />
                                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="50px" />
                                            <ext:BoundField DataField="customercode" HeaderText="单位代码" />
                                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位名称" Width="280px" />
                                            <ext:BoundField DataField="productname" DataToolTipField="productname" HeaderText="套餐名称" />
                                            <ext:BoundField DataField="testname" DataToolTipField="testname" HeaderText="项目、组合" Width="230px" />
                                            <ext:BoundField DataField="province" DataToolTipField="province" HeaderText="省市区" Width="100px" />
                                            <ext:BoundField DataField="section" DataToolTipField="section" HeaderText="区域" Width="70px" />
                                            <ext:BoundField DataField="remarks" HeaderText="备注" ExpandUnusedSpace="true" />
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
    </form>
</body>
</html>
