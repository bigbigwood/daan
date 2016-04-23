using System;
using System.Collections.Generic;
using System.Linq;
using daan.service.common;
using System.Collections;
using daan.domain;
using System.Data;
using daan.service.dict;
using daan.service.login;
using System.Configuration;
using daan.service.order;
using daan.util.Web;
using System.Web;
using System.IO;

namespace daan.service.proceed
{
    public class ProDataReceiveService : BaseService
    {

        readonly OrdersService orderservice = new OrdersService();
        readonly CommonFuncLibService comm = new CommonFuncLibService();
        readonly OrderTestService orderTestService = new OrderTestService();
        readonly LoginService loginservice = new LoginService();
        //static DictmemberService memberservice = new DictmemberService();
        readonly OrderbarcodeService service = new OrderbarcodeService();
        readonly OrderlabdeptresultService labdeptresultService = new OrderlabdeptresultService();
        //static OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();

        /// <summary>自动接收数据
        /// 自动接收数据
        /// </summary>
        /// <param name="b"></param>
        /// <param name="userinfo"></param>
        /// <param name="labid"></param>
        /// <param name="ordernums"></param>
        /// <returns></returns>
        public string DownResult(bool b, UserInfo userinfo, double? labid, string ordernums)
        {
            string msg = string.Empty;
            try
            {

                string sysusername = "体检自动程序";
                string interfacemode = "1";

                string url = ConfigurationManager.AppSettings["URL"];   //调用webservice地址
                string username = ConfigurationManager.AppSettings["UserName"]; //登录用户名
                string password = ConfigurationManager.AppSettings["Password"]; //登录用户密码

                //获得选中分点的编码
                List<Dictlab> labList = loginservice.GetLoginDictlab();
                List<Dictlab> list = labList;
                //手动接收
                if (b)
                {
                    sysusername = userinfo.userName;
                    interfacemode = userinfo.sysSetting.Interfacemode;
                    list = (from a in labList where a.Dictlabid == labid select a).ToList<Dictlab>();
                }
                else
                {
                    list = (from a in labList where a.IsActive=='1' select a).ToList<Dictlab>();

                    IList<Initsyssetting> sysset = new InitsyssettingService().GetInitSysSetting();
                    if (sysset.Count > 0)
                    {
                        interfacemode = sysset[0].Interfacemode;
                    }
                }

                foreach (Dictlab labitem in list)
                {
                    if (labitem.IsActive=='1')
                    {                        
                        #region 各地旧方式
                        //选中订单数组
                        string[] ordernumList;
                        if (b)
                        {
                            ordernumList = ordernums.Split(',');
                        }
                        else
                        {
                            ordernumList = orderservice.SelectGetResultOrderNum(labitem.Dictlabid);
                        }
                        if (ordernumList.Count() == 0)
                        {
                            const string sstr = "没有接收的数据！";
                            if (!b) { msg += string.Format("分点[{0}]" + sstr, labitem.Labcode) + Environment.NewLine; continue; } else { return sstr; }
                        }
                        //设置调用webservice登录方法的参数
                        object[] par = new object[] { labitem.Labcode, username, password, sysusername };

                        //登录验证
                        object loginResult = WebUtils.InvokeWebservice(url, "WebService.Center", "CenterService", "Login", par);

                        //返回登录验证信息:1|SID,0|errorMsg
                        string[] loginMsg = loginResult.ToString().Split('|');

                        if (loginMsg.Length == 1)
                        {
                            if (!b) { msg += loginMsg[0] + Environment.NewLine; continue; } else { return loginMsg[0]; }
                        }
                        else if (loginMsg.Length > 1 && loginMsg[0] == "0") //登录失败     
                        {
                            if (!b) { msg += loginMsg[1] + Environment.NewLine; continue; } else { return loginMsg[1]; }
                        }
                        //获得登录成功后返回的SID
                        string sid = loginMsg[1];
                        for (int i = 0; i < ordernumList.Length; i++)
                        {
                            //判断是否有检验项目，如果没有，则标记为已接收，并继续下一循环
                            if (!labdeptresultService.IsHaveJianYan(ordernumList[i]))
                            {
                                Hashtable htorder = new Hashtable();
                                htorder["ordernum"] = ordernumList[i];
                                htorder["iolis"] = "2";//接收成功
                                orderservice.UpdateOrderIOLIS(htorder);
                                //接收结果后处理
                                UpdateTreatment(ordernumList[i], true, ref msg, b, 0);
                                continue;
                            }
                            //检查所有的物理检查项是否已录入结果，没有不做处理
                            if (orderTestService.SelectWLResultByOrdernum(ordernumList[i]))
                            {
                                continue;
                            }

                            //根据ordernum获得barcode
                            string barcodes = service.SelectOrderbarcodeString(ordernumList[i]);
                            object[] par1 = new object[] { sid, labitem.Labcode, barcodes };
                            //调用webservice 查询结果方法
                            string strxml = WebUtils.InvokeWebservice(url, "WebService.Center", "CenterService", "QueryResult", par1).ToString();

                            //将查询结果xml字符串转DataSet xml字符串格式为：errorMsg|xmlstr
                            string[] val = strxml.Split('|');
                            if (val[0] == "0")
                            {
                                msg += string.Format("分点[{0}]订单号[{1}]获取结果失败:{2}|{3}\r\n", labitem.Labcode, ordernumList[i], val[0], val[1]);
                                continue;
                            }
                            DataSet ds = comm.CXmlToDataSet(val[1]);
                            if (ds == null || ds.Tables.Count <= 0 || !ds.Tables.Contains("Testresults"))
                            {
                                //msg += string.Format("分点[{0}]订单号[{1}]没有获取到结果数据!\r\n", labitem.Labcode, ordernumList[i]);
                                continue;
                            }

                            DataTable dtdatarow = ds.Tables["data_row"];
                            DataTable dtTestresults = ds.Tables["Testresults"];
                            Hashtable ht = new Hashtable();
                            ht["data_row"] = dtdatarow;
                            ht["Testresults"] = dtTestresults;
                            ht["ordernum"] = ordernumList[i];
                            bool result = orderTestService.UpdateOrdertestByBarcode(ht);
                            UpdateTreatment(ordernumList[i], result, ref msg, b, labitem.Dictlabid);
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                //程序出错时iolis=3 接收失败
                Hashtable ht = new Hashtable();
                ht["ordernum"] = ordernums;
                ht["iolis"] = "3";
                orderservice.UpdateOrderIOLIS(ht);
                if (!b) { msg += ex.Message + Environment.NewLine; } else { return ex.Message; }
            }
            return msg;
        }

        /// <summary>广州接受结果后处理
        /// 广州接受结果后处理
        /// </summary>
        /// <param name="ordernumList"></param>
        /// <param name="i"></param>
        /// <param name="result">dictlab 分点对象，用于各地的参考范围不同时</param>
        private void UpdateTreatmentGZ(string ordernum, double? dictlabid)
        {
            //自动小结
            labdeptresultService.AutoSummary(ordernum, dictlabid);
           
            Hashtable htorder = new Hashtable();
            htorder["ordernum"] = ordernum;
            htorder["iolis"] = "2";
            orderservice.UpdateOrderIOLIS(htorder);
        }


        /// <summary>接受结果后处理
        /// 接受结果后处理
        /// </summary>
        /// <param name="ordernumList"></param>
        /// <param name="i"></param>
        /// <param name="result">dictlab 分点对象，用于各地的参考范围不同时</param>
        private void UpdateTreatment(string ordernum, bool result, ref string msg, bool b, double? dictlabid)
        {
            if (result)
            {
                new OrderbarcodeService().AddOperationLog(ordernum, "", "数据接收", "Lis数据接收", "修改留痕", "");
            }

            //当同一订单号ORDERTEST表的所有检验记录取到结果，更新ORDERS表IOLIS状态为1
            int count = orderTestService.SelectTestResultByOrdernum(ordernum);
            Hashtable htorder = new Hashtable();
            htorder["ordernum"] = ordernum;
            string str = ">>>接收完成";
            if (!result)
            { htorder["iolis"] = "3"; str = "***接收失败"; } //接收失败
            else
            {
                if (count == 0)
                {
                    htorder["iolis"] = "2";//接收成功
                    //自动小结
                    labdeptresultService.AutoSummary(ordernum, dictlabid);
                }
                else if (count >= 1)
                {
                    htorder["iolis"] = "1"; //部分接受
                    str = "---部分接收";
                }
            }
            if (!b)
            {
                msg += string.Format("订单号[{0}]:{1}\r\n", ordernum, str);
            }
            orderservice.UpdateOrderIOLIS(htorder);
        }

        /// <summary>日志记录
        /// 日志记录
        /// </summary>
        /// <param name="msg"></param>
        public void WirteLog(string msg)
        {
            var s = this;
            //string path = Request.ApplicationPath;
            //string path = Request.GetServerString("PATH_INFO");
            string path = HttpContext.Current.Request.MapPath("~/TimeLog");
            //若文件夹不存在则新建文件夹
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            string filepath = String.Format("{0}//{1:yyyy-MM-dd}.txt", path, DateTime.Now);

            StreamWriter sr = File.Exists(filepath) ? File.AppendText(filepath) : File.CreateText(filepath);
            sr.WriteLine(msg);
            sr.Close();
        }
    }
}
