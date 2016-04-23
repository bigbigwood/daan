<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropMemberControl.ascx.cs"
    Inherits="daan.web.usercontrol.DropMemberControl" %>
<%--<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $('#selmember').combobox({
            url: '../../usercontroldata/DictMember.aspx',
            valueField: 'Id',
            textField: 'Name',
            width: 375,
            formatMatch: function (row, i, max) {
                alert(i+'Z');
                return row.Name;
            },
            formatResult: function (row) {
                return row.Name;
            }
        });

    });
 
</script>--%>
<script language="javascript" type="text/javascript">
    //加载可输入下拉框
    $().ready(function () {

        //会员
        var memberinfo = new Object();
        $.ajax({
            url: '../../usercontroldata/DictMember.aspx',
            dataType: 'json',
            async: false,
            success: function (data) {
                memberinfo = eval(data)
            }
        });

        $("#txtmember").autocomplete(memberinfo, {
            minChars: 0,
            matchContains: true,
            max: 2000, //设置最大记录数
            delay: 100, //延迟时间
            width: 350, //宽度
            scrollHeight: 400, //达到多少高度出现滚动条
            autoFill: false,
            formatItem: function (row, i, max) {
                var str = i + "/" + max + "  " + "  [" + row.Name + "]" + "  " + "  [" + row.EntranceYear + "]" + "  " + "  [" + row.Major + "]";
                maxlength = max;
                return str;
            },
            formatMatch: function (row, i, max) {
                maxlength = max;
                return row.Name + row.EntranceYear + row.Major;
            },
            formatResult: function (row) {
                return row.Name;
            }
            //  
        }).result(function (event, row) {
            $("#txtmember")[0].value = row.Name;
            $("#hidmember")[0].value = row;
        });
    })    
//    会员end   
</script>
<%--<input type="text" id="txtmember"  style="width: 100px" />--%>
<%--<select runat="server" id="selmember" style="width: 100px">--%>
<%--required="true"--%>
<%--</select>--%>
<%--<input class="easyui-combobox" required="true" 
			name="language"
			url="../../usercontroldata/DictMember.aspx" 
			valueField="Id" 
			textField="Name" 
			multiple="true"
            
			/>--%><%--panelHeight="auto" --%>