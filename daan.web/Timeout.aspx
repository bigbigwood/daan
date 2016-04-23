<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Timeout.aspx.cs" Inherits="daan.web.Timeout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        alert("操作超时,请重新登录！");
        top.location.href = '/Login.aspx';
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <table width="60%">
                <tr>
                    <td align="center">
                        <img src="images/timeout.jpg" alt="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="font-size: 12pt" cellspacing="1">
                            <tr>
                                <td colspan="4" align="center">
                                    <button onclick="top.location.href='/Login.aspx'">
                                        重新登录</button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
