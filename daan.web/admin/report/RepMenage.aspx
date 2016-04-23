<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepMenage.aspx.cs" Inherits="daan.web.admin.report.RepMenage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function showReport(reportType, order_num) {
            window.open("./RepShowView.aspx?reportType=" + reportType + "&&order_num=" + order_num);
        }
    </script>
    <style type="text/css">
        table.x-table-layout td
        {
            padding: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <ext:Toolbar ID="Toolbar2" Position="Top" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" OnClick="btnAdd_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="SystemSaveNew" OnClick="btnSave_Click" ValidateForms="SimpleFormEdit">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnDelete" runat="server" Text="删除" Icon="Delete" OnClick="btnDelete_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" CollapseMode="Mini" ShowHeader="false" Width="320px" Margins="5 0 0 0" Icon="Outline" 
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:Grid ID="gridReportList" runat="server" Width="250px" Height="320px"
                        EnableBackgroundColor="true" EnableRowNumber="true" Title="报告单模板列表" DataKeyNames="Dictreporttemplateid"
                        OnRowClick="gridReportList_RowClick" EnableRowClick="True" EnableMultiSelect="False"
                        EnableCheckBoxSelect="false">
                        <Columns>
                            <ext:BoundField Width="130px" DataField="Dictreporttemplateid" DataFormatString="{0}"
                                HeaderText="编码" Hidden="true" />
                            <ext:BoundField Width="120px" DataField="Templatename" DataFormatString="{0}" HeaderText="模板名称" />
                            <ext:BoundField Width="130px" DataField="Templatecode" DataFormatString="{0}" HeaderText="模板代码" />
                            <ext:BoundField Width="120px" DataField="Papersize" DataFormatString="{0}" Hidden="true"
                                HeaderText="纸张大小" />
                            <ext:BoundField Width="120px" DataField="Singleappraise" DataFormatString="{0}" Hidden="true"
                                HeaderText="是否单独评价" />
                            <ext:BoundField Width="120px" DataField="Reporttype" DataFormatString="{0}" Hidden="true"
                                HeaderText="报告类型" />
                            <ext:BoundField Width="120px" DataField="Remark" DataFormatString="{0}" Hidden="true"
                                HeaderText="备注" />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="5 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:Form ID="SimpleFormEdit" ShowBorder="false" ShowHeader="true" EnableBackgroundColor="false"
                        runat="server" LabelWidth="90px" Title="当前状态：新增" LabelAlign="Right">
                        <Rows>
                            <ext:FormRow runat="server">
                                <Items>
                                    <ext:TextBox ID="txtTemName" runat="server" Label="模板名称" Text="" Required="true" ShowRedStar="true" MaxLength="20">
                                    </ext:TextBox>
                                    <ext:TextBox ID="txtTemCode" runat="server" Label="模板代码" Text="" Required="true" ShowRedStar="true" MaxLength="50">
                                    </ext:TextBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:TextBox ID="txtPageSize" runat="server" Label="纸张大小" Text="" Required="true" ShowRedStar="true"  MaxLength="20">
                                    </ext:TextBox>
                                    <ext:DropDownList ID="dropReportType" AutoPostBack="true" Resizable="True" runat="server" Label="报告类型">
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:CheckBox ID="cbSingleAppraise" runat="server" Label="是否单独评价" Text="是">
                                    </ext:CheckBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                        <Rows>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:TextBox ID="txtRemark" runat="server" Label="备注" Text=""  MaxLength="50">
                                    </ext:TextBox>
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
