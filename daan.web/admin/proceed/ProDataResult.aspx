<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProDataResult.aspx.cs"
    Inherits="daan.web.admin.proceed.ProDataResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看结果</title>
    <style>
        .right
        {
            margin-right: 5px;
            float: left;
            margin-top: 5px;
            text-align: right;
        }
        .force
        {
            border-style: none;
            border-color: White;
            width: auto;
            height: 100%;
        }
        .table
        {
            border: 0px solid #99BBE8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" />
    <ext:Panel ID="pnl1" runat="server" ShowHeader="false" Layout="Row">
        <Items>
            <ext:ContentPanel ID="contentPanel1" runat="server" ShowHeader="false" ShowBorder="false">
                <table style="height: 200; width: 100%;">
            <tr>
                <td class="right">
                    <ext:Label ID="lblOrdernum" runat="server" Text="体检流水号:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxOrdernum" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblCustomer" runat="server" Text="体检单位:" ColumnWidth="80">
                    </ext:Label>
                </td>
                <td style="width: 170px; text-align: left;">
                    <ext:TextBox ID="tbxCustomer" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    <ext:Label ID="lblName" runat="server" Text="姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxName" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblSex" runat="server" Text="性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxSex" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    <ext:Label ID="lblAge" runat="server" Text="年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxAge" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
                <td class="right">
                    <ext:Label ID="lblEnterdate" runat="server" Text="登记时间:">
                    </ext:Label>
                </td>
                <td>
                    <ext:TextBox ID="tbxEnterdate" runat="server" Enabled="false">
                    </ext:TextBox>
                </td>
            </tr>
        </table>
            </ext:ContentPanel>
        </Items>
    </ext:Panel>
    <ext:Panel runat="server" ShowHeader="False" Layout="Row" ID="panel2" ShowBorder="false">
        <Items>
            <ext:Grid runat="server" AutoScroll="true" OnRowDataBound="gvList_RowDataBound" Title="表格"
                ShowHeader="False" AutoWidth="True" Height="342px" ID="gvList">
                <Columns>
                    <ext:BoundField Width="100px" DataField="Labdeptname" HeaderText="科室" />
                    <ext:BoundField Width="200px" DataField="Testname" HeaderText="项目" />
                    <ext:BoundField Width="60px" DataField="Isexception" HeaderText="异常" />
                    <ext:BoundField Width="60px" DataField="Testresult" HeaderText="检验结果" />
                    <ext:BoundField Width="100px" DataField="Hlflag" HeaderText="提示" />
                    <ext:BoundField Width="50px" DataField="Unit" HeaderText="单位" />
                    <ext:BoundField Width="60px" DataField="Lastresult" HeaderText="上次结果" />
                    <ext:BoundField Width="100px" DataField="Lastdate" HeaderText="上次时间" />
                    <ext:BoundField Width="100px" DataField="Textshow" HeaderText="参考范围" />
                </Columns>
            </ext:Grid>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
