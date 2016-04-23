<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillAdjustPrice.aspx.cs"
    Inherits="daan.web.admin.bill.BillAdjustPrice" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>调整价钱</title>
    <style>
        .right
        {
            margin-right: 5px;
            float: left;
            margin-top: 5px;
            text-align: right;
        }
        .force
        {
            border-style: none;
            border-color: White;
            width: auto;
            height: 100%;
        }
        .table
        {
            border: 0px solid #99BBE8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" />
    <ext:Panel ID="pnl" runat="server" ShowHeader="false" Height="352" Width="565" Layout="Column"
        BodyPadding="5" ShowBorder="false" EnableBackgroundColor="false">
        <Items>
            <ext:ContentPanel ID="contentpanel" runat="server" ShowHeader="false" Width="550"
                ShowBorder="false">
            <table style="height: 200; width: 100%;" class="table" ><tr><td 
                    class="right">
                    <ext:Label runat="server" Text="体 检 号:" ID="lblOrdernum"></ext:Label>
                    </td>
                    <td>
                    <ext:TextBox runat="server" ID="tbxOrdernum" Enabled="False"></ext:TextBox>
                    </td>
                    <td class="right">
                    <ext:Label runat="server" Text="体检单位:" ColumnWidth="80" ID="lblCustomer"></ext:Label>
                    </td>
                    <td style="width: 170px; text-align: left;">
                    <ext:TextBox runat="server" ID="tbxCustomer" Enabled="False"></ext:TextBox>
                    </td>
                </tr>
                <tr><td class="right">
                    <ext:Label runat="server" 
                        Text="姓&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;名:" ID="lblName"></ext:Label>
                    </td>
                    <td>
                    <ext:TextBox runat="server" ID="tbxName" Enabled="False"></ext:TextBox>
                    </td>
                    <td class="right">
                    <ext:Label runat="server" 
                            Text="性&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;别:" ID="lblSex"></ext:Label>
                    </td>
                    <td>
                    <ext:TextBox runat="server" ID="tbxSex" Enabled="False"></ext:TextBox>
                    </td>
                </tr>
                <tr><td class="right">
                    <ext:Label runat="server" 
                        Text="年&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;龄:" ID="lblAge"></ext:Label>
                    </td>
                    <td>
                    <ext:TextBox runat="server" ID="tbxAge" Enabled="False" ></ext:TextBox>
                    </td>
                    <td class="right">
                    <ext:Label runat="server" Text="登记时间:" ID="lblEnterdate"></ext:Label>
                    </td>
                    <td>
                    <ext:TextBox runat="server" ID="tbxEnterdate" Enabled="False"></ext:TextBox>
                    </td>
                </tr>
            </table>
            </ext:ContentPanel>
            <ext:Panel runat="server" ShowHeader="False" Width="555" ShowBorder="false" Layout="Row"
                ID="panel2">
                <Items>
                    <ext:Grid runat="server" Title="表格" ID="gvList" ShowBorder="true" ShowHeader="false"
                        Height="240" AutoWidth="true" AutoScroll="true" EnableRowNumber="true" EnableTextSelection="true">
                        <Columns>
                            <ext:BoundField DataField="Billdetailid" Hidden="True" ColumnID="ct0" HeaderText="编号">
                            </ext:BoundField>
                            <ext:BoundField DataField="Testname" ColumnID="ct1" HeaderText="项目名称" Width="270px">
                            </ext:BoundField>
                            <ext:BoundField DataField="Standardprice" ColumnID="ct2" HeaderText="标准收费" Width="60px">
                            </ext:BoundField>
                            <ext:BoundField DataField="Contractprice" ColumnID="ct3" HeaderText="应收金额" Width="60px">
                            </ext:BoundField>
                            <ext:TemplateField ColumnID="ct4" HeaderText="实收金额" Width="60px">
                                <ItemTemplate>
                                    <%--  onkeydown="if(isNaN(value))execCommand('undo')" onkeyup="if(isNaN(value))execCommand('undo')"--%>
                                    <asp:TextBox ID="tbxFinalprice" runat="server" Text='<%# Eval("Finalprice") %>' ToolTip='<%# Eval("Finalprice") %>'
                                        CssClass="force" onpropertychange='if(value == "") {alert("实收金额不能为空！"); this.value = this.title;}else{TextValueShow(this);}'></asp:TextBox>
                                </ItemTemplate>
                            </ext:TemplateField>
                            <ext:BoundField DataField="Status" ColumnID="ct5" HeaderText="状态" Width="60px"></ext:BoundField>
                        </Columns>
                        <Toolbars>
                            <ext:Toolbar runat="server" ID="Toolbar1">
                                <Items>
                                    <ext:ToolbarFill runat="server" ID="ToolbarFill1">
                                    </ext:ToolbarFill>
                                    <ext:Button runat="server" Icon="Add" Text="保存" CssClass="inline" ID="btnSave" OnClick="btnSave_Click">
                                    </ext:Button>
                                    <ext:Button runat="server" Icon="Cancel" Text="取消" CssClass="inline" ID="btnCanCel">
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Toolbars>
                    </ext:Grid>
                    <ext:ContentPanel runat="server" ShowHeader="False" ShowBorder="False" ID="contentpanel1">
                    <table style=" width: 100%;" class="table">
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="合计金额:" CssClass="right" ID="lblTotalprice"></ext:Label>
                            </td>
                            <td>
                                <ext:TextBox runat="server" Readonly="True" CssClass="right" ID="tbxTotalprice" Enabled="False"></ext:TextBox>
                            </td>
                            <td>
                                <ext:Label runat="server" Text="修改后合计金额:" CssClass="right" ID="lblModifytotalprice"></ext:Label>
                            </td>
                            <td>
                                <ext:TextBox runat="server" Readonly="True" CssClass="right" ID="tbxModifytotalprice" Enabled="False"></ext:TextBox>
                           </td>
                        </tr>
                    </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
    </form>
    <script type="text/javascript">
        function TextValueShow(obj) {
            //obj.replace(/\s+/g, "");
            var oldvalue = obj.title;
            var replacestr = obj.value.replace(/\s+/g, "").replace("-", "")
            if (obj.value != replacestr) {
                obj.value = replacestr;
            }
            var newvalue = obj.value;
            if (isNaN(newvalue)) {
                var a = newvalue.substr(0, (newvalue.length - 1));
                obj.value = newvalue = a;
            }
            else {
                if (newvalue.length > 1 && (newvalue.substr(0, 1) == "0" || newvalue.substr(0, 1) == "-") && newvalue.substr(0, 2) != "0.") {
                    obj.value = newvalue.substr(1, (newvalue.length - 1));
                } else {
                    var tbxModifytotalprice = Ext.getCmp('<%= tbxModifytotalprice.ClientID %>');
                    var val = parseFloat(tbxModifytotalprice.getValue()) + (newvalue - oldvalue);
                    obj.title = newvalue;
                    tbxModifytotalprice.setValue(val);
                }
            } //&& newvalue.length > 2
        }
    </script>
</body>
</html>
