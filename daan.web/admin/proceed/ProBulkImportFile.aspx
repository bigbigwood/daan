<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProBulkImportFile.aspx.cs" Inherits="daan.web.admin.proceed.ProBulkImportFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1"/>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:Label ID="Label1" runat="server" Text="Excel格式请严格参照导入模版说明" CssStyle="color:red;font-weight:bold;"></ext:Label>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnImport" runat="server" OnClick="btnImport_Click" Icon="TelevisionIn"
                        Text=" 导 入 " CssClass="right">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator> 
                    <ext:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Icon="BulletCross"
                        Text=" 关 闭 " CssClass="marginleft">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="100px" ShowHeader="False" ShowBorder="false"
                Split="false">
                <Items>
                    <ext:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                        <table border="0" width="100%">
                            <tr height="30">
                                <td width="80">
                                    <ext:label id="lblDictLab" runat="server" text="选择分点：" showlabel="False" cssstyle="float:right;"></ext:label>
                                </td>
                                <td width="200">
                                    <ext:dropdownlist id="DropDictLab" Resizable="True" comparetype="String" comparevalue="-1" compareoperator="NotEqual" comparemessage="请选择省份！" runat="server" autopostback="true" onselectedindexchanged="DropDictLab_SelectedIndexChanged"  width="200px"> </ext:dropdownlist>
                                </td>
                                <td width="80">
                                    <ext:label runat="server" text="体检单位11：" cssstyle="float:right;" id="lblCustomer"></ext:label>
                                </td>
                                <td width="200">
                                    <ext:dropdownlist id="DropCustomer"   runat="server" width="200px" Resizable="True" EnableEdit="true"/>
                                </td>
                                <td>
                                    <ext:TextBox runat="server" CssClass="inline" ID="tbxmember" EmptyText="输入后回车" Width="90px"></ext:TextBox>
                                </td>
                            </tr>
                            <tr style="height:30px">
                                <td align="right">省：</td>
                                <td><ext:DropDownList runat="server" Width="200px" Resizable="true" AutoPostBack="true" EnableEdit="true" ID="dpProvince" OnSelectedIndexChanged="dpProvince_SelectedIndexChanged"></ext:DropDownList></td>
                                <td align="right">市：</td>
                                <td colspan="2"><ext:DropDownList runat="server" Width="200px" Resizable="true" AutoPostBack="true" EnableEdit="true" ID="dpCity" OnSelectedIndexChanged="dpCity_SelectedIndexChanged"></ext:DropDownList></td>
                            </tr>
                            <tr> 
                                <td align="right">县/区：</td>
                                <td><ext:DropDownList runat="server" Width="200px" Resizable="true" ID="dpCounty" EnableEdit="true" AutoPostBack="true" OnSelectedIndexChanged="dpCounty_SelectedIndexChanged"></ext:DropDownList></td>
                                <td height="25">
                                    <ext:label runat="server" text="选择文件：" showlabel="False" cssstyle="float:right;" id="lblFile"></ext:label>
                                </td>
                                <td valign="top" colspan="2">
                                    <ext:fileupload runat="server" id="fileExcel" emptytext="请选择要上传的文件" enabled="true" readonly="false" cssstyle="width:70%"></ext:fileupload>
                                </td>
                            </tr>
                            <tr style="height:35px">
                                <td></td>
                                <td colspan="4">
                                    <ext:CheckBox runat="server" ID="ck1" Text="<font color='red'>该批导入客户报告书统一寄送到以下地址，注：勾选此项后以下三项为必填</font>" 
                                    CssStyle="color:red;" AutoPostBack="true" Checked="true" OnCheckedChanged="ck1_CheckedChanged"></ext:CheckBox>
                                </td>
                            </tr>
                            <tr style="height:30px">
                                <td align="right">邮寄地址：</td>
                                <td colspan="4"><ext:TextBox runat="server" ID="txtAddress" Width="525px"></ext:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">收件人：</td>
                                <td><ext:TextBox runat="server" ID="txtRecName" Width="200px"></ext:TextBox></td>
                                <td align="right">联系电话：</td>
                                <td colspan="2"><ext:TextBox runat="server" ID="txtTelphone" Width="240px"></ext:TextBox></td>
                            </tr>
                        </table>
                    </ext:ContentPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:HiddenField runat="server" ID="hidProvince"></ext:HiddenField>
    <ext:HiddenField runat="server" ID="hidCity"></ext:HiddenField>
    <ext:HiddenField runat="server" ID="hidCounty"></ext:HiddenField>
    <script type="text/javascript">
        function onReady() {
            var tbxmemberID = '<%= tbxmember.ClientID %>';
            var tbxmember = Ext.getCmp(tbxmemberID);
            tbxmember.on("specialkey", function (box, e) {
                __doPostBack(tbxmemberID, 'specialkey');
            });
        }
    </script>
    </form>
</body>
</html>
