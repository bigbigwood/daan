<%@ Page Title="异常建议规则公式维护" Language="C#" AutoEventWireup="true" CodeBehind="DictRuleFormular.aspx.cs"
    Inherits="daan.web.admin.dict.DictRuleFormular" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <script src="../../js/Resources/JQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.insertContent.js" type="text/javascript"></script>
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
    <!--公式编辑-->
</head>
<body>
    <script type="text/javascript">
        //获取录入值
        function GetCup(obj, type, b) {

            var strValue = "";
            if (b == 1) {
                strValue = obj;
            } else { strValue = obj.value; }
            if (type == 1)
                strValue = "[" + strValue + "]";
            if (strValue == "并且")
                strValue = "&&";
            if (strValue == "或")
                strValue = "||";
            if (strValue == "包含")
                strValue = ".Contains(\"\")";
            $("#<%=txtruleformularcontent.ClientID %>").insertContent(strValue);
        }
    </script>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" CssClass="inline" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" CssClass="inline"
                        OnClick="btnSave_Click" ValidateForms="SimpleFormEdit">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDel" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnExport" runat="server" Text="导出Excel" Icon="PageExcel" CssClass="inline"
                        EnableAjax="false" OnClick="btnExport_Click" DisableControlBeforePostBack="false">
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
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Top" Height="85px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form18" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        Title="Form" ShowBorder="false" ShowHeader="false" LabelWidth="50">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlDictlabSearch" runat="server" Label="分点" Resizable="True"
                                                        EnableEdit="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:DropDownList ID="ddlgoupLibrary" runat="server" Label="科室" AutoPostBack="true"
                                                        Resizable="True" EnableEdit="true" OnSelectedIndexChanged="ddlgoupLibrary_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TwinTriggerBox ID="ttbSearch" runat="server" Label="关键字" Trigger1Icon="Clear"
                                                        Trigger2Icon="Search" ShowTrigger1="False" OnTrigger2Click="ttbSearch_Trigger2Click">
                                                    </ext:TwinTriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gdFormularName" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        Title="Grid" ShowHeader="false" Width="220px" Height="440" DataKeyNames="Dictruleformularid"
                                        OnRowClick="gdFormularName_RowClick" AllowPaging="true" EnableRowClick="true"
                                        EnableMultiSelect="false" IsDatabasePaging="true" OnPageIndexChange="gdFormularName_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="60px" DataField="Dictrulecode" HeaderText="规则代码" />
                                            <ext:BoundField Width="250px" DataField="Formularname" HeaderText="公式名称" />
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
                    <ext:Form ID="SimpleFormEdit" runat="server" ShowBorder="false" ShowHeader="false"
                        BodyPadding="5px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right"
                        LabelWidth="100" Title="当前状态：编辑">
                        <Rows>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtFormularName" Label="公式名称" ShowLabel="true" Text="" Required="true" ShowRedStar="true" runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtdictrulecode" Label="规则代码" ShowLabel="true" Text="" runat="server"
                                        Required="true" ShowRedStar="true">
                                    </ext:TextBox>
                                    <ext:DropDownList ID="ddldiagnosisname" EnableEdit="true" Label="得出异常名称" ShowLabel="true"
                                        Resizable="True" runat="server">                                        
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="ddlDictlab" runat="server" AutoPostBack="False" Label="分点名称"
                                        Resizable="True" ShowLabel="true">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextBox ID="txtagelow" Label="年龄(低值)" ShowLabel="true" Text="" Width="100" runat="server" Required="true" ShowRedStar="true">
                                    </ext:TextBox>
                                    <ext:TextBox ID="txtagehight" Label="-年龄(高值)" ShowLabel="true" Text="" Width="100"
                                        runat="server" Required="true" ShowRedStar="true">
                                    </ext:TextBox>
                                    <ext:DropDownList runat="server" ID="ddlageunit" Resizable="True" Label="年龄单位" Width="100"
                                        ShowLabel="true">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList runat="server" ID="ddlsex" Label="性别" ShowLabel="true" Width="100"
                                        Resizable="True">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="ddlmarry" Resizable="True" Label="婚姻" Width="100"
                                        ShowLabel="true">
                                    </ext:DropDownList>
                                    <ext:TextBox ID="txtdisplayorder" Label="次序" ShowLabel="true" Text="1" runat="server">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea ID="txtruleformulardescription" Label="公式描述" ShowLabel="true" Readonly="true"
                                        AutoGrowHeightMax="50" AutoGrowHeightMin="50" Text="" Height="50px" runat="server">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:TextArea ID="txtruleformularcontent" Label="公式内容" ShowLabel="true" Text="" Height="50px"
                                        AutoGrowHeightMax="50" AutoGrowHeightMin="50" runat="server" Required="true" ShowRedStar="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:DropDownList ID="ddlPhysicalGourp1" AutoPostBack="true" runat="server" Resizable="True"
                                        Label="科室" Required="true" ShowLabel="true" OnSelectedIndexChanged="ddlPhysicalGourp1_SelectedIndexChanged">
                                    </ext:DropDownList>
                                    <ext:DropDownList ID="ddlTestitem" Label="增加项目" Resizable="True" AutoPostBack="true"
                                        ShowLabel="true" runat="server" OnSelectedIndexChanged="ddlTestitem_SelectedIndexChanged"
                                        EnableEdit="true">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow>
                                <Items>
                                    <ext:ContentPanel ID="ContentPanel9" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                                        ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                                        <table style="vertical-align:middle;">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td width="80"></td>
                                                            <td>
                                                                <input id="btn_10" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="0" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_1" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="1" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_2" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="80"></td>
                                                            <td>
                                                                <input id="btn_3" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="3" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_4" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="4" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_5" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="5" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="80"></td>
                                                            <td>
                                                                <input id="btn_6" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="6" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_7" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="7" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_8" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="8" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="80"></td>
                                                            <td>
                                                                <input id="btn_9" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="9" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_11" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="." />
                                                            </td>
                                                            <td>
                                                                <ext:Button ID="btnVerify" runat="server" Text="检验" OnClick="btnVerify_Click" CssClass="btnCSS">
                                                                </ext:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="50">
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input id="Button1" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 0px;
                                                                    margin-top: 10px;" type="button" value="+" />
                                                            </td>
                                                            <td>
                                                                <input id="Button2" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="-" />
                                                            </td>
                                                            <td>
                                                                <input id="Button3" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="*" />
                                                            </td>
                                                            <td>
                                                                <input id="Button4" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="/" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input id="Button5" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 0px;
                                                                    margin-top: 10px;" type="button" value="(" />
                                                            </td>
                                                            <td>
                                                                <input id="Button6" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value=")" />
                                                            </td>
                                                            <td>
                                                                <input id="Button7" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="[" />
                                                            </td>
                                                            <td>
                                                                <input id="Button8" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="]" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input id="Button9" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 0px;
                                                                    margin-top: 10px;" type="button" value="&lt;" />
                                                            </td>
                                                            <td>
                                                                <input id="Button10" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px;
                                                                    margin-left: 8px; margin-top: 10px;" type="button" value="=" />
                                                            </td>
                                                            <td>
                                                                <input id="Button11" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px;
                                                                    margin-left: 8px; margin-top: 10px;" type="button" value="&gt;" />
                                                            </td>
                                                            <td>
                                                                <input id="btn_012" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px; margin-left: 8px;
                                                                    margin-top: 10px;" type="button" value="!=" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input id="Button014" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px;
                                                                    margin-left: 0px; margin-top: 10px;" type="button" value="并且" />
                                                            </td>
                                                            <td>
                                                                <input id="Button015" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px;
                                                                    margin-left: 8px; margin-top: 8px;" type="button" value="或" />
                                                            </td>
                                                            <td>
                                                                <input id="Button016" onclick="GetCup(this,0,0);" style="width: 45px; height: 30px;
                                                                    margin-left: 8px; margin-top: 10px;" type="button" value="包含" />
                                                            </td>
                                                            <td>
                                                                <ext:Button ID="btnClear" runat="server" Text="清空" OnClick="btnClear_Click"  CssClass="btnCSS">
                                                                </ext:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </td>
                                                    </tr>
                                                    </table>
                                                    <asp:HiddenField runat="server" ID="hdRuleFormularID"></asp:HiddenField>
                                    </ext:ContentPanel>
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
