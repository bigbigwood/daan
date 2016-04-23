using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.domain;
using daan.service.common;
using System.Collections;
using daan.util.Common;
using System.Data;
using daan.service.login;
using daan.service.dict;
namespace daan.service.order
{
    public class OrderlabdeptresultService : BaseService
    {
        LoginService loginservice = new LoginService();
        OrderTestService orderTestService = new OrderTestService();

        /// <summary>自动小结
        /// 自动小结
        /// </summary>
        /// <param name="strordernum">dictlab 分点对象，用于各地的参考范围不同时</param>
        public void AutoSummary(string strordernum,double? dictlabid)
        {
            #region ***********************科室小结***********************

            #region 科室小结实现方法描述
            //[1]汇总异常结果组成字符串，字符串组成方式：
            //  1、数值型结果组成方式 ：  项目名称 +“：” + 结果 + 高低提示符
            //  2、描述性结果组成方式 ：  项目名称 +“：” + 结果
            //[2]多个异常项目用分号隔开。如果结果全部是正常，则默认“未见异常”。
            #endregion


            //1,获取体检号对应的结果
            List<Ordertest> orderTestListAll = orderTestService.GetAllOrderLabdeptresultList(strordernum);//全部结果
            List<Ordertest> orderTestList = orderTestListAll.Where(c => c.Testresult != null && c.Testresult != null).ToList<Ordertest>();//去掉结果为null的
           
            //2，获取无重复的不为空的科室ID
            var labdepidList = (from o in orderTestList
                                select o.Dictlabdeptid).Distinct();
            //3，组合科室小结内容
            List<Orderlabdeptresult> editTemp = new List<Orderlabdeptresult>();//存放待编辑科室小结
            foreach (Double? lab in labdepidList)
            {
                //过滤掉有结果为空的科室
                if (orderTestListAll.Where(c => (c.Testresult == "" || c.Testresult == null) && c.Dictlabdeptid == lab).Count() > 0)
                {
                    continue;
                }

                //获取科室所属检查项目
                List<Ordertest> orderTestTemp = orderTestList.Where(test => test.Dictlabdeptid == lab.Value).ToList<Ordertest>();

                //实例化科室小结对象
                Orderlabdeptresult orderlabdeptresultTemp = new Orderlabdeptresult();
                orderlabdeptresultTemp.Ordernum = strordernum;
                orderlabdeptresultTemp.Dictlabdeptid = lab.Value;
                orderlabdeptresultTemp.Appraiseby = 4;//系统自动小结，4为User帐号


                //判断是否全部为正常，如正常则结果为未见异常，否则组合异常结果       
                if (orderTestTemp.Count<Ordertest>(c => c.Isexception == "1") == 0)
                {
                    orderlabdeptresultTemp.Labdeptresult = "未见异常";
                    editTemp.Add(orderlabdeptresultTemp);
                    continue;//继续下一循环
                }

                //有异常，组合异常结果
                StringBuilder sb = new StringBuilder();
                foreach (Ordertest orderTest in orderTestTemp)
                {
                    if (orderTest.Isexception == "0") continue;//只记录异常的
                    if (ValidateUtils.IsNumeric(orderTest.Testresult))
                    {
                        //数值型
                        sb.AppendFormat("{0}:{1} {2}；", orderTest.Testname, orderTest.Testresult, orderTest.Hlflag);
                    }
                    else
                    {
                        //描述性
                        sb.AppendFormat("{0}:{1}；", orderTest.Testname, orderTest.Testresult);
                    }
                }
                orderlabdeptresultTemp.Labdeptresult = sb.ToString();
                editTemp.Add(orderlabdeptresultTemp);
            }
            #endregion


            #region ***********************诊断建议***********************
            #region 诊断建议实现方法描述
            //1,匹配符合条件的规则公式[规则公式中的项目清单是该体检检验项目的子集]，年龄，性别判断等
            //2,由传入参数判断规则公式[sourcecode]，false则将该诊断建议存至临时List中保存，以待插入诊断建议表
            //3,科室检查项目有多项异常，则逐条记录，如果全部没有，则记录"未见异常"
            #endregion
            List<Dictruleformular> ruleformularAll = loginservice.GetLoginDictruleformularresultList();  //从缓存读取规则公式
            List<Dictdiagnosis> diagnosisAll = loginservice.GetLoginDictdiagnosisresultList();//从缓存读取诊断建议

            #region >>>获取体检项目ID集合和检查项目结果集合
            string[] testIdArray = new string[orderTestList.Count];
            object[] testResultArray = new object[orderTestList.Count];
            int nIndex = 0;
            string tempTestResult = string.Empty;
            for (int i = 0; i < orderTestList.Count; i++)
            {
                testIdArray[i] = orderTestList[i].Dicttestitemid.ToString();
                //结果中包含有><号时要去掉><号号，将原值加或减掉0.0000001
                tempTestResult = orderTestList[i].Testresult.Replace('>', ' ').Replace('<', ' ').Trim();
                //加上前面的判断是为了区分有些结果为阴性的不是传文字类型而是直接传一个-过来的情况
                //tempTestResult != "-" && tempTestResult!="+"&&
                if (ValidateUtils.IsNumeric(tempTestResult))//数字类型
                {
                    if (orderTestList[i].Testresult.Contains(">"))
                    {
                        testResultArray[nIndex++] = double.Parse(tempTestResult) + 0.0000001;
                    }
                    else if (orderTestList[i].Testresult.Contains("<"))
                    {
                        testResultArray[nIndex++] = double.Parse(tempTestResult) - 0.0000001;
                    }
                    else
                    {
                        testResultArray[nIndex++] = double.Parse(orderTestList[i].Testresult);
                    }
                }
                else
                {
                    //HPV 人乳头瘤 结果有科学计数法，要转成普通的
                    if (tempTestResult.Contains("E+"))
                    {
                        try
                        {
                            testResultArray[nIndex++] = double.Parse(tempTestResult);
                        }
                        catch
                        {
                            testResultArray[nIndex++] = orderTestList[i].Testresult;
                        }
                    }
                    else
                    {
                        testResultArray[nIndex++] = orderTestList[i].Testresult;
                    }
                }
            }
            #endregion

            #region >>>获取匹配的自动诊断建议
            //获取用户的基本信息
            Dictmember member = new DictmemberService().SelectDictmemberByOrderNum(strordernum);

            //通用公式
            List<Dictruleformular> isUserRuleformularLstTemp = ruleformularAll.Where(r => (r.Sex == member.Sex || r.Sex == "B" || r.Sex == "U") && (r.Ismarry == member.Ismarried || r.Ismarry == "2") && r.Dictlabid == 0
                                                       && (member.Caculatedage > r.Caculatedagelow && member.Caculatedage < r.Caculatedagehigh)).ToList<Dictruleformular>();
            //先根据年龄，性别,婚否及分点第一次筛选   
            if (dictlabid != 0)
            {
                //分点公式
                List<Dictruleformular> labRuleformularLst = ruleformularAll.Where(r => (r.Sex == member.Sex || r.Sex == "B" || r.Sex == "U") && (r.Ismarry == member.Ismarried || r.Ismarry == "2") && r.Dictlabid == dictlabid
                                                                        && (member.Caculatedage > r.Caculatedagelow && member.Caculatedage < r.Caculatedagehigh)).ToList<Dictruleformular>();
                //分点中有和通用相同的规则代码，则取分点中的规则公式
                if (labRuleformularLst.Count > 0)
                {
                    foreach (Dictruleformular ruleItem in labRuleformularLst)
                    {

                        Dictruleformular tempRule = isUserRuleformularLstTemp.FirstOrDefault<Dictruleformular>(c => c.Dictrulecode == ruleItem.Dictrulecode);
                        if (tempRule != null)
                        {
                            isUserRuleformularLstTemp.Remove(tempRule);
                            isUserRuleformularLstTemp.Add(ruleItem);
                        }
                        else
                        {
                            isUserRuleformularLstTemp.Add(ruleItem);
                        }
                    }
                }
            }
            
            //保存该体检项目集合的子集            
            List<Dictruleformular> isUserRuleformularLst = new List<Dictruleformular>();
            //规则公式是否是该体检项目集合的子集，不是子集，移除            
            foreach (Dictruleformular ruleformular in isUserRuleformularLstTemp)
            {
                if (isSubSet(testIdArray, ruleformular.Formulartests.Split(',')))
                {
                    isUserRuleformularLst.Add(ruleformular);
                }
            }
            #endregion

            #region 计算规则公式[false],匹配诊断建议

            //临时存储计算后的诊断建议
            List<Dictdiagnosis> tempDiagnosisLst = new List<Dictdiagnosis>();

            //规则公式返回false，则生成评价
            ArrayList lstHealthCheck = new ArrayList();
            //循环该次体检包含的科室
            foreach (Double? lab in labdepidList)
            {
                //遍历该科室的规则公式
                foreach (Dictruleformular ruleitem in isUserRuleformularLst.Where(r => r.Dictlabdeptid == lab))
                {
                    //计算公式项目清单集合
                    string[] ruleTest = ruleitem.Formulartests.Split(new char[] { ',' });

                    //组装计算公示用到的参数
                    ArrayList results = new ArrayList();
                    for (int w = 0; w < ruleTest.Length - 1; w++)//遍历规则公式中的项目清单
                    {
                        for (int k = 0; k < testIdArray.Length; k++)//遍历该体检项目集合
                        {
                            if (testIdArray[k] == ruleTest[w]) { results.Add(testResultArray[k]); continue; }
                        }
                    }
                    if (DictRuleFormularService.GetRuleFormularResult(ruleitem.Sourcecode, results.ToArray()))
                    {
                        Dictdiagnosis tempDiagnosis = diagnosisAll.Find(c => c.Dictdiagnosisid == ruleitem.Dictdiagnosisid);
                        //将科室ID添加到诊断实体中
                        tempDiagnosis.Dictlabdeptid = lab;
                        tempDiagnosisLst.Add(tempDiagnosis);
                    }
                }
            }
            #endregion

            #endregion

            //#region add by lee 20121218 过滤掉重复的或者相冲突的异常建议，如HPV分型的一些建议
            //List<Dictdiagnosis> diagnosisLst = tempDiagnosisLst;
            //List<Dictdiagnosesmutex> mutexLst = loginservice.GetLoginDictdiagnosesmutexList();
            //foreach (Dictdiagnosis diagnosisitem in tempDiagnosisLst)
            //{
            //    List<Dictdiagnosesmutex> mutexLstTemp = mutexLst.Where<Dictdiagnosesmutex>(c => c.Dictdiagnosisid == diagnosisitem.Dictdiagnosisid).ToList<Dictdiagnosesmutex>();
            //    if (mutexLstTemp.Count > 0)
            //    {
            //        foreach (Dictdiagnosesmutex mutexItem in mutexLstTemp)
            //        {
            //            if (isUserRuleformularLst.Count<Dictruleformular>(d => d.Dictdiagnosisid == mutexItem.Dictmutexdiagnosisid) > 0)
            //            {
            //                //move
            //                diagnosisLst.Remove(diagnosisitem);
            //            }
            //        }
            //    }
            //}
            //#endregion


            #region ***********************保存自动小结与诊断建议内容，并做审核操作***********************

            SaveOrderlabdeptresult(editTemp, tempDiagnosisLst, true);

            #endregion **********************************************

            //写日志
            new BaseService().AddOperationLog(strordernum, "", "检查结果录入", "对[" + strordernum + "]执行自动小结", "修改留痕", "");
            
        }

