<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProCollectBloodVerify.aspx.cs"
    Inherits="daan.web.admin.proceed.CollectBloodVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form4" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server">
    </ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" EnableBackgroundColor="false" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarText Text="确认条码号" runat="server">
                    </ext:ToolbarText>
                    <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbEnsureBarcode"
                        Label="确认条码号" OnTriggerClick="tbEnsureBarcode_TriggerClick">
                    </ext:TriggerBox>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" Icon="Magnifier">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="确认采样" Icon="Disk" runat="server" OnClick="btnSave_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Title="Center Region" Position="Center" ShowHeader="false" runat="server"
                Layout="Fit">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:ToolbarText Text="分点：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList ID="dropDictLab" ShowLabel="false" Resizable="True" runat="server"
                                Label="分点" EnableEdit="true" OnSelectedIndexChanged="dropDictLab_SelectedIndexChanged"
                                AutoPostBack="true" Width="150px">
                            </ext:DropDownList>
                            <ext:DropDownList runat="server" Resizable="True" Width="120px" ID="dropCustomer"
                                EnableEdit="true">
                            </ext:DropDownList>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="登记时间段：" runat="server">
                            </ext:ToolbarText>
                            <ext:DatePicker runat="server" Label="登记时间从" ID="dpFrom" ShowLabel="false" Width="86px">
                            </ext:DatePicker>
                            <ext:DatePicker runat="server" Required="True" CompareControl="dpFrom" Width="86px"
                                CompareOperator="GreaterThanEqual" CompareMessage="结束日期应该大于开始日期" Label="到" ID="dpTo">
                            </ext:DatePicker>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="状态：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList runat="server" Label="采样状态" ID="dropStatus" ShowLabel="false" Resizable="True"
                                Width="60px">
                                <ext:ListItem Selected="True" Text="未采血" Value="5"></ext:ListItem>
                                <ext:ListItem Text="已采血" Value="10"></ext:ListItem>
                                <ext:ListItem Text="全部" Value="-1"></ext:ListItem>
                            </ext:DropDownList>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="关键字：" runat="server">
                            </ext:ToolbarText>
                            <ext:TextBox runat="server" Label="体检号" ID="tbStrKey" EmptyText="体检号,姓名,条码号" ShowLabel="false"
                                Width="110px">
                            </ext:TextBox>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="gdCollectBlood" EnableCheckBoxSelect="True" EnableRowNumber="True"
                        EnableTextSelection="true" runat="server" ShowHeader="false" DataKeyNames="ORDERBARCODEID,BARCODE,ORDERNUM">
                        <Columns>
                            <ext:BoundField DataField="Ordernum" HeaderText="订单流水号" Width="105px" />
                            <ext:BoundField DataField="Barcode" HeaderText="条码号" Width="105px" />
                            <ext:BoundField DataField="Realname" HeaderText="姓名" />
                            <ext:BoundField DataField="Itemname" HeaderText="样本类型" />
                            <ext:BoundField DataField="Ensurestatus" HeaderText="采样状态" />
                            <ext:BoundField DataField="Username" HeaderText="确认人" />
                            <ext:BoundField DataField="Collectdate" HeaderText="确认时间" />
                            <ext:BoundField DataField="Labdeptname" HeaderText="科室" />
                            <ext:BoundField DataField="Testnames" HeaderText="检验项目" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
