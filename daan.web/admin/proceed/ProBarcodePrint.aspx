<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" EnableEventValidation="false"
    Title="体检集中管理" CodeBehind="ProBarcodePrint.aspx.cs" Inherits="daan.web.admin.proceed.ProBarcodePrint" %>

<%@ Register Src="../../usercontrol/DropInitBasic.ascx" TagName="DropInitBasic" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>体检集中管理</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
</head>
<body>
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
    <form id="form1" runat="server">
    <ext:pagemanager id="PageManager1" autosizepanelid="RegionPanel1" runat="server" />
    <ext:regionpanel id="RegionPanel1" runat="server" showborder="False">
        <Toolbars>
                <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                    <Items>
                        <ext:label runat="server" text="体检号：" showlabel="False" id="lblOrderNum"/>                             
                        <ext:textbox runat="server"  Label="体检流水号" id="tbxOrderNum" cssclass="tbxwidth100" />
                        <ext:label runat="server" text="条码号：" showlabel="False" cssclass="mright_lable"  id="lblBarcode"/>  
                        <ext:textbox runat="server"  id="tbxBarcode" cssclass="tbxwidth100" />
                        <ext:label runat="server" text="姓名：" showlabel="False" cssclass="mright_lable"  id="lblName"/>  
                        <ext:textbox runat="server"  id="tbxName" cssclass="tbxwidth100" />
                        <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                        <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier"  OnClick="btnSearch_Click">
                        </ext:Button>
                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                        </ext:ToolbarSeparator>
                        <ext:Button ID="btnPrint" runat="server" Icon="Printer" Text="打印" OnClick="btnPrint_Click" OnClientClick="test()">
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:grid id="GridBarcodes" title="条码列表" datakeynames="ordernum,Barcode" autoscroll="true" enablecheckboxselect="true"
                        showheader="false"  isdatabasepaging="true" enabletextselection="true"
                         runat="server" autowidth="true" autoheight="true" enablerownumber="true">
                            <Columns>
                                <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="105px"/>
                                <ext:BoundField DataField="Barcode"  HeaderText="条码号" Width="105px"/>
                                <ext:BoundField DataField="realname" HeaderText="姓名" />
                                <ext:BoundField DataField="sex" HeaderText="性别" Width="60px" />
                                <ext:BoundField DataField="age" HeaderText="年龄" Width="60px"/>
                                <ext:BoundField DataField="iscancel" HeaderText="是否作废" Width="60px"/>
                                <ext:BoundField DataField="statusname" HeaderText="订单状态" Width="60px"/>
                                <ext:BoundField DataField="enterby"  HeaderText="录单人"  Width="60px"/>
                                <ext:BoundField DataField="createdate"  HeaderText="登记时间" Width="150px"/>
                                <ext:BoundField DataField="testnames"  HeaderText="条码项目" />
                                <ext:BoundField DataField="remarks"  HeaderText="备注" />
                            </Columns>
                     </ext:grid>
               </Items>
            </ext:Region>
        </Regions>
    </ext:regionpanel>
    <ext:hiddenfield id="hdMac" runat="server">
    </ext:hiddenfield>
    <script type="text/javascript">
        function onReady() {
            //获取mac地址取该用户的本地打印机IP等信息
            document.getElementById("<%=hdMac.ClientID %>").value = document.ActiveXPrint.GetMAC();
        }
    </script>
    </form>
</body>
</html>
