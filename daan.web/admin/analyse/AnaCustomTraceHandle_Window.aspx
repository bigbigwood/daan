<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnaCustomTraceHandle_Window.aspx.cs" Inherits="daan.web.admin.analyse.AnaCustomTraceHandle_Window" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../style/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <ext:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px" EnableBackgroundColor="true">
        <Items>
            <ext:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="0px" EnableBackgroundColor="true" ShowHeader="false"
                        Title="Form">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                            <Items>
                            <ext:TextArea runat="server" Label="跟进内容" ID="tbServicecontent"  Height="100" Width="700"
                               ShowRedStar="true">
                            </ext:TextArea>
                            </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <ext:DatePicker ID="dpRerundate" runat="server" Label="预约复查时间" Width="200px">
                                </ext:DatePicker> <ext:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click">
                                </ext:Button>
                            </Items>
                            </ext:FormRow>
                          
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
