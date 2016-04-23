<%@ Page Title="分点维护" Language="C#" AutoEventWireup="true" CodeBehind="DictLabInfo.aspx.cs"
    Inherits="daan.web.admin.dict.DictLabInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .inline
        {
            margin-right: 5px;
            float: left;
        }
        .btnCSS
        {
            width: 45px;
            height: 30px;
            margin-left: 8px;
            margin-top: 10px;
        }
        .inline01
        {
            color: Red;
            margin-right: 5px;
            float: right;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" ValidateForms="Form1"
                        CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelAll" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDelAll_Click">
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
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Margins="5 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="30px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:SimpleForm ID="SimpleForm4" ShowBorder="false" ShowHeader="false" runat="server"
                                        EnableCollapse="True" LabelWidth="50px" LabelAlign="Right">
                                        <Items>
                                            <ext:TwinTriggerBox ID="btSearch" Label="关键字" Trigger1Icon="Clear" ShowTrigger1="False"
                                                Trigger2Icon="Search" runat="server" OnTrigger1Click="btSearch_Trigger1Click"
                                                EmptyText="请输入分点名称" OnTrigger2Click="btSearch_Trigger2Click">
                                            </ext:TwinTriggerBox>
                                        </Items>
                                    </ext:SimpleForm>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gvList" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        EnableTextSelection="true" Title="Grid" ShowHeader="false" DataKeyNames="Dictlabid,Labname"
                                        EnableRowClick="true" Height="425" EnableMultiSelect="false" OnRowClick="gvList_RowClick"
                                        AllowPaging="true" PageSize="20" IsDatabasePaging="true" OnPageIndexChange="gvList_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="120px" DataField="Labname" DataFormatString="{0}" HeaderText="分点名称"
                                                DataToolTipField="Labname" />
                                            <ext:BoundField Width="120px" DataField="Addres" DataFormatString="{0}" HeaderText="地点"
                                                DataToolTipField="Addres" />
                                            <ext:BoundField Width="100px" DataField="Phone" DataFormatString="{0}" HeaderText="联系电话"
                                                DataToolTipField="Phone" />
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
                    <ext:Panel ID="SimpleFormEdit" ColumnWidth="100%" Layout="Fit" BodyPadding="5px"
                        AutoHeight="true" Title="当前状态-新增" EnableBackgroundColor="false" runat="server"
                        ShowHeader="true" Height="460px">
                        <Items>
                            <ext:SimpleForm ID="SimpleForm3" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false"
                                runat="server" EnableCollapse="True" LabelWidth="100px" LabelAlign="Right">
                                <Items>
                                    <ext:Form ID="Form1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px"
                                        LabelWidth="100px" AutoWidth="true">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="tbxLABNAME" Label="分点名称" FocusOnPageLoad="true" Required="true"
                                                        MaxLength="25" ShowRedStar="true" runat="server">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="tbxADDRES" Label="地址" Required="true" MaxLength="250" ShowRedStar="true"
                                                        runat="server">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:NumberBox Label="联系电话" ID="tbxPHONE" runat="server" MinValue="0" MaxLength="15"
                                                        NoDecimal="true" NoNegative="True" Required="True" ShowRedStar="True" />
                                                    <ext:NumberBox Label="排序" ID="tbxDISPLAYORDER" NoNegative="True" Required="true"
                                                        MaxLength="4" MinValue="0" RegexPattern="NUMBER" ShowRedStar="true" runat="server">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="dropSheng" Label="省份" ShowRedStar="true" CompareType="String"
                                                        Resizable="True" CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择省份！"
                                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropSheng_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                    <ext:DropDownList ID="dropShi" Label="地区市" ShowRedStar="true" CompareType="String"
                                                        Resizable="True" CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择地区市！"
                                                        runat="server" AutoPostBack="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="tbxLabCode" Label="分点简称" Required="true" ShowRedStar="true" MaxLength="5"
                                                        runat="server">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="tbxCONTACTMAN" Label="联系人" runat="server" MaxLength="30">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="tbxFax" Label="传真" MaxLength="20" runat="server">
                                                    </ext:TextBox>
                                                    <ext:TextBox ID="tbxEsiteName" Label="英文名称" MaxLength="100" runat="server">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextBox ID="tbxWebSite" Label="网站名称" MaxLength="100" runat="server">
                                                    </ext:TextBox>
                                                    <ext:NumberBox Label="邮编" ID="tbxZPCODE" RegexPattern="NUMBER" runat="server" MaxLength="15"
                                                        MinValue="0">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                                        ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                                              <Items>
                                              <table>
                                              <tr>
                                              <td>
                                              <ext:Label ID="Label1" Text="分点图片" Label="*" ShowRedStar="true"   runat="server"></ext:Label>

</td>
                                              <td><span style="color:Red;">*</span>：</td>
                                              <td><asp:FileUpload ID="FileUp" runat="server"  Width="170px" />
</td>
                                              </tr>
                                              <tr><td colspan="3"><asp:Label ID="Label2" runat="server" ForeColor="Red" Text="只能上传：JPG,GIF,PNG,BMP格式图片！"></asp:Label>
</td></tr>
                                              </table>
                                               </Items>
                                                    </ext:ContentPanel>
                                                    <ext:Image ID="imgA" runat="server" ImageWidth="150px" ImageHeight="120px" Label="图像"
                                                        EnableAjax="true">
                                                    </ext:Image>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TextArea runat="server" Label="报告尾页内容" ID="Txa" EmptyText="文本框的高度会自动扩展" Height="100"
                                                        AutoGrowHeight="true" AutoGrowHeightMin="100" AutoGrowHeightMax="100" MaxLength="1000">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:SimpleForm>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WinLibraryEdit" Title="编辑" Popup="false" EnableIFrame="true" runat="server"
        CloseAction="HidePostBack" EnableConfirmOnClose="true" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="760px" Height="450px">
    </ext:Window>
    </form>
</body>
</html>
