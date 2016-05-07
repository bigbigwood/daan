using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using daan.dao.common;
using System.Data;
using System.Collections;
using daan.domain;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using System.Web;

namespace daan.service.common
{
    public class BaseService
    {
        private readonly BaseDAO dao;
        public delegate object transFunc(object _sqlmapper);

        protected BaseDAO baseDao
        {
            get
            {
                return dao ?? new BaseDAO();
            }
        }

        #region >>>查询 DataSet
        public DataSet selectDS(string tableName)
        {
            DataSet result = new DataSet();
            string sql = UtilService.GetSelectStr(tableName);
            result = baseDao.SelectDS("Common.Select", sql);
            return result;
        }

        public DataSet selectDS(string statementName, object param)
        {
            try
            {
                //var str = baseDao.GetSql(statementName, param);
                DataSet ds = baseDao.SelectDS(statementName, param);
                return ds;
            }
            catch (Exception ex)
            {
                //return null;
                throw new Exception(ex.Message);
            }
        }

        public DataSet selectDS(string statementName, object param, object _sqlmapper)
        {
            return baseDao.SelectDS(statementName, param, _sqlmapper);
        }
        #endregion

        #region >>>查询 IList
        public IList selectIList(string statementName, object param)
        {
            //var str = baseDao.GetSql(statementName, param);
            return baseDao.getMapper().QueryForList(statementName, param);
        }
        public IList selectIList(string statementName, object param, object _sqlmapper)
        {
            return (_sqlmapper as ISqlMapper).QueryForList(statementName, param);
        }
        public T selectObj<T>(string statementName, object param)
        {
            //var str = baseDao.GetSql(statementName, param);
            return baseDao.getMapper().QueryForObject<T>(statementName, param);
        }
        /// <summary>
        /// 模板方法取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> QueryList<T>(string statementName, object param)
        {
            //var str = baseDao.GetSql(statementName, param);
            return baseDao.getMapper().QueryForList<T>(statementName, param);
        }
        #endregion

