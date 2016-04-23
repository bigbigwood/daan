<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="source_file.aspx.cs" Inherits="daan.web.source_file" %>

<%@ Register Assembly="Wilco.SyntaxHighlighter" Namespace="Wilco.Web.SyntaxHighlighting"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            font-size: 12px;
            background-color: White;
        }
        pre span
        {
            font-family: consolas,arial,tahoma,verdana,helvetica;
        }
        .shl_ln
        {
            border-right: 1px solid #999;
            padding-right: 2px;
            color: #999;
            margin-right: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:SyntaxHighlighter ID="SyntaxHighlighter1" runat="server">
    </cc1:SyntaxHighlighter>
    </form>
</body>
</html>
