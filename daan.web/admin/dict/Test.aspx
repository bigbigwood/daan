<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="daan.web.admin.dict.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <%-- <script src="../../js/Resources/JQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.insertContent.js" type="text/javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
   <%-- <ext:RegionPanel ID="RegionPanel1" runat="server">
        <Regions>
            <ext:Region Title="左" Width="200px" Layout="Fit" Position="Left">
                <Items>
                </Items>
            </ext:Region>
            <ext:Region Title="右" Layout="Fit" Position="Center">
                <Toolbars>
                    <ext:Toolbar runat="server" Width="100px">
                        <Items>
                            <ext:Button runat="server" Text="button1">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:RegionPanel ID="RegionPanel3" runat="server">
                        <Regions>
                            <ext:Region Title="左" Width="400px"  Layout="Fit" Position="Left">
                                <Items>
                                    <ext:Grid runat="server">
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                            <ext:Region Title="右" Layout="Fit" Position="Center">
                                <Items>
                                    <ext:Grid ID="Grid1" runat="server">
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>--%>
     <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Toolbars>
            <ext:Toolbar ID="Toolbar3" runat="server">
                <Items>
                 <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDownload" runat="server" Text="下载"  Icon="Printer" OnClick="btnDownload_Click" DisableControlBeforePostBack="false">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
            </Toolbars>
            </ext:RegionPanel>
    </form>
</body>
</html>
