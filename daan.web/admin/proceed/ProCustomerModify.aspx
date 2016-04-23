<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProCustomerModify.aspx.cs" Inherits="daan.web.admin.proceed.ProCustomerModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改单位</title>
     <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <script src="../../js/webpagecontrol.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"/>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                    <ext:Button ID="btnSave" runat="server" Text="保 存" Icon="SystemSaveNew" OnClick="BtnSave_Click" CssClass="inline"></ext:Button> 
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" OnClick="btnClose_Click" Text="关闭" runat="server" Icon="BulletCross" />
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="100px" Position="Top" ShowHeader="False" ShowBorder="False" runat="server">
                <Items>
                    <ext:ContentPanel ID="ContentPanel1" ShowHeader="False" ShowBorder="False" runat="server">
                        <table border="0" width="100%" >
                            <tr>
                                <td  style="height:25px; text-align:right">
                                    <ext:label runat="server" text="选择分点<font color=red>*</font>："  EncodeText="false"  cssclass="mright_lable" id="lblLabName">                                    
                                    </ext:label>
                                </td>
                                <td>
                                    <ext:dropdownlist id="DropDictLab" comparetype="String" comparevalue="-1" compareoperator="NotEqual"  Resizable="True"
                                        comparemessage="请选择分点！" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged"
                                        width="128">
                                    </ext:dropdownlist>
                                </td> 
                            </tr>
                            <tr>
                                <td style="height:25px; text-align:right">
                                    <ext:label runat="server" text="选择单位 ：" showlabel="False" cssclass="mright_lable" id="lblCustomer">
                                    </ext:label>
                                </td>
                                <td>
                                    <ext:dropdownlist id="DropCustomer" Resizable="True"  enableedit="true" autopostback="true" runat="server" width="128">
                                    </ext:dropdownlist>
                                </td>
                            </tr>
                        </table>
                    </ext:ContentPanel>
                    <ext:HiddenField runat="server" ID="hidOrderNum"></ext:HiddenField>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
