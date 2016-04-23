<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProRegister.aspx.cs" Inherits="daan.web.admin.proceed.ProRegister" %>

<%@ Register Src="../../usercontrol/DropInitBasic.ascx" TagName="DropInitBasic" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>体检登记</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
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
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" runat="server" Text="新 增" Icon="Add" OnClick="btnAdd_Click"
                        CssClass="inline">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保 存" Icon="SystemSaveNew" OnClick="BtnSave_Click"
                        CssClass="inline" ValidateForms="Form2" ValidateTarget="Parent" ValidateMessageBox="true">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnArchive" runat="server" Text="会员档案" Icon="User" CssClass="inline"
                        OnClick="btnArchive_Click">
                    </ext:Button>
                </Items>    
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="250px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                <ext:SimpleForm ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                    Title="Form" ShowBorder="false" ShowHeader="False">
                    <Items>
                        <ext:Panel ID="Panel2" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items> 
                                <ext:Label ID="Label5" Width="60px" runat="server" Text="会员搜索：" />
                                <ext:TextBox runat="server" ID="tbxmember" EmptyText="输入姓名按Enter键快速选择" Width="155px">
                                </ext:TextBox>
                                <ext:Label ID="Label6" Width="60px" runat="server" Text="&nbsp;&nbsp;体检号：" />
                                <ext:TextBox runat="server" ID="tbxOrderNum" Readonly="true" Width="156px">
                                </ext:TextBox>
                                <ext:Label ID="Label7" Width="80px" runat="server" Text="&nbsp;&nbsp;分&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;点：" />
                                <ext:DropDownList ID="DropDictLab" Resizable="True" runat="server" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                </ext:DropDownList>
                                <ext:Label ID="Label8" Width="60px" runat="server" Text="&nbsp;&nbsp;单位：" />
                                <ext:DropDownList ID="DropCustomer" Resizable="True" EnableEdit="true" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DropCustomer_SelectedIndexChanged" runat="server">
                                </ext:DropDownList>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel3" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label9" Width="60px" runat="server" Text="姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名：" />
                                <ext:TextBox runat="server" ID="tbxName" Required="true" Width="155px" Label="姓名" RequiredMessage="姓名不能为空！">
                                </ext:TextBox>
                                <ext:Label ID="Label10" Width="60px" runat="server" Text="&nbsp;&nbsp;身份证：" />
                                <ext:TextBox runat="server" ID="tbxIDNumber" Width="156px" Label="身份证">
                                </ext:TextBox>
                                <ext:Label ID="Label11" Width="80px" runat="server" Text="&nbsp;&nbsp;手&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;机：" />
                                <ext:NumberBox runat="server" ID="tbxMobile" Width="206px">
                                </ext:NumberBox>
                                <ext:Label ID="Label12" Width="60px" runat="server" Text="&nbsp;&nbsp;性别：" />
                                <ext:DropDownList ID="DropSex" Resizable="True" runat="server" Width="200px">
                                </ext:DropDownList>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel4" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label14" Width="60px" runat="server" Text="婚&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;否：" />
                                <ext:DropDownList ID="radlIsMarried" EnableEdit="true" runat="server" Width="150px">
                                    <ext:ListItem Text="未知" Value="2" Selected="true"></ext:ListItem>
                                    <ext:ListItem Selected="True" Text="未婚" Value="0"></ext:ListItem>
                                    <ext:ListItem Text="已婚" Value="1"></ext:ListItem>
                                </ext:DropDownList>
                                <ext:Label ID="Label13" Width="60px" runat="server" Text="&nbsp;&nbsp;生&nbsp;&nbsp;&nbsp;日：" />
                                <ext:DatePicker ID="dateBirthday" Required="true" runat="server" DateFormatString="yyyy-MM-dd" Label="生日"  RequiredMessage="生日不能为空"
                                    Width="150px" EnableDateSelect="true" AutoPostBack="true" OnTextChanged="dateBirthday_TextChanged">
                                </ext:DatePicker>
                                <ext:Label runat="server" ID="lb1" Text="&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄："
                                    Width="80px">
                                </ext:Label>
                                <ext:NumberBox runat="server" ID="tbxAge" Regex="^\d+$" RegexMessage="只能为正整数" AutoPostBack="true"
                                    Width="60px" OnTextChanged="tbxAge_TextChanged">
                                </ext:NumberBox>
                                <ext:Label runat="server" ID="Label1" Text="岁">
                                </ext:Label>
                                <ext:NumberBox runat="server" ID="tbxMonth" Regex="^\d+$" RegexMessage="只能为正整数" Width="40px"
                                    AutoPostBack="true" OnTextChanged="tbxAge_TextChanged">
                                </ext:NumberBox>
                                <ext:Label runat="server" ID="Label2" Text="月">
                                </ext:Label>
                                <ext:NumberBox runat="server" ID="tbxDay" Regex="^\d+$" RegexMessage="只能为正整数" Width="40px"
                                    AutoPostBack="true" OnTextChanged="tbxAge_TextChanged">
                                </ext:NumberBox>
                                <ext:Label runat="server" ID="Label3" Text="日">
                                </ext:Label>
                                <ext:NumberBox runat="server" ID="tbxHour" Regex="^\d+$" RegexMessage="只能为正整数" Width="40px">
                                </ext:NumberBox>
                                <ext:Label runat="server" ID="Label4" Text="时">
                                </ext:Label>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel6" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label17" Width="60px" runat="server" Text="&nbsp;&nbsp;&nbsp;E-Mail：" />
                                <ext:TextBox runat="server" ID="tbxEMail" RegexPattern="EMAIL" RegexMessage="E-Mail格式不对"
                                    Width="155px">
                                </ext:TextBox>
                                <ext:Label ID="Label18" Width="60px" runat="server" Text="&nbsp;&nbsp;部&nbsp;&nbsp;&nbsp;门：" />
                                <ext:TextBox runat="server" ID="tbxSection" Width="156px">
                                </ext:TextBox>
                                <ext:Label ID="Label19" Width="80px" runat="server" Text="&nbsp;&nbsp;联系地址：" />
                                <ext:TextBox runat="server" ID="tbxAddres" Width="206px">
                                </ext:TextBox>
                                <ext:Label ID="Label20" Width="60px" runat="server" Text="&nbsp;&nbsp;备注：" />
                                <ext:TextBox runat="server" ID="tbxRemark" Width="206px">
                                </ext:TextBox>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="PanelArea" ShowHeader="false" CssClass="x-form-item" ShowBorder="false" Layout="Column" runat="server">
                            <Items>
                                <ext:Label runat="server" Width="60px" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;省："></ext:Label>
                                <ext:DropDownList runat="server" ID="dpProvince" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="dpProvince_SelectedIndexChanged"></ext:DropDownList>
                                <ext:Label runat="server" Width="60px" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;市："></ext:Label>
                                <ext:DropDownList runat="server" ID="dpCity" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="dpCity_SelectedIndexChanged"></ext:DropDownList>
                                <ext:Label runat="server" Width="80px" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;县/区："></ext:Label>
                                <ext:DropDownList runat="server" ID="dpCounty" Width="200px" AutoPostBack="true"></ext:DropDownList>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel7" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label21" Width="60px" runat="server" Text="&nbsp;&nbsp;已选项：" />
                                <ext:TextArea Enabled="False" ID="tbxItemTest" runat="server" Height="35px" Width="900px">
                                </ext:TextArea>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel8" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label22" Width="60px" runat="server" Text="套餐选择：" />
                                <ext:DropDownList ID="DropItem" EnableEdit="true" runat="server" AutoPostBack="true"
                                    Width="360px" Resizable="True" OnSelectedIndexChanged="DropItem_SelectedIndexChanged">
                                </ext:DropDownList>
                                <ext:Label ID="Label23" Width="80px" runat="server" Text="&nbsp;&nbsp;组合选择：" />
                                <ext:DropDownList ID="DropTest" EnableEdit="true" runat="server" AutoPostBack="true"
                                    Width="400px" Resizable="True" OnSelectedIndexChanged="DropTest_SelectedIndexChanged">
                                </ext:DropDownList>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel5" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <ext:Label ID="Label15" Width="60px" runat="server" Text="条码扫描：" />
                                <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbxbarcode"
                                    Width="200px" OnTriggerClick="tbxbarcode_TriggerClick">
                                </ext:TriggerBox>
                                <ext:Label ID="Label16" runat="server" CssStyle="margin-left:10px" Text="说明：条码扫描供护美类产品使用" />
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:SimpleForm>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridTest" ShowHeader="false" ShowBorder="False" Title="体检项目" EnableRowNumber="true"
                        EnableCheckBoxSelect="false" EnableMultiSelect="false" AutoScroll="true" AutoWidth="true"
                        AutoHeight="true" runat="server" OnRowCommand="GridTest_RowCommand" OnRowDataBound="GridTest_RowDataBound">
                        <Columns>
                            <%--
                            <ext:BoundField DataField="Productid" HeaderText="套餐ID" Hidden="true" />
                            <ext:BoundField DataField="Productname" HeaderText="套餐名" Hidden="true" />--%>
                            <ext:BoundField DataField="Id" HeaderText="ID" Hidden="true" />
                            <ext:BoundField DataField="Code" HeaderText="编码" />
                            <ext:BoundField DataField="Name" HeaderText="组合/项目名称" Width="250px" />
                            <ext:BoundField DataField="Type" HeaderText="项目类型" />
                            <%--
                            <ext:BoundField DataField="Testtype" HeaderText="是否套餐" Hidden="true"/>
                            <ext:BoundField DataField="Isadd" HeaderText="是否追加"  Hidden="true" />--%><%--                            
                            <ext:BoundField DataField="Adduserid" HeaderText="追加人ID"  Hidden="true" />
                            <ext:BoundField DataField="Addusername" HeaderText="追加人"  Hidden="true"/>--%>
                            <%-- <ext:BoundField DataField="Isactive" HeaderText="状态" />--%>
                            <%--
                            <ext:BoundField DataField="Billed" HeaderText="账单清单生成状态"  Hidden="true"/>
                            <ext:BoundField DataField="SendbilleD" HeaderText="外包账单生成状态" Hidden="true" />--%>
                            <ext:TemplateField HeaderText="外包单位" Width="105px">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DropSendCustomer" Resizable="True" runat="server" Width="100">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </ext:TemplateField>
                            <ext:BoundField DataField="Barcode" HeaderText="条码号" />
                            <%-- <ext:LinkButtonField HeaderText="停止/继续测试" Width="100px" Icon="DatabaseEdit" CommandName="Stop"
                                Text="" ToolTip="停止/继续测试" />--%>
                            <ext:LinkButtonField HeaderText="删除" Width="60px" Icon="BulletCross" ConfirmText="你确定要删除组合|项目吗？"
                                ConfirmTarget="Top" CommandName="Delete" Text="" ToolTip="删除" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="winMemberSelect" Width="520px" Height="300" IsModal="False" Title="会员选择"
        Hidden="true" runat="server" WindowPosition="Center">
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
                 <ext:grid id="GridMember" ShowHeader="false" EnableTextSelection="true" ShowBorder="false" height="240" 
                 DataKeyNames="Dictmemberid" enablerownumber="true" enablecheckboxselect="false"   
                 enablemultiselect="false" autowidth="true" runat="server" EnableRowDoubleClick="true" 
                 OnRowDoubleClick="GridMember_RowClick">
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
    <%--档案窗口--%>
    <ext:Window ID="winArchive" Hidden="true" EnableIFrame="true" runat="server" IFrameUrl="about:blank"
        Title="用户档案" Target="Top" IsModal="True" Width="700" Height="500">
    </ext:Window>
    <%--会员ID  是否新增--%>
    <ext:TextBox runat="server" ID="tbxmemberID" CssClass="tbxwidth100" CssStyle="display: none">
    </ext:TextBox>
    <ext:TextBox runat="server" ID="PageIsInsert" CssClass="diaplay_none">
    </ext:TextBox>
    <script type="text/javascript">
        function onReady() {
            var tbxmemberID = '<%= tbxmember.ClientID %>';
            var tbxmember = Ext.getCmp(tbxmemberID);
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
