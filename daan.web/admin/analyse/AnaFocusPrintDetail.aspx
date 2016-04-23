<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnaFocusPrintDetail.aspx.cs"
    Inherits="daan.web.admin.analyse.AnaFocusPrintDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .star
        {
            padding-left: 42px;
            padding-bottom: 5px;
        }
        .bot
        {
            padding-left: 84px;
            padding-bottom: 5px;
        }
    </style>
    <script type="text/javascript">
        function Print() {
            document.getElementById('divnoPrint').style.display = "none";
            window.print();
            document.getElementById('divnoPrint').style.display = "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 910px;">
        <div id="divLogo" style="float: left; padding-left: 15px">
            <img src="/images/LOGOByJianCe.jpg" style="width: 102px; height: 145px;" alt="LOGO" />
        </div>
        <div id="MianTitle">
            <div style="padding-top: 50px; font-size: 38px;">
                <div style="text-align: center;"><b runat="server" id="labName">广州达安临床检验中心</b></div>
                <div style="text-align: center;"><b runat="server" id="reportType">标本迟发通知</b></div>
            </div>
        </div>
        <br />
        <div style="padding-left: 25px; padding-top: 10px; font-size: 20px;">
            <div>
                尊敬的“<asp:Label ID="lblCustomername" runat="server"></asp:Label>”：</div><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 贵司 <asp:Label ID="lblDate" runat="server"></asp:Label> 日所送患者 <asp:Label runat="server" ID="lblRealname"></asp:Label> 条码号为：<asp:Label runat="server" ID="lblBarcode"></asp:Label>
            的标本的下列项目因以下原因<asp:Label runat="server" ID="lblType">迟发</asp:Label>。
            <div class="bot">
                项目：<asp:Label runat="server" ID="lblProductName"></asp:Label>
            </div>
            <div class="bot">
                原因：<asp:Label runat="server" ID="lblReason"></asp:Label>
            </div>
            <br />
            <div class="star" runat="server" id="divDelay">
                <%--预计发单日期：<asp:Label runat="server" ID="lblDelayTime"></asp:Label>--%>
            </div>
            <div class="star">
                不便之处，敬请谅解。
            </div>
        </div>
        <div style="padding-left: 625px; padding-top: 30px; font-size: 20px;">
            <asp:Label runat="server" ID="lblLabname">广州达安临床检验中心</asp:Label>
            <br />
            电话：400-830-3620
            <br />
            日期：<asp:Label runat="server" ID="lblDatetime"></asp:Label>
        </div>
        <div id="divnoPrint" style="text-align: center; padding-top: 20px;">
            <input type="button" id="btnPrint" value="打   印" onclick="javascript:Print()" style="height: 25px;" />
        </div>
    </div>
    </form>
</body>
</html>

