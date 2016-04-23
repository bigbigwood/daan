 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnaResult_Windows.aspx.cs" Inherits="daan.web.admin.analyse.AnaResult_Windows" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title></title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <ext:Panel ID="Panel1" runat="server" Layout="Row" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px" EnableBackgroundColor="true">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                    </ext:Button>    
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSaveContinue" Text="保存-关闭" runat="server" Icon="SystemSaveNew"
                        OnClick="btnSaveContinue_Click">
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Items>
            <ext:Grid ID="gvTestItemResult" ShowBorder="true" ShowHeader="false" Title="结果" runat="server" AutoPostBack="false"
                DataKeyNames="Dicttestitemresultid,Result,Isexception" EnableCheckBoxSelect="true" EnableMultiSelect="false"
                EnableRowClick="true">
                <Columns>
                    <ext:BoundField DataField="Result" HeaderText="结果" />
                    <ext:BoundField DataField="Isexception" HeaderText="异常" />
                    <ext:BoundField DataField="Displayorder" HeaderText="排序"></ext:BoundField>
                </Columns>
            </ext:Grid>
        </Items>      
    </ext:Panel>
    </form>
</body>
</html>
