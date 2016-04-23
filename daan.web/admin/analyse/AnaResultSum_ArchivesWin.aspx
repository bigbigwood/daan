<%@ Page Title="" Language="C#" AutoEventWireup="true"
    CodeBehind="AnaResultSum_ArchivesWin.aspx.cs" Inherits="daan.web.admin.analyse.AnaResultSum_ArchivesWin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <ext:HiddenField runat="server" ID="hidMemberID"></ext:HiddenField>
    <ext:RegionPanel ID="RegionPanel1" runat="server" ShowBorder="false">
        <Regions>
            <ext:Region ID="Region2" runat="server" Position="Center" ShowHeader="false" Title="Center Region" Layout="Fit">
                <Items>
                    <ext:TabStrip ID="TabStrip1" runat="server" ActiveTabIndex="0" ShowBorder="false">
                        <Tabs>
                            <ext:Tab ID="Tab3" runat="server" BodyPadding="0px" EnableBackgroundColor="true"
                                Layout="Fit" Title="健康档案">
                                <Toolbars>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill ID="ToolbarFill2" runat="server">
                                            </ext:ToolbarFill>
                                            <ext:ToolbarSeparator ID="ts1" runat="server"></ext:ToolbarSeparator>
                                            <ext:Button ID="btnAdd" runat="server" Text="添加" Icon="ADD" OnClick="btnAdd_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ts2" runat="server"></ext:ToolbarSeparator>
                                            <ext:Button ID="btnDel" runat="server" Text="删除"
                                             ConfirmIcon="Question" ConfirmTarget="Top" ConfirmText="是否确定删除此条记录？" 
                                             ConfirmTitle="体检系统" Icon="DELETE" OnClick="btnDel_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                            <ext:Button ID="btnClo" runat="server" Icon="BulletCross" Text="关闭" OnClick="btnClose_Click"></ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Toolbars>
                                <Items>
                                    <ext:RegionPanel ID="RegionPanel4" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="Region4" runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit" Title="Center Region">
                                                <Items>
                                                    <ext:Grid ID="gdHEALTHRECORDS" runat="server" EnableRowNumber="true" ShowHeader="false"
                                                       DataKeyNames="Dicthealthrecordsid" EnableMultiSelect="false" EnableCheckBoxSelect="true" Title="Grid">
                                                        <Columns>
                                                            <ext:BoundField HeaderText="类型" DataField="DictrecordtypeText" Width="50px" />
                                                            <ext:BoundField HeaderText="详细内容" DataField="Dictrecordtext" DataToolTipField="Dictrecordtext" ExpandUnusedSpace="true" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region></Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                            <ext:Tab ID="Tab4" runat="server" BodyPadding="0px" EnableBackgroundColor="true" 
                                Layout="Fit" Title="既往体检档案">
                                <Items>
                                    <ext:RegionPanel ID="rgPastOrder" runat="server" ShowBorder="false">
                                        <Regions>
                                            <ext:Region ID="rgList" runat="server" Height="150px" Position="Top" ShowBorder="true" ShowHeader="false"
                                             Layout="Fit" Split="true" Icon="Outline" EnableSplitTip="true" CollapseMode="Mini" EnableCollapse="true">
                                                <Items>
                                                    <ext:Grid runat="server" ID="gdPASTORDERS" EnableRowNumber="true" ShowHeader="false"
                                                     Title="Grid" DataKeyNames="ordernum" EnableRowClick="true" OnRowClick="gdPASTORDERS_RowClick">
                                                        <Columns>
                                                            <ext:BoundField DataField="ordernum" HeaderText="体检流水号" Width="100px" />
                                                            <ext:BoundField DataField="customername" DataToolTipField="customername" HeaderText="体检单位" Width="200px" />
                                                            <ext:BoundField DataField="createdate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="体检日期" Width="100px" />
                                                            <ext:BoundField DataField="remarks" DataToolTipField="remarks" HeaderText="备注" Width="150px" ExpandUnusedSpace="true" />
                                                        </Columns>
                                                    </ext:Grid>
                                                </Items>
                                            </ext:Region>
                                            <ext:Region runat="server" ID="RegionDetail" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit">
                                                <Items>
                                                    <ext:TabStrip runat="server" ID="tabDetail" ActiveTabIndex="0" ShowBorder="false">
                                                        <Tabs>
                                                            <ext:Tab runat="server" ID="tab6" Title="科室小结与诊断信息" EnableBackgroundColor="true" Layout="Fit">
                                                                <Items>
                                                                    <ext:RegionPanel runat="server" ID="r" AutoScroll="true">
                                                                        <Regions>
                                                                            <ext:Region runat="server" Position="Top" Layout="Fit" ShowBorder="false" ShowHeader="false"
                                                                            Split="true" EnableSplitTip="true" CollapseMode="Mini" EnableCollapse="true" Height="80px">
                                                                                <Items>
                                                                                    <ext:Grid ID="gdOrderlabdeptResult" ShowBorder="false" ShowHeader="false" EnableRowNumber="true" runat="server">
                                                                                        <Columns>
                                                                                            <ext:BoundField DataField="LABDEPTNAME" Width="150px" HeaderText="科室" />
                                                                                            <ext:BoundField DataField="LABDEPTRESULT" DataToolTipField="LABDEPTRESULT" HeaderText="结果小结" Width="200px" />
                                                                                            <ext:BoundField DataField="USERNAME" HeaderText="小结医生" Width="80px" />
                                                                                            <ext:BoundField DataField="APPRAISEDATE" HeaderText="小结日期" DataFormatString="{0:yyyy-MM-dd}" ExpandUnusedSpace="true" />
                                                                                        </Columns>
                                                                                    </ext:Grid>
                                                                                </Items>
                                                                            </ext:Region>
                                                                            <ext:Region runat="server" Position="Center" Layout="Fit" ShowBorder="false" ShowHeader="false">
                                                                                <Items>
                                                                                     <ext:Grid ID="gdOrderdiagnosis" ShowBorder="true" ShowHeader="false" EnableRowNumber="true" Title="诊断信息" runat="server" >
                                                                                        <Columns>
                                                                                            <ext:BoundField DataField="LABDEPTNAME" HeaderText="科室" Width="150px" />
                                                                                            <ext:BoundField DataField="DIAGNOSISNAME" HeaderText="诊断名" Width="150px" />
                                                                                            <ext:BoundField DataField="DISEASEDESCRIPTION" DataToolTipField="DISEASEDESCRIPTION" HeaderText="医学解释" Width="130px" />
                                                                                            <ext:BoundField DataField="DISEASECAUSE" DataToolTipField="DISEASECAUSE" HeaderText="常见原因" Width="130px" />
                                                                                            <ext:BoundField DataField="SUGGESTION" DataToolTipField="SUGGESTION" HeaderText="本次建议" ExpandUnusedSpace="true" />
                                                                                        </Columns>
                                                                                    </ext:Grid>
                                                                                </Items>
                                                                            </ext:Region>
                                                                        </Regions>
                                                                    </ext:RegionPanel>
                                                                </Items>
                                                            </ext:Tab>
                                                            <ext:Tab runat="server" ID="tab5" Title="详细结果" EnableBackgroundColor="true" Layout="Fit">
                                                                <Items>
                                                                    <ext:Grid runat="server" ID="gdOrdertest" EnableRowNumber="true" ShowBorder="false" ShowHeader="false"
                                                                    AutoHeight="true" AutoScroll="true">
                                                                        <Columns>
                                                                            <ext:BoundField Width="100px" DataField="DICTGROUPNAME" HeaderText="组合名称" />
                                                                            <ext:BoundField Width="100px" DataField="TESTNAME" HeaderText="项目名称" />
                                                                            <ext:BoundField Width="70px" DataField="TESTRESULT" HeaderText="检查结果" />
                                                                            <ext:BoundField Width="100px" DataField="LASTRESULT" HeaderText="上次结果" />
                                                                            <ext:BoundField Width="100px" DataField="LASTDATE" HeaderText="上次时间" />
                                                                            <ext:BoundField Width="100px" DataField="HLFLAG" HeaderText="提示" />
                                                                            <ext:BoundField Width="100px" DataField="HLHINT" HeaderText="是否异常" />
                                                                            <ext:BoundField Width="200px" DataField="TEXTSHOW" HeaderText="参考范围" DataToolTipField="TEXTSHOW" />
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Tab>
                                                            <ext:Tab runat="server" ID="tab7" Title="总体评价" EnableBackgroundColor="true" Layout="Fit">
                                                                <Items>
                                                                    <ext:RegionPanel ID="RegionPanel9" runat="server" ShowBorder="false">
                                                                        <Regions>
                                                                            <ext:Region ID="Region18" EnableSplitTip="true" CollapseMode="Mini" Margins="5 5 0 5"
                                                                                ShowHeader="false" Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Top"
                                                                                Height="150px" Split="true" runat="server" ShowBorder="false">
                                                                                <Toolbars>
                                                                                    <ext:Toolbar ID="Toolbar8" runat="server">
                                                                                        <Items>
                                                                                            <ext:Label runat="server" ID="Label5" Text="结果评价">
                                                                                            </ext:Label>
                                                                                        </Items>
                                                                                    </ext:Toolbar>
                                                                                </Toolbars>
                                                                                <Items>
                                                                                    <ext:TextArea ID="tbxComment" runat="server" />
                                                                                </Items>
                                                                            </ext:Region>
                                                                            <ext:Region ID="Region20" runat="server" Position="Center" Margins="0 5 5 5" ShowHeader="false"
                                                                                Layout="Fit" ShowBorder="false" Title="Center Region">
                                                                                <Toolbars>
                                                                                    <ext:Toolbar ID="Toolbar7" runat="server">
                                                                                        <Items>
                                                                                            <ext:Label runat="server" ID="lbltitle" Text="评估建议">
                                                                                            </ext:Label>
                                                                                        </Items>
                                                                                    </ext:Toolbar>
                                                                                </Toolbars>
                                                                                <Items>
                                                                                    <ext:TextArea ID="tbxSuggestion" runat="server" />
                                                                                </Items>
                                                                            </ext:Region>
                                                                        </Regions>
                                                                    </ext:RegionPanel>
                                                                </Items>
                                                            </ext:Tab>
                                                            <ext:Tab runat="server" ID="tab8" Title="推荐项目" EnableBackgroundColor="true" Layout="Fit">
                                                                <Items>
                                                                    <ext:Grid ID="gdOrdernexttest" ShowBorder="true" Title="已选项目" EnableRowNumber="true"
                                                                        ShowHeader="false" runat="server">
                                                                        <Columns>
                                                                            <ext:BoundField DataField="TESTNAME" HeaderText="项目名称" />
                                                                            <ext:BoundField DataField="RERUNDATE" DataFormatString="{0:yyyy-MM-dd}" Width="150px" HeaderText="预约复查开始时间" />
                                                                            <ext:BoundField DataField="RERUNENDDATE" DataFormatString="{0:yyyy-MM-dd}" ExpandUnusedSpace="true" HeaderText="预约复查结束时间" />
                                                                        </Columns>
                                                                    </ext:Grid>
                                                                </Items>
                                                            </ext:Tab>
                                                        </Tabs>
                                                    </ext:TabStrip>
                                                </Items>
                                            </ext:Region>
                                        </Regions>
                                    </ext:RegionPanel>
                                </Items>
                            </ext:Tab>
                        </Tabs>
                    </ext:TabStrip>
                </Items>
            </ext:Region></Regions>
    </ext:RegionPanel>
    <ext:Window runat="server" Popup="false" Hidden="true" ID="win1" Target="Top" EnableIFrame="true" IFrameUrl="about:blank"
        Width="500px" Height="300px" Title="健康档案">
    </ext:Window>
    </form>
</body>
</html>
