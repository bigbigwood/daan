<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProBulkImportManage.aspx.cs" Inherits="daan.web.admin.proceed.ProBulkImportManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>达安健康体检系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="../../images/favicon.ico" />
    <link href="../../style/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .inline
        {
            margin-right: 5px;
            float: left;
        }
        .inline01
        {
            color: Red;
            margin-right: 5px;
            float: right;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"
        HideScrollbar="true"></ext:PageManager>
    <ext:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
        <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnUploadFile"  runat="server" Icon="TelevisionIn" Text="单位批量导入(新)" OnClick="btnUploadFile_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region ID="Region2" Title="列表" Split="true" EnableSplitTip="true" CollapseMode="Mini"
                Width="260px" Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true"
                Layout="Fit" Position="Left" runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel2" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region1" Split="false" EnableSplitTip="true" CollapseMode="Mini"
                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                Height="145px" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Form ID="Form6" runat="server" EnableBackgroundColor="false" Title="Form" ShowBorder="false"
                                        ShowHeader="false" LabelWidth="62" LabelAlign="Right">
                                        <Rows>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false"
                                                        Title="ContentPanel">
                                                          <table width="260px" >
                                                          <tr>
                                                          <td style=" text-align: right; width: 60px; height: 28px;">
                                                              <ext:Label ID="Label1" runat="server" Text="选择分点："> </ext:Label>
                                                          </td>
                                                            <td style="text-align:left;">
                                                            <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="选择分点" Width="180"
                                                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                                            </ext:DropDownList>
                                                            </td>
                                                          </tr>                  
                                                        <tr>
                                                            <td style="text-align: right; width: 60px; height: 28px;">
                                                                <ext:Label ID="Label2" runat="server" Text="单位：" >
                                                                </ext:Label>
                                                            </td>
                                                            <td  style="text-align:left;">
                                                            <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True" Width="180"
                                                                runat="server">
                                                            </ext:DropDownList> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=" text-align: right; width: 60px; height: 28px;">
                                                                <ext:Label ID="Label3" runat="server" Text="上传时间：">
                                                                </ext:Label>
                                                            </td>
                                                            <td  style="text-align:left;">   
                                                            <ext:DatePicker ID="datebegin" Label="上传时间" runat="server" Width="180" DateFormatString="yyyy-MM-dd" />
                                                            </td>
                                                        </tr>       
                                                        <tr>
                                                            <td  style=" text-align: right; width: 60px; height: 28px;">
                                                                <ext:Label ID="Label4" runat="server" Text="至：">
                                                                </ext:Label>
                                                            </td>
                                                            <td  style="text-align:left;">
                                                             <ext:DatePicker ID="dateend" Label="至" runat="server" Width="180" DateFormatString="yyyy-MM-dd" />   
                                                            </td>
                                                        </tr>                 
                                                     </table>
                                                    </ext:ContentPanel>
                                                </Items>
                                            </ext:FormRow>
                                            <ext:FormRow>
                                                <Items>
                                                    <ext:TwinTriggerBox runat="server" ShowTrigger1="False" Trigger1Icon="Clear" Trigger2Icon="Search"
                                                        Label="关键字" ID="ttbSearch" OnTrigger2Click="ttbSearch_Trigger2Click" Width="180">
                                                    </ext:TwinTriggerBox>
                                                </Items>
                                            </ext:FormRow>
                                        </Rows>
                                    </ext:Form>
                                </Items>
                            </ext:Region>
                            <ext:Region ID="Region3" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gdBulkImportManageItem" runat="server" EnableCheckBoxSelect="false"
                                        EnableRowNumber="true" Title="Grid" ShowHeader="false" Width="250" 
                                        Height="420" AutoPostBack="true" EnableMultiSelect="false" DataKeyNames="Orderfileheaderid "
                                        OnRowClick="gdBulkImportManageItem_RowClick" IsDatabasePaging="true">
                                        <Columns>
                                            <ext:BoundField DataField="Orderfileheaderid" DataFormatString="{0}" HeaderText="表头id" Hidden="true" />
                                            <ext:BoundField Width="150px" DataField="Filename" DataToolTipField="Filename" HeaderText="文件名" />
                                            <ext:BoundField DataField="statustext" Width="70px" HeaderText="状态" />
                                            <ext:BoundField Width="60px" DataField="username" HeaderText="录单人" />
                                            <ext:BoundField Width="100px" DataField="Createdate" DataToolTipField="Createdate" HeaderText="上传日期" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
            <ext:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <ext:RegionPanel ID="RegionPanel3" runat="server" ShowBorder="false">
                        <Regions>
                            <ext:Region ID="Region5" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                                Margins="0 0 0 0" ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit"
                                Position="Center" runat="server" ShowBorder="false">
                                <Items>
                                    <ext:Grid ID="gdBulkImportDetailItem" runat="server" EnableCheckBoxSelect="false"
                                        EnableRowNumber="true" Title="Grid" ShowHeader="false" Width="250" AllowPaging="true"
                                        Height="420" AutoPostBack="true" EnableMultiSelect="false" IsDatabasePaging="true"
                                        OnPageIndexChange="gdBulkImportDetailItem_PageIndexChange">
                                        <Columns>
                                            <ext:BoundField Width="100px" DataField="Barcode" HeaderText="条码号" />
                                            <ext:BoundField Width="100px" DataField="Status" HeaderText="上传是否成功" />
                                            <ext:BoundField DataField="Realname" HeaderText="姓名" Width="60px" />
                                            <ext:BoundField DataField="Mobile" HeaderText="手机" Width="100px" />
                                            <ext:BoundField DataField="Idnumber" HeaderText="身份证" Width="100px" DataToolTipField="Idnumber" />
                                            <ext:BoundField Width="100px" DataField="labname" DataToolTipField="labname" HeaderText="分点" />
                                            <ext:BoundField Width="100px" DataField="customername" DataToolTipField="customername" HeaderText="单位" />
                                            <ext:BoundField Width="80px" DataField="username" HeaderText="录单人" />
                                            <ext:BoundField Width="300px" DataField="Reason" DataToolTipField="Reason" HeaderText="失败原因" />
                                        </Columns>
                                    </ext:Grid>
                                </Items>
                            </ext:Region>
                        </Regions>
                    </ext:RegionPanel>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WindowFrame" Hidden="true" EnableIFrame="true" runat="server" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="700" Height="400">
    </ext:Window>
    </form>
</body>
</html>
