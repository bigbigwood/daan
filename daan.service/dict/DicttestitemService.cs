using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.service.common;
using daan.domain;
using System.Data;
using System.Collections;
using daan.util.Common;

namespace daan.service.dict
{
    public class DicttestitemService : BaseService
    {
        protected const string modulename = "检查项目维护";
        protected const string modulename1 = "检查组合维护";
        protected const string modulename2 = "检查套餐维护";

        #region >>>>新增编辑后保存 单项 ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="testitem"></param>
        /// <returns></returns>
        public double? SaveDictTestItem(Dicttestitem testitem, Dicttestitem testitemOld)
        {
            double? nflag = -1;

            //新增
            if (testitem.Dicttestitemid == null)
            {
                try
                {
                    testitem.Dicttestitemid = getSeqID("SEQ_DICTTESTITEM");
                    this.insert("Dict.InsertDicttestitem", testitem);
                  
                    nflag = testitem.Dicttestitemid;
                    //日志
                    List<LogInfo> logLst = getLogInfo<Dicttestitem>(new Dicttestitem(), testitem);
                    this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "新增", testitem.Testname, testitem.Testcode, modulename);

                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            else//保存
            {
                try
                {
                    //fhp 修改转进来的testitemOld跟修改后的对象一样的问题
                    testitemOld = SelectDicttestitemByDicttestitemid(testitem.Dicttestitemid);
                    this.update("Dict.UpdateDicttestitem", testitem);
                
                    nflag = 0;
                    //日志 
                    List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitemOld, testitem);
                    this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "修改", testitem.Testname, testitem.Testcode, modulename);
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            return nflag;
        }

        #endregion

        #region >>>>新增编辑后保存 组合项目 ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="testitem"></param>
        /// <returns></returns>
        public double? SaveDictTestItem(Dicttestitem testitem, List<Dicttestitem> testLst, Dicttestitem testitemOld,ref string  strerr)
        {
            SortedList sortedlist = new SortedList(new MySort());
            double? nflag = -1;

            Dicttestgroupdetail testgoutpdetail = new Dicttestgroupdetail();
            //新增
            if (testitem.Dicttestitemid == null)
            {
                try
                {
                    testitem.Dicttestitemid = getSeqID("SEQ_DICTTESTITEM");
                    sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDicttestitem" } }, testitem);
                    //this.insert("Dict.InsertDicttestitem", testitem);

                    //新增组合下的项目
                    if (testLst.Count != 0)
                    {
                        foreach (Dicttestitem item in testLst)
                        {
                            testgoutpdetail = new Dicttestgroupdetail();
                            testgoutpdetail.Testgroupid = testitem.Dicttestitemid;
                            testgoutpdetail.Dicttestitemid = item.Dicttestitemid;
                            testgoutpdetail.Createdate = DateTime.Now;
                            //this.insert("Dict.InsertDicttestgroupdetail", testgoutpdetail);
                            sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDicttestgroupdetail" } }, testgoutpdetail);
                        }
                    }
                   
                    bool b = this.ExecuteSqlTran(sortedlist,ref strerr);
                    if (b)
                    {
                        nflag = testitem.Dicttestitemid;
                        //日志 fhp
                        List<LogInfo> logLst = getLogInfo<Dicttestitem>(new Dicttestitem(), testitem);
                        this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "新增", testitem.Testname, testitem.Testcode, modulename1);
                        //新增组合下的项目
                        if (testLst.Count != 0)
                        {
                            foreach (Dicttestitem item in testLst)
                            {
                                testgoutpdetail.Testgroupid = testitem.Dicttestitemid;
                                testgoutpdetail.Dicttestitemid = item.Dicttestitemid;
                                testgoutpdetail.Createdate = DateTime.Now;
                                //日志 fhp
                                List<LogInfo> logLstDetail = getLogInfo<Dicttestgroupdetail>(new Dicttestgroupdetail(), testgoutpdetail);
                                this.AddMaintenanceLog("Dicttestgroupdetail", testgoutpdetail.Testgroupid, logLstDetail, "新增", testgoutpdetail.Testgroupid.ToString(), testgoutpdetail.Dicttestitemid.ToString(), modulename1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    strerr += ex.Message;
                }
            }
            else//保存
            {
                try
                {

                    //this.update("Dict.UpdateDicttestitem", testitem);
                    sortedlist.Add(new Hashtable { { "UPDATE", "Dict.UpdateDicttestitem" } }, testitem);

                    //修改组合下的项目  先删除再新增
                    //this.delete("Dict.DeleteDicttestgroupdetail", testitem.Dicttestitemid);
                    sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestgroupdetail" } }, testitem.Dicttestitemid);

                    if (testLst.Count != 0)
                    {
                        foreach (Dicttestitem item in testLst)
                        {
                            testgoutpdetail = new Dicttestgroupdetail();
                            testgoutpdetail.Testgroupid = testitem.Dicttestitemid;
                            testgoutpdetail.Dicttestitemid = item.Dicttestitemid;
                            testgoutpdetail.Createdate = DateTime.Now;
                            //this.insert("Dict.InsertDicttestgroupdetail", testgoutpdetail);
                            sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDicttestgroupdetail" } }, testgoutpdetail);
                        }
                    }
                    bool b = this.ExecuteSqlTran(sortedlist,ref strerr);
                    if (b)
                    {
                        nflag = 0;
                        List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitemOld, testitem);
                        this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "修改", testitem.Testname, testitem.Testcode, modulename1);
                        //日志 使用LogInfo类展现删除信息 fhp
                        LogInfo strLogInfo = new LogInfo();
                        strLogInfo.Operation = "套餐:" + testitem.Testname + "下的所有组合";
                        List<LogInfo> logLstDel = getLogInfo<LogInfo>(strLogInfo, new LogInfo());
                        this.AddMaintenanceLog("Dicttestgroupdetail", testitem.Dicttestitemid, logLstDel, "删除", null, null, modulename1);
                        if (testLst.Count != 0)
                        {
                            foreach (Dicttestitem item in testLst)
                            {
                                testgoutpdetail.Testgroupid = testitem.Dicttestitemid;
                                testgoutpdetail.Dicttestitemid = item.Dicttestitemid;
                                testgoutpdetail.Createdate = DateTime.Now;
                                //日志 fhp
                                List<LogInfo> logLstDetail = getLogInfo<Dicttestgroupdetail>(new Dicttestgroupdetail(), testgoutpdetail);
                                this.AddMaintenanceLog("Dicttestgroupdetail", testgoutpdetail.Testgroupid, logLstDetail, "新增", testgoutpdetail.Testgroupid.ToString(), testgoutpdetail.Dicttestitemid.ToString(), modulename1);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    strerr += ex.Message;
                }
            }
            return nflag;
        }

        #endregion

        #region >>>>新增编辑后保存 套餐 ylp
        /// <summary>
        /// 新增编辑后保存
        /// </summary>
        /// <param name="testitem"></param>
        /// <returns></returns>
        public double? SaveDictTestItemProduct(Dicttestitem testitem, List<Dicttestitem_productdetail> testLst, Dicttestitem testitemOld)
        {
            SortedList sortedlist = new SortedList(new MySort());
            double? nflag = -1;
            Dictproductdetail productdetail = new Dictproductdetail();
            //新增
            if (testitem.Dicttestitemid == null)
            {
                try
                {
                    testitem.Dicttestitemid = getSeqID("SEQ_DICTTESTITEM");
                    //this.insert("Dict.InsertDicttestitem", testitem);
                    sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDicttestitem" } }, testitem);
                    //新增套餐下的项目
                    if (testLst.Count != 0)
                    {
                        foreach (Dicttestitem_productdetail item in testLst)
                        {
                            productdetail = new Dictproductdetail();
                            productdetail.Productid = testitem.Dicttestitemid;
                            productdetail.Testgroupid = item.Dicttestitem.Dicttestitemid;
                            productdetail.Sendoutcustomerid = item.Sendoutcustomerid;
                            productdetail.Issendouttest = item.Issendouttest;
                            productdetail.Finalprice = item.Finalprice;
                            productdetail.Createdate = DateTime.Now;
                            //this.insert("Dict.InsertDictproductdetail", productdetail);
                            sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDictproductdetail" } }, productdetail);
                        }
                    }
                    bool b = this.ExecuteSqlTran(sortedlist);
                    if (b)
                    {
                        nflag = testitem.Dicttestitemid;
                        //日志 fhp
                        List<LogInfo> logLst = getLogInfo<Dicttestitem>(new Dicttestitem(), testitem);
                        this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "新增", testitem.Testname, testitem.Testcode, modulename2);
                        if (testLst.Count != 0)
                        {
                            foreach (Dicttestitem_productdetail item in testLst)
                            {
                                productdetail = new Dictproductdetail();
                                productdetail.Productid = testitem.Dicttestitemid;
                                productdetail.Testgroupid = item.Dicttestitem.Dicttestitemid;
                                productdetail.Sendoutcustomerid = item.Sendoutcustomerid;
                                productdetail.Issendouttest = item.Issendouttest;
                                productdetail.Finalprice = item.Finalprice;
                                productdetail.Createdate = DateTime.Now;
                                //日志 fhp
                                List<LogInfo> logLstDetail = getLogInfo<Dictproductdetail>(new Dictproductdetail(), productdetail);
                                this.AddMaintenanceLog("Dictproductdetail", productdetail.Productid, logLstDetail, "新增", productdetail.Testgroupid.ToString(), productdetail.Productid.ToString(), modulename2);

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            else//保存
            {
                try
                {
                    //this.update("Dict.UpdateDicttestitem", testitem);
                    sortedlist.Add(new Hashtable { { "UPDATE", "Dict.UpdateDicttestitem" } }, testitem);
                    //修改组合下的项目  先删除再新增
                    //this.delete("Dict.DeleteDictproductdetail", testitem.Dicttestitemid);
                    sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDictproductdetail" } }, testitem.Dicttestitemid);

                    if (testLst.Count != 0)
                    {
                        foreach (Dicttestitem_productdetail item in testLst)
                        {
                            productdetail = new Dictproductdetail();
                            productdetail.Productid = testitem.Dicttestitemid;
                            productdetail.Testgroupid = item.Dicttestitem.Dicttestitemid;
                            productdetail.Createdate = DateTime.Now;
                            productdetail.Sendoutcustomerid = item.Sendoutcustomerid;
                            productdetail.Issendouttest = item.Issendouttest;
                            productdetail.Finalprice = item.Finalprice;
                            //this.insert("Dict.InsertDictproductdetail", productdetail);
                            sortedlist.Add(new Hashtable { { "INSERT", "Dict.InsertDictproductdetail" } }, productdetail);
                        }
                    }
                    bool b = this.ExecuteSqlTran(sortedlist);
                    if (b)
                    {
                        nflag = 0;
                        List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitemOld, testitem);
                        this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "修改", testitem.Testname, testitem.Testcode, modulename2);
                        //日志 使用LogInfo类展现删除信息 fhp
                        LogInfo strLogInfo = new LogInfo();
                        strLogInfo.Operation = "套餐:" + testitem.Testname + "下的所有组合";
                        List<LogInfo> logLstDel = getLogInfo<LogInfo>(strLogInfo, new LogInfo());
                        this.AddMaintenanceLog("Dictproductdetail", testitem.Dicttestitemid, logLstDel, "删除", null, null, modulename2);
                        if (testLst.Count != 0)
                        {
                            foreach (Dicttestitem_productdetail item in testLst)
                            {
                                productdetail = new Dictproductdetail();
                                productdetail.Productid = testitem.Dicttestitemid;
                                productdetail.Testgroupid = item.Dicttestitem.Dicttestitemid;
                                productdetail.Createdate = DateTime.Now;
                                productdetail.Sendoutcustomerid = item.Sendoutcustomerid;
                                productdetail.Issendouttest = item.Issendouttest;
                                productdetail.Finalprice = item.Finalprice;
                                //日志 fhp
                                List<LogInfo> logLstDetail = getLogInfo<Dictproductdetail>(new Dictproductdetail(), productdetail);
                                this.AddMaintenanceLog("Dictproductdetail", productdetail.Productid, logLstDetail, "新增", productdetail.Testgroupid.ToString(), productdetail.Productid.ToString(), modulename2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    nflag = -1;
                    throw new Exception(ex.Message);
                }
            }
            return nflag;
        }

        #endregion

        #region >>>>删除项目及其结果 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">项目主键</param>
        /// <returns></returns>
        public bool DelDicttestitemByID(Dicttestitem testitem)
        {
            SortedList sortedlist = new SortedList(new MySort());

            bool b = false;

            try
            {
                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestitemresultBytestitemid" } }, testitem.Dicttestitemid);//根据项目Id删除其对应的项目结果
                //this.delete("Dict.DeleteDicttestitem", testitem.Dicttestitemid);
                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestitem" } }, testitem.Dicttestitemid);
                //this.delete("Dict.DeleteDicttestitemresultBytestitemid", testitem.Dicttestitemid);

                b = this.ExecuteSqlTran(sortedlist);
                if (b)
                {
                    //日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitem, new Dicttestitem());
                    this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "删除", testitem.Testname, testitem.Testcode, modulename);
                    //日志 使用LogInfo类展现删除信息 fhp
                    LogInfo strLogInfo = new LogInfo();
                    strLogInfo.Operation = "套餐:" + testitem.Testname + "下的所有组合";
                    List<LogInfo> logLstDel = getLogInfo<LogInfo>(strLogInfo, new LogInfo());
                    this.AddMaintenanceLog("Dicttestitemresult", testitem.Dicttestitemid, logLstDel, "删除", null, null, modulename);

                }
            }
            catch (Exception ex)
            {
                b = false;
                throw new Exception(ex.Message);
            }
            return b;
        }
        #endregion

        #region >>>>删除组合项目 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">项目主键</param>
        /// <returns></returns>
        public bool DelDictGrouptestitemByID(Dicttestitem testitem)
        {
            bool b = false;
            SortedList sortedlist = new SortedList(new MySort());
            try
            {
                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestgroupdetail" } }, testitem.Dicttestitemid);//根据组合项目Id删除组合项目明细

                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestitem" } }, testitem.Dicttestitemid);

                b = this.ExecuteSqlTran(sortedlist);
                if (b)
                {
                    //日志 fhp
                    List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitem, new Dicttestitem());
                    this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "删除", testitem.Testname, testitem.Testcode, modulename1);
                    //日志 使用LogInfo类展现删除信息 fhp
                    LogInfo strLogInfo = new LogInfo();
                    strLogInfo.Operation = "套餐:" + testitem.Testname + "下的所有组合";
                    List<LogInfo> logLstDel = getLogInfo<LogInfo>(strLogInfo, new LogInfo());
                    this.AddMaintenanceLog("Dicttestgroupdetail", testitem.Dicttestitemid, logLstDel, "删除", null, null, modulename1);
                }
            }
            catch (Exception ex)
            {
                b = false;
                throw new Exception(ex.Message);
            }
            return b;
        }
        #endregion

        #region >>>>删除套餐项目 ylp
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">套餐项目主键</param>
        /// <returns></returns>
        public bool DelDictProducttestitemByID(Dicttestitem testitem)
        {
            SortedList sortedlist = new SortedList(new MySort());
            bool b = false;

            try
            {

                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDictproductdetail" } }, testitem.Dicttestitemid);//根据套餐Id删除套餐明细
                sortedlist.Add(new Hashtable { { "DELETE", "Dict.DeleteDicttestitem" } }, testitem.Dicttestitemid);
                b = this.ExecuteSqlTran(sortedlist);
                if (b)
                {
                    //日志
                    List<LogInfo> logLst = getLogInfo<Dicttestitem>(testitem, new Dicttestitem());
                    this.AddMaintenanceLog("Dicttestitem", testitem.Dicttestitemid, logLst, "删除", testitem.Testname, testitem.Testcode, modulename2);
                    //日志 使用LogInfo类展现删除信息 fhp
                    LogInfo strLogInfo = new LogInfo();
                    strLogInfo.Operation = "套餐:" + testitem.Testname + "下的所有组合";
                    List<LogInfo> logLstDel = getLogInfo<LogInfo>(strLogInfo, new LogInfo());
                    this.AddMaintenanceLog("Dictproductdetail", testitem.Dicttestitemid, logLstDel, "删除", null, null, modulename2);
                }
            }
            catch (Exception ex)
            {
                b = false;
                throw new Exception(ex.Message);
            }
            return b;
        }
        #endregion

        #region >>>> 查询所有测试项
        public DataSet GetTestItem()
        {
            return this.selectDS("Dict.SelectDicttestitem", null);
        }

      
        #endregion

        #region >>>> 查询所有测试项 zhangwei
        public IList<Dicttestitem> GetTestItemById()
        {
            return this.QueryList<Dicttestitem>("Dict.SelectDicttestitemById", null);
        }
        #endregion

        #region >>>>套餐未包含分页 ylp
        /// <summary>
        /// 套餐未包含项目分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetDictproductPageLst(Hashtable ht)
        {

            return this.QueryList<Dicttestitem>("Dict.GettDictproductPageLst", ht).ToList<Dicttestitem>();
        }


        /// <summary>
        /// 套餐未包含项目总页数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDictproductPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictproductPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>>组合未包含项分页 ylp
        /// <summary>
        /// 组合未包含项目分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetDicttestgroupPageLst(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.GettDicttestgroupPageLst", ht).ToList<Dicttestitem>();
        }

        /// <summary>
        /// 组合未包含项目总页数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDicttestgroupPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDicttestgroupPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>>项目分页 ylp
        /// <summary>
        /// 项目分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetDictTestItemPageLst(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.GetDictTestItemPageLst", ht).ToList<Dicttestitem>();
        }

        /// <summary>
        /// 项目分页总数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDictTestItemPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictTestItemPageLstCount", ht).Tables[0].Rows[0][0]); 
        }
        #endregion

        #region >>>>根据条件查找项目 用于导出 ylp
        /// <summary>
        /// 根据条件查找项目
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetSearchData(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.GetDictTestItemLst", ht).ToList<Dicttestitem>();
        }
        #endregion

        #region >>>>根据ID得到实体对象 ylp
        /// <summary>
        /// 根据ID得到实体
        /// </summary>
        /// <param name="dicttestitemid"></param>
        /// <returns></returns>
        public Dicttestitem SelectDicttestitemByDicttestitemid(double? dicttestitemid)
        {
            try
            {
                return this.selectObj<Dicttestitem>("Dict.SelectDicttestitemById", dicttestitemid);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region >>>>组合包含项目 ylp
        /// <summary>
        /// 组合包含项目
        /// </summary>
        /// <param name="testitemid"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetGroupInTestItem(double? testitemid)
        {
            return this.QueryList<Dicttestitem>("Dict.GetDicttestgroupIn", testitemid).ToList<Dicttestitem>();
        }
        #endregion

        #region >>>>套餐分页 ylp
        /// <summary>
        /// 套餐分页
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetDictTestProductPageLst(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.GetDictTestProductPageLst", ht).ToList<Dicttestitem>();
        }

        /// <summary>
        /// 套餐总页数
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetDictTestProductPageLstCount(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Dict.GetDictTestProductPageLstCount", ht).Tables[0].Rows[0][0]);
        }
        #endregion

        #region >>>>根据条件查找套餐 用于导出 ylp
        /// <summary>
        /// 根据条件查找套餐
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetSearchTestProductData(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.GetDictTestProductPageLstCount", ht).ToList<Dicttestitem>();
        }
        #endregion

        #region >>>>根据分点查询项目组合
        /// <summary>
        /// 根据条件查找组合项目
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetGroupTestByLabId(object labid)
        {
            return this.QueryList<Dicttestitem>("Dict.SelectGroupTestByLabId", labid).ToList<Dicttestitem>();
        }


        /// <summary>
        /// 根据条件查找组合项目
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetInitgdGroupTestByLabId(object labid, int startpage, int endpage, string key)
        {
            Hashtable ht = new Hashtable();
            ht["labid"] = labid;
            ht["pageStart"] = startpage;
            ht["pageEnd"] = endpage;
            ht["key"] = key;
            return this.QueryList<Dicttestitem>("Dict.SelectInitgdGroupTestByLabId", ht).ToList<Dicttestitem>();
        }

        /// <summary>
        /// 根据条件查找组合项目
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int GetInitgdGroupTestByLabIdCount(object labid, string key)
        {
            Hashtable ht = new Hashtable();
            ht["labid"] = labid;
            ht["key"] = key;
            object obj=this.selectObj<object>("Dict.SelectInitgdGroupTestByLabIdCount", ht);
            return Convert.ToInt32(obj);
        }
        #endregion

        #region >>>>根据分点查询套餐
        /// <summary>
        /// 根据分点查询套餐+公用套餐
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<Dicttestitem> GetProduct(object customerid)
        {
            return this.QueryList<Dicttestitem>("Dict.SelectProductByCustomerId", customerid).ToList<Dicttestitem>();
        }
        #endregion


        public List<Dicttestitem> GetDicttestitemByCode(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.SelectDicttestitemByCode", ht).ToList<Dicttestitem>();
        }

        public List<Dicttestitem> GetDicttestitemByID(Hashtable ht)
        {
            return this.QueryList<Dicttestitem>("Dict.SelectDicttestitemByID", ht).ToList<Dicttestitem>();
        }
    }
}
