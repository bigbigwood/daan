<%@ Page Title="基础字典明细维护" Language="C#" AutoEventWireup="true" CodeBehind="DictionaryItem.aspx.cs"
    Inherits="daan.web.admin.dict.DictionaryItem" %>

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
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini" Margins="5 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="60px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        runat="server" EnableCollapse="True" LabelWidth="60px" LabelAlign="Right">
                                        <Items>
                                            <ext:DropDownList runat="server" ID="ddlDictLibrary1" Resizable="True" Label="字典类型" 
                                                ShowLabel="true" EnableEdit="true" Width="160px">
                                            </ext:DropDownList>
                                            <ext:TwinTriggerBox ID="btSearch" Trigger1Icon="Clear" ShowTrigger1="False" EmptyText=""
                                                Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true" OnTrigger1Click="btSearch_Trigger1Click"
                                                OnTrigger2Click="btSearch_Trigger2Click" Width="160px">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="true" ShowHeader="false"
                                        Height="405px" EnableCheckBoxSelect="false" AutoHeight="true" AllowPaging="true"  AutoScroll="true"
                                        EnableRowClick="true" runat="server" EnableRowNumber="true"
                                        DataKeyNames="Dictlibraryitemid,Dictlibraryid,Wubicode,Pinyincode,Fastcode,Itemname,Displayorder,Remark,Isactive"
                                        IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange" OnRowClick="gvList_RowClick">
                                        <Columns>
                                            <ext:BoundField Width="60px" DataField="Dictlibraryitemid" DataFormatString="{0}"
                                                HeaderText="编号" />
                                            <ext:BoundField Width="100px" DataField="Itemname" DataFormatString="{0}" HeaderText="名称" />
                                            <ext:BoundField Width="100px" DataField="Libraryname" DataFormatString="{0}" HeaderText="字典类别" />
                                           <%-- <ext:BoundField Width="60px" DataField="Wubicode" DataFormatString="{0}" HeaderText="五笔代码" />
                                            <ext:BoundField Width="60px" DataField="Pinyincode" DataFormatString="{0}" HeaderText="拼音码" />
                                            <ext:BoundField Width="60px" DataField="Fastcode" DataFormatString="{0}" HeaderText="自定义码" />--%>
                                            <ext:CheckBoxField Width="60px" DataField="BoolIsactive" RenderAsStaticField="true"
                                                SortField="BoolIsactive" HeaderText="是否可用" />
                                            <ext:BoundField Width="80px" DataField="Createdate" HeaderText="创建时间" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:Form ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" BodyPadding="5px" Title="当前状态-新增"
                        runat="server" LabelWidth="70px" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtItemname" Label="名称" Required="true" ShowRedStar="true" runat="server" MaxLength="90"
                                        AutoPostBack="true">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                           <%-- <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtWubicode" Label="五笔代码" Required="true" ShowRedStar="true" runat="server" MaxLength="20">
                                    </ext:TextBox>
                                    <ext:TextBox ID="txtPinyincode" Label="拼音码" Required="true" ShowRedStar="true" runat="server" MaxLength="20">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>--%>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" Label="字典类别" ID="ddlDictLibrary2" Resizable="True">
                                    </ext:DropDownList>
                                    <ext:CheckBox ID="chkIsactive" Label="是否可用" ShowLabel="true" Checked="true" runat="server">
                                    </ext:CheckBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                  <%--  <ext:TextBox ID="txtFastcode" Label="自定义码" Required="true" ShowRedStar="true" runat="server" MaxLength="20">
                                    </ext:TextBox>--%>
                                    <ext:NumberBox ID="txtDisplayorder" Label="排序" Required="true" ShowRedStar="true" MaxLength="5"
                                        runat="server" NoDecimal="true" NoNegative="True">
                                    </ext:NumberBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea runat="server" ID="txtRemark" EmptyText="" Height="100" AutoGrowHeight="true" MaxLength="100"
                                        AutoGrowHeightMin="100" AutoGrowHeightMax="200" Label="备注" ShowLabel="true">
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
