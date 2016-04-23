using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;


namespace daan.web.code
{

    /// <summary>
    /// 文本工具类
    /// </summary>
    public class TextUtility
    {

        public TextUtility()
        {

        }
        /// <summary>
        /// 用于防止简单的查询时的SQL注入
        /// </summary>
        /// <param name="oldStr">原始文本</param>
        /// <returns>替换后的文本</returns>
        public static string ReplaceText(string oldStr)
        {
            //需要被替换的字符

            int num = oldStr.Length;
            string[] str_illegal = new string[num];
            for (int i = 0; i < str_illegal.Count(); i++)
            {
                str_illegal[i] = oldStr[i].ToString();
            }
            string[] str_ill = new string[num];
            //替换后的字符(索引对应)
            string[] str_legitimate = new string[num];
            for (int j = 0; j < str_legitimate.Count(); j++)
            {
                str_legitimate[j] = " ";
            }
            for (int i = 0; i < num; i++)
            {
                if (str_illegal[i].ToString() == "'")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "?")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "*")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "#")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "&")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "$")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "!")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "^")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "%")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "@")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "~")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
                else if (str_illegal[i].ToString() == "`")
                {
                    oldStr = oldStr.Replace(str_illegal[i], str_legitimate[i]);
                }
            }
            return oldStr;
        }

        public static string ReplaceTable(string tablenName)
        {
            switch (tablenName)
            {
                case "用户资源管理":
                    tablenName = "DICTUSER";
                    break;
                case "账单明细":
                    tablenName = "BILLDETAIL";
                    break;
                case "账单信息头表":
                    tablenName = "BILLHEAD";
                    break;
                case "账单跟踪":
                    tablenName = "BILLTRACE";
                    break;
                case "单位下次体检项目推荐":
                    tablenName = "CUSTOMERNEXTTEST";
                    break;
                case "客户结果评价":
                    tablenName = "CUSTOMERRESULTCOMMENT";
                    break;
                case "团检客户有效诊断":
                    tablenName = "CUSTOMERVALIDDIAGNOSIS";
                    break;
                case "体检单位维护":
                    tablenName = "DICTCUSTOMER";
                    break;
                case "客户总体折扣维护":
                    tablenName = "DICTCUSTOMERDISCOUNTED";
                    break;
                case "外包客户项目折扣":
                    tablenName = "DICTCUSTOMERTESTDISCOUNT";
                    break;
                case "诊断信息":
                    tablenName = "DICTDIAGNOSIS";
                    break;
                case "家族病史":
                    tablenName = "DICTFAMILYMEDHISTORY";
                    break;
                case "快速录入模版维护":
                    tablenName = "DICTFASTCOMMENT";
                    break;
                case "分点维护":
                    tablenName = "DICTLAB";
                    break;
                case "分点检测项维护":
                    tablenName = "DICTLABANDTEST";
                    break;
                case "分点检测项维护价格维护":
                    tablenName = "DICTLABANDTESTPRICE";
                    break;
                case "科室维护":
                    tablenName = "DICTLABDEPT";
                    break;
                case "基础字典维护":
                    tablenName = "DICTLIBRARY";
                    break;
                case "基础字典明细维护":
                    tablenName = "DICTLIBRARYITEM";
                    break;
                case "基因座简介":
                    tablenName = "DICTLOCUSREMARK";
                    break;
                case "既往病史":
                    tablenName = "DICTMEDHISTORY";
                    break;
                case "会员用户表":
                    tablenName = "DICTMEMBER";
                    break;
                case "其它病史":
                    tablenName = "DICTOTHERMEDHISTORY";
                    break;
                case "用户物理组对应表":
                    tablenName = "DICTUSERANDLABDEPT";
                    break;
                case "用户分点对应表":
                    tablenName = "DICTUSERANDLAB";
                    break;
                case "检查项目组合明细可选结果":
                    tablenName = "DICTTESTITEMRESULT";
                    break;
                case "检查项目维护":
                    tablenName = "DICTTESTITEM";
                    break;
                case "检查项目组合明细":
                    tablenName = "DICTTESTGROUPDETAIL";
                    break;
                case "易感基因结果得分表":
                    tablenName = "DICTSCORES";
                    break;
                case "产品建议规则公式":
                    tablenName = "DICTRULEFORMULAR";
                    break;
                case "报告模板":
                    tablenName = "DICTREPORTTEMPLATE";
                    break;
                case "初始化的基本资料":
                    tablenName = "INITBASIC";
                    break;
                case "本地参数设定":
                    tablenName = "INITLOCALSETTING";
                    break;
                case "系统设定":
                    tablenName = "INITSYSSETTING";
                    break;
                case "接口日志":
                    tablenName = "INTERFACELOG";
                    break;
                case "接口管理表":
                    tablenName = "INTERFACEMANAGER";
                    break;
                case "基础资料表维护日志表":
                    tablenName = "MAINTENANCELOG";
                    break;
                case "用于订单的修改留痕和节点信息":
                    tablenName = "OPERATIONLOG";
                    break;
                case "订单条码表":
                    tablenName = "ORDERBARCODE";
                    break;
                case "订单诊断表":
                    tablenName = "ORDERDIAGNOSIS";
                    break;
                case "订单对应组合":
                    tablenName = "ORDERGROUPTEST";
                    break;
                case "科室小结":
                    tablenName = "ORDERLABDEPTRESULT";
                    break;
                case "医生推荐下次检测项目":
                    tablenName = "ORDERNEXTTEST";
                    break;
                case "订单套餐表":
                    tablenName = "ORDERPRODUCTS";
                    break;
                case "订单总检信息表":
                    tablenName = "ORDERRESULTCOMMENT";
                    break;
                case "订单主表":
                    tablenName = "ORDERS";
                    break;
                case "客户订单追踪表情况记录":
                    tablenName = "ORDERSERVICEINFO";
                    break;
                case "订单对应明细项目":
                    tablenName = "ORDERTEST";
                    break;
                case "临时订单表":
                    tablenName = "TEMPORDERNUM";
                    break;
                case "套餐明细":
                    tablenName = "DICTPRODUCTDETAIL";
                    break;
                default:
                    tablenName = "无匹配的表名";
                    break;
            }
            return tablenName;
        }

    }
}
