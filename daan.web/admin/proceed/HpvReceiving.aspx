<%@ Page Language="C#" AutoEventWireup="true" Title="HPV标本接收" EnableViewStateMac="false"
    EnableEventValidation="false" CodeBehind="HpvReceiving.aspx.cs" Inherits="daan.web.admin.proceed.HpvReceiving" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <%--  <link href="../../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />--%>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" EnableBackgroundColor="false">
        <Regions>
            <ext:Region ID="Region2" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                Position="Top" Height="35px" runat="server" ShowBorder="false">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="100px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbxinstrumentsbarcode" MinLength="5" MinLengthMessage="请扫描耗材号"
                                        Label="耗材条码扫描" OnTriggerClick="tbxinstrumentsbarcode_TriggerClick" Width="150px">                                        
                                    </ext:TriggerBox>
                                    <ext:TriggerBox runat="server" ShowTrigger="False" TriggerIcon="Search" ID="tbxbarcode" MinLength="5" MinLengthMessage="请扫描耗材号"
                                        Label="标本条码扫描" OnTriggerClick="tbxbarcode_TriggerClick" Width="150px">
                                    </ext:TriggerBox>
                                </Items>
                            </ext:FormRow>
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region ID="Region3" Title="列表" ShowHeader="false" Layout="Fit" Position="Center"
                EnableBackgroundColor="false" runat="server">
                <Items>
                    <ext:Grid ID="gvList" Title="表格" PageSize="20" ShowBorder="false" ShowHeader="false"
                        EnableTextSelection="true" EnableRowNumber="true" AutoHeight="true" runat="server"
                        Width="800px" DataKeyNames="Hpvinstrumentsid">
                        <Columns>
                        <ext:BoundField Width="100px" DataField="Instrumentsbarcode" HeaderText="耗材条码"/>  
                            <ext:BoundField Width="100px" DataField="Barcode" HeaderText="标本条码" />
                            <ext:BoundField Width="200px" DataField="Customername" HeaderText="客户名称"/>
                            <ext:BoundField Width="200px" DataField="Testname" HeaderText="项目名称" />
                            <ext:BoundField Width="100px" DataField="Barcodeenterby" HeaderText="初始化操作人"/>
                            <ext:BoundField Width="120px" DataField="Barcodecreatedate" DataFormatString="{0:yyyy-MM-dd}"
                                HeaderText="标本条码激活时间" />                            
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    </form>
</body>
</html>
