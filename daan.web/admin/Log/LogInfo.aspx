<%@ Page Title="操作日志查询" Language="C#" AutoEventWireup="true" CodeBehind="LogInfo.aspx.cs"
    Inherits="daan.web.admin.Log.LogInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" EnableBackgroundColor="false">
        <Regions>
            <ext:Region ID="Region2" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="35px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="60px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DropDownList runat="server" Label="表名" ID="Drop_table" EnableEdit="true" Resizable="True" ShowLabel="true">
                                    </ext:DropDownList>
                                    <ext:TextBox runat="server" Label="操作内容" ID="tbxCode" ShowLabel="true">
                                    </ext:TextBox>
                                    <ext:DatePicker runat="server" AutoPostBack="true" Label="开始日期" EmptyText="请选择日期"
                                        ShowLabel="true" ID="Dp_BinginDate">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" AutoPostBack="true" Label="结束日期" EmptyText="请选择日期"
                                        ID="Dp_EndDate">
                                    </ext:DatePicker>
                                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" ValidateForms="Form2"
                                        OnClick="btnSearch_Click">
                                    </ext:Button>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Region3" Title="列表" ShowHeader="false" Layout="Fit" Position="Center" EnableBackgroundColor="false"
                runat="server">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="false" ShowHeader="false" EnableTextSelection="true"
                        EnableRowNumber="true" AutoHeight="true" AllowPaging="true" runat="server" Width="800px"
                        DataKeyNames="Maintenancelogid,Tablename" IsDatabasePaging="true"
                        OnPageIndexChange="gvList_PageIndexChange">
                        <Columns>
                            <ext:BoundField Width="150px" DataField="BackTablename" DataFormatString="{0}" HeaderText="操作表名"
                                DataToolTipField="BackTablename" />
                            <ext:BoundField Width="60px" DataField="Operationtype" DataFormatString="{0}" HeaderText="操作类型"
                                DataToolTipField="Operationtype" />
                            <ext:BoundField Width="250px" DataField="Operation" DataFormatString="{0}" HeaderText="操作内容"
                                DataToolTipField="Operation" />
                            <ext:BoundField Width="100px" DataField="Columnname" DataFormatString="{0}" HeaderText="列名"
                                DataToolTipField="Columnname" />
                            <ext:BoundField Width="100px" DataField="Username" DataFormatString="{0}" HeaderText="操作人"
                                DataToolTipField="Username" />
                            <ext:BoundField Width="150px" DataField="Operatedate" DataFormatString="{0}" HeaderText="操作时间"
                                DataToolTipField="Operatedate" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
