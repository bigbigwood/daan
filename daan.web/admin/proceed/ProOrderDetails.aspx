<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProOrderDetails.aspx.cs"
    Inherits="daan.web.admin.proceed.ProOrderDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>体检登记</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:pagemanager id="PageManager1" autosizepanelid="RegionPanel1" runat="server" />
    <ext:regionpanel id="RegionPanel1" runat="server" showborder="False" > 
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" OnClick="btnClose_Click" Text="关闭" runat="server" Icon="BulletCross" />
                </Items>
            </ext:Toolbar>
        </Toolbars>     
        <Regions>
            <ext:Region Layout="Fit" Height="210px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                    <ext:ContentPanel ShowHeader="False" ShowBorder="False">
                    <table border="0" width="100%"  >
                        <tr>
                            <td  style="height:25px; text-align:right">
                                <ext:label runat="server" text="分　　点："  cssclass="mright_lable" id="lblLabName"></ext:label>
                            </td>
                            <td colspan="5">
                                <ext:textbox  Enabled="false"  runat="server" id="tbxDictLab"  cssclass="tbxwidth100"  />    
                            </td>
                            <td>
                                <ext:label runat="server" text="生　　日："    cssclass="mright_lable" id="Label1"></ext:label>
                            </td>
                            <td>
                                 <ext:textbox  Enabled="false"  runat="server" id="tbxBirthday"  width="152" />     
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="体 检 号："  cssclass="mright_lable" id="lblOrderNum"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxOrderNum"  cssclass="tbxwidth100" ></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="姓　　名："   cssclass="mright_lable" id="lblName"></ext:label>
                            </td>
                            <td >
                                <ext:textbox  Enabled="false"  runat="server" id="tbxName" label=""  cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>    
                                <ext:label runat="server" text="身 份 证："   cssclass="mright_lable" id="lblIDNumber"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxIDNumber" required="true" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="年　　龄："   cssclass="mright_lable" id="lblAge"></ext:label>
                            </td>
                            <td>
                                <ext:numberbox runat="server"  id="tbxAge" cssclass="inline" width="22%" Enabled="false"   ></ext:numberbox>
                                <ext:label runat="server" text="岁"  cssclass="inlineTop" id="lblYear"></ext:label>
                                <ext:numberbox runat="server"  id="tbxMonth" cssclass="inline"  width="22%" Enabled="false"></ext:numberbox>
                                <ext:label runat="server" text="月"  cssclass="inlineTop" id="lblMonth"></ext:label>
                                <ext:numberbox runat="server"  id="tbxDay" cssclass="inline"  width="22%" Enabled="false"> </ext:numberbox>
                                <ext:label runat="server" text="日"  cssclass="inlineTop" id="lblDay"> </ext:label>
                                <ext:numberbox runat="server"  id="tbxHour" cssclass="inline"  width="20%" Enabled="false"> </ext:numberbox>
                                <ext:label runat="server" text="时"  cssclass="inlineTop" id="lblHour"></ext:label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="性　　别："   cssclass="mright_lable" id="lblSex"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxSex" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="电　　话："  cssclass="mright_lable" id="lblPhone"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxPhone" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="手　　机："  cssclass="mright_lable" id="lblMobile"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxMobile" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="E-Mail："  cssclass="mright_lable" id="lblEMail"></ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxEMail" width="152" > </ext:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="婚　　否："   cssclass="mright_lable" id="lblIsMarried"></ext:label>
                            </td>
                            <td>
                                <ext:radiobuttonlist runat="server" id="radlIsMarried" Enabled="false" ><ext:RadioItem Selected="True" Text="未婚&nbsp;&nbsp;&nbsp;" Value="0"></ext:RadioItem><ext:RadioItem Text="已婚" Value="1"></ext:RadioItem><ext:RadioItem Text="未知" Value="2"></ext:RadioItem></ext:radiobuttonlist>
                            </td>
                            <td>
                                <ext:label runat="server" text="客户分类："   cssclass="mright_lable"  id="lblCustomerType"></ext:label>
                            </td>
                            <td>
                                <ext:radiobuttonlist runat="server" id="radlCustomerType"   Enabled="false"><ext:RadioItem Selected="True" Text="个人&nbsp;&nbsp;&nbsp;" Value="0"></ext:RadioItem><ext:RadioItem Text="单位" Value="1"></ext:RadioItem></ext:radiobuttonlist>
                            </td>
                            <td>
                                <ext:label runat="server" text="选择单位："  cssclass="mright_lable"  id="lblCustomer">  </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxCustomer" cssclass="tbxwidth100"></ext:textbox>
                            </td>
                            <td>
                                <ext:label runat="server" text="单位部门："  cssclass="mright_lable"  id="lblPhysicalType"> </ext:label>
                            </td>
                            <td>
                                <ext:textbox  Enabled="false"  runat="server" id="tbxSection" width="152"></ext:textbox>
                            </td>
                        </tr>
                        <tr>
                           <td>
                                <ext:Label runat="server" Text="省份：" CssClass="mright_lable"></ext:Label>
                           </td>
                           <td>
                                <ext:textbox runat="server" id="tbxProvincename" CssClass="tbxwidth100" Enabled="false"></ext:textbox>
                           </td>
                           <td>
                                <ext:Label ID="lblcityname" runat="server" Text="城市：" CssClass="mright_lable"></ext:Label>
                           </td>
                            <td>
                                <ext:textbox runat="server" id="tbxCityname" CssClass="tbxwidth100" Enabled="false"></ext:textbox>
                           </td>
                           <td>
                                <ext:Label ID="lblcountyname" runat="server" Text="地区：" CssClass="mright_lable"></ext:Label>
                           </td>
                            <td colspan="3">
                                <ext:textbox runat="server" id="tbxCountyname" CssClass="width:99%" Enabled="false"></ext:textbox>
                           </td>
                        </tr>
                        <tr > 
                            <td style="height:25px">
                                <ext:label runat="server" text="联系地址："  cssclass="mright_lable" id="lblAddres"></ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textbox  Enabled="false"  runat="server" id="tbxAddres" cssstyle="width:99%"></ext:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label ID="Label2" runat="server" text="备　　注："  cssclass="mright_lable"></ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textbox  Enabled="false"  runat="server" id="tbxRemark" cssstyle="width:99%"></ext:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:25px">
                                <ext:label runat="server" text="套餐组合："  cssclass="mright_lable" id="lblItemTest"></ext:label>
                            </td>
                            <td colspan="7">
                                <ext:textarea  id="tbxItemTest" runat="server"  Enabled="false" height="35px" cssstyle="width:99%"></ext:textarea>
                            </td>
                        </tr>
                    </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridTest" ShowHeader="false" ShowBorder="False" Title="体检项目" EnableRowNumber="true"
                        EnableCheckBoxSelect="false" EnableMultiSelect="false" AutoScroll="true" AutoWidth="true" EnableTextSelection="true"
                        AutoHeight="true" runat="server"  OnRowDataBound="GridTest_RowDataBound">
                        <Columns>
                            <ext:BoundField DataField="Id" HeaderText="ID" Hidden="true" />
                            <ext:BoundField DataField="Uniqueid" HeaderText="全国统一码" />
                            <ext:BoundField DataField="Name" HeaderText="组合/项目名称" />
                            <ext:BoundField DataField="Type" HeaderText="项目类型" />
                            <ext:BoundField DataField="Isactive" HeaderText="状态" />
                            <ext:BoundField DataField="Sencustomername" HeaderText="外包单位" />
                            <ext:BoundField DataField="Barcode" HeaderText="条码号" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:regionpanel>
    </form>
</body>
</html>