        /// <summary>保存科室小结及诊断建议
        /// 保存科室小结及诊断建议
        /// </summary>
        /// <param name="list"></param>
        /// <param name="diagnosisLst"></param>
        /// <param name="isAuto">是否自动接收结果  True自动 false手动</param>
        /// <returns></returns>
        public bool SaveOrderlabdeptresult(List<Orderlabdeptresult> list, List<Dictdiagnosis> diagnosisLst,bool isAuto)
        {
            #region >>>变量定义
            SortedList sqlLst = new SortedList(new MySort());
            bool isFirst = true;
            //体检流水号变量
            string tempOrdernum=null;
            #endregion

            #region >>>组装科室小结
            foreach (Orderlabdeptresult temp in list)
            {

                tempOrdernum=temp.Ordernum;

                //第一次循环要删除之前录入的科室小结、诊断建议
                if (isFirst)
                {                    
                    //科室小结
                    Hashtable htOrderlabdeptresult = new Hashtable();
                    htOrderlabdeptresult.Add("DELETE", "Order.DeleteOrderlabdeptresultByOrderNum");
                    sqlLst.Add(htOrderlabdeptresult, tempOrdernum);

                    //诊断建议
                    Hashtable htOrderdiagnosis = new Hashtable();
                    htOrderdiagnosis.Add("DELETE", "Order.DeleteOrderdiagnosisByOrderNum");
                    sqlLst.Add(htOrderdiagnosis, tempOrdernum);

                    isFirst = false;
                }
                Hashtable ht2 = new Hashtable();
                temp.Ordertlabdeptresultid = this.getSeqID("SEQ_ORDERLABDEPTRESULT");
                ht2.Add("INSERT", "Order.InsertOrderlabdeptresult");
                sqlLst.Add(ht2, temp);
            }
            #endregion

            #region >>>循环组装体检建议
            foreach (Dictdiagnosis temp in diagnosisLst)
            {
                Orderdiagnosis orderDiagnosis=new Orderdiagnosis();
                orderDiagnosis.Orderdiagnosisid=this.getSeqID("SEQ_ORDERDIAGNOSIS");
                orderDiagnosis.Createdate=DateTime.Now;
                orderDiagnosis.Dictdiagnosisid = temp.Dictdiagnosisid;
                orderDiagnosis.Diagnosisname=temp.Diagnosisname;
                orderDiagnosis.Diagnosistype=temp.Diagnosistype;
                orderDiagnosis.Dictlabdeptid=temp.Dictlabdeptid;
                orderDiagnosis.Diseasecause=temp.Diseasecause;
                orderDiagnosis.Diseasedescription=temp.Diseasedescription;
                orderDiagnosis.Displayorder=temp.Displayorder;
                orderDiagnosis.Engdiseasecause=temp.Engdiseasecause;
                orderDiagnosis.Engdiseasedescription=temp.Engdiseasedescription;
                orderDiagnosis.Engsuggestion=temp.Engsuggestion;
                orderDiagnosis.LastUpdateDate=DateTime.Now;
                orderDiagnosis.Ordernum=tempOrdernum;
                orderDiagnosis.Suggestion=temp.Suggestion;
                orderDiagnosis.Isdisease = temp.Isdisease;
         

                Hashtable ht = new Hashtable();
                ht.Add("INSERT", "Order.InsertOrderdiagnosis");
                sqlLst.Add(ht, orderDiagnosis);
            }
            #endregion

            #region >>>更改Orderlabdeptresult表状态为已小结
            return UpdateState(sqlLst, tempOrdernum,isAuto);
            #endregion            
        }
      
