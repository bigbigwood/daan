<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AnaResultSum_Window.aspx.cs" Inherits="daan.web.admin.analyse.AnaResultSum_Window" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:Grid ID="gdOrdertest" runat="server" EnableRowNumber="true" ShowBorder="false"
        ShowHeader="false" Title="检查结果明细" AutoScroll="true" AutoHeight="true">
        <Columns>
            <ext:BoundField Width="100px" DataField="DICTGROUPNAME" HeaderText="组合名称" />
            <ext:BoundField Width="100px" DataField="TESTNAME" HeaderText="项目名称" />
            <ext:BoundField Width="70px" DataField="TESTRESULT" HeaderText="检查结果" />
            <ext:BoundField Width="100px" DataField="LASTRESULT" HeaderText="上次结果" />
            <ext:BoundField Width="100px" DataField="LASTDATE" HeaderText="上次时间" />
            <ext:BoundField Width="100px" DataField="HLFLAG" HeaderText="提示" />
            <%--<ext:BoundField Width="100px" DataField="ISEXCEPTION" HeaderText="是否异常" />--%>
            <ext:BoundField Width="100px" DataField="HLHINT" HeaderText="是否异常" />
            <ext:BoundField Width="100px" DataField="TEXTSHOW" HeaderText="参考范围" />
        </Columns>
    </ext:Grid>
</asp:Content>
