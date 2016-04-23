<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" EnableEventValidation="false" Title="异常报告"
CodeBehind="ProExceptionReport.aspx.cs" Inherits="daan.web.admin.proceed.ProExceptionReport" %>

<%@ Register Src="../../usercontrol/DropInitBasic.ascx" TagName="DropInitBasic" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>异常报告</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/ActiveXPrint.js"></script>
</head>
<body>
    <object classid="clsid:E6E802E0-8429-4B65-9927-DEE9CD6E422D" width="742" height="0"
        id="ActiveXPrint" name="ActiveXPrint">
    </object>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"/>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier"  OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSeeDetail" runat="server" Icon="PageWhiteStack" Text="查看订单" OnClick="btnSeeDetail_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="作废" ConfirmText="是否作废选中订单？"
                        ConfirmTitle="体检系统" ConfirmIcon="Question" ConfirmTarget="Top" OnClick="btnCancel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnValid" Text="取消作废" runat="server" Icon="ApplicationOsxDelete"
                        OnClick="btnValid_Click" ConfirmText="是否取消作废选中订单？">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="90px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                    <ext:ContentPanel ID="ContentPanel1" ShowBorder="false" ShowHeader="false" runat="server">
                      <table width="100%" border=0 >
                            <tr>
                                <td style="height:25px">
                                    <ext:Label runat="server" text="选择分点：" cssclass="mright_lable" id="lblLabName"></ext:Label>
                                </td>
                                <td >
                                     <ext:DropDownList ID="DropDictLab"  CompareType="String" Resizable="True"
                                        CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择分点！" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged" Width="128">
                                    </ext:DropDownList>
                                </td>
                                <td > 
                                    <ext:label runat="server" text="体 检 号：" showlabel="False" cssclass="mright_lable"  id="lblOrderNum"/>
                                </td>
                                <td >
                                    <ext:textbox runat="server"  id="tbxOrderNum" EmptyText="此条件忽略其他条件" cssclass="tbxwidth100" />
                                </td>
                                <td >
                                    <ext:label runat="server" text="登记时间："  showlabel="False" cssclass="mright_lable" id="lblRegisterDate"/>
                                </td>
                                <td style="width:140px">
                                    <ext:datepicker id="datebegin" runat="server" width="120px"   dateformatstring="yyyy-MM-dd" />
                                </td>
                                <td style="width:120px; ">
                                    <ext:datepicker id="dateend" runat="server" width="120px"  dateformatstring="yyyy-MM-dd"  />
                                </td>
                            </tr>
                            <tr>
                                <td style="height:25px">
                                   <ext:label runat="server" text="会 员 号：" showlabel="False" cssclass="mright_lable" id="lblMemberNum"  ></ext:label>
                                </td>
                                <td  valign=top>
                                    <ext:textbox runat="server" id="tbxmember"  cssclass="tbxwidth100" EmptyText="昵称/登录名/真实姓名"></ext:textbox>
                                </td>
                                <td >
                                    <ext:label runat="server" text="客户分类：" showlabel="False" cssclass="mright_lable" id="lblCustomerType"></ext:label>                            
                                </td>
                                <td >
                                 <ext:DropDownList runat="server"  Width="128px" ID="DropCustomerType" Resizable="True" AutoPostBack="true" OnSelectedIndexChanged="DropCustomerType_SelectedIndexChanged">
                                    <ext:ListItem Selected="true" Text="全部" Value="-1" />
                                    <ext:ListItem Text="个人" Value="0" />
                                    <ext:ListItem Text="单位" Value="1" />
                                 </ext:DropDownList>                            
                                </td>
                                 <td >
                                 <ext:label runat="server" text="是否作废：" showlabel="False" cssclass="mright_lable" id="lblIScancel"></ext:label>
                                </td>
                                <td >
                                 <ext:DropDownList runat="server"  Width="120px" ID="DropIScancel" Resizable="True">
                                    <ext:ListItem  Text="全部" Value="-1" />
                                    <ext:ListItem Text="已作废" Value="1" />
                                    <ext:ListItem Selected="true"  Text="未作废" Value="0" />
                                 </ext:DropDownList>
                                </td>   
                                <td >
                                &nbsp;
                                </td>
                             </tr>
                             <tr>
                                <td style="height:25px">
                                   <ext:label runat="server" text="姓　　名："  EncodeText="false"   showlabel="False" cssclass="mright_lable" id="lblName"/>
                                </td>
                                <td >
                                   <ext:textbox runat="server" id="tbxName" cssclass="tbxwidth100" />
                                </td>
                                <td >
                                    <ext:label runat="server" text="单　　位：" showlabel="False" cssclass="mright_lable" id="lblEmployer"></ext:label>
                                </td>
                                <td>
                                  <ext:DropDownList ID="DropCustomer" EnableEdit="true" Resizable="True"  runat="server" Width="128"></ext:DropDownList>                  
                                </td>                         
                                <td >
                                    <ext:label runat="server" text="状　　态：" showlabel="False" cssclass="mright_lable" id="lblStatus"/>
                                </td>
                                <td>
                                    <uc2:dropinitbasic id="DropStatus" runat="server" basictype="ORDERSTATUS" width="120"  />
                                </td>
                                <td >
                                &nbsp;
                                </td>
                            </tr>
                            </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridOrders" Title="订单列表" DataKeyNames="ordernum,cancel,labname,statusvalue,realname" AutoScroll="true"
                        EnableCheckBoxSelect="true" ShowHeader="false" PageSize="20" IsDatabasePaging="true" EnableTextSelection="true"
                        AllowPaging="true" runat="server" AutoWidth="true" AutoHeight="true" EnableRowNumber="true"
                        OnPageIndexChange="GridOrders_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="ordernum" DataToolTipField="ordernum" HeaderText="体检号" Width="105px" />
                            <ext:BoundField DataField="realname" DataToolTipField="realname"   HeaderText="姓名" Width="50px" />
                            <ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="实验室分点" Width="100px" />
                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="80px" />
                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField DataField="createdate" DataToolTipField="createdate" HeaderText="登记时间" Width="85px" />
                            <ext:BoundField DataField="enterby" DataToolTipField="enterby" HeaderText="录单人" Width="60px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="单位" Width="80px" />
                            <ext:BoundField DataField="status" DataToolTipField="status" HeaderText="状态" Width="50px" />
                            <ext:BoundField DataField="cancel"  DataToolTipField="cancel" HeaderText="是否作废" Width="60px" />
                            <ext:BoundField DataField="ismarried"  DataToolTipField="ismarried" HeaderText="婚否" Width="50px" />
                            <ext:BoundField DataField="remarks"  DataToolTipField="remarks" HeaderText="备注" Width="150px" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="winMemberSelect" Width="520px" Height="300" IsModal="False" Title="会员选择"
        Hidden="true" runat="server" WindowPosition="Center" >
        <Items>
            <ext:ContentPanel ShowBorder="false" ShowHeader="false" ID="ContentPanel2" EnableBackgroundColor="true"
                BodyPadding="0px" runat="server">
                 <ext:Toolbar id="Toolbar2" runat="server">
                    <Items>               
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server"></ext:ToolbarFill>           
                        <ext:Button id="btnSelectMember"  runat="server" text="选择" onclick="btnSelectMember_Click" Icon="BulletTick" cssclass="inline"></ext:Button>
                        <ext:Button id="btnClose"  runat="server" text="关闭" onclick="btnClose_Click" Icon="BulletCross" cssclass="inline"></ext:Button>
                    </Items>
                 </ext:Toolbar>
                  <ext:grid id="GridMember"   ShowHeader="false" EnableTextSelection="true" ShowBorder="false" height="240" DataKeyNames="Dictmemberid"    enablerownumber="true" enablecheckboxselect="false"   enablemultiselect="false" autowidth="true" runat="server" EnableRowDoubleClick="true" OnRowDoubleClick="GridMember_RowClick">
                    <Columns>
                        <ext:BoundField DataField="Realname" HeaderText="姓名" width="50" />
                        <ext:BoundField DataField="SexName" HeaderText="性别" width="40"/>   
                        <ext:BoundField DataField="FormatBirthday" HeaderText="出生日期" width="100"/>   
                        <ext:BoundField DataField="Idnumber" HeaderText="身份证" width="140" />
                        <ext:BoundField DataField="Mobile" HeaderText="手机号码" width="80"/>   
                        <ext:BoundField DataField="Addres" HeaderText="地址" width="150"/>      
                    </Columns>                    
                 </ext:grid>   
            </ext:ContentPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="WindowFrame" Hidden="true" EnableIFrame="true" runat="server"   IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="900" Height="518">
    </ext:Window>
    <ext:HiddenField ID="hdMac" runat="server">
    </ext:HiddenField>
    <script type="text/javascript">
        function onReady() {
            var tbxmemberID = '<%= tbxmember.ClientID %>';
            var tbxmember = Ext.getCmp(tbxmemberID);
            //获取mac地址取该用户的本地打印机IP等信息
            document.getElementById("<%=hdMac.ClientID %>").value = document.ActiveXPrint.GetMAC();
            //注册会员文本回车事件
            tbxmember.on("specialkey", function (box, e) {
                if (e.getKey() == e.ENTER) {
                    if (tbxmember.getValue() == "") {
                        top.X.util.alert("请输入要搜索的内容！");
                        return false
                    } else {
                        __doPostBack(tbxmemberID, 'specialkey');
                    }
                }
            });
        }
    </script>
    </form>
</body>
</html>
