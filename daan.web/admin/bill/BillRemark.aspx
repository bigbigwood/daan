<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillRemark.aspx.cs" Inherits="daan.web.admin.bill.BillRemark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备注录入</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" />
    <ext:Panel ID="pnl" runat="server" ShowHeader="false" Layout="Column" ShowBorder="false">
        <Items>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:Button ID="btnSave" runat="server" Icon="Add" Text="保存" CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:Button ID="btnCanCel" runat="server" Icon="Cancel" Text="取消" CssClass="inline">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
            <ext:ContentPanel ID="contentpanel" runat="server" ShowHeader="false" ShowBorder="false">
        <table style="height: 550; width: 580;">
            <tr>
                <td colspan="2" style=" vertical-align:middle;">
                    <ext:Label ID="lblRemark" runat="server" Text="&nbsp;财务备注" Height="30">
                    </ext:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <ext:TextArea ID="tbaRemark" runat="server" Text="" Width="550" Height="120">
                    </ext:TextArea>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <ext:Label ID="lblSelfRemark" runat="server" Text="&nbsp;财务说明" Height="30">
                    </ext:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    <ext:TextArea ID="tbaSelfRemark" runat="server" Width="550" Height="120">
                    </ext:TextArea>
                </td>
            </tr>
           <tr>
            <td>&nbsp;</td>
           </tr>
        </table>
            </ext:ContentPanel>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
