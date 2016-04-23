<%@ Page Language="C#" AutoEventWireup="true" Title="耗材管理" EnableViewStateMac="false"
    EnableEventValidation="false" CodeBehind="HpvManage.aspx.cs" Inherits="daan.web.admin.proceed.HpvManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" EnableBackgroundColor="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Icon="Magnifier"
                        Text="查询">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="60px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" EnableBackgroundColor="false" BodyPadding="5 0 0 0"
                        LabelWidth="100px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="分点"
                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="Dictcustomerid" Resizable="True" Label="选择单位"
                                        ShowLabel="true">
                                    </ext:DropDownList>
                                    <ext:DatePicker runat="server" ID="Start" Label="选择日期">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" ID="End" Label="至">
                                    </ext:DatePicker>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:DropDownList runat="server" ID="Dicttestitemid" Resizable="True" Label="选择套餐"
                                        ShowLabel="true">
                                        <ext:ListItem Selected="true" Text="全部" Value="-1" />
                                    </ext:DropDownList>
                                    <ext:TextBox ID="Instrumentsbarcode" runat="server" Label="耗材条码">
                                    </ext:TextBox>
                                    <ext:TextBox ID="Barcode" runat="server" Label="样本条码">
                                    </ext:TextBox>
                                    <ext:Label ID="lbmsg" runat="server"></ext:Label>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Region3" Title="列表" ShowHeader="false" Layout="Fit" Position="Center"
                EnableBackgroundColor="false" runat="server">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="false" ShowHeader="false"
                        EnableTextSelection="true" EnableRowNumber="true" AutoHeight="true" AllowPaging="true"
                        OnPageIndexChange="gvList_PageIndexChange" IsDatabasePaging="true" runat="server"
                        Width="800px" DataKeyNames="Hpvinstrumentsid,Barcode" OnRowCommand="gvList_RowCommand">
                        <Columns>
                            <ext:BoundField Width="100px" DataField="Instrumentsbarcode" HeaderText="耗材条码" />
                            <ext:BoundField Width="100px" DataField="Barcode" HeaderText="样本条码" />
                            <ext:BoundField Width="200px" DataField="Customername" HeaderText="客户名称" DataToolTipField="Customername" />
                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="检测套餐" DataToolTipField="Testname" />
                            <ext:BoundField Width="100px" DataField="Instenterby" HeaderText="初始化操作人" />
                            <ext:BoundField Width="100px" DataField="Instcreatedate" HeaderText="耗材激活时间" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField Width="100px" DataField="Barcodecreatedate" HeaderText="标本激活时间" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:LinkButtonField CommandArgument="Hpvinstrumentsid" Width="60px" Icon="Delete"
                                ConfirmText="你确定要删除吗？" ColumnID="LbDictcustomerid" HeaderText="删除" CommandName="delete"
                                ToolTip="删除" />
                            <ext:LinkButtonField Width="60px" Icon="Cancel" CommandName="Cancel" ConfirmText="你确定要要取消关联吗？"
                                HeaderText="取消关联" ToolTip="取消关联" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