        /// <summary>接收后更改Orderlabdeptresult表状态为已小结,如果是自动接收，一并做审核
        ///接收后更改Orderlabdeptresult表状态为已小结,如果是自动接收，一并做审核
        ///操作顺序
        ///1，直接标记科室小结为已审核，忽略已小结状态
        ///2,判断是否全是检验项目，如果是则下一步,否则结束        
        ///3，更改orders表状态为待总检
        /// </summary>
        /// <param name="sqlLst"></param>
        /// <param name="strOrdernum"></param>
        /// <param name="isAuto">是否自动接收结果  True自动 false手动</param>
        private bool UpdateState(SortedList sqlLst, string strOrdernum, bool isAuto)
        {
            Hashtable htPara = new Hashtable();
            htPara.Add("OrderNum", strOrdernum);
            htPara.Add("Status", (int)ParamStatus.Orderlabdepstatus.Audited);

            ////审核全部科室小结
            Hashtable htUpdateState = new Hashtable();
            htUpdateState.Add("UPDATE", "Order.UpdateOrderlabdeptresultStateAuto");
            sqlLst.Add(htUpdateState, htPara);  

            if (this.ExecuteSqlTran(sqlLst))
            {                
                //判断所有科室是否有小结，并已审核，如果审核，则改orders状态为待总检
                bool iHaveLabResult = true;//最后结果为true时改orders状态为待总检

                //1,获取体检号对应的检查项目
                List<Ordertest> orderTestListPara = new OrderTestService().GetAllOrderLabdeptresultList(strOrdernum);
                //做为参数取全部科室,供审核时使用。
                var labdepidList = (from o in orderTestListPara
                                      select o.Dictlabdeptid).Distinct();
                foreach (double labdepid in labdepidList)
                {
                    Hashtable htLabdeptPara = new Hashtable();
                    htLabdeptPara.Add("ordernum", strOrdernum);
                    htLabdeptPara.Add("dictlabdeptid", labdepid);
                    iHaveLabResult = IsOrderlabdeptresultHaveAudio(htLabdeptPara);
                    if (!iHaveLabResult)
                        break;
                }
                if (iHaveLabResult)
                {
                    Hashtable htUpdateOrderState = new Hashtable();
                    htUpdateOrderState.Add("OrderNum", strOrdernum);
                    htUpdateOrderState.Add("Status", (int)ParamStatus.OrdersStatus.WaitCheck);

                    if (this.update("Order.EditStatus",htUpdateOrderState)>0)
                    {
                        //写日志                   
                        new BaseService().AddOperationLog(strOrdernum, "", "自动数据接收", "对[" + strOrdernum + "]自动小结并进入待总检", "修改留痕", "");
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>查询
        /// 查询
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList<Orderlabdeptresult> GetOrderlabdeptresultLst(Hashtable ht)
        {
            return this.QueryList<Orderlabdeptresult>("Order.SelectOrderlabdeptresultLst", ht);
        }

        /// <summary>总检查询，根据订单号查询科室小结
        /// 总检查询，根据订单号查询科室小结
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable DataForOrderlabdept(string ordernum)
        {
            return selectDS("Order.DataForOrderlabdept", ordernum).Tables[0];
        }

        /// <summary>根据订单号和科室查找是否有小结记录
        /// 根据订单号和科室查找是否有小结记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int SelectOrderlabdeptresultLstByOrderNum(Hashtable ht)
        {
            return Convert.ToInt32(this.selectDS("Order.SelectOrderlabdeptresultLstByOrderNum", ht).Tables[0].Rows[0][0]); 
        }

        /// <summary>更改审核者权限内科室的结果审核状态
        /// 更改审核者权限内科室的结果审核状态
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public bool UpdateOrderlabdeptresultState(Hashtable ht,out bool flag)
        {
            flag = false;
            //1，改权限内科室小结状态
            bool isOk = this.update("Order.UpdateOrderlabdeptresultState", ht) > 0;
            //2， 判断是否全部审核通过，如果通过，则标记orders表状态为待总检
            Hashtable hashtable = new Hashtable();
            hashtable.Add("OrderNum", ht["OrderNum"].ToString());
            DataTable dt  = new OrderTestService().GetOrdertestByOrdernumGroupBy(hashtable);
            int num = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hashtable htTo = new Hashtable();
                htTo.Add("ordernum", ht["OrderNum"].ToString());
                htTo.Add("dictlabdeptid", dt.Rows[i]["Dictlabdeptid"]);
                num +=  SelectOrderlabdeptresultLstByOrderNum(htTo);
            }
            if (num == dt.Rows.Count)
            {     
                if (isOk && SelectIsAudioByOrderNum(ht))
                {
                    Hashtable newht = new Hashtable();
                    newht.Add("OrderNum", ht["OrderNum"].ToString());
                    newht.Add("Status", (int)ParamStatus.OrdersStatus.WaitCheck);
                    new OrdersService().EditStatus(newht);
                    flag = true;
                }     
            }
            return isOk;
            

        }

        /// <summary>按ID更改科室小结状态
        /// 按ID更改科室小结状态
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public bool UpdateOrderlabdeptresultStateById(Hashtable ht)
        {
            return this.update("Order.UpdateOrderlabdeptresultStateById", ht) > 0;
        }
        /// <summary>更改科室小结内容
        /// 更改科室小结内容
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool UpdateOrderlabdeptresult(Orderlabdeptresult domain)
        {
            return this.update("Order.UpdateOrderlabdeptresult", domain)>0;
        }

        /// <summary>查询指定流水号所有项目是否审核完毕，如果审核完成则更改orders表状态为待总检
        /// 查询指定流水号所有项目是否审核完毕，如果审核完成则更改orders表状态为待总检
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool SelectIsAudioByOrderNum(Hashtable ht)
        {
            ht.Remove("UserId");
            ht["Status"]=((int)ParamStatus.Orderlabdepstatus.Summary).ToString();
            DataSet dsTemp = this.selectDS("Order.SelectIsAudioByOrderNum", ht);

            if (dsTemp != null)
            {
                return dsTemp.Tables[0].Rows.Count == 0;
            }
            else
                return true;
        }

        /// <summary>查询指定流水号所是否全是检验项目,配合自动接收
        /// 查询指定流水号所是否全是检验项目,配合自动接收
        /// </summary>
        /// <param name="strOrdernum"></param>
        /// <returns></returns>
        public bool IsAllJianYan(string strOrdernum)
        {
            //true  全是检验项目，false有其他类型项目
            DataTable dtTemp = this.selectDS("Order.IsAllJianYan", strOrdernum).Tables[0];
            if (dtTemp.Rows[0]["counts"].ToString()== "0")
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>查询指定流水号所有项目是有检验项目，如果没有则在数据接收时更改orders表状态为已接收
        /// 查询指定流水号所有项目是有检验项目，如果没有则在数据接收时更改orders表状态为已接收
        /// </summary>
        /// <param name="strOrdernum"></param>
        /// <returns>false没有  true 有</returns>
        public bool IsHaveJianYan(string strOrdernum)
        {
            //true  有检验项目，false有其他类型项目
            DataTable dtTemp = this.selectDS("Order.IsHaveJianYan", strOrdernum).Tables[0];
            if (dtTemp.Rows[0]["counts"].ToString() == "0")
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>查询指定流水号的科室是否审核通过，配合自动数据接收
        /// 查询指定流水号的科室是否审核通过，配合自动数据接收
        /// </summary>
        /// <param name="strOrdernum"></param>
        /// <returns>true,存在  false 不存在</returns>
        public bool IsOrderlabdeptresultHaveAudio(Hashtable htLabdeptPara)
        {
            //true  有检验项目，false有其他类型项目
            DataTable dtTemp = this.selectDS("Order.SelectOrderlabdeptresultHaveAudio", htLabdeptPara).Tables[0];
            if (dtTemp.Rows[0]["counts"].ToString() == "0")
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>社区网站数据上传，根据订单号查询科室小结内容
        /// 社区网站数据上传，根据订单号查询科室小结内容
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetOrderlabdeptresultByordernum(Hashtable ht)
        {
            return selectDS("Order.GetOrderlabdeptresultByordernum", ht).Tables[0];
        }

        #region >>>9.辅助方法
        /// <summary>
        /// 判断subset的无重复集合是不是bigset的子集
        /// </summary>
        /// <param name="bigSet"></param>
        /// <param name="subSet"></param>
        /// <returns></returns>
        private static bool isSubSet(string[] bigSet, string[] subSet)
        {

            for (int i = 0; i < subSet.Length; i++)
            {
                if (string.IsNullOrEmpty(subSet[i])) continue;
                if (!bigSet.Contains(subSet[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 将集合串成字符串
        /// </summary>
        /// <param name="lst">集合</param>
        /// <param name="ch">集合元素分隔符</param>
        /// <returns></returns>
        private static string arrayToString(ICollection lst, char ch)
        {
            string result = "";
            IEnumerator e = lst.GetEnumerator();
            int k = 0;
            while (e.MoveNext())
            {
                k++;
                if (ch == '\n')
                {
                    result += k.ToString() + ": " + e.Current.ToString() + ch;

                }
                else
                {
                    result += e.Current.ToString() + ch;
                }
            }
            return result.TrimEnd(new char[] { ch });
        }
        #endregion


        /// <summary>查询待自动小结记录
        /// 
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public DataTable GetFrmXiaoJieAudioRecord()
        {
            return selectDS("Order.FrmXiaoJie.select", null).Tables[0];
        }
    }
}
