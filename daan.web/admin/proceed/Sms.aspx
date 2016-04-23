<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sms.aspx.cs" Inherits="daan.web.admin.proceed.Sms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>短信中心</title>
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
                    <ext:ToolbarSeparator runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="查询" Icon="Magnifier" ID="btnSearch" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="导出人员名单" Icon="PageExcel" EnableAjax="false" DisableControlBeforePostBack="false"
                        ID="btnExcel" OnClick="btnExcel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSend" Text="发送短信" Icon="Disk" runat="server" OnClick="btnSend_Click">
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
                                Position="Top" Height="60px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        LabelWidth="90px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="dropDictLab" Resizable="True" runat="server" EnableEdit="true"
                                                        Label="分点" OnSelectedIndexChanged="dropDictLab_SelectedIndexChanged" AutoPostBack="true">
                                                    </ext:DropDownList>                                                    
                                                    <ext:DropDownList ID="DropCustomer" EnableEdit="true" Resizable="True" runat="server"
                                                        Label="单位">
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="ddlStatus" runat="server" Label="状态">
                                                        <ext:ListItem Text="全部" Value="-1" Selected="true" />
                                                        <ext:ListItem Text="已登记" Value="5" />
                                                        <ext:ListItem Text="条码已打印" Value="10" />
                                                        <ext:ListItem Text="待总检" Value="15" />
                                                        <ext:ListItem Text="初步总检" Value="20" />
                                                        <ext:ListItem Text="完成总检" Value="25" />
                                                        <ext:ListItem Text="报告已打印" Value="30" />
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DatePicker ID="datebegin" runat="server" Label="登记时间" DateFormatString="yyyy-MM-dd" />
                                                    <ext:DatePicker ID="dateend" runat="server" DateFormatString="yyyy-MM-dd" Label="至" />    
                                                    <ext:TextBox runat="server" ID="txtOrderNum" EmptyText="体检流水号,姓名" Label="关键字">
                                                    </ext:TextBox>                                                        
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
                                    <ext:Grid ID="gvList" EnableCheckBoxSelect="True" EnableRowNumber="True" runat="server"
                                        AllowPaging="true" ShowHeader="false" DataKeyNames="ordernum,realname,mobile" PageSize="50"
                                        IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="ordernum" HeaderText="体检流水号" Width="105px" />
                                            <ext:BoundField DataField="realname" HeaderText="姓名" />
                                            <ext:BoundField DataField="mobile" HeaderText="手机号码" />
                                            <ext:BoundField DataField="status" HeaderText="标本状态" />
                                            <ext:BoundField DataField="sms" Width="200" HeaderText="已发内容" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Right"
                Width="300px" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" Title="短信模板"
                                Margins="5 0 0 0" Icon="Outline" EnableCollapse="false" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvSmsList" Title="表格" ShowBorder="true" ShowHeader="false" EnableMultiSelect="false"
                                        EnableCheckBoxSelect="true" AutoWidth="true" EnableRowClick="true" runat="server"  AutoScroll="true" DataKeyNames="DictSmsModuleid,SmsTitle" OnRowClick="gvSmsList_RowClick"
                                        EnableRowNumber="true">
                                        <Columns>
                                            <ext:BoundField Width="100px" DataField="DictSmsModuleid" HeaderText="编号" Hidden="true" />
                                            <ext:BoundField Width="100px" DataField="SmsTitle" HeaderText="标题" />
                                            <ext:BoundField Width="200px" DataField="SmsContent" HeaderText="短信模板内容" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Height="200" Title="编辑短信"
                                Margins="0 0 0 0" Icon="Outline" EnableCollapse="false" Layout="Fit"
                                Position="Bottom" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:TextArea ID="txtSmsContent" runat="server">
                                    </ext:TextArea>
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
