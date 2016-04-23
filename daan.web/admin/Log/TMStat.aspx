<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TMStat.aspx.cs" Inherits="daan.web.admin.Log.TMStat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TM交付管理信息报表</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>    
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="预查询" ConfirmIcon="Question" ConfirmTarget="Top" ConfirmText="预加载待导出数据，此功能可能需要加载很长时间，是否确定开始加载?"
                     ConfirmTitle="体检系统" Icon="Magnifier" OnClick="btnSearch_Click">
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
                        LabelWidth="90px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList ID="dropDictLab" Resizable="True" runat="server" EnableEdit="true" Label="分点"  AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged"></ext:DropDownList>
                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" runat="server"></ext:DropDownList>
                                    <ext:DatePicker ID="Dp_BeginDate" runat="server" Label="登记时间" DateFormatString="yyyy-MM-dd" />
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
                    <ext:Grid ID="gridStat" DataKeyNames="ordercode,dictlabid,dictcustomerid" Title="统计结果" runat="server" ShowBorder="false" ShowHeader="false" PageSize="20" IsDatabasePaging="true"
                     AllowPaging="true" AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="gridStat_PageIndexChange"> 
                        <Toolbars>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:CheckBox Text="只看本人" runat="server" ID="ckSelf" OnCheckedChanged="ckSelf_CheckedChanged" AutoPostBack="true" CssStyle="padding-left:10px" ></ext:CheckBox>
                                    <ext:Label runat="server" CssStyle="padding-left:10px" Text="结果生成状态："></ext:Label>
                                    <ext:DropDownList runat="server" Width="80px" OnSelectedIndexChanged="dropStatus_SelectedIndexChanged" ID="dropStatus" AutoPostBack="true">
                                        <ext:ListItem Value="-1" Text="全部" />
                                        <ext:ListItem Value="1" Text="已生成" Selected="true"  />
                                        <ext:ListItem Value="0" Text="未生成" />
                                    </ext:DropDownList>
                                    <ext:Label ID="Label1" runat="server" CssStyle="padding-left:20px" Text="预查询条件："></ext:Label>
                                    <ext:DropDownList runat="server" Label="预查询条件" Width="550px" ID="dropSearch" EnableEdit="true" Resizable="true" OnSelectedIndexChanged="dropSearch_SelectedIndexChanged" AutoPostBack="true"></ext:DropDownList>
                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server"></ext:ToolbarFill>    
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                    <ext:Button runat="server" Text="导出到Excel" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false"
                                        ID="btnExcel" OnClick="btnExcel_Click" >
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Toolbars>
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
                            <ext:BoundField DataField="importTime" DataToolTipField="importTime" HeaderText="最后上传时间" Width="110px" /> 
                            <ext:BoundField DataField="resultCount" HeaderText="结果录入" Width="60px" />
                            <ext:BoundField DataField="resultTime" DataToolTipField="resultTime" HeaderText="最后录入时间" Width="110px" />
                            <ext:BoundField DataField="finishedCount" HeaderText="总检" Width="60px" />
                            <ext:BoundField DataField="finishedTime" DataToolTipField="finishedTime" HeaderText="最后总检时间" Width="110px" />
                            <ext:BoundField DataField="printCount" HeaderText="报告打印" Width="60px" />
                            <ext:BoundField DataField="printTime" DataToolTipField="printTime" HeaderText="最后打印时间" Width="110px" />
                        </Columns> 
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
