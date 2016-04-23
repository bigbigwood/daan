<%@ Page Title="检查套餐维护" Language="C#" AutoEventWireup="true" CodeBehind="DictTestProduct.aspx.cs"
    Inherits="daan.web.admin.dict.DictTestProduct" %>

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
    <form id="form1" runat="server">
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
                    <ext:Button ID="btnAdd" runat="server" Text="新增" CssClass="inline" Icon="Add" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" ValidateForms="SimpleFormEdit"
                        Icon="SystemSaveNew" CssClass="inline" OnClick="btnSave_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDel" runat="server" Text="删除" Icon="Delete" CssClass="inline"
                        OnClick="btnDel_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
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
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="110px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form6" runat="server" EnableBackgroundColor="false" Title="Form" ShowBorder="false"
                                        ShowHeader="false" LabelWidth="62" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false"
                                                        Title="ContentPanel">
                                                          <table width="260px" >  
                                                          <tr>
                                                          <td style="height:23px;">
                                                              <ext:Label runat="server" Text="套餐类型：" ID="lblproductType"> </ext:Label>
                                                          </td>
                                                            <td style="text-align:left;">
                                                              <ext:DropDownList runat="server" ShowLabel="true" Resizable="True" Label="套餐类型" ID="ddlProductType" Width="180">
                                                                    <ext:ListItem Text="全部" Value="0"></ext:ListItem>
                                                                    <ext:ListItem Text="公用套餐" Value="2"></ext:ListItem>
                                                                    <ext:ListItem Text="单位套餐" Value="3"></ext:ListItem>
                                                                </ext:DropDownList>
                                                            </td>
                                                          </tr>   
                                                                              
                                                        <tr>
                                                            <td style="text-align: right; width: 60px; height: 28px;">
                                                                <ext:Label runat="server" Text="客户：" ID="lblUser">
                                                                </ext:Label>
                                                            </td>
                                                            <td  style="text-align:left;">
                                                            <ext:DropDownList runat="server" ID="DropCustomer" Resizable="True" ShowLabel="true" Label="单位" Width="180"></ext:DropDownList>                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 60px; height: 28px;">
                                                                <ext:Label runat="server" Text="可用：" ID="Label1">
                                                                </ext:Label>
                                                            </td>
                                                            <td  style="text-align:left;">
                                                               <ext:CheckBox ID="chkActive" runat="server" Checked="true" Label="可用" ShowLabel="true">
                                                    </ext:CheckBox>
                                                            </td>
                                                        </tr>                      
                                                     </table>
                                                    </ext:ContentPanel>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TwinTriggerBox runat="server" ShowTrigger1="False" Trigger1Icon="Clear" Trigger2Icon="Search"
                                                        Label="关键字" ID="ttbSearch" OnTrigger2Click="ttbSearch_Trigger2Click" Width="180">
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
                                    <ext:Grid ID="gdProductTestItem" runat="server" EnableCheckBoxSelect="false" EnableRowNumber="true"
                                        Title="Grid" ShowHeader="false" Width="250" AllowPaging="true" Height="420" AutoPostBack="true"
                                        EnableMultiSelect="false" DataKeyNames="Dicttestitemid,Testname" OnRowClick="gdProductTestItem_RowClick"
                                        IsDatabasePaging="true" OnPageIndexChange="gdProductTestItem_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField DataField="Dicttestitemid" DataFormatString="{0}" HeaderText="编号"
                                                Hidden="true" />
                                            <ext:BoundField Width="100px" DataField="Testcode" HeaderText="套餐代码" />
                                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="套餐名称"
                                                DataToolTipField="Testname" />
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
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region4" EnableSplitTip="false" CollapseMode="Default" Margins="0 0 0 0"
                                ShowHeader="false" Icon="Outline" EnableCollapse="false" Layout="Fit" Position="Top"
                                Height="130px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="SimpleFormEdit" Title="当前状态：新增" runat="server" ShowBorder="false" ShowHeader="true"
                                        BodyPadding="5px" EnableBackgroundColor="false" AutoWidth="true" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow ID="FormRow1" runat="server">
                                                <Items>
                                                    <ext:TextBox runat="server" Required="True" Label="套餐代码" ShowRedStar="True" ID="txtProductTestCode">
                                                    </ext:TextBox>
                                                    <ext:TextBox runat="server" Required="True" Label="套餐名称" ShowRedStar="True" ID="txtProductTestName">
                                                    </ext:TextBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow ID="FormRow5" runat="server">
                                                <Items>
                                                    <ext:TextBox runat="server" Required="True" Label="助记符" ShowRedStar="True" ID="txtFastCode">
                                                    </ext:TextBox>
                                                    <ext:NumberBox runat="server" Label="成交价" ID="txtStandardPrice" Enabled="false">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow ID="FormRow6" runat="server">
                                                <Items>
                                                    <ext:RadioButtonList runat="server" AutoPostBack="True" Label="是否公用套餐" ID="rdobtnPublicProduLst"
                                                        OnSelectedIndexChanged="rdobtnPublicProduLst_SelectedIndexChanged">
                                                        <ext:RadioItem Text="公用套餐" Value="publicP"></ext:RadioItem>
                                                        <ext:RadioItem Text="单位套餐" Value="unitP"></ext:RadioItem>
                                                    </ext:RadioButtonList>
                                                    <ext:DropDownList ID="ddlTestUnit" Label="体检单位" Resizable="True" ShowLabel="true" runat="server" EnableEdit="true" OnSelectedIndexChanged="ddlTestUnit_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                        <Rows>
                                            <ext:FormRow ID="FormRow7" runat="server">
                                                <Items>
                                                    <ext:CheckBox runat="server" Text="可用" Checked="True" ShowLabel="true" Label="可用"
                                                        ID="chbActive">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox runat="server" Text="必检套餐" Checked="false" Hidden="true" ShowLabel="true" Label="必检套餐" ID="chbInspection">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbIsonlyForBill" runat="server"  Text="是否只是收费项" ShowLabel="false">
                                                    </ext:CheckBox>
                                                    <ext:CheckBox ID="chbIsProject" runat="server"  Text="是否只是产品" ShowLabel="false">
                                                    </ext:CheckBox>
                                                </Items>
                                                <Items>
                                                    <ext:DropDownList ID="ddlforsex" runat="server" ShowRedStar="True" Label="测试项性别" Resizable="True">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region5" Split="true" EnableSplitTip="true" CollapseMode="Mini" ShowHeader="false"
                                Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center" runat="server"
                                BodyPadding="5px" ShowBorder="false">
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region6" Split="true" CollapseMode="Mini" Width="400px" Margins="0 0 0 0"
                                                ShowHeader="true" Icon="Outline" Layout="Fit" Title="已包含组合" Position="Left" runat="server"
                                                ShowBorder="false">
                                                <Items>
                                                    <ext:Grid runat="server" EnableRowNumber="True" EnableCheckBoxSelect="True" DataKeyNames="Dicttestitem.Dicttestitemid"
                                                        Title="Grid" ShowHeader="False" AutoScroll="True" Width="188px" Height="275px" EnableTextSelection="true"
                                                        AnchorValue="100%" ID="gdProductIncludeTestItem" OnRowDataBound="gdProductIncludeTestItem_RowDataBound1">
                                                        <Columns>
                                                            <ext:BoundField DataField="Dicttestitem.Uniqueid" DataFormatString="{0}" ColumnID="ct5"
                                                                HeaderText="全国统一码" Width="70px"></ext:BoundField>
                                                            <ext:BoundField DataField="Dicttestitem.Testname" DataFormatString="{0}" ColumnID="ct0"
                                                                HeaderText="组合名称" Width="120px" DataToolTipField="Dicttestitem.Testname"></ext:BoundField>
                                                            <ext:BoundField DataField="Dicttestitem.Price" DataFormatString="{0}" ColumnID="ct1"
                                                                HeaderText="标准价" Width="50px"></ext:BoundField>
                                                            <ext:TemplateField ColumnID="ct2" HeaderText="成交价" Width="60px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tbxFinalpricess" runat="server" Text='<%# Eval("Finalprice") %>'
                                                                        CssClass="force" Width="40"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </ext:TemplateField>
                                                            <ext:TemplateField ColumnID="ct3" HeaderText="外包" Width="60px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chbSendouttest" runat="server" Enabled="false" Checked='<%# CheckSendouttest(Eval("Issendouttest")) %>' />
                                                                </ItemTemplate>
                                                            </ext:TemplateField>
                                                            <ext:TemplateField ColumnID="ct4" HeaderText="外包单位" Width="90px">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="DropDownList1" runat="server" Resizable="True" Width="80">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </ext:TemplateField>                                                            
                                                            <ext:TemplateField ColumnID="ct6" HeaderText="项目类型" Width="60px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# CheckTestItemType(Eval("Dicttestitem.Testtype")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </ext:TemplateField>
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region ID="Region8" Split="true" EnableSplitTip="true" CollapseMode="Mini" Margins="0 0 0 0"
                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Center"
                                                runat="server" ShowBorder="false">
                                                <Items>
                                                    <ext:RegionPanel ID="RegionPanel5" runat="server" ShowBorder="false">
                                                        <Regions>
                                                            <ext:Region ID="Region9" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="30px"
                                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left"
                                                                runat="server" ShowBorder="false">
                                                                <Items>
                                                                    <ext:Form runat="server" Title="Form" ShowHeader="False" ShowBorder="False" AnchorValue="100%"
                                                                        CssClass="inline" ID="Form3">
                                                                        <Rows>
                                                                            <ext:FormRow runat="server" ID="FormRow2">
                                                                                <Items>
                                                                                    <ext:Panel runat="server" Title="Panel" ShowHeader="False" ShowBorder="False" Height="80px"
                                                                                        ID="Panel5">
                                                                                    </ext:Panel>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow runat="server" ID="FormRow3">
                                                                                <Items>
                                                                                    <ext:Button runat="server" Size="Large" Text="＞" ID="btnRemove" OnClick="btnRemove_Click">
                                                                                    </ext:Button>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow runat="server" ID="FormRow4">
                                                                                <Items>
                                                                                    <ext:Panel runat="server" Title="Panel" ShowHeader="False" ShowBorder="False" Height="50px"
                                                                                        ID="Panel6">
                                                                                    </ext:Panel>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                            <ext:FormRow runat="server" ID="FormRow15">
                                                                                <Items>
                                                                                    <ext:Button runat="server" Size="Large" Text="＜" ID="btnAppend" OnClick="btnAppend_Click">
                                                                                    </ext:Button>
                                                                                </Items>
                                                                            </ext:FormRow>
                                                                        </Rows>
                                                                    </ext:Form>
                                                                </Items>
                                                            </ext:Region>
                                                            <ext:Region ID="Region7" Split="true" CollapseMode="Mini" Margins="0 0 0 0" ShowHeader="true"
                                                                Icon="Outline" Layout="Fit" Position="Center" Title="未包含组合" runat="server" ShowBorder="false">
                                                                <Toolbars>
                                                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                                                        <Items>
                                                                            <ext:TwinTriggerBox ID="btnSearchNoIn" Trigger1Icon="Clear" ShowTrigger1="False"
                                                                                EmptyText="请输入关键字搜索" Trigger2Icon="Search" runat="server" Label="关键字" ShowLabel="true"
                                                                                OnTrigger1Click="btnSearchNoIn_Trigger1Click" OnTrigger2Click="btnSearchNoIn_Trigger2Click">
                                                                            </ext:TwinTriggerBox>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </Toolbars>
                                                                <Items>
                                                                    <ext:Grid runat="server" EnableRowNumber="True" EnableCheckBoxSelect="True" DataKeyNames="Dicttestitem.Dicttestitemid"
                                                                        Title="Grid" ShowHeader="False" Width="240px" Height="210px" ID="gdProductNotIncludeTestItem" EnableTextSelection="true"
                                                                        AllowPaging="true" IsDatabasePaging="true" PageSize="20" OnPageIndexChange="gdProductNotIncludeTestItem_PageIndexChange">
                                                                        <Columns>
                                                                            <ext:BoundField DataField="Dicttestitem.Uniqueid" DataFormatString="{0}" ColumnID="ct3"
                                                                                HeaderText="全国统一码" Width="70px"></ext:BoundField>         
                                                                            <ext:BoundField DataField="Dicttestitem.Testname" DataFormatString="{0}" ColumnID="ct0"
                                                                                HeaderText="组合名称" Width="200px" DataToolTipField="Dicttestitem.Testname"></ext:BoundField> 
                                                                            <ext:TemplateField ColumnID="ct5" HeaderText="项目类型" Width="60px">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label3" runat="server" Text='<%# CheckTestItemType(Eval("Testtype")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </ext:TemplateField>
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Region>
                                                        </Regions>
                                                    </ext:RegionPanel>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
