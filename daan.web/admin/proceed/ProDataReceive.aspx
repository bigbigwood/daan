<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProDataReceive.aspx.cs"
    Inherits="daan.web.admin.proceed.ProDataReceive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据接收</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        AjaxLoadingType="Mask" HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar runat="server" ID="Toolbar2">
                <Items>
                    <ext:ToolbarFill runat="server" ID="ToolbarFill1">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Icon="Magnifier" Text="查询" CssClass="inlineButton" ID="btnSearch"
                        OnClick="btnSearch_Click" ValidateForms="Form3">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" IconUrl="~/icon/building_key.png" Text="查看结果" CssClass="inlineButton"
                        ID="btnSearchResult" OnClick="btnSearchResult_Click">
                    </ext:Button>                    
                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" IconUrl="~/icon/database_go.png" Text="数据接收" CssClass="inlineButton"
                        OnClick="btnReceive_Click" ID="btnReceive">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="regiontop" Layout="Fit" Height="80px" Position="Top" ShowHeader="False"
                ShowBorder="False" Split="true">
                <Items>
                    <ext:Form ID="Form3" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        Title="Form" ShowBorder="false" ShowHeader="false" LabelWidth="80" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="dropLab" Resizable="True" AutoPostBack="true"
                                        EnableEdit="true" OnSelectedIndexChanged="dropLab_SelectedIndexChanged" Label="分点">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" Resizable="True" ID="dropCustomer"
                                        EnableEdit="true" Label="体检单位">
                                    </ext:DropDownList>
                                    <ext:DatePicker runat="server" ID="dtpStart" Label="体检日期" Required="true" RequiredMessage="请选择开始日期">
                                    </ext:DatePicker>
                                    <ext:DatePicker runat="server" ID="dtpEnd" Label="到" Required="true" RequiredMessage="请选择结束日期">
                                    </ext:DatePicker>                                    
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items> 
                                    <ext:DropDownList runat="server" Resizable="True" ID="dropIOLIS" EnableEdit="true" Label="接收状态">
                                        <ext:ListItem Selected="true" Text="全部" Value="-1" />
                                        <ext:ListItem Text="未接收" Value="0" />
                                        <ext:ListItem Text="部分接收" Value="1" />
                                        <ext:ListItem Text="接收完成" Value="2" />
                                        <ext:ListItem Text="接收失败" Value="3" />
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" Resizable="true" ID="dropStatus" Label="状态"></ext:DropDownList>
                                    <ext:TextBox ID="tbxName" runat="server" Label="姓名"></ext:TextBox>
                                    <ext:TextBox runat="server" ID="tbxordernum" Label="体检号/条码号"></ext:TextBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="region11" ShowHeader="false" Layout="Fit" Position="Center" ShowBorder="false"
                runat="server">
                <Items>
                    <ext:RegionPanel ID="regionpanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="regioncenter" ShowHeader="false" Layout="Fit" Position="Center" runat="server"
                                ShowBorder="false">
                                <Items>
                                    <ext:Grid runat="server" AllowPaging="True" PageSize="20" EnableRowNumber="True" DataKeyNames="ordernum,statusvalue"
                                        EnableTextSelection="true" EnableMultiSelect="true" EnableCheckBoxSelect="true" IsDatabasePaging="true"
                                        Title="表格" ShowHeader="False" AutoWidth="True" Height="535px" ID="gvList" OnPageIndexChange="gvList_PageIndexChange"
                                        OnRowDataBound="gvList_RowDataBound">
                                        <Columns>
                                            <ext:TemplateField HeaderText="B超" Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpacs" runat="server" Text='<%# Eval("iopacs") %>' ForeColor="Red"></asp:Label>
                                                </ItemTemplate>
                                            </ext:TemplateField>
                                            <ext:TemplateField HeaderText="LIS" Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllis" runat="server" Text='<%# Eval("iolis") %>' ForeColor="Red"></asp:Label>
                                                </ItemTemplate>
                                            </ext:TemplateField>
                                            <ext:BoundField DataField="ordernum" HeaderText="体检编号" Width="100px"></ext:BoundField>
                                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="90px"></ext:BoundField>
                                            <ext:BoundField DataField="sex" HeaderText="性别" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="age" HeaderText="年龄" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="ismarried" HeaderText="婚姻状况" Width="60px"></ext:BoundField>
                                            <ext:BoundField DataField="customername" HeaderText="体检单位" Width="250px"></ext:BoundField>
                                            <ext:BoundField DataField="createdate" HeaderText="体检时间" Width="100px" DataFormatString="{0:yyyy-MM-dd}"></ext:BoundField>
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>                           
                            <ext:Region ID="regionbutton" ShowHeader="false" Layout="Fit" Height="20" ShowBorder="false"
                                Position="Bottom" runat="server">
                                <Items>
                                    <ext:Label ID="lblMsg" runat="server" CssClass="fontcolor" Text="说明：-未接收；√成功接收；○部分接受；×接收失败">
                                    </ext:Label>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinDataReceive" Hidden="true" EnableIFrame="true" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="900" Height="450">
    </ext:Window>
    </form>
</body>
</html>
