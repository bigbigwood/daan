<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmExceptional.aspx.cs"
    Inherits="daan.web.admin.exceptional.FrmExceptional" Title="异常标本处理中心" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>异常标本处理中心</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="False">
        <Toolbars>
            <ext:Toolbar ID="Toolbar1" runat="server" Position="Top">
                <Items>
                    <ext:Label runat="server" Text="每页数据量："></ext:Label>
                    <ext:DropDownList runat="server" AutoPostBack="true" ID="dropPageSize" Width="70px" OnSelectedIndexChanged="dropPageSize_SelectedIndexChanged">
                        <ext:ListItem Text="50" Value="50"/>
                        <ext:ListItem Text="100" Value="100" />
                        <ext:ListItem Text="200" Value="200" />
                        <ext:ListItem Text="300" Value="300" />
                        <ext:ListItem Text="400" Value="400" />
                        <ext:ListItem Text="500" Value="500" Selected="true"/>
                    </ext:DropDownList>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                    </ext:ToolbarFill>
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Magnifier" OnClick="btnSearch_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSeeDetail" runat="server" Icon="PageWhiteStack" Text="查看订单" OnClick="btnSeeDetail_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnEdit" runat="server" Icon="PageWhiteEdit" Text="修改订单" OnClick="btnEdit_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnEditDictcustomerid" runat="server" Icon="PageWhiteEdit" Text="批量修改订单" OnClick="btnEditDictcustomerid_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnLog" runat="server" Text="操作记录" Icon="SystemNew" OnClick="btnLog_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnSuperAddition" runat="server" Icon="TagBlueAdd" Text="追加项目" OnClick="btnSuperAddition_Click">
                    </ext:Button>
                    <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server">
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnCancel" runat="server" Icon="Cancel" Text="作废" OnClick="btnCancel_Click">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </Toolbars>
        <Regions>
            <ext:Region Layout="Fit" Height="110px" Position="Top" ShowHeader="False" ShowBorder="False"
                Split="true">
                <Items>
                    <ext:Form ID="Form2" runat="server" BodyPadding="5px" EnableBackgroundColor="false"
                        LabelWidth="90px" LabelAlign="Right" Title="Form" ShowBorder="false" ShowHeader="False">
                        <Rows>
                            <ext:FormRow ID="FormRow2" runat="server">
                                <Items>
                                    <ext:DropDownList ID="DropDictLab" CompareType="String" Resizable="True" Label="选择分点"
                                        CompareValue="-1" CompareOperator="NotEqual" CompareMessage="全部" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDictLab_SelectedIndexChanged">
                                    </ext:DropDownList>                                   
                                    <ext:DropDownList ID="DropCustomer" Label="单位" EnableEdit="true" Resizable="True"
                                        runat="server">
                                    </ext:DropDownList>
                                     <ext:DropDownList ID="dropStatus" runat="server" Label="状态">
                                    </ext:DropDownList>
                                    <ext:DropDownList runat="server" ID="DropIScancel" Label="是否作废" Resizable="True">
                                        <ext:ListItem Text="全部" Value="-1" />
                                        <ext:ListItem Text="已作废" Value="1" />
                                        <ext:ListItem Selected="true" Text="未作废" Value="0" />
                                    </ext:DropDownList>
                                </Items>
                            </ext:FormRow>
                            <ext:FormRow ID="FormRow3" runat="server">
                                <Items>
                                    <ext:DatePicker ID="datebegin" Label="登记时间" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:DatePicker ID="dateend" Label="至" runat="server" DateFormatString="yyyy-MM-dd" />
                                    <ext:TextBox runat="server" ID="tbxOrderNum" Label="体检号/条码号" EmptyText="此条件忽略其他条件"
                                        CssClass="tbxwidth100" />                     
                                    <ext:TextBox runat="server" ID="tbxName" Label="姓　　名" CssClass="tbxwidth100" />
                                </Items>
                            </ext:FormRow>     
                            <ext:FormRow ID="FormRow1" runat="server">
                                <Items>
                                    <ext:DatePicker runat="server" Label="采样日期" ID="dpSFrom"></ext:DatePicker>
                                    <ext:DatePicker runat="server" Label="到" ID="dpSTo" CompareControl="dpSFrom"
                                        CompareMessage="结束日期应该大于开始日期" CompareOperator="GreaterThanEqual">
                                    </ext:DatePicker>
                                    <ext:DropDownList runat="server" Resizable="true" Label="省" AutoPostBack="true" EnableEdit="true" ID="dpProvince" OnSelectedIndexChanged="dpProvince_SelectedIndexChanged"></ext:DropDownList>
                                    <ext:DropDownList runat="server" Resizable="true" Label="市" AutoPostBack="true" EnableEdit="true" ID="dpCity"></ext:DropDownList>
                                </Items>
                            </ext:FormRow>        
                             <ext:FormRow ID="FormRow4" runat="server">
                                <Items>
                                    <ext:TextBox runat="server" ID="tbxSection" Label="部门机构" EmptyText="个险部、银保部等" />
                                    <ext:TextBox runat="server" ID="tbxArea" Label="营业区" />
                                    <ext:TextBox runat="server" ID="tbxBatchNumber" Label="场次号"/>
                                    <ext:Label runat="server" Hidden="true"></ext:Label>
                                </Items>
                            </ext:FormRow>               
                        </Rows>
                    </ext:Form>
                </Items>
            </ext:Region>
            <ext:Region Layout="Fit" Position="Center" ShowHeader="False" ShowBorder="False">
                <Items>
                    <ext:Grid ID="GridOrders" Title="订单列表" DataKeyNames="ordernum,cancel,labname,statusvalue,realname,dictcustomerid"
                        AutoScroll="true" EnableCheckBoxSelect="true" ShowHeader="false" PageSize="20"
                        IsDatabasePaging="true" EnableTextSelection="true" AllowPaging="true" runat="server"
                        AutoWidth="true" AutoHeight="true" EnableRowNumber="false" OnPageIndexChange="GridOrders_PageIndexChange">
                        <Columns>
                            <ext:BoundField DataField="rn" HeaderText="序号" Width="40px" />
                            <ext:BoundField DataField="status" DataToolTipField="status" HeaderText="状态" Width="70px" />
                            <ext:BoundField DataField="ordernum" DataToolTipField="ordernum" HeaderText="体检号" Width="100px" />
                            <ext:BoundField DataField="realname" DataToolTipField="realname" HeaderText="姓名" Width="60px" />
                            <ext:BoundField DataField="age" DataToolTipField="age" HeaderText="年龄" Width="50px" />
                            <ext:BoundField DataField="sex" DataToolTipField="sex" HeaderText="性别" Width="40px" />
                            <ext:BoundField DataField="ismarried" DataToolTipField="ismarried" HeaderText="婚否" Width="50px" />
                            <ext:BoundField DataField="cancel" DataToolTipField="cancelreason" HeaderText="是否作废" Width="60px" />
                            <ext:BoundField DataField="createdate" DataToolTipField="createdate" HeaderText="登记时间" Width="85px" DataFormatString="{0:yyyy-MM-dd}" />
                            <ext:BoundField DataField="labname" DataToolTipField="labname" HeaderText="分点" Width="120px" />
                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="体检单位" Width="150px" />
                            <ext:BoundField DataField="section" DataToolTipField="section" HeaderText="部门机构" Width="60" />
                            <ext:BoundField DataField="area" DataToolTipField="area" HeaderText="营业区" Width="80px" />
                            <ext:BoundField DataField="accountmanager" DataToolTipField="accountmanager" HeaderText="客户经理" Width="70px" />
                            <ext:BoundField DataField="enterby" DataToolTipField="enterby" HeaderText="录单人" Width="70px" />
                            <ext:BoundField DataField="samplingdate" HeaderText="采样日期" Width="85px" DataFormatString="{0:yyyy-MM-dd}"  />
                            <ext:BoundField DataField="remarks" DataToolTipField="remarks" HeaderText="备注" Width="100"/>
                            <ext:BoundField HeaderText="邮寄地址" DataField="POSTADDRESS" DataToolTipField="POSTADDRESS" Width="250" />
                            <ext:BoundField HeaderText="收件人" DataField="RECIPIENT" DataToolTipField="RECIPIENT" Width="60" />
                            <ext:BoundField HeaderText="联系电话" DataField="CONTACTNUMBER" DataToolTipField="CONTACTNUMBER" ExpandUnusedSpace="true"  />
                        </Columns>
                    </ext:Grid>
                </Items>
            </ext:Region>
        </Regions>
    </ext:RegionPanel>
    <ext:Window ID="WindowFrame" Hidden="true" EnableIFrame="true" runat="server" IFrameUrl="about:blank"
        Target="Top" IsModal="True" Width="900" Height="600">
    </ext:Window>
    <ext:Window runat="server" ID="WinCancel" Hidden="true" Title="订单作废原因录入" Target="Top"
     IsModal="true" AutoScroll="true" Icon="TagBlue" Layout="Fit" Width="400" Height="200"
      EnableIFrame="true" IFrameUrl="about:blank" OnClose="WinCancel_Close">
    </ext:Window>
    <ext:Window ID="WinBillRemark" Hidden="true" EnableIFrame="true" runat="server" CloseAction="HidePostBack"
        EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Top" IsModal="True"
        Width="580" Height="385">
    </ext:Window>
    </form>
</body>
</html>
