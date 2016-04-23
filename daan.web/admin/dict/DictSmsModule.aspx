<%@ Page Title="短信模板维护" Language="C#" AutoEventWireup="true" CodeBehind="DictSmsModule.aspx.cs"
    Inherits="daan.web.admin.dict.DictSmsModule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" Position="Top" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" Text="新增" Icon="Add" runat="server" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" Text="保存" Icon="SystemSaveNew" runat="server" OnClick="btnSave_Click"
                        ValidateForms="SimpleFormEdit">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" Text="删除" Icon="Delete" runat="server" OnClick="btnDelAll_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="260px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false"
                        Height="405px" EnableCheckBoxSelect="false" AutoHeight="true" AllowPaging="true"
                        AutoScroll="true" EnableRowClick="true" runat="server" EnableRowNumber="true"
                        DataKeyNames="DictSmsModuleid" IsDatabasePaging="true" OnRowClick="gvList_RowClick">
                        <Columns>
                            <ext:BoundField Width="100px" DataField="DictSmsModuleid" HeaderText="编号" Hidden="true" />
                            <ext:BoundField Width="100px" DataField="SmsTitle" HeaderText="标题" />
                            <ext:BoundField Width="200px" DataField="SmsContent" HeaderText="短信模板内容" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:Form ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                        Title="当前状态-新增" runat="server" LabelWidth="100px" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtTitle" Label="标题" Required="true" ShowRedStar="true" runat="server"
                                        MaxLength="90" AutoPostBack="true">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea runat="server" ID="txtContent" Required="true" ShowRedStar="true" Height="100" AutoGrowHeight="true"
                                        MaxLength="100" AutoGrowHeightMin="100" AutoGrowHeightMax="200" Label="短信模板内容"
                                        ShowLabel="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
