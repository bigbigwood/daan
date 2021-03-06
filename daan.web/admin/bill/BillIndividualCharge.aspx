﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillIndividualCharge.aspx.cs"
    EnableViewStateMac="false" Inherits="daan.web.admin.bill.BillIndividualCharge" %>

<%@ Register Assembly="FastReport.Web, Version=1.7.1.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c"
    Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>个人收费</title>
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
    <style>
        .right
        {
            width: 100px;
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
            border: 1px solid #99BBE8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" id="ActiveXPrint" name="ActiveXPrint"
        style="display: none">
    </object>
    <ext:PageManager ID="PageManager1" runat="server" />
    <ext:Panel ID="pnl1" runat="server" ShowBorder="false" BodyPadding="5" ShowHeader="false" Layout="Row"
        Height="445">
        <Items>
            <ext:ContentPanel ID="contentPanel1" runat="server" ShowHeader="false" ShowBorder="false">
                <table style="height: 200; width: 78%;">
            <tr>
                <td class="right">
                    <ext:Label ID="lblOrdernum" runat="server" Text="体&nbsp;&nbsp;&nbsp;&nbsp;检&nbsp;&nbsp;&nbsp;&nbsp;号:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxOrdernum" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblCustomer" runat="server" Text="体检单位:" ColumnWidth="80">
                    </ext:Label>
                </td>
                <td style="width: 170px; text-align: left;">
                    <ext:TextBox ID="tbxCustomer" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    <ext:Label ID="lblName" runat="server" Text="姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxName" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblSex" runat="server" Text="性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxSex" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    <ext:Label ID="lblAge" runat="server" Text="年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxAge" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblEnterdate" runat="server" Text="登记时间:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxEnterdate" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
        </table>
        <hr style="border-bottom-style:none; border-bottom-color:Gray;" />
        <table  style="height: 190; width: 100%;">
            <tr>
                <td class="right">
                    <ext:Label ID="lblDiscount" runat="server" Text="折&nbsp;&nbsp;&nbsp;&nbsp;扣&nbsp;&nbsp;&nbsp;&nbsp;率:"></ext:Label>
                </td>
                <td>
                <table><tr><td> <ext:NumberBox ID="tbDiscount" runat="server" EmptyText="请输入0～1之间的数字" FocusOnPageLoad="true" MaxLength="5" MinValue="0" MaxValue="1" 
                 ShowRedStar="true" NoNegative="True" CompareMessage="请输入0～1之间的数字"  ></ext:NumberBox></td><td><ext:Button ID="BtnShow" Text="刷新价格" Icon="ZoomIn" runat="server" CssStyle="margin-right: 10px; float: left; margin-top:10px;"
                                OnClick="BtnShow_Click"></ext:Button></td><td><%-- %（请输入0～100之间的数字，如：输入90，就是9折[90%]）--%><font color=red><%--请输入0～1之间的数字--%></font></td></tr></table>
             
                </td>
            </tr>
            <tr>
                 <td class="right">
                     <ext:Label ID="lblRemark" runat="server" Text="备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注:"></ext:Label>
                </td>
                <td>
                    <ext:TextArea ID="txtareaRemark" runat="server" Height="30" Width="300"></ext:TextArea>
                </td>
            </tr>
        </table>
            </ext:ContentPanel>
            <ext:Grid ID="gvList" Title="表格" ShowBorder="true" ShowHeader="false" runat="server"
                Height="280" AutoWidth="true" AutoScroll="true" EnableRowNumber="true" EnableTextSelection="true">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:Button ID="btnDelete" Text="删除明细" Icon="Delete" runat="server" CssStyle="margin-right: 10px; float: left;"
                                OnClick="btnDelete_Click">
                            </ext:Button>
                            <ext:Button ID="btnSave" Text="划价" Icon="Add" runat="server" CssStyle="margin-right: 10px; float: left;"
                                OnClick="btnSave_Click">
                            </ext:Button>
                            <ext:Button ID="btnPrint" Text="打印" Icon="Printer" runat="server" Enabled="false"
                                CssStyle="margin-right: 10px; float: left;" OnClick="btnPrint_Click"
                                OnClientClick="test()">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Columns>
                    <ext:BoundField DataField="Ordergrouptestid" HeaderText="编号" Hidden="true" />
                    <ext:BoundField DataField="Testname" HeaderText="项目名称" Width="330" />
                    <ext:BoundField DataField="Standardprice" HeaderText="标准收费" Width="70" />
                    <ext:BoundField DataField="Contractprice" HeaderText="应收金额" Width="70" />
                    <ext:TemplateField HeaderText="实收金额">
                        <ItemTemplate>
                            <asp:TextBox ID="tbxFinalprice" runat="server" Text='<%# Eval("Finalprice") %>' CssClass="force"
                                ToolTip='<%# Eval("Finalprice") %>' onpropertychange='if(value =="") {alert("实收金额不能为空！"); this.value = this.title;}else{TextValueShow(this);}'></asp:TextBox>
                            <asp:HiddenField ID="HF_Finalprice" runat="server" Value='<%# Eval("Standardprice") %>' />
                        </ItemTemplate>
                    </ext:TemplateField>
                    <ext:BoundField DataField="Dicttestitemid" HeaderText="项目编号" Hidden="true" />
                </Columns>
            </ext:Grid>
        </Items>
    </ext:Panel>
    <table width="100%">
        <tr>
            <td class="right" style="height: 35px;">
                <ext:Label ID="lblTotalprice" runat="server" Text="合计金额:">
                </ext:Label>
            </td>
            <td>
                <ext:TextBox ID="tbxTotalprice" runat="server" Enabled="false">
                </ext:TextBox>
            </td>
            <td style="width: 160; text-align: right;">
                <ext:Label ID="lblModifytotalprice" runat="server" Text="修改后合计金额:">
                </ext:Label>
            </td>
            <td>
                <ext:TextBox ID="tbxModifytotalprice" runat="server"  />
            </td>
        </tr>
    </table>
    <!--获取mac地址取该用户的本地打印机IP等信息-->
    <ext:TextBox ID="hdMac" runat="server" CssStyle="display: none" />
    <script type="text/javascript">
        function onReady() {
            //获取mac地址取该用户的本地打印机IP等信息
            Ext.getCmp("<%=hdMac.ClientID %>").setValue(document.ActiveXPrint.GetMAC());
        }
        function TextValueShow(obj) {
            var oldvalue = obj.title;

            var replacestr = obj.value.replace(/\s+/g, "").replace("-", "")
            if (obj.value != replacestr) {
                obj.value = replacestr;
            }
            var newvalue = obj.value;
            if (isNaN(newvalue)) { var a = newvalue.substr(0, (newvalue.length - 1)); obj.value = newvalue = a; } else {
                if (newvalue.length > 1 &&  (newvalue.substr(0, 1) == "0" ||newvalue.substr(0, 1) == "-")  && newvalue.substr(0, 2) != "0.") {
                    obj.value = newvalue.substr(1, (newvalue.length - 1));
                } else {
                    var Modifytotalprice = Ext.getCmp("<%=tbxModifytotalprice.ClientID %>");
                    var val = parseFloat(Modifytotalprice.getValue()) + (newvalue - oldvalue);

                    obj.title = obj.value;
                    Modifytotalprice.setValue(val);
                }
            }
        }
    </script>
    </form>
</body>
</html>
