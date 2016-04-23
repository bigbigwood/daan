using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    public class CommonConst
    {
        /// <summary>
        /// 标记要用sqlserver数据库的 getdate函数获取时间
        /// </summary>
        public static string getdate = "1900-01-01 00:00:00";

        public static string reportpath = @"report\";
        public static char testSplitChar = '\0';

        public struct specHeadStatus
        {
            public const string downloaded = "10";
            public const string barcode_printed = "20";
            public const string collected = "30";
            public const string taked = "40";
            public const string distributed = "50"; //已分发扫描

            public const string received = "60";

            public const string canceled = "300";
        }

        public struct ageUnit
        {
            public const string year = "岁";
            public const string month = "月";
            public const string day = "天";
        }

        /// <summary>
        /// 报表模板代码
        /// </summary>
        public struct reportCode
        {
            /// <summary>
            /// 常规报表
            /// </summary>
            public const string specNormal = "SpecNormalA";

            /// <summary>
            /// 微生物报表
            /// </summary>
            public const string specOrg = "SpecOrg";
        }


        /// <summary>
        /// 导入模板固定列   ,"营业区" 
        /// </summary>
        public static string[] ImportExcelCols = new string[] { "套餐代码", "条码号", "姓名", "性别", "出生日期", "年龄",
            "婚否", "手机", "身份证", "住址", "部门", "备注", "电话", "邮箱", "采样日期"};

        public struct patientType
        {
            public const string clinical = "1";
            public const string hospital = "2";
        }
    }
}
