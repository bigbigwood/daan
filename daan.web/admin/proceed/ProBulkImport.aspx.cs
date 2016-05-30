using System;
using System.Collections.Generic;
using System.Linq;
using ExtAspNet;
using System.Data;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using daan.service.login;
using daan.domain;
using daan.service.proceed;
using daan.service.dict;
using daan.util.Common;
using daan.service.common;
using System.Text.RegularExpressions;

namespace daan.web.admin.proceed
{
    public partial class ProBulkImport : PageBase
    {
        readonly LoginService loginservice = new LoginService();
        readonly ProRegisterService registerservice = new ProRegisterService();
        readonly OrderbarcodeService barcodeservice = new OrderbarcodeService();
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript(String.Format("(Ext.getCmp('{0}')).listWidth=350;", DropCustomer.ClientID));
            if (!IsPostBack)
            {
                BindDictLab();
                BindAddress();
            }
        }

        #region >>>> zhouy 绑定分点 选分点筛选单位
        /// <summary>
        /// 绑定分点
        /// </summary>
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        /// <summary>
        /// 绑定单位
        /// </summary>
        private void BindCustomer(double labid)
        {
            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();

            DropCustomer.DataSource = CustomerList.Where<Dictcustomer>(c => (c.Dictlabid == labid && c.Customertype == "0" && c.Active == "1") || c.IsPublic == "1");
            DropCustomer.DataValueField = "Dictcustomerid";
            DropCustomer.DataTextField = "Customername";
            DropCustomer.DataBind();
            DropCustomer.Items.Insert(0, new ListItem("请选择体检单位", "-1"));
        }

