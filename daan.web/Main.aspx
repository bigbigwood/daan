<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="daan.web.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <link href="style/default.css" rel="stylesheet" type="text/css" />
    <link href="style/main.css" rel="stylesheet" type="text/css" />
    <script src="js/webpagecontrol.js" type="text/javascript"></script>
     <script type="text/javascript">
         function addTab(id, title, url) {
             var treeMenu = Ext.getCmp(IDS.treeMenu);

             var node = new Object();
             node.id = id;
             node.text = title;
             var attr = new Object();
             attr.href = url;
             node.attributes= attr;
             addExampleTab(node);
         }
         function app() {
             alert("成功！");
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnExpandAll" IconUrl="~/images/expand-all.gif" Text="展开全部" EnablePostBack="false"
                        runat="server">
                    </ext:Button>
                    <ext:Button ID="btnCollapseAll" IconUrl="~/images/collapse-all.gif" Text="折叠全部" EnablePostBack="false"
                        runat="server">
                    </ext:Button>
                    <ext:Button ID="btnClearCache" runat="server" Text="清空缓存" Icon="CutRed" OnClick="btnClearCache_Click">
                    </ext:Button>  
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:Label ID="label" Text="当前用户：" runat="server">
                    </ext:Label>
                    <ext:Label ID="lblCurrentUser" Text="" Width="80px" CssStyle="text-align: left;color:Blue;"
                        runat="server">
                    </ext:Label>
                    <ext:Button ID="btnEditPassword" runat="server"  Text="修改密码" Icon="DoorOut" OnClientClick="addTab('-1', '修改密码', 'EditPassword.aspx')" >                
                    </ext:Button>
                    <ext:Button ID="btnExit" runat="server" Text="退出登录" Icon="UserCross" OnClick="btnExit_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="170px"
                Margins="0 0 0 0" ShowHeader="true" Title="导航" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>                    
                    <ext:Tree runat="server" EnableArrows="true" ShowBorder="false" ShowHeader="false"
                        EnableIcons="false" AutoScroll="true" ID="treeMenu">
                    </ext:Tree>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                        <Tabs>
                            <ext:Tab Title="逾期待办" Layout="Fit" Icon="House" runat="server">
                                <Items>
                                    <%--<ext:ContentPanel ShowBorder="false" BodyPadding="10px" ShowHeader="false" AutoScroll="true"
                                        CssClass="intro" runat="server">
                                        <img src="images/bar-right.png" 
                                        id="welcome" alt="" style="position: 
                                            absolute;left: 0px;filter: alpha(opacity=80);
                                            -moz-opacity: 0.8;opacity: 0.8;
                                            z-index: 100000;width:100%;height:100%;" /> 
                                        <label id="lab_msg" style="position: absolute;left:
                                             0px;z-index: 900000;width:100%; font-size:16px;
                                             font-weight:bold; color:Red;" ></label>
                                    </ext:ContentPanel>--%>

                                    <ext:RegionPanel ID="aa" ShowBorder="false" runat="server">
                                        <Regions>
                                            <ext:Region Layout="Fit" Position="Center" ShowBorder="false" ShowHeader="false">
                                                <Items>
                                                    <ext:Grid runat="server" ID="GridOrders" Title="逾期未办列表" AutoScroll="true"
                                                     DataKeyNames="" EnableCheckBoxSelect="false" ShowHeader="false" AllowPaging="true"
                                                     IsDatabasePaging="true" PageSize="20" EnableTextSelection="true" AutoHeight="true" AutoWidth="true"
                                                     EnableRowNumber="true" OnPageIndexChange="GridOrders_PageIndexChange">
                                                        <Columns>
                                                            <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="110px" />
                                                            <ext:BoundField DataField="basicname" HeaderText="状态" Width="80px" />
                                                            <ext:BoundField DataField="realname" HeaderText="姓名" Width="60px" />
                                                            <ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="分点" Width="200px"/>
                                                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位名称" Width="230px"/>
                                                            <ext:BoundField DataField="ordertestlst" DataToolTipField="ordertestlst" HeaderText="检查项目" Width="250px" />
                                                            <ext:BoundField DataField="enterdate" HeaderText="登记时间" Width="80px" DataFormatString="{0:yyyy-MM-dd}" />
                                                            <ext:BoundField DataField="samplingdate" HeaderText="采样日期" Width="80px" DataFormatString="{0:yyyy-MM-dd}" />
                                                            <ext:BoundField DataField="enterby" HeaderText="录单人" ExpandUnusedSpace="true" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                        </Tabs>
                    </ext:TabStrip>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/menu.xml"></asp:XmlDataSource>
    </form>
    <script type="text/javascript">
        var IDS = {
            treeMenu: '<%= treeMenu.ClientID %>',
            btnExpandAll: '<%= btnExpandAll.ClientID %>',
            btnCollapseAll: '<%= btnCollapseAll.ClientID %>',
            //           
            mainTabStrip: '<%= mainTabStrip.ClientID %>'
        };
    </script>
    <script src="./js/default.js" type="text/javascript"></script>
</body>
</html>
