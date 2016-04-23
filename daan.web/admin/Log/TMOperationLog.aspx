<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TMOperationLog.aspx.cs" Inherits="daan.web.admin.Log.TMOperationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></ext:PageManager>
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
                    <ext:Button runat="server" Text="导出到Excel" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false"
                        ID="btnExcel" OnClick="btnExcel_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region4" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="35px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                        LabelWidth="70px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList ID="DropOperaType" runat="server" Label="节点类型">
                                        <ext:ListItem Text="全部" Value="-1" Selected="true" />
                                        <ext:ListItem Text="订单导入" Value="单位批量上传" />
                                        <ext:ListItem Text="采血确认" Value="采血确认" />
                                        <ext:ListItem Text="样本接收" Value="外勤标本接收" />
                                        <ext:ListItem Text="结果录入" Value="检查结果录入" />
                                        <ext:ListItem Text="总检" Value="总检" />
                                        <ext:ListItem Text="报告打印" Value="报告单集中打印" />
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="dropDictLab" Resizable="True" runat="server" EnableEdit="true" Label="分点"  AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged"></ext:DropDownList>
                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" runat="server"></ext:DropDownList>
                                    <ext:DatePicker ID="Dp_BeginDate" runat="server" Label="操作时间" DateFormatString="yyyy-MM-dd" />
                                    <ext:DatePicker ID="Dp_EndDate" runat="server" CompareControl="Dp_BeginDate" CompareMessage="结束时间不能小于开始时间" CompareOperator="GreaterThanEqual" DateFormatString="yyyy-MM-dd" Label="至" />   
                                    <ext:TextBox ID="txtSection" runat="server" Label="区域"></ext:TextBox>
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
                    <ext:Grid ID="gridStat" DataKeyNames="ordercode,dictlabid,dictcustomerid" runat="server" ShowBorder="false" ShowHeader="false" PageSize="20" IsDatabasePaging="true"
                     AllowPaging="true" AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="gridStat_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="ordercode" HeaderText="订单编码" Width="70px" />
                            <ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="分点" Width="180px"/>
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="220px" />
                            <ext:BoundField DataField="section" HeaderText="区域" DataToolTipField="section" Width="80px" />
                            <ext:BoundField DataField="ordertestlst" DataToolTipField="ordertestlst" HeaderText="套餐" Width="200px" />
                            <ext:BoundField DataField="createdate" HeaderText="登记日期" Width="80px" />
                            <ext:BoundField DataField="samplingdate" HeaderText="采样日期" Width="80px" />
                            <ext:BoundField DataField="ordercount" HeaderText="订单数量" Width="60px" />
                            <ext:BoundField DataField="importCount" HeaderText="订单上传" Width="60px" /> 
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
