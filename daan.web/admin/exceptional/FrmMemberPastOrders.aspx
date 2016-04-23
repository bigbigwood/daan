<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMemberPastOrders.aspx.cs" Inherits="daan.web.admin.exceptional.FrmMemberPastOrders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="Pagemanager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar runat="server" Position="Top">
                <Items>
                    <ext:Label runat="server" Text="姓名：" CssStyle="margin-left:10px"></ext:Label>
                    <ext:TextBox runat="server" EmptyText="输入全名进行关联" ID="txtRealName"></ext:TextBox>
                    <ext:Label runat="server" Text="性别：" CssStyle="margin-left:10px"></ext:Label>
                    <ext:DropDownList runat="server" ID="dpSex" Width="80px">
                        <ext:ListItem Text="男" Value="M" Selected="true" />
                        <ext:ListItem Text="女" Value="F" />
                    </ext:DropDownList>
                     <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="查询" ID="btnSearch" Icon="Magnifier" OnClick="btnSearch_Click"></ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="手动关联" ID="btnGuanLian" ConfirmText="是否确定为同一个会员并进行关联操作吗？"
                     ConfirmTarget="Top" ConfirmTitle="体检系统" ConfirmIcon="Question" Icon="ArrowIn" OnClick="btnGuanLian_Click"></ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region runat="server" ID="reg2" Height="230px" BodyPadding="5" Layout="Fit" Position="Top" ShowBorder="false" ShowHeader="false" Split="true"
             Icon="Outline" EnableSplitTip="true" CollapseMode="Mini" EnableCollapse="true">
                <Items>
                    <ext:GroupPanel runat="server" Layout="Fit" Title="主会员选择(单选)" EnableCollapse="true">
                        <Items>
                            <ext:Grid runat="server" EnableCheckBoxSelect="true" ID="gdPastOrdersList" ShowBorder="false" ShowHeader="false" AutoScroll="true"
                             DataKeyNames="dictmemberid" EnableMultiSelect="false" AutoHeight="true" AutoWidth="true" EnableRowNumber="true">
                                 <Columns>
                                    <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="100px" />
                                    <ext:BoundField DataField="dictmemberid" Width="50px" HeaderText="会员ID" />
                                    <ext:BoundField DataField="realname" Width="50px" HeaderText="姓名" />
                                    <ext:BoundField DataField="sexText" Width="40px" HeaderText="性别" />
                                    <ext:BoundField DataField="age" Width="40px" HeaderText="年龄" />
                                    <ext:BoundField DataField="ismarriedText" Width="40px" HeaderText="婚否" />
                                    <ext:BoundField DataField="mobile" Width="90px" HeaderText="手机" />
                                    <ext:BoundField DataField="idnumber" Width="135px" HeaderText="身份证号" />
                                    <ext:BoundField DataField="addres" DataToolTipField="addres" Width="150px" HeaderText="地址" />
                                    <ext:BoundField DataField="status" Width="70px" HeaderText="状态" />
                                    <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="体检单位" Width="150px" />
                                    <ext:BoundField DataField="enterby" Width="70px" HeaderText="体检人" />
                                    <ext:BoundField DataField="createdate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="体检时间" Width="80px" />
                                    <ext:BoundField DataField="remarks" DataToolTipField="remarks" HeaderText="备注" Width="200px"/>
                                 </Columns>
                            </ext:Grid>
                        </Items>
                    </ext:GroupPanel>
                </Items>
            </ext:Region>
            <ext:Region runat="server" ID="reg3" Layout="Fit" BodyPadding="5" Position="Center" ShowBorder="false" ShowHeader="false">
                <Items>
                     <ext:GroupPanel ID="GroupPanel1" runat="server" Title="从会员选择(多选)" AutoHeight="true" Layout="Fit" EnableCollapse="true">
                        <Items>
                            <ext:Grid runat="server" EnableCheckBoxSelect="true" ID="Grid1" ShowBorder="false" ShowHeader="false" AutoScroll="true"
                             DataKeyNames="ordernum,dictmemberid" EnableMultiSelect="true" AutoHeight="true" AutoWidth="true" EnableRowNumber="true">
                                 <Columns>
                                    <ext:BoundField DataField="ordernum" HeaderText="体检号" Width="100px" />
                                    <ext:BoundField DataField="dictmemberid" Width="50px" HeaderText="会员ID" />
                                    <ext:BoundField DataField="realname" Width="50px" HeaderText="姓名" />
                                    <ext:BoundField DataField="sexText" Width="40px" HeaderText="性别" />
                                    <ext:BoundField DataField="age" Width="40px" HeaderText="年龄" />
                                    <ext:BoundField DataField="ismarriedText" Width="40px" HeaderText="婚否" />
                                    <ext:BoundField DataField="mobile" Width="90px" HeaderText="手机" />
                                    <ext:BoundField DataField="idnumber" Width="135px" HeaderText="身份证号" />
                                    <ext:BoundField DataField="addres" DataToolTipField="addres" Width="150px" HeaderText="地址" />
                                    <ext:BoundField DataField="status" Width="70px" HeaderText="状态" />
                                    <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="体检单位" Width="150px" />
                                    <ext:BoundField DataField="enterby" Width="70px" HeaderText="体检人" />
                                    <ext:BoundField DataField="createdate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="体检时间" Width="80px" />
                                    <ext:BoundField DataField="remarks" DataToolTipField="remarks" HeaderText="备注" Width="200px"/>
                                 </Columns>
                            </ext:Grid>
                        </Items>
                    </ext:GroupPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <script language="javascript" type="text/javascript">
        function onReady() 
        {
            var txtRealNameID = '<%=txtRealName.ClientID %>';
            var txtrealname = Ext.getCmp(txtRealNameID);
            txtrealname.on("specialkey", function (box, e) 
            {
                if (e.getKey() == e.ENTER) 
                {
                    __doPostBack(txtRealNameID, 'specialkey');
                }
            });
        }
    </script>
    </form>
</body>
</html>
