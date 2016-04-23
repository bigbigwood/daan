using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using daan.domain;

namespace daan.service.common
{
   public class UtilService
    {

        #region
        /// <summary>
        /// String查询类型
        /// </summary>
        public struct UT_StringTypes
        {
            /// <summary>
            ///等于
            /// </summary>
            public const string opEqual = "{0}='{1}'";
            //不区分大小写
            /// <summary>
            /// 前匹配





            /// </summary>
            public const string opBeforeLike = "{0} like '{1}%'";

            /// <summary>
            /// 后匹配





            /// </summary>
            public const string opBehindLike = "{0} like '%{1}'";
            /// <summary>
            /// 包含了





            /// </summary>
            public const string opLike = "{0} like '%{1}%'";
            /// <summary>
            ///在其中','分隔   
            /// </summary>
            public const string opIn = "{0} in ({1})";
            /// <summary>
            /// 不在其中','分隔
            /// </summary>
            public const string opNotIn = "{0} not in ({1})";
            /// <summary>
            /// 不等于





            /// </summary>
            public const string opUnEqual = "{0}!='{1}'";

            /// <summary>
            /// 为空
            /// </summary>
            public const string opNull = "{0} is {1}";

        }
        /// <summary>
        /// Int查询类型
        /// </summary>
        public struct UT_IntTypes
        {
            /// <summary>
            ///等于
            /// </summary>
            public const string opEqual = "{0}={1}";

            /// <summary>
            /// 大于等于
            /// </summary>
            public const string opHEqual = "{0}>={1}";

            /// <summary>
            /// 小于等于
            /// </summary>
            public const string opLEqual = "{0}<={1}";
            /// <summary>
            /// 大于
            /// </summary>
            public const string opMore = "{0}>{1}";
            /// <summary>
            /// 小于
            /// </summary>
            public const string opLess = "{0}<{1}";
            /// <summary>
            /// 不等于





            /// </summary>
            public const string opUnEqual = "{0}<>{1}";


            /// <summary>
            ///在其中','分隔   
            /// </summary>
            public const string opIn = "{0} in ({1})";

            /// <summary>
            /// 不在其中','分隔
            /// </summary>
            public const string opnotIn = "{0} not in ({1})";

        }

        /// <summary>
        /// DateTime查询类型
        /// </summary>
        public struct UT_DateTimeTypesOra
        {

            /// <summary>
            ///等于
            /// </summary>
            public const string opEqual = "{0}= TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')";


            /// <summary>
            ///在一天内
            /// </summary>
            public const string opTheDay = "{0} >=TO_DATE('{1}', 'YYYY-MM-DD') and {0} <TO_DATE('{1}', 'YYYY-MM-DD')+1 ";


            /// <summary>
            /// 大于等于
            /// </summary>
            public const string opHEqual = "{0}>= TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')";

            /// <summary>
            /// 小于等于
            /// </summary>
            public const string opLEqual = "{0} < TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')+1";
            /// <summary>
            /// 大于
            /// </summary>
            public const string opMore = "{0}> TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')";
            /// <summary>
            /// 小于
            /// </summary>
            public const string opLess = "{0}< TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')";
            /// <summary>
            /// 不等于





            /// </summary>
            public const string opUnEqual = "{0}<> TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')";


        }


        public struct UT_DateTimeTypes
        {

            /// <summary>
            ///等于
            /// </summary>
            public const string opEqual = "{0}= '{1}'";
            /// <summary>
            /// 大于等于
            /// </summary>
            public const string opHEqual = "{0}>= '{1}'";

            /// <summary>
            /// 大于
            /// </summary>
            public const string opMore = "{0}> '{1}'";
            /// <summary>
            /// 小于
            /// </summary>
            public const string opLess = "{0}< '{1}'";
            /// <summary>
            /// 不等于

            /// </summary>
            public const string opUnEqual = "{0}<> '{1}'";