        #region >>>编辑
        public int update(DataTable dt, String[] primarykey)
        {
            int result = 0;
            ArrayList sql = UtilService.GetUpdateStr(dt, primarykey);
            foreach (string s in sql)
                result += baseDao.getMapper().Update("Common.Update", s);
            return result;
        }
        public int update(string statementName, object param)
        {
            try
            {
                //var str = baseDao.GetSql(statementName, param);
                return baseDao.getMapper().Update(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' update.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        public int update(string statementName, object param, object _sqlmapper)
        {
            try
            {
                return (_sqlmapper as ISqlMapper).Update(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' update.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        public int updateByMapper(string statementName, object param)
        {
            try
            {
                return MapperDAO.NewMapper().Update(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' update.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        #endregion

        #region >>>新增
        public object insert(DataTable dt)
        {
            object result = new object();
            ArrayList sql = UtilService.GetInsertStr(dt);
            foreach (String s in sql)
                result = baseDao.getMapper().Insert("Common.Insert", s);
            return result;
        }
        public object insert(DataTable dt, string tablename)
        {
            DataTable dt1 = dt.Copy();
            dt1.TableName = tablename;
            object result = new object();
            ArrayList sql = UtilService.GetInsertStr(dt1);
            foreach (String s in sql)
                result = baseDao.getMapper().Insert("Common.Insert", s);
            return result;
        }
        public object insert(DataRow dr, string tablename)
        {
            DataTable dt = dr.Table.Clone();
            dt.TableName = tablename;
            dt.Rows.Add(dr.ItemArray);
            object result = new object();
            ArrayList sql = UtilService.GetInsertStr(dt);
            foreach (String s in sql)
                result = baseDao.getMapper().Insert("Common.Insert", s);
            return result;
        }
        public object insert(string statementName, object param)
        {

            try
            {
                return baseDao.getMapper().Insert(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' Insert.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        public object insertByNewMapper(string statementName, object param)
        {

            try
            {
                return MapperDAO.NewMapper().Insert(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' Insert.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        public object insert(string statementName, object param, object _sqlmapper)
        {
            try
            {
                return (_sqlmapper as ISqlMapper).Insert(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' Insert.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        #endregion

        #region >>>删除
        public int delete(string statementName, object param)
        {

            try
            {
                //var str = baseDao.GetSql(statementName, param);
                return baseDao.getMapper().Delete(statementName, param);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("执行出错{0}' delete.  错误描述: {1}", statementName, e.Message), e);
            }
        }
        #endregion

        #region >>>事务
        public object runTrans(transFunc func)
        {
            ISqlMapper mapper = baseDao.getMapper();

            object result = null;
            try
            {
                using (IDalSession session = mapper.BeginTransaction())
                {

                    result = func(mapper);
                    session.Complete();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("出错:" + ex);
            }
            return result;

        }
        #endregion

        #region >>>获取ORACLE序列
        /// <summary>
        /// 获取ORACLE序列
        /// </summary>
        /// <param name="seq_name"></param>
        /// <returns></returns>
        public virtual int getSeqID(String seq_name)
        {
            object obj = baseDao.getMapper().QueryForObject("Common.GetSeqID", seq_name);
            int result = (int)obj;
            return result;
        }
        #endregion

        #region >>>根据表最大ID获取下一个ID值
        public int getNextId(string tableName)
        {
            int id = -1;
            object o = runTrans(delegate(object _tableName) { return new TableIDDAO().GetNextId(tableName); });
            id = o == null ? -1 : int.Parse(o.ToString());
            return id;
        }
        #endregion

        #region >>>获取服务器时间
        public DateTime GetServerTime()
        {
            return baseDao.getMapper().QueryForObject<DateTime>("Common.GetServerTime", null);
        }
        #endregion >>>获取服务器时间


        #region >>>写日志
        /// <summary>
        /// 通过比较新旧对象，得到获取修改留痕信息(基础资料日志)
        /// <typeparam name="T"></typeparam>
        /// <param name="oldObj">原对象</param>
        /// <param name="newObj">修改后的对象</param>
        /// <returns></returns>
        /// </summary>
        public static List<LogInfo> getLogInfo<T>(T oldObj, T newObj)
        {
            String caption = "";
            List<LogInfo> logLst = new List<LogInfo>();
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                LogInfo info = new LogInfo();
                Object oldValue = prop.GetValue(oldObj, null);
                // if (oldValue == null) continue;
                Object newValue = prop.GetValue(newObj, null);
                LogInfoAttribute att = (LogInfoAttribute)Attribute.GetCustomAttribute(prop, typeof(LogInfoAttribute));
                caption = att == null ? prop.Name : att.Caption;
                info.Caption = caption;
                info.FieldName = prop.Name;
                info.OldValue = (oldValue == null || oldValue.ToString() == "") ? "空值" : oldValue.ToString();
                info.NewValue = (newValue == null || newValue.ToString() == "") ? "空值" : newValue.ToString();
                if (info.OldValue != info.NewValue)
                {
                    logLst.Add(info);
                }
            }

            return logLst;
        }

        /// <summary>
        /// 基础字典表维护记录保存
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="RecordId"></param>
        /// <param name="item"></param>
        /// <param name="OPERATIONTYPE"></param>
        /// <param name="itemname"></param>
        /// <param name="code"></param>
        public void AddMaintenanceLog(String TableName, Double? RecordId, List<LogInfo> item, String OPERATIONTYPE, String itemname, String code, string ModuleName)
        {
            Hashtable ht = new Hashtable();
            ht["TableName"] = TableName;
            ht["RecordId"] = RecordId;
            ht["item"] = item;
            ht["OPERATIONTYPE"] = OPERATIONTYPE;
            ht["itemname"] = itemname;
            ht["code"] = code;
            ht["ModuleName"] = ModuleName;
            // t.Start(ht);
            doAddMaintenanceLog(ht);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="RecordId"></param>
        /// <param name="item"></param>
        /// <param name="OPERATIONTYPE"></param>
        /// <returns></returns>
        /// <Author> qianz   2009-07-26  
        private void doAddMaintenanceLog(object obj)
        {
            UserInfo userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return;
            }
            Hashtable ht = (Hashtable)obj;

            String TableName = ht["TableName"] == null ? "" : ht["TableName"].ToString();
            Double? RecordId = ht["RecordId"] == null ? -1 : Double.Parse(ht["RecordId"].ToString());
            List<LogInfo> item = (List<LogInfo>)ht["item"];
            String OPERATIONTYPE = ht["OPERATIONTYPE"] == null ? "" : ht["OPERATIONTYPE"].ToString();
            String itemname = ht["itemname"] == null ? "" : ht["itemname"].ToString();
            String code = ht["code"] == null ? "" : ht["code"].ToString();
            string modulename = ht["ModuleName"] == null ? "" : ht["ModuleName"].ToString();

            Maintenancelog log = new Maintenancelog();
            log.Tablename = TableName;

            log.Operateby = userInfo.userId == 0 ? 1 : userInfo.userId;

            log.Recordid = RecordId;
            log.Operatedate = DateTime.Now;
            log.Operationtype = OPERATIONTYPE;
            log.Itemname = itemname;
            log.Code = code;
            log.Columnname = modulename;

            if (item == null)
            {
                log.Maintenancelogid = this.getSeqID("seq_maintenanceLog");
                insert("dict.InsertMaintenancelog", log);
            }
            else
            {
                log.Maintenancelogid = 0;
                foreach (LogInfo it in item)
                {
                    string columnname = string.Empty;
                    if (it.Caption == "IsChange" || it.Caption == "IsChanged") continue;
                    if (it.Caption == null || it.Caption.Trim() == "")
                    {
                        columnname = it.FieldName;
                    }
                    else
                    {
                        columnname = it.Caption;
                    }
                    if (it.Operation == null || it.Operation.Trim() == "")
                    {
                        log.Operation += String.Format("{0}由[{1}]更改为[{2}]，", columnname, it.OldValue, it.NewValue);
                    }
                    else { log.Operation = it.Operation+"，"; }

                }
                log.Maintenancelogid = this.getSeqID("seq_maintenanceLog");
                insert("dict.InsertMaintenancelog", log);
            }
        }

        /// <summary>
        /// 取得新增日志片段(操作日志)
        /// </summary>
        /// <param name="dtobject"></param>
        /// <returns></returns>
        public static string GetAddString(object dtobject)
        {
            string result = "";
            IEnumerator enumator =
                dtobject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).GetEnumerator();
            while (enumator.MoveNext())
            {
                var info = (PropertyInfo)enumator.Current;
                if (info == null) continue;

                var att = (LogInfoAttribute)Attribute.GetCustomAttribute(info, typeof(LogInfoAttribute));
                if (att == null) continue;

                //此字段需要进行写日志操作
                //  if (att.option != LogInfoAttribute.LOGFIELDOPTION) continue;
                PropertyInfo thisprop = info;

                //记录日志的字段不是使用此字段
                if (info.Name != att.LinkField && att.LinkField.Length > 0)
                {
                    thisprop = dtobject.GetType().GetProperty(att.LinkField);
                }

                if (thisprop == null) continue;
                var sourcevalue = thisprop.GetValue(dtobject, null) ?? "";

                if (result.Length > 0) result += ",";
                result += string.Format("“{0}”=“{1}”", att.Caption, GetTypeValue(sourcevalue));
            }
            return result;
        }

        /// <summary>
        /// 取得修改日志片段（操作日志）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newobject"></param>
        /// <returns></returns>
        public static string GetModifyString(object source, object newobject)
        {
            var result = "";
            var propenum =
                source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).GetEnumerator();
            while (propenum.MoveNext())
            {
                var info = (PropertyInfo)propenum.Current;
                if (info == null) continue;

                var att = (LogInfoAttribute)Attribute.GetCustomAttribute(info, typeof(LogInfoAttribute));
                if (att == null) continue;

                //判断此字段才进行写日志操作
                // if (att.option != LogInfoAttribute.LOGFIELDOPTION) continue;

                PropertyInfo sourceprop = info;
                PropertyInfo newprop = null;

                //对应字段不一样通过memo 来维护
                if (att.Memo != "")
                    newprop = newobject.GetType().GetProperty(att.Memo);
                else
                    newprop = newobject.GetType().GetProperty(sourceprop.Name);

                //记录日志的字段不是使用此字段
                if (info.Name != att.LinkField && (att.LinkField ?? "").Length > 0)
                {
                    sourceprop = source.GetType().GetProperty(att.LinkField);
                    newprop = newobject.GetType().GetProperty(att.LinkField);
                }

                if (sourceprop == null) continue;
                if (newprop == null) continue;

                //两个字段不为空

                var sourcevalue = sourceprop.GetValue(source, null) ?? "";
                var newvalue = newprop.GetValue(newobject, null) ?? "";
                if (sourcevalue.ToString() == newvalue.ToString()) continue;

                if (result.Length > 0) result += ",";
                result += string.Format("{0}由“{1}” 更改为“{2}”", att.Caption, GetTypeValue(sourcevalue), GetTypeValue(newvalue));
            }

            return result;
        }

        /// <summary>
        /// 类型值转换，提供默认的类型值转换
        /// </summary>
        private const string TRUE_VALUE = "是";
        private const string FALSE_VALUE = "否";
        private static string GetTypeValue(object value)
        {
            var result = value;
            if (value is bool)
            {
                if ((bool)value)
                    result = TRUE_VALUE;
                else
                    result = FALSE_VALUE;
            }
            return result.ToString();
        }

        /// <summary>
        /// 操作日志记录保存
        /// </summary>
        /// <param name="OrderNum">体检订单号</param>
        /// <param name="BarCode">条码</param>
        /// <param name="ModuleName">模块名称如：财务管理，总检</param>
        /// <param name="Content">操作内容</param>
        /// <param name="OperationType">操作的类型如：修改留痕，节点信息，增加备注</param>
        /// <param name="Remark">备注内容</param>
        public void AddOperationLog(string OrderNum, string BarCode, string ModuleName, string Content, string OperationType, string Remark,Hashtable htScan=null)
        {
            Hashtable ht = new Hashtable();
            ht["OrderNum"] = OrderNum;
            ht["BarCode"] = BarCode;
            ht["ModuleName"] = ModuleName;
            ht["Content"] = Content;
            ht["OperationType"] = OperationType;
            ht["Remark"] = Remark;
            ht["Scan"] = htScan;
            doAddOperationLog(ht);
        }

        /// <summary>
        /// 操作日志记录保存
        /// </summary>
        /// <param name="OrderNum">体检订单号</param>
        /// <param name="BarCode">条码</param>
        /// <param name="ModuleName">模块名称如：财务管理，总检</param>
        /// <param name="Content">操作内容</param>
        /// <param name="OperationType">操作的类型如：修改留痕，节点信息，增加备注</param>
        /// <param name="Remark">备注内容</param>
        /// <param name="currentUserInfo">当前操作用户</param>
        public void AddOperationLog(string OrderNum, string BarCode, string ModuleName, string Content, string OperationType, string Remark, UserInfo currentUserInfo, Hashtable htScan = null)
        {
            Hashtable ht = new Hashtable();
            ht["OrderNum"] = OrderNum;
            ht["BarCode"] = BarCode;
            ht["ModuleName"] = ModuleName;
            ht["Content"] = Content;
            ht["OperationType"] = OperationType;
            ht["Remark"] = Remark;
            ht["CurrentUserInfo"] = currentUserInfo;
            ht["Scan"] = htScan;
            doAddOperationLog(ht);
        }

        private void doAddOperationLog(object obj)
        {
            Hashtable ht = (Hashtable)obj;

            String OrderNum = ht["OrderNum"] == null ? "" : ht["OrderNum"].ToString();
            String BarCode = ht["BarCode"] == null ? "" : ht["BarCode"].ToString();
            String ModuleName = ht["ModuleName"] == null ? "" : ht["ModuleName"].ToString();
            String Content = ht["Content"] == null ? "" : ht["Content"].ToString();
            String OperationType = ht["OperationType"] == null ? "" : ht["OperationType"].ToString();
            String Remark = ht["Remark"] == null ? "" : ht["Remark"].ToString();
            Hashtable htScan = ht["Scan"] == null ? null : (Hashtable)ht["Scan"];
            UserInfo currentUserInfo = ht["CurrentUserInfo"] == null ? null : (UserInfo)ht["CurrentUserInfo"];

            UserInfo userInfo = currentUserInfo ?? GetUserInfo();

            Operationlog log = new Operationlog();
            log.Operationid = this.getSeqID("seq_OperationLog");
            log.Ordernum = OrderNum;
            log.Barcode = BarCode;
            log.Modulename = ModuleName;
            log.Createdate = DateTime.Now;
            log.Operationtype = OperationType;

            
            string Operatername = string.IsNullOrEmpty(userInfo.userName) ? "admin" : userInfo.userName;
            double? Operaterid =userInfo.userId == 0 ? 4 : userInfo.userId;
            if (htScan != null)
            {
                bool isScan = Convert.ToBoolean(htScan["isScan"]);
                if (isScan)
                {
                    Operatername = htScan["EnterBy"].ToString();
                    Operaterid = Convert.ToDouble(htScan["EnterByID"]);
                }
            }
            log.Operatername = Operatername;
            log.Operaterid = Operaterid;
            log.Content = Content;
            log.Remark = Remark;
            insert("dict.InsertOperationlog", log);
        }
        #endregion

        #region >>>> zhouy 获取用户信息 不存在则跳出 进行登录
        /// <summary>
        /// 获取用户信息 
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            try
            {
                if (HttpContext.Current.Session["UserInfo"] != null)
                {
                    userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                }                
            }
            catch
            {
            }
            return userInfo;
           
        }
        #endregion

        public SortList<T> IListToSortList<T>(IList lst)
        {
            IEnumerator e = lst.GetEnumerator();
            SortList<T> list = new SortList<T>();
            while (e.MoveNext())
            {
                list.Add((T)e.Current);
            }
            return list;
        }

        public List<T> IListToList<T>(IList lst)
        {
            IEnumerator e = lst.GetEnumerator();
            List<T> list = new List<T>();
            while (e.MoveNext())
            {
                list.Add((T)e.Current);
            }
            return list;
        }
        
        /// <summary>
        /// 执行事务，返回执行出错的错误信息
        /// </summary>
        /// <param name="SQLlist"></param>
        /// <param name="error">传入接受错误信息的字符串</param>
        /// <returns></returns>
        public bool ExecuteSqlTran(SortedList SQLlist ,ref string error)
        {
            bool b = true;
            ISqlMapper mapper = baseDao.getMapper();
            try
            {
                //事务
                using (IDalSession session = mapper.BeginTransaction())
                {

                    //循环
                    foreach (DictionaryEntry myDE in SQLlist)
                    {
                        Hashtable ht = (Hashtable)myDE.Key;
                        if (ht["INSERT"] != null)
                        {
                            mapper.Insert(ht["INSERT"].ToString(), myDE.Value);
                        }
                        else if (ht["UPDATE"] != null)
                        {
                            mapper.Update(ht["UPDATE"].ToString(), myDE.Value);
                        }
                        else if (ht["DELETE"] != null)
                        {
                            mapper.Delete(ht["DELETE"].ToString(), myDE.Value);
                        }
                    }
                    //提交事务
                    session.Complete();
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="SQLlist"></param>
        /// <returns></returns>
        public bool ExecuteSqlTran(SortedList SQLlist)
        {
            bool b = true;
            ISqlMapper mapper = baseDao.getMapper();
            try
            {
                //事务
                using (IDalSession session = mapper.BeginTransaction())
                {

                    //循环
                    foreach (DictionaryEntry myDE in SQLlist)
                    {
                        Hashtable ht = (Hashtable)myDE.Key;
                        if (ht["INSERT"] != null)
                        {
                            mapper.Insert(ht["INSERT"].ToString(), myDE.Value);
                        }
                        else if (ht["UPDATE"] != null)
                        {
                            mapper.Update(ht["UPDATE"].ToString(), myDE.Value);
                        }
                        else if (ht["DELETE"] != null)
                        {
                            mapper.Delete(ht["DELETE"].ToString(), myDE.Value);
                        }
                    }
                    //提交事务
                    session.Complete();
                }
            }
            catch (Exception e)
            {
                b = false;
            }

            return b;
        }
    }

}
