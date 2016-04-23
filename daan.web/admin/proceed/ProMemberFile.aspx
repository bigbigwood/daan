<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProMemberFile.aspx.cs"
    Inherits="daan.web.admin.proceed.ProMemberFile" %>

<%@ Register Src="../../usercontrol/DropInitBasic.ascx" TagName="DropInitBasic" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .inLeft
        {
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:PageManager ID="PageManager1" runat="server"/>
        <ext:TabStrip ID="TabStrip1" runat="server" ActiveTabIndex="0" ShowBorder="false"
            Height="468">
            <Tabs>
                <ext:Tab ID="tabMemberInfo" runat="server" BodyPadding="0px" Width="100%" Height="100%"
                    Title="基本信息">
                    <Items>
                        <ext:Toolbar ID="Toolbar1" runat="server" Position="Bottom">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                </ext:ToolbarFill>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btnSubmit" OnClick="btnSubmit_Click" Icon="SystemSaveNew" Text="保存"
                                    runat="server" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btnClose" OnClick="btnClose_Click" Text="关闭" runat="server" Icon="BulletCross" />
                            </Items>
                        </ext:Toolbar>
                        <ext:ContentPanel runat="server" ShowHeader="False" ShowBorder="False">
                         <table border="0"  width="100%" height="350" >                                
                                <tr>
                                    <td align=right>真实姓名：</td>
                                    <td><ext:TextBox ID="tbxRealname" Label="Label"  runat="server"></ext:TextBox></td>
                                    <td align=right>昵称：</td>
                                    <td><ext:TextBox ID="tbxNickname" Label="Label"  runat="server"></ext:TextBox></td>
                                </tr>
                                <tr>
                                    <td align=right>身份证号：</td>
                                    <td><ext:TextBox ID="tbxIdnumber" Label="Label"  runat="server"></ext:TextBox></td>
                                    <td align=right>性别：</td>
                                    <td><uc2:dropinitbasic id="DropSex" runat="server" basictype="SEX" Width="128" /></td>
                                </tr>
                                <tr>                                   
                                    <td align=right>电话号码：</td>
                                    <td><ext:TextBox id="tbxPhone" width="128" runat="server" /></td>
                                    <td align=right>手机号码：</td>
                                    <td><ext:TextBox ID="tbxMobile" Label="Label"  runat="server" ></ext:TextBox></td>
                                </tr>
                                <tr>                                   
                                    <td align=right>出生日期：</td>
                                    <td><ext:datepicker id="dateBirthday" width="128" runat="server" dateformatstring="yyyy-MM-dd" enabledateselect="true"  />
                                    </td>
                                    <td align=right>E-Mail：</td>
                                    <td><ext:TextBox ID="tbxEmail" Label="Label"  runat="server"  regex="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" regexmessage="E-Mail格式不对"></ext:TextBox></td>
                                </tr>
                                <tr>
                                    <td align=right>QQ：</td>
                                    <td><ext:TextBox ID="tbxQq" Label="Label"  runat="server"></ext:TextBox></td>
                                    <td align=right>MSN：</td>
                                    <td><ext:TextBox ID="tbxMsn" Label="Label"  runat="server"></ext:TextBox></td>
                                </tr>
                                <tr>
                                    <td align=right>个人网址：</td>
                                    <td><ext:TextBox ID="tbxUrl" Label="Label"  runat="server"></ext:TextBox></td>
                                    <td align=right>激活锁定</td>
                                    <td >
                                        <table width="100%">
                                            <tr>
                                                <td  width="25%"><ext:CheckBox ID="ckbIsactive" Label="Label" Text="锁定" runat="server" ></ext:CheckBox></td>
                                                <td width="50%"><ext:CheckBox ID="ckbIslock" Label="Label" Text="激活" runat="server"></ext:CheckBox></td>
                                            </tr>
                                        </table>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td align=right>详细地址：</td>
                                    <td colspan="3"><ext:TextBox ID="tbxAddres" Label="Label"  runat="server" width="480"></ext:TextBox></td>                                 
                                </tr>   
                                <tr>
                                    <td align=right>ID：</td>
                                    <td ><ext:Label ID="lblId" Label="Label"  runat="server"></ext:Label></td>
                                    <td align=right>用户名：</td>
                                    <td><ext:Label ID="lblLoginname" Label="Label"  runat="server"></ext:Label></td>
                                </tr>                              
                                <tr>
                                    <td align=right>消费：</td>
                                    <td><ext:Label id="lblConsumer" Text="Label" showlabel=false Label="Label" runat=server /></td>
                                    <td align=right>注册时间：</td>
                                    <td><ext:Label id="lblCreatedate" Text="Label"  showlabel=false Label="Label" runat=server /></td>
                                </tr>
                                <tr>                                
                                    <td align=right>积分：</td>
                                    <td><ext:Label id="lblscores" Text="Label" Label="Label" showlabel=false  runat=server /></td>
                                    <td align=right>最后登录时间：</td>
                                    <td><ext:Label id="lblLogintime" Text="Label" Label="Label" showlabel=false  runat=server /></td>                                    
                                </tr>
                                <tr>
                                   
                                    <td align=right>登录次数：</td>                                                       
                                    <td><ext:Label id="lblLoginnum" Text="Label" Label="Label" runat=server  showlabel=false /></td> 
                                    <td align=right>最后登录IP：</td>  
                                    <td><ext:Label id="lblLoginip" Text="Label" Label="Label" runat=server showlabel=false  /></td>  
                                </tr>
                            </table>
                        </ext:ContentPanel>
                    </Items>
                </ext:Tab>
                <ext:Tab ID="tabPastMedicalHistory" Layout="Fit" runat="server" Width="100%"
                    Height="100%" Title="健康档案">
                    <Items>
                        <ext:Grid ID="GridMedHistory" AutoScroll="true" CssStyle="padding:1px;" AutoWidth="true"
                            AutoHeight="true" EnableMultiSelect="false" EnableCheckBoxSelect="true" DataKeyNames="Dicthealthrecordsid" EnableRowNumber="true" runat="server" ShowHeader="false" EnableTextSelection="true">
                            <Toolbars>
                                <ext:Toolbar runat="server" Height="25">
                                    <Items>
                                        <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                        </ext:ToolbarFill>
                                        <ext:ToolbarSeparator ID="ts1" runat="server"></ext:ToolbarSeparator>
                                        <ext:Button ID="btnAdd" runat="server" Text="添加" Icon="ADD" OnClick="btnAdd_Click">
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ts2" runat="server"></ext:ToolbarSeparator>
                                        <ext:Button ID="btnDel" runat="server" Text="删除"
                                         ConfirmIcon="Question" ConfirmTarget="Top" ConfirmText="是否确定删除此条记录？" 
                                         ConfirmTitle="体检系统" Icon="DELETE" OnClick="btnDel_Click">
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                        <ext:Button ID="btnClo" runat="server" Icon="BulletCross" Text="关闭" OnClick="btnClose_Click"></ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </Toolbars>
                            <Columns>
                                <ext:BoundField HeaderText="类型" DataField="DictrecordtypeText" Width="50px" />
                                <ext:BoundField HeaderText="详细内容" DataField="Dictrecordtext" DataToolTipField="Dictrecordtext" ExpandUnusedSpace="true" />
                            </Columns>
                        </ext:Grid>
                        <ext:Label ID="lblMemberID" runat="server" ShowLabel="false" Label="">
                        </ext:Label>
                    </Items>
                </ext:Tab>
            </Tabs>
        </ext:TabStrip>
        <ext:Window runat="server" Popup="false" Hidden="true" ID="win1" Target="Top" EnableIFrame="true" IFrameUrl="about:blank"
         Width="500px" Height="300px" Title="健康档案">
        </ext:Window>
    </div>
    </form>
</body>
</html>
