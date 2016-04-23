<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProDictcustomerModify.aspx.cs" Inherits="daan.web.admin.exceptional.ProDictcustomerModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>批量修改订单信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:Button ID="btnSave" runat="server" Text="保 存" Icon="SystemSaveNew" OnClick="BtnSave_Click" />
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" OnClick="btnClose_Click" Text="关闭" runat="server" Icon="BulletCross" />
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="80px" Position="Top" ShowHeader="False" ShowBorder="False" Split="true">
                <Items>
                    <ext:Form ID="Form3" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="选择分点"
                                        CompareValue="-1" CompareOperator="NotEqual" CompareMessage="全部" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChangeds">
                                    </ext:DropDownList>                                   
                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" runat="server">
                                    </ext:DropDownList>
                                    <ext:TextBox runat="server" Label="部门" ID="txtSection" EmptyText="如果需要统一修改则填写"></ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:CheckBox runat="server" ID="ck1" AutoPostBack="true" OnCheckedChanged="ck1_CheckedChanged" Text="<font color='red'>该批报告统一邮寄信息，请注意：勾选此项后以下三项为必填</font>" ></ext:CheckBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox runat="server" Enabled="false" ID="txtAddress" Label="邮寄地址"></ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" ID="txtRecName" Label="收件人"></ext:TextBox>
                                    <ext:TextBox runat="server" Enabled="false" ID="txtTelphone" Label="联系电话"></ext:TextBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridOrders" Title="订单列表" AutoScroll="true" ShowHeader="false" PageSize="20"
                        IsDatabasePaging="true" EnableTextSelection="true" AllowPaging="true" runat="server"
                        AutoWidth="true" AutoHeight="true" EnableRowNumber="true" OnPageIndexChange="GridOrders_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="110px" />
                            <ext:BoundField DataField="status" HeaderText="状态" Width="80px" />
                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="60px" />
                            <ext:BoundField DataField="age" HeaderText="年龄" Width="40px" />
                            <ext:BoundField DataField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField DataField="ismarried" HeaderText="婚否" Width="40px" />
                            <%--<ext:BoundField DataField="cancel" HeaderText="是否作废" Width="60px" />--%>
                            <ext:BoundField DataField="createdate" HeaderText="登记时间" Width="85px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="120px" />
                            <ext:BoundField DataField="section" DataToolTipField="section" HeaderText="部门" Width="150px" />
                            <ext:BoundField DataField="enterby" HeaderText="录单人" Width="50px" />
                            <ext:BoundField DataField="postaddress" DataToolTipField="postaddress" HeaderText="邮寄地址" Width="120px" />
                            <ext:BoundField DataField="recipient" HeaderText="收件人" Width="50px" />
                            <ext:BoundField DataField="contactnumber" HeaderText="联系电话" Width="100px" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:HiddenField runat="server" ID="hidOrderNums"></ext:HiddenField>
    </form>
</body>
</html>
