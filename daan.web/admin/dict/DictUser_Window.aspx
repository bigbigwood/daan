<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.Master" AutoEventWireup="true"
 CodeBehind="DictUser_Window.aspx.cs" Inherits="daan.web.admin.dict.DictUser_Window" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ext:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px" EnableBackgroundColor="true">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSaveRefresh" Text="保存-关闭" ValidateForms="SimpleForm1" runat="server" Icon="SystemSaveNew"
                        OnClick="btnSaveRefresh_Click">
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Items>
            <ext:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" Height="400px">
                <Items>
                   <ext:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" EnableBackgroundColor="false" 
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True" Height="350px">
                        <Items>
                        <ext:DropDownList runat="server" Label="分点" ID="Drop_Dictlab" ShowRedStar="true"  FocusOnPageLoad="true"
                                CompareType="String" Resizable="True" Width="200px" CompareValue="-1" CompareOperator="NotEqual"
                                AutoPostBack="true">
                            </ext:DropDownList>
                            <ext:DropDownList runat="server" Label="科室" ID="Drop_DictLabDepTid" ShowRedStar="true"
                                CompareType="String" Resizable="True" Width="200px" CompareValue="-1" CompareOperator="NotEqual"
                                AutoPostBack="true">
                            </ext:DropDownList>
                            <ext:RadioButtonList runat="server" Label="是否可用" Width="150px" ID="radlIsactive">
                                <ext:RadioItem Selected="True" Text="可用" Value="1"></ext:RadioItem>
                                <ext:RadioItem Text="不可用" Value="0"></ext:RadioItem>
                            </ext:RadioButtonList>
                    </Items>
                    </ext:SimpleForm>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
   
</asp:Content>