            /// <summary>
            ///在一天内
            /// </summary>
            public const string opTheDay = "{0} >='{1}' and {0} <dateadd(day,1, '{1}') ";
            /// <summary>
            /// 小于
            /// </summary>
            public const string opLEqual = "{0} < dateadd(day,1, '{1}')";

            /// <summary>
            /// 在一月内
            /// </summary>
            public const string opTheMon = "{0} >='{1}' and {0} <dateadd(month,1, '{1}') ";
            /// <summary>
            /// 小于
            /// </summary>
            public const string opMEqual = "{0} < dateadd(month,1, '{1}')";
        }
        #endregion

        #region   TestString


        private static bool TestString(string strSql)
        {
            bool bol = false;
            if (strSql == null)
            {
                bol = false;
                throw new Exception("字符串不可以为null");

            }
            else if (strSql.Length == 0)
            {
                bol = false;
                throw new Exception("字符串不可以为空");

            }
            else
            {
                bol = true;
            }
            return bol;
        }
        #endregion

        #region  TestObject

        private static bool TestObject(object obj)
        {
            if (obj == null)
            {

                return false;
            }
            else if (obj.ToString().Length == 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


        #region 


        public static string GetInsertStr(string tableName, string valstr)
        {
            string oleStr = null;
            if (TestString(tableName))
            {
                oleStr = "insert into " + tableName + "  VALUES (" + valstr + ")";

            }
            return oleStr;

        }


        public static string GetInsertStr(string columns, string tableName, string valstr)
        {
            string oleStr = GetInsertStr(columns, tableName);
            if (TestObject(columns))
            {

                oleStr = "insert into " + tableName + " (" + columns + ")  VALUES (" + valstr + ")";
            }
            return oleStr;

        }


        public static ArrayList GetInsertStr(DataTable dtSave)
        {
            ArrayList strSql = new ArrayList();

            for (int i = 0; i < dtSave.Rows.Count; i++)
            {
                string[] columns = new string[dtSave.Columns.Count];
                string[] vals = new string[dtSave.Columns.Count];

                for (int j = 0; j < dtSave.Columns.Count; j++)
                {
                    columns[j] = dtSave.Columns[j].ColumnName;   
                    if (dtSave.Columns[j].DataType.Equals(Type.GetType("System.String")))
                    {
                        vals[j] = dtSave.Rows[i][j].ToString();
                                                                                          
                    }
                    else if (dtSave.Columns[j].DataType.Equals(Type.GetType("System.Decimal")) || 
                        dtSave.Columns[j].DataType.Equals(Type.GetType("System.Int32")))
                    {
                        vals[j] = (dtSave.Rows[i][j].ToString() == "" ? null : dtSave.Rows[i][j].ToString());
                    }
                    else if (dtSave.Columns[j].DataType.Equals(Type.GetType("System.DateTime")))
                    {
                        vals[j] = ToDate(dtSave.Rows[i][j].ToString());
                    }
                    else { vals[j] = dtSave.Rows[i][j].ToString(); }

                }
                strSql.Add(GetInsertStr(columns, dtSave.TableName, vals));

            }
            return strSql;

        }

        private static string ToDate(string p)
        {
            return p == ""? null:p;
        }
        #endregion

        #region 


        public static string GetDeleteStr(string tableName, string condition)
        {
            string oleStr = GetDeleteStr(tableName);

            if (TestObject(condition))
            {
                oleStr += " where " + condition;
            }
            return oleStr;
        }

        public static string GetDeleteStr(string tableName)
        {
            string oleStr = null;
            if (TestString(tableName))
            {
                oleStr = "delete " + tableName;

            }
            return oleStr;
        }

        public static ArrayList GetDeleteStr(DataSet ds)
        {
            ArrayList arrDel = new ArrayList();
            foreach (DataTable dt in ds.Tables)
            {
                string tableName = dt.TableName;
                foreach (DataRow dr in dt.Rows)
                {
                    string condition = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        condition += String.Format(UT_StringTypes.opEqual, dc.ColumnName, dr[dc.ColumnName]);
                        condition += " And ";
                    }
                    condition = condition.Remove(condition.Length - 5);
                    arrDel.Add(GetDeleteStr(tableName, condition));
                }
            }
            return arrDel;
        }
        #endregion

        #region 


        public static string GetUpdateStr(string columns, string tableName, string condition)
        {
            // update jobs set job_desc='20',min_lvl=30,max_lvl=40 where job_id=18
            string oleStr = GetUpdateStr(columns, tableName);

            if (TestObject(condition))
            {
                oleStr += " where " + condition;
            }
            return oleStr;

        }


        public static ArrayList GetUpdateStr(DataTable dtSave, String[] primarykey)
        {//直接update//判断update
            ArrayList strSql = new ArrayList();

            for (int i = 0; i < dtSave.Rows.Count; i++)
            {
                string[] columns = new string[dtSave.Columns.Count];
                string[] vals = new string[dtSave.Columns.Count];

                for (int j = 0; j < dtSave.Columns.Count; j++)
                {
                    columns[j] = dtSave.Columns[j].ColumnName;
                    if (dtSave.Columns[j].DataType.Equals(Type.GetType("System.DateTime")))
                    {
                        vals[j] = ToDate(dtSave.Rows[i][j].ToString());
                    }
                    else
                    {
                        vals[j] = dtSave.Rows[i][j].ToString();
                    }
                }

                string updateStr = "";
                foreach (string key in primarykey)
                {
                    updateStr += key + "='" + dtSave.Rows[i][key].ToString() + "'";
                    updateStr += " AND ";
                }

                if (updateStr.Length > 0)
                {
                    updateStr = updateStr.Substring(0, updateStr.Length - 5);
                }

                strSql.Add(GetUpdateStr(columns, vals, dtSave.TableName, updateStr));
            }
            return strSql;

        }

 
        public static string GetUpdateStr(string columns, string tableName)
        {
            string oleStr = null;

            if (TestString(columns) && TestString(tableName))
            {

                oleStr = "update " + tableName + " set " + columns;
            }
            return oleStr;
        }
        #endregion

        #region 组合查询SQL语句


        public static string GetSelectStr(string tableName)
        {
            string oleStr = null;
            if (TestString(tableName))
            {
                oleStr = "select * from " + tableName;
            }
            return oleStr;
        }


        public static string GetSelectStr(string columns, string tableName)
        {
            string oleStr = null;

            if (TestString(tableName))
            {
                if (TestObject(columns))
                {
                    oleStr = "select " + columns + " from " + tableName;

                }
                else
                {
                    oleStr = "select * from " + tableName;
                }

            }
            return oleStr;

        }


 
        public static string GetSelectStr(string columns, string tableName, string condition)
        {
            string oleStr = GetSelectStr(columns, tableName);
            if (TestObject(condition))
            {
                oleStr += " where " + condition;
            }


            return oleStr;
        }


        public static string GetSelectStr(string columns, string tableName, string condition, string bystr)
        {
            string oleStr = GetSelectStr(columns, tableName, condition);

            if (TestObject(bystr))
            {
                oleStr += " " + bystr;
            }
            return oleStr;
        }



        #endregion

        #region 组合SQL语句(数组)



        private static string ColumList(string[] columns)
        {
            string colStr = "";
            if (TestObject(columns))
            {
                foreach (string colum in columns)
                {
                    colStr += colum + ",";
                }
                colStr = colStr.TrimEnd(',');

            }
            return colStr;
        }

        private static string ValueList(string[] val)
        {
            string colStr = "";
            if (TestObject(val))
            {

                foreach (string colum in val)
                {
                    if (colum == null) {
                        colStr += "null" + ",";
                        continue;
                    }
                    if (colum.IndexOf("'") >= 0)
                    {
                        colStr += colum + ",";
                    }
                    else
                    {
                        colStr += "'" + colum + "',";
                    }
                }
                colStr = colStr.TrimEnd(',');

            }
 
            colStr = colStr.Replace(CommonConst.getdate, "GetDate()");
            if (colStr.IndexOf("'GetDate()'") >= 0)
            {
                colStr = colStr.Replace("'GetDate()'", "GetDate()");
            }
            return colStr;
        }


        private static string ConnStr(string[] condition, string[] sign)
        {
            string condiStr = "";
            for (int i = 0; i < condition.Length; i++)
            {
                condiStr += condition[i] + " " + sign[i] + " ";
            }
            condiStr = condiStr.TrimEnd(' ');
            return condiStr;


        }


        private static string[] Connstr(string[] condition, string[] sign, string[] valstr)
        {
            if (condition.Length == valstr.Length && sign.Length > 0 && sign.Length == valstr.Length)
            {
                string[] condiStr = new string[condition.Length];
                for (int i = 0; i < condition.Length; i++)
                {

                    condiStr[i] = condition[i] + " " + sign[i] + " " + valstr[i];
                }

                return condiStr;
            }
            else
            {
                throw new Exception("条件列，值个数要和运算符相同，并且大与0");

            }
        }



        private static string ColumName(string[] columns, string[] valstr)
        {//update oms_order_head set  ORDER_STATUS='" +
            if (columns.Length == valstr.Length && columns.Length > 0)
            {
                string colStr = "";
                for (int i = 0; i < columns.Length; i++)
                {
                    valstr[i] = forVal(valstr[i]);
                    if (valstr[i].IndexOf("'") < 0)
                    {
                        valstr[i] = "'" + valstr[i] + "'";
                    }

                    colStr += columns[i] + "=" + valstr[i] + ",";
                }
                colStr = colStr.TrimEnd(',');
                colStr = colStr.Replace(CommonConst.getdate, "GetDate()");
                if (colStr.IndexOf("'GetDate()'") >= 0)
                {
                    colStr = colStr.Replace("'GetDate()'", "GetDate()");
                }
                colStr = colStr.Replace("'null'", "null");
                
                return colStr;
            }
            else
            {

                throw new Exception("列名称和列的值的个数要一致，并且大与0");


            }
        }
        private static  string forVal(string sql)
        {
            if (sql == null) return "null";
            if (sql.Length > 0 && (sql.Substring(sql.Length - 1) == "\0"))
            {
                sql = sql.Substring(0, sql.Length - 1);
            }
            if (sql.Length > 0 && (sql.Substring(sql.Length - 1) == "//") && (sql.Substring(sql.Length - 1) == "/"))
            {
                sql = sql.Substring(0, sql.Length - 1);

            }
            else if (sql.Length > 3 && sql.Substring(sql.Length - 2, 2).IndexOf('、') >= 0)
            {
                sql = sql.Substring(0, sql.Length - 2);
            }
            return sql;
        }

        public static string GetInsertStr(string[] colStr, string tableName, string[] valStr)
        {
            return GetInsertStr(ColumList(colStr), tableName, ValueList(valStr));
        }



        public static string GetSelectStr(string[] columns, string tableName, string[] condition, string[] sign, string bystr)
        {

            return GetSelectStr(ColumList(columns), tableName, ConnStr(condition, sign), bystr);
        }


        public static string GetUpdateStr(string[] columns, string[] valstr, string tableName, string[] condition, string[] sign)
        {

            return GetUpdateStr(ColumName(columns, valstr), tableName, ConnStr(condition, sign));
        }

        public static string GetUpdateStr(string[] columns, string[] valstr, string tableName, string condition)
        {

            return GetUpdateStr(ColumName(columns, valstr), tableName, condition);
        }

        public static string GetDeleteStr(string tableName, string[] condition, string[] sign)
        {
            return GetDeleteStr(tableName, ConnStr(condition, sign));
        }

        #endregion
    }
}
