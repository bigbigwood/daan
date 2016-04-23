<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProOutWorkerAccept.aspx.cs"
    Inherits="daan.web.admin.proceed.ProOutWorkerAccept" %>

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
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarText Text="确认条码号：">
                    </ext:ToolbarText>
                    <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbEnsureBarcode"
                        Label="确认条码号"   OnTriggerClick="tbEnsureBarcode_TriggerClick">
                    </ext:TriggerBox>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                     <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button runat="server" Text="查询" Icon="Magnifier" ID="btnSearch" OnClick="btnSearch_Click">
                            </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="确认接收" Icon="Disk" runat="server" OnClick="btnSave_Click">
                    </ext:Button>
                    
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Layout="Fit" EnableBackgroundColor="false"
                Title="Center Region">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:ToolbarText Text="分点：" runat="server">
                            </ext:ToolbarText>
                            <ext:DropDownList ID="dropDictLab"  Resizable="True" runat="server" Label="分点" EnableEdit="true" Width="150px">
                            </ext:DropDownList>
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="登记时间段：">
                            </ext:ToolbarText>
                            <ext:DatePicker runat="server" Label="登记时间从" ID="dpFrom" Width="86px">
                            </ext:DatePicker>
                            <ext:DatePicker runat="server" Required="True" CompareControl="dpFrom" CompareOperator="GreaterThanEqual"
                                CompareMessage="结束日期应该大于开始日期" Label="到" ID="dpTo" Width="86px">
                            </ext:DatePicker>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="状态：">
                            </ext:ToolbarText>
                            <ext:DropDownList runat="server" Label="标本接收状态" Resizable="True" ID="dropStatus" Width="60px">
                                <ext:ListItem Selected="True" Text="未接收" Value="10"></ext:ListItem>
                                <ext:ListItem Text="已接收" Value="15"></ext:ListItem>
                                <ext:ListItem Text="全部" Value="-1"></ext:ListItem>
                            </ext:DropDownList>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:ToolbarText Text="关键字：">
                            </ext:ToolbarText>
                            <ext:TextBox runat="server" Label="体检号" Width="110px" ID="tbStrKey" EmptyText="体检号,姓名,条码号"
                                ShowLabel="false">
                            </ext:TextBox>
                           
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="gdOutWorkerAccept" EnableCheckBoxSelect="True" EnableTextSelection="true" EnableRowNumber="True" runat="server" ShowHeader="false" DataKeyNames="ORDERBARCODEID,BARCODE,ORDERNUM">
                        <Columns>
                         <ext:BoundField DataField="ORDERNUM" HeaderText="订单流水号" Width="105px" />
                            <ext:BoundField DataField="BARCODE" HeaderText="条码号" Width="105px" />
                            <ext:BoundField DataField="REALNAME" HeaderText="姓名" />
                            <ext:BoundField DataField="ITEMNAME" HeaderText="样本类型" />
                            <ext:BoundField DataField="EnSureSTATUS" HeaderText="标本接收状态" />
                            <ext:BoundField DataField="USERNAME" HeaderText="确认人" />
                            <ext:BoundField DataField="RECEIVEDATE" HeaderText="确认时间" />
                            <ext:BoundField DataField="LABDEPTNAME" HeaderText="科室" />
                            <ext:BoundField DataField="TESTNAMES" HeaderText="检验项目" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
