<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnaResultSum_AddDictdiagnosisWin.aspx.cs"
    Inherits="daan.web.admin.analyse.AnaResultSum_AddDictdiagnosisWin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Regions>
            <ext:Region ID="Region1" runat="server" Position="Left" ShowHeader="false" ShowBorder="false"
                Split="true" Layout="Fit" Title="Left Region" Width="240px">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:ToolbarText ID="ToolbarText1" runat="server" Text="诊断名称">
                            </ext:ToolbarText>
                            <ext:TwinTriggerBox ID="ttbSearch" runat="server" Label="Label" Trigger1Icon="Clear"
                                OnTrigger2Click="ttbSearch_Trigger2Click" ShowTrigger1="false" Trigger2Icon="Search">
                            </ext:TwinTriggerBox>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Grid ID="gdDiagnosis" runat="server" ShowHeader="false" EnableRowNumber="true"
                        OnRowClick="gdDiagnosis_RowClick" AllowPaging="true" IsDatabasePaging="true"
                        EnableCheckBoxSelect="true" EnableMultiSelect="false" AutoPostBack="true" OnPageIndexChange="gdDiagnosis_PageIndexChange"
                        Title="Grid" DataKeyNames="Dictdiagnosisid">
                        <Columns>
                            <ext:BoundField DataField="Diagnosisname" HeaderText="诊断名称" Width="150px" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Layout="Fit"
                Title="Center Region">
                <Toolbars>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:Button ID="btnSave" runat="server" Text="添加" Icon="CdrAdd" OnClick="btnSave_Click">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </Toolbars>
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="true"
                        ShowHeader="false" ShowBorder="false" LabelAlign="Right" Title="Form">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:TextBox ID="txtdiagnosisname" runat="server" Readonly="true" Label="诊断名称">
                                    </ext:TextBox>
                                    <ext:TextBox ID="txtdiagnosiscode" runat="server" Label="疾病代码" Readonly="true">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:DropDownList ID="ddldiagnosistype" runat="server" Readonly="true" Resizable="True" Label="疾病分类">
                                    </ext:DropDownList>
                                    <ext:CheckBox ID="chbisdisease" runat="server" ShowEmptyLabel="true" Readonly="true"
                                        Text="是否疾病">
                                    </ext:CheckBox>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow4" runat="server">
                                <Items>
                                    <ext:TextArea ID="tadiseasedescription" runat="server" Height="50px" Readonly="true"
                                        Label="医学解释">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow5" runat="server">
                                <Items>
                                    <ext:TextArea ID="tadiseasecause" runat="server" Height="50px" Readonly="true" Label="常见原因">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow6" runat="server">
                                <Items>
                                    <ext:TextArea ID="tasuggestion" runat="server" Height="50px" Label="建议" Readonly="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:TextArea ID="taengdiseasedescription" runat="server" Height="50px" Label="医学解释(英文)"
                                        Readonly="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow7" runat="server">
                                <Items>
                                    <ext:TextArea ID="taengdiseasecause" runat="server" Height="50px" Label="常见原因(英文)"
                                        Readonly="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow8" runat="server">
                                <Items>
                                    <ext:TextArea ID="taengsuggestion" runat="server" Height="50px" Label="建议(英文)" Readonly="true">
                                    </ext:TextArea>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region></Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
