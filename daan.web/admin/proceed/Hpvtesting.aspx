<%@ Page Language="C#" AutoEventWireup="true" Title="HPV检测耗材初始化" EnableViewStateMac="false"
    EnableEventValidation="false" CodeBehind="Hpvtesting.aspx.cs" Inherits="daan.web.admin.proceed.Hpvtesting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" EnableBackgroundColor="false">
        <Regions>
            <ext:Region ID="Region2" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Left" Width="400px" runat="server" ShowBorder="false">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server" EnableBackgroundColor="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Title="项目信息" Height="120px" ShowHeader="true" Icon="Outline" EnableCollapse="true"
                                Layout="Fit" Position="Top" runat="server" ShowBorder="true">
                                <Items>
                                    <ext:Form ID="Form2" runat="server" EnableBackgroundColor="false" BodyPadding="5 0 0 0"
                                        LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                                        <Rows>
                                            <ext:FormRow ID="FormRow5" runat="server">
                                                <Items>
                                                    <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="分点"
                                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow1" runat="server">
                                                <Items>
                                                    <ext:DropDownList runat="server" ID="Drop_Ctcustomer" Resizable="True" ShowLabel="true"
                                                        Label="单位">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow2" runat="server">
                                                <Items>
                                                    <ext:DropDownList runat="server" Label="选择项目" ID="Drop_Dicttestitem" Resizable="True"
                                                        ShowLabel="true">
                                                    </ext:DropDownList>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region4" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Title="条码扫描" BodyPadding="5 0 0 0" ShowHeader="true" Icon="Outline" EnableCollapse="true"
                                Layout="Fit" Position="Center" runat="server" ShowBorder="true">
                                <Items>
                                    <ext:Form ID="Form3" runat="server" EnableBackgroundColor="false" BoxMargin="5px"
                                        LabelWidth="80px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                                        <Rows>
                                            <ext:FormRow ID="FormRow3" runat="server">
                                                <Items>
                                                    <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbxbarcode"
                                                        Label="条码扫描" OnTriggerClick="tbxbarcode_TriggerClick">
                                                    </ext:TriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region5" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                Height="300px" Title="条码段生成" BodyPadding="5 0 0 0" ShowHeader="true" Icon="Outline"
                                EnableCollapse="true" Layout="Fit" Position="Bottom" runat="server" ShowBorder="true">
                                <Items>
                                    <ext:Form ID="Form4" runat="server" EnableBackgroundColor="false" LabelWidth="80px"
                                        LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                                        <Rows>
                                            <ext:FormRow ID="FormRow4" runat="server">
                                                <Items>
                                                    <ext:NumberBox ID="TextStar" runat="server" Label="起始条码" Width="200px" Required="true"
                                                        ShowRedStar="true" RequiredMessage="必填项,且最大值为12个数字" MaxLength="12" MaxLengthMessage="最大条码号长度为12个数字">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow7" runat="server">
                                                <Items>
                                                    <ext:NumberBox ID="TextEnd" runat="server" Label="结束条码" Width="200px" Required="true"
                                                        ShowRedStar="true" RequiredMessage="必填项,且最大值为12个数字" MaxLength="12" MaxLengthMessage="最大条码号长度为12个数字">
                                                    </ext:NumberBox>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow ID="FormRow6" runat="server">
                                                <Items>
                                                    <ext:NumberBox ID="txtSpaceNum" runat="server" Text="100" Label="数字间隔" Required="true"
                                                        ShowRedStar="true" RequiredMessage="必填项,间隔最大不能超过1000" MaxValue="1000" MaxLengthMessage="间隔最大不能超过1000">
                                                    </ext:NumberBox>
                                                    <ext:Button ID="Create" runat="server" Text="生成" ValidateForms="Form4" OnClick="Create_Click">
                                                    </ext:Button>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="Region3" Title="列表" ShowHeader="false" Layout="Fit" Position="Center"
                EnableBackgroundColor="false" runat="server">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="false" ShowHeader="false"
                        EnableTextSelection="true" EnableRowNumber="true" AutoHeight="true" runat="server"
                        Width="800px">
                        <Columns>
                            <ext:BoundField Width="100px" DataField="Instrumentsbarcode" HeaderText="耗材条码" DataToolTipField="Instrumentsbarcode" />
                            <ext:BoundField Width="200px" DataField="Customername" HeaderText="客户名称" DataToolTipField="Customername" />
                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="检测项目" DataToolTipField="Testname" />
                            <ext:BoundField Width="100px" DataField="Instenterby" HeaderText="初始化操作人" />
                            <ext:BoundField Width="100px" DataField="Instcreatedate" HeaderText="耗材激活时间" DataFormatString="{0:yyyy-MM-dd}" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
