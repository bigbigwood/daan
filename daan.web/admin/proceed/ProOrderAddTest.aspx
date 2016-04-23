<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProOrderAddTest.aspx.cs"
    Inherits="daan.web.admin.proceed.ProOrderAddTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>体检登记</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:pagemanager id="PageManager1" autosizepanelid="RegionPanel1" runat="server"/>
    <ext:regionpanel id="RegionPanel1" runat="server" showborder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server"> </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保 存" Icon="SystemSaveNew" OnClick="BtnSave_Click"  CssClass="inline"> </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" OnClick="btnClose_Click" Text="关闭" runat="server" Icon="BulletCross" />
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="230px" Position="Top" ShowHeader="False" ShowBorder="False" Split="true">
                <Items>
                    <ext:ContentPanel ShowHeader="False" ShowBorder="False" runat="server" >
                    <table border="0" width="100%" >
                        <tr>
                            <td  style="height:25px; text-align:right">
                                <ext:label runat="server" text="选择分点<font color=red>*</font>：" EncodeText="false"  cssclass="mright_lable" id="lblLabName">                                    </ext:label>
                            </td>
                            <td colspan="5">
                                  <ext:textbox  Enabled="False" runat="server" id="tbxDictLab"  cssclass="tbxwidth100" ></ext:textbox>
                            </td>
                            <td style="height:25px">
                                <ext:label runat="server" text="体 检 号<font color=red>*</font>：" EncodeText="false" showlabel="False" cssclass="mright_lable" id="lblOrderNum">                                    </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxOrderNum" readonly="true" cssclass="tbxwidth100">                                    </ext:textbox>
                            </td>                            
                        </tr>
                        <tr>                            
                            <td>
                                <ext:label runat="server" text="姓　　名<font color=red>*</font>：" EncodeText="false"  showlabel="False" cssclass="mright_lable" id="lblName">                                    </ext:label>
                            </td>
                            <td >
                                <ext:textbox  Enabled="False" runat="server" id="tbxName" label="" required="true" cssclass="tbxwidth100">                                    </ext:textbox>
                            </td>
                            <td>    
                                <ext:label runat="server" text="身 份 证<font color=red>*</font>：" EncodeText="false"  showlabel="False" cssclass="mright_lable" id="lblIDNumber">                                    </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxIDNumber"  cssclass="tbxwidth100"> </ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="生　　日<font color=red>*</font>：" EncodeText="false"   showlabel="False" cssclass="mright_lable" id="Label1">                                    </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxBirthday"  width="152" />     
                            </td>
                            <td>
                                <ext:label runat="server" text="年　　龄<font color=red>*</font>：" EncodeText="false"  showlabel="False" cssclass="mright_lable" id="lblAge">                                    </ext:label>
                            </td>
                            <td>
                                <ext:numberbox runat="server" showlabel="False" id="tbxAge" cssclass="inline" width="20%"  Enabled="False"> </ext:numberbox>
                                <ext:label runat="server" text="岁" showlabel="False" cssclass="inlineTop" id="lblYear"></ext:label>
                                <ext:numberbox runat="server" showlabel="False" id="tbxMonth" cssclass="inline"  Enabled="False" width="22%"></ext:numberbox>
                                <ext:label runat="server" text="月" showlabel="False" cssclass="inlineTop" id="lblMonth"></ext:label>
                                <ext:numberbox runat="server" showlabel="False" id="tbxDay" cssclass="inline"  Enabled="False" width="22%"> </ext:numberbox>
                                <ext:label runat="server" text="日" showlabel="False" cssclass="inlineTop" id="lblDay"> </ext:label>
                                <ext:numberbox runat="server" showlabel="False" id="tbxHour" cssclass="inline"  Enabled="False" width="22%"> </ext:numberbox>
                                <ext:label runat="server" text="时" showlabel="False" cssclass="inlineTop" id="lblHour"></ext:label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="性　　别<font color=red>*</font>：" EncodeText="false"  showlabel="False" cssclass="mright_lable" id="lblSex">
                                    </ext:label>
                            </td>
                            <td>
                              <ext:textbox  Enabled="false"  runat="server" id="tbxSex" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="电　　话 ：" showlabel="False" cssclass="mright_lable" id="lblPhone">
                                    </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxPhone" cssclass="tbxwidth100"> </ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="手　　机 ：" showlabel="False" cssclass="mright_lable" id="lblMobile"> </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxMobile" cssclass="tbxwidth100"> </ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="E-Mail　：" showlabel="False" cssclass="mright_lable" id="lblEMail"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxEMail" width="152" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="婚　　否<font color=red>*</font>：" EncodeText="false"  showlabel="False" cssclass="mright_lable" id="lblIsMarried">
                                    </ext:label>
                            </td>
                            <td>
                                <ext:radiobuttonlist runat="server" id="radlIsMarried" Enabled="false">
                                        <ext:RadioItem Selected="True" Text="未婚&nbsp;&nbsp;&nbsp;" Value="0"></ext:RadioItem>
                                        <ext:RadioItem Text="已婚" Value="1"></ext:RadioItem>
                                        <ext:RadioItem Text="未知" Value="2"></ext:RadioItem>
                                    </ext:radiobuttonlist>
                            </td>                            
                            <td>
                                <ext:label runat="server" text="单位 ：" showlabel="False" cssclass="mright_lable"
                                    id="lblCustomer">
                                    </ext:label>
                            </td>
                            <td>
                               <ext:textbox  Enabled="false"  runat="server" id="tbxCustomer" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="单位部门 ：" showlabel="False" cssclass="mright_lable"
                                    id="lblPhysicalType">
                                    </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="False" runat="server" id="tbxSection" width="152 "></ext:textbox>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="联系地址 ：" showlabel="False" cssclass="mright_lable"
                                    id="lblAddres">
                                    </ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textbox  Enabled="False" runat="server" id="tbxAddres" cssstyle="width:99%">
                                    </ext:textbox>
                            </td>
                        </tr>
                        <tr >
                            <td style="height:25px">
                                <ext:label runat="server" text="备　　注 ：" showlabel="False" cssclass="mright_lable">
                                    </ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textbox  Enabled="False" runat="server" id="tbxRemark" cssstyle="width:99%">
                                    </ext:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="套餐组合 ：" showlabel="False" cssclass="mright_lable"
                                    id="lblItemTest">
                                    </ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textarea  Enabled="False" readonly="true" id="tbxItemTest" runat="server" height="35px" cssstyle="width:99%">
                                    </ext:textarea>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="套　　餐 ：" showlabel="False" cssclass="mright_lable" id="lblProductItem">
                                    </ext:label>
                            </td>
                            <td colspan="3">
                                <ext:dropdownlist id="DropItem" enableedit="true" runat="server" width="250" autopostback="true" Resizable="True"
                                    onselectedindexchanged="DropItem_SelectedIndexChanged">
                                        </ext:dropdownlist>
                            </td>
                            <td>
                                <ext:label runat="server" text="组合项目 ：" showlabel="False" id="lblProductTest" cssclass="mright_lable" >
                                    </ext:label>
                            </td>
                            <td colspan="3">
                                <ext:dropdownlist id="DropTest" enableedit="true" runat="server" width="250" autopostback="true" Resizable="True"
                                    onselectedindexchanged="DropTest_SelectedIndexChanged">
                                        </ext:dropdownlist>
                            </td>
                        </tr>
                    </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridTest" ShowHeader="false" ShowBorder="False" Title="体检项目" EnableRowNumber="true"
                        EnableCheckBoxSelect="false" EnableMultiSelect="false" AutoScroll="true" AutoWidth="true"
                        AutoHeight="true" runat="server" OnRowCommand="GridTest_RowCommand" OnRowDataBound="GridTest_RowDataBound" EnableTextSelection="true">
                        <Columns>
                            <ext:BoundField DataField="Id" HeaderText="ID" Hidden="true" />
                            <ext:BoundField DataField="Uniqueid" HeaderText="全国统一码" />
                            <ext:BoundField DataField="Name" HeaderText="组合/项目名称" />
                            <ext:BoundField DataField="Type" HeaderText="项目类型" />
                            <ext:BoundField DataField="Isactive" HeaderText="状态" />
                            <ext:TemplateField HeaderText="外包单位" Width="105px">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DropSendCustomer" runat="server" Resizable="True" Width="100">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </ext:TemplateField>
                            <ext:BoundField DataField="Barcode" HeaderText="条码号" />
                           <%-- <ext:LinkButtonField HeaderText="停止/继续测试" Width="100px" Icon="DatabaseEdit" CommandName="Stop"
                                Text="" ToolTip="停止/继续测试" />--%>
                          <%--  <ext:LinkButtonField HeaderText="删除" Width="60px" Icon="BulletCross" ConfirmText="你确定要删除组合|项目吗？"
                                ConfirmTarget="Top" CommandName="Delete" Text="" ToolTip="删除" />--%>
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:regionpanel>
    <%--会员ID--%>
    <ext:textbox enabled="False" runat="server" id="tbxmemberID" cssclass="tbxwidth100"
        cssstyle="display: none"></ext:textbox>
    </form>
</body>
</html>
