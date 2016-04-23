<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillProduct.aspx.cs" Inherits="daan.web.admin.bill.BillProduct"
    Title="产品财务结算" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
</head>
<body>
    <form id="form7" runat="server">
    <ext:PageManager ID="PageManager2" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="查询" ID="btnSearch" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                   <ext:Button ID="btnExcel" runat="server" Icon="TableGear" Text="导出Excel" CssClass="inlineButton" OnClick="btnExcel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="regiontop" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="60px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="1px"
                        Height="100px" LabelWidth="60px" EnableBackgroundColor="false" AutoWidth="true"
                        LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropLab" AutoPostBack="true" Resizable="True"
                                        Label="分点" OnSelectedIndexChanged="dropLab_SelectedIndexChanged" EnableEdit="true">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="dropCustomer" EnableEdit="false" Resizable="True"
                                        AutoPostBack="true" Label="选择单位">
                                    </ext:DropDownList>
                                    <ext:DatePicker runat="server" ID="dtpStart" Label="录入日期">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" ID="dtpEnd" Label="至">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropDicttestitem" EnableEdit="false" Resizable="True"
                                        AutoPostBack="true" Label="选择套餐">
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="drpState" Label="状态" runat="server">         
                                        <ext:ListItem Selected="true" Text=" 已出报告" Value="0" />
                                        <ext:ListItem Selected="true" Text="未出报告" Value="1" />          
                                    <ext:ListItem Selected="true" Text="未返回检验" Value="2" />                                                                     
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Regioncenter" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Center" runat="server" ShowBorder="false">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false"
                        AutoWidth="true" EnableRowNumber="True" AutoHeight="true" AllowPaging="true"
                        EnableTextSelection="true" IsDatabasePaging="true" runat="server" EnableMultiSelect="false"
                        OnPageIndexChange="gvList_PageIndexChange">
                        <Columns>
                            <ext:BoundField Width="100px" DataField="INSTRUMENTSBARCODE" HeaderText="耗材条码" />
                            <ext:BoundField Width="100px" DataField="BARCODE" HeaderText="标本条码" />
                            <ext:BoundField Width="200px" DataField="customername" HeaderText="单位" />
                            <ext:BoundField Width="250px" DataField="testname" HeaderText="体检项目" />
                            <ext:BoundField Width="80px" DataField="STANDARDPRICE" HeaderText="标准价" />
                            <ext:BoundField Width="80px" DataField="GROUPPRICE" HeaderText="分点价" />
                            <ext:BoundField Width="80px" DataField="CONTRACTPRICE" HeaderText="应收价钱" />
                            <ext:BoundField Width="80px" DataField="FINALPRICE" HeaderText="成交价" />
                            <ext:BoundField Width="80px" DataField="createdate" HeaderText="录入时间" DataFormatString="{0:yyyy-MM-dd}" />                            
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