        /// <summary>选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
        }
        #endregion

        #region >>>省市区
        private void BindAddress()
        {
            DropProvinceBinder(dpProvince);
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        /// <summary>
        /// 选择省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpCity.Items.Clear();
            DropCityBinder(dpProvince, dpCity);

            dpCounty.Items.Clear();
            DropCountyBinder(dpCity, dpCounty);

            if (dpProvince.SelectedValue != "-1")
            {
                hidProvince.Text = dpProvince.SelectedText;
                hidCity.Text = string.Empty;
                hidCounty.Text = string.Empty;
            }
            if (ck1.Checked)
                setAddress();
        }
        /// <summary>
        /// 选择市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpCounty.Items.Clear();
            DropCountyBinder(dpCity, dpCounty);
            if (dpCity.SelectedValue != "-1")
            {
                hidCity.Text = dpCity.SelectedText;
                hidCounty.Text = string.Empty;
            }
            if (ck1.Checked)
                setAddress();
        }
        /// <summary>
        /// 选择县/区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpCounty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpCounty.SelectedValue != "-1")
                hidCounty.Text = dpCounty.SelectedText;
            if (ck1.Checked)
                setAddress();
        }
        #endregion

        /// <summary>
        /// 勾选是否统一使用报告邮寄信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ck1_CheckedChanged(object sender, EventArgs e)
        {
            if (ck1.Checked)
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = true;
                setAddress();
            }
            else
            {
                txtTelphone.Enabled = txtRecName.Enabled = txtAddress.Enabled = false;
                txtAddress.Text = txtRecName.Text = txtTelphone.Text = string.Empty;
            }
        }
        //关闭
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        //清空列表
        protected void btnClear_Click(object sender, EventArgs e)
        {
            GridUpload.Rows.Clear();
        }

        //导入
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (DropDictLab.SelectedValue == null)
            {
                MessageBoxShow("请先选择分点！"); return;
            }
            if (DropCustomer.SelectedIndex == 0)
            {
                MessageBoxShow("请先选择单位！"); return;
            }
            if (dpProvince.SelectedValue == "-1" || dpCity.SelectedValue == "-1")
            {
                MessageBoxShow("请选择省、市"); return;
            }
            string fileType = fileExcel.PostedFile.ContentType;
            if (fileType != "application/octet-stream" && fileType != "application/vnd.ms-excel" && fileType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && fileType != "application/kset")
            {
                MessageBoxShow("请上传指定格式的Excel文件"); return;
            }
            if (ck1.Checked)
            {
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()) || string.IsNullOrEmpty(txtRecName.Text.Trim()) || string.IsNullOrEmpty(txtTelphone.Text.Trim()))
                {
                    MessageBoxShow("邮寄地址、收件人、联系电话不能为空"); return;
                }
            }
            string fileName = fileExcel.ShortFileName;
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = String.Format("{0}_{1}", DateTime.Now.Ticks, fileName);

            string savePath = Server.MapPath("~/upload/ExcelFiles/" + fileName);

            if (fileExcel.HasFile)
            {
                if (!Directory.Exists(Server.MapPath("~/upload/ExcelFiles")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/upload/ExcelFiles"));
                }
                try { fileExcel.SaveAs(savePath); }
                catch (Exception ex) { MessageBoxShow("上传错误：" + ex.Message); return; }
            }
            DataTable dt;
            try
            {
                dt = RenderDataTableFromExcel(savePath);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBoxShow("上传的文件数据不能为空！"); return;
                }
                //验证上传的excel是否符合模板标准，否则不让上传订单并删除上传的文件
                string str=CheckDate(dt,fileName);
                if (!string.IsNullOrEmpty(str))
                {
                    MessageBoxShow(str);
                    File.Delete(savePath);//删除文件
                    return;
                }
            }
            catch (Exception ex) { MessageBoxShow("请上传指定格式的Excel文件!\r\n提示信息:" + ex.Message); return; }

            List<Dicttestitem> TestItemList = loginservice.GetLoginDicttestitemList();//项目字典表
            List<Dictproductdetail> ProductDetail = loginservice.GetLoginDictproductdetail();//套餐组合字典

            //查询分点+公用套餐
            List<Dicttestitem> productlistTemp = new DicttestitemService().GetProduct(TypeParse.StrToDouble(DropCustomer.SelectedValue, 0));

            string _productname = string.Empty;

            #region 读取excel数据并上传订单
            try
            {
                for (int i = (dt.Rows.Count - 1); i >= 0; i--)
                {
                    bool b = false;//添加是否成功
                    string errstr = "";
                    DataRow dr = dt.Rows[i];

                    string productTestCode = dr["套餐代码"].ToString().Replace('_', ' ').Trim();
                    string sex = dr["性别"].ToString().Replace('_', ' ').Trim() == "女" ? "F" : (dr["性别"].ToString().Replace('_', ' ').Trim() == "男" ? "M" : "U");

                    List<Dicttestitem> productList = productlistTemp.Where<Dicttestitem>(c => c.Testcode == productTestCode && (c.Forsex == sex || c.Forsex == "B")).ToList<Dicttestitem>();
                    List<Dicttestitem> grouptestList = new List<Dicttestitem>();
                    Dicttestitem productinfo = null;
                    if (productList.Count == 0)
                    {
                        errstr = String.Format("套餐代码[{0}]无匹配项，请查看性别是否匹配或者是否有该套餐。", productTestCode);
                        SetTableValue(false, errstr, dr);
                        continue;
                    }
                    else if (productList.Count > 1)
                    {
                        errstr = String.Format("存在多个套餐代码为[{0}]的套餐", productTestCode);
                        SetTableValue(false, errstr, dr);
                        continue;
                    }
                    else
                    {
                        productinfo = productList.First<Dicttestitem>();

                        List<OrderRegister> _gridtestList = null;
                        string msg = registerservice.AddProduct(ref _gridtestList, sex, productinfo.Dicttestitemid, false, Userinfo, ref _productname, null);
                        if (msg != string.Empty)
                        {
                            //错误
                            SetTableValue(false, msg, dr);
                            continue;
                        }

                        string barcode = dr["条码号"].ToString().Replace('_', ' ').Trim();

                        if (barcode != string.Empty && barcode.Length != 12)
                        {
                            errstr = string.Format("条码号[{0}]必须为12位数字，可以为空！", barcode);
                            SetTableValue(false, errstr, dr);
                            continue;
                        }

                        if (barcode != string.Empty && barcode.Substring(barcode.Length - 2) != "00")
                        {
                            errstr = string.Format("条码号[{0}]必须以00结尾！", barcode);
                            SetTableValue(false, errstr, dr);
                            continue;
                        }
                        if (barcodeservice.CheckBarCode(barcode))//条码号存在
                        {
                            errstr = string.Format("此条码号[{0}]已在本系统内生成，请更改条码号！", barcode);
                            SetTableValue(false, errstr, dr);
                            continue;
                        }

                        //套餐下组合项目
                        IEnumerable<Dictproductdetail> IEgroup = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid);

                        bool iscontinue = true;
                        foreach (Dictproductdetail item in IEgroup)
                        {
                            IEnumerable<Dicttestitem> IEgruptest = TestItemList.Where<Dicttestitem>(c => c.Dicttestitemid == item.Testgroupid);
                            if (IEgruptest.Count() <= 0)
                            {
                                errstr = string.Format("没有找到套餐[{0}]下ID为[{1}]的组合！", _productname, item.Testgroupid);
                                SetTableValue(false, errstr, dr);
                                continue;
                            }

                            Dicttestitem groupinfo = IEgruptest.First<Dicttestitem>();

                            //校验性别是否符合
                            string str = registerservice.checkSex(groupinfo.Dicttestitemid, sex);
                            if (str != string.Empty)
                            {
                                SetTableValue(false, str, dr);
                                iscontinue = false;
                                break;
                            }

                            groupinfo.Productid = productinfo.Dicttestitemid;
                            groupinfo.Productname = productinfo.Testname;///套餐名
                            groupinfo.IsActive = "1";//是否停止测试
                            groupinfo.Isadd = "0";///是否追加 
                            groupinfo.Billed = "0";
                            groupinfo.Sendbilled = "0";
                            groupinfo.Adduserid = null;//追加人ID


                            if (barcode == string.Empty)
                            {
                                IEnumerable<Dicttestitem> IEtempbarcodeList = grouptestList.Where<Dicttestitem>(c => c.Tubegroup == groupinfo.Tubegroup);
                                if (IEtempbarcodeList.Count() > 0)
                                {
                                    groupinfo.Barcode = IEtempbarcodeList.First<Dicttestitem>().Barcode;
                                }
                                else
                                {
                                    groupinfo.Barcode = registerservice.GetBarCode();
                                }
                            }
                            else
                            {
                                groupinfo.Barcode = barcode;
                            }


                            //获取外包客户
                            Dictproductdetail detail = ProductDetail.Where<Dictproductdetail>(c => c.Productid == productinfo.Dicttestitemid && c.Testgroupid == groupinfo.Dicttestitemid).First<Dictproductdetail>();
                            groupinfo.Sendoutcustomerid = detail.Sendoutcustomerid;

                            grouptestList.Add(groupinfo);

                        }

                        if (!iscontinue) { continue; }

                    }

                    #region >>>> zhouy 不存在此会员添加会员
                    string realname = dr["姓名"].ToString().Trim();
                    Dictmember member = new Dictmember() { Realname = realname};

                    string idnumber = dr["身份证"].ToString().Trim();

                    member.Idnumber = idnumber;
                    //检查会员
                    errstr = registerservice.checkmember(null, ref member);
                    if (errstr != string.Empty)
                    {
                        SetTableValue(false, errstr, dr);
                        continue;
                    }

                    //List<Dictmember> memberList = memberservice.GetDictmemberList(member);
                    //if (memberList.Count == 1)//存在此会员记录且只有一条
                    //{
                    //    member = memberList[0];
                    //    member.isAdd = false;//标识是否添加
                    //}
                    //else
                    //{
                    //    member.Dictmemberid = loginservice.getSeqID("SEQ_DICTMEMBER");
                    //    member.Islock = "F";//是否锁定
                    //    member.isAdd = true;
                    //}
                    member.Nickname = member.Realname;
                    member.Sex = sex;
                    DateTime datebirthday;
                    bool dateb = DateTime.TryParse(dr["出生日期"].ToString(), out datebirthday);
                    if (dateb) { member.Birthday = datebirthday; }
                    member.Addres = dr["住址"].ToString().Trim();
                    member.Phone = dr["电话"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(dr["手机"].ToString().Trim()))
                    {
                        double a;
                        bool mobileb = double.TryParse(dr["手机"].ToString().Trim(), out a);
                        if (!mobileb)
                        {
                            SetTableValue(false, "手机号码填写不正确,不要有特殊字符，[-]也不能包含", dr);
                            continue;
                        }
                    }
                    member.Mobile = dr["手机"].ToString().Trim();
                    member.Email = dr["邮箱"].ToString().Trim();
                    #endregion


                    double year = 0, month = 0, day = 0;
                    double hours = 0;//小时
                    double age = 0;
                    string agestr = dr["年龄"].ToString().Replace('_', ' ').Trim();
                    bool ageb = double.TryParse(agestr, out age);

                    if (member.Birthday == null)
                    {
                        if (agestr != string.Empty && ageb)
                        {
                            year = age;
                            day = age * 365;
                            member.Birthday = datebirthday = DateTime.Now.AddDays((0 - day));
                        }
                        else
                        {
                            SetTableValue(false, "生日和年龄必须填写一项或者两项均填写错误", dr);
                            continue;
                        }
                    }
                    TimeSpan ts = DateTime.Now - Convert.ToDateTime(member.Birthday);//时间差
                    year = Math.Truncate((double)(ts.Days / 365));
                    month = (ts.Days % 365) / 30;
                    day = (ts.Days % 365) % 30;
                    hours = ts.TotalHours;


                    Orders _orders = new Orders();
                    _orders.Ordernum = new ProRegisterService().GetOrderNum(); ;//体检流水号
                    _orders.Remarks = dr["备注"].ToString().Trim();//备注
                    _orders.Dictmemberid = member.Dictmemberid;  //会员ID
                    _orders.Dictcustomerid = Convert.ToDouble(DropCustomer.SelectedValue);//所属客户ID 界面选择
                    _orders.Realname = member.Realname;
                    _orders.Sex = member.Sex; //性别 对应INITBASIC表
                    _orders.Caculatedage = hours;//计算后的年龄（小时为单位）
                    _orders.Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, 0);//年龄字符串拼接 岁月日时
                    _orders.Enterby = Userinfo.userName;//录入人
                    _orders.Ordertestlst = _productname + ",";//项目清单（冗余字段）
                    _orders.Dictlabid = Convert.ToDouble(DropDictLab.SelectedValue);//实验室分点
                    _orders.Ordersource = "1";//单位上传 全是单位来源
                    _orders.Ismarried = dr["婚否"].ToString() == "未婚" ? "0" : (dr["婚否"].ToString() == "已婚" ? "1" : "2");
                    _orders.Section = dr["部门"].ToString().Trim();
                    _orders.Status = ((int)ParamStatus.OrdersStatus.BarCodePrint).ToString();
                    DateTime samplingdate;
                    bool s = DateTime.TryParse(dr["采样日期"].ToString(), out samplingdate);
                    if (s) { _orders.SamplingDate = samplingdate; }

                    _orders.Province = dpProvince.SelectedValue == "-1" ? "" : dpProvince.SelectedText;
                    _orders.City = dpCity.SelectedValue == "-1" ? "" : dpCity.SelectedText;
                    _orders.County = dpCounty.SelectedValue == "-1" ? "" : dpCounty.SelectedText;

                    if (ck1.Checked)
                    {
                        _orders.PostAddress = Regex.Replace(txtAddress.Text, @"\s", ""); 
                        _orders.ContactNumber = Regex.Replace(txtTelphone.Text, @"\s", ""); 
                        _orders.Recipient = Regex.Replace(txtRecName.Text, @"\s", ""); 
                    }
                    else
                    {
                        _orders.PostAddress = dr["住址"].ToString().Trim();
                        _orders.Recipient = realname;
                        _orders.ContactNumber = dr["手机"].ToString().Trim();
                    }
                    //add 20160421 增加营业区、场次号
                    _orders.Area = dr["营业区"].ToString().Replace('_', ' ').Trim();
                    _orders.BatchNumber = dr["场次号"].ToString().Replace('_', ' ').Trim();

                    //add 20160530 增加客户经理字段
                    if (dt.Columns.Contains("客户经理"))
                    {
                        _orders.AccountManager = dr["客户经理"].ToString().Replace('_', ' ').Trim();
                    }
                    b = registerservice.insertUpdateOrders("单位批量上传", "", true, productList, grouptestList, member, _orders, "", ref errstr);

                    SetTableValue(b, errstr, dr);
                }
                GridUpload.DataSource = dt;
                GridUpload.DataBind();
            }
            catch (Exception ee)
            {
                string str = string.Format("导入失败，异常信息：{0}【Excel格式请严格参照导入模版说明】\n\n导入过程中如有疑问，请将异常信息截图后发与系统管理员。", ee.Message);
                MessageBoxShow(str);
            }
            #endregion

        }

        private static void SetTableValue(bool b, string errstr, DataRow dr)
        {
            if (b)
            {
                dr["上传状态"] = "导入成功";//添加成功移除            
            }
            else
            {
                dr["上传状态"] = "导入失败";//添加失败    
                dr["失败原因"] = errstr + " 【Excel格式请严格参照导入模版说明】";
            }
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="path">服务器excel文档路径</param>
        /// <returns></returns>
        public DataTable RenderDataTableFromExcel(string path)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return null;
            }
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);


                ISheet sheet = hssfworkbook.GetSheetAt(0);
                //System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    if (headerRow.GetCell(j) == null)
                    {
                        continue;
                    }
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());
                }
                dt.Columns.Add("上传状态", typeof(string));
                dt.Columns.Add("失败原因", typeof(string));

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    //套餐代码为空
                    if (row.GetCell(4) == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(row.GetCell(4).ToString()))
                        {
                            continue;
                        }
                    }
                    //姓名为空
                    if (row.GetCell(6) == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(row.GetCell(6).ToString()))
                        {
                            continue;
                        }
                    }
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < (cellCount + 2); j++)
                    {
                        if (j == cellCount)
                        {
                            dataRow[j] = "未上传";
                        }
                        else
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }

        }

        #region 检测导入EXCEL文件列头是否标准
        string[] arr = CommonConst.ImportExcelCols;
        private string CheckDate(DataTable dt, string fileName)
        {
            string res = string.Empty;
            foreach (string str in arr)
            {
                if (!dt.Columns.Contains(str))
                {
                    res = SetErrorMessage(str, fileName);
                    break;
                }
            }
            return res;
        }
        private string SetErrorMessage(string colName, string fileName)
        {
            string errstring = string.Format("文件名:{0} 缺少列[{1}]。【Excel格式请严格参照导入模版说明】\n\n导入过程中如有疑问，请将异常信息截图后发与系统管理员。", fileName.Substring(fileName.LastIndexOf('_') + 1), colName);
            return errstring;
        }
        private void setAddress()
        {
            txtAddress.Text = string.Format("{0} {1} {2}", hidProvince.Text, hidCity.Text, hidCounty.Text);
        }
        #endregion
    }
}
