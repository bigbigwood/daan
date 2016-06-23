using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;

using ExtAspNet;
using daan.util.Common;
using daan.domain;
using daan.service.proceed;
using daan.service.login;
using hlis.service.common;
using daan.service.common;
using daan.service;
using daan.service.dict;
using System.Text.RegularExpressions;

namespace daan.web.admin.proceed
{
    public partial class ProOrderModify : PageBase
    {
        static ProRegisterService registerserver = new ProRegisterService();
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();
        static DictCustomerService dictCustomerService = new DictCustomerService();
        static DicttestitemService dicttestservice = new DicttestitemService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["OrderNum"] != null && Request.QueryString["OrderNum"].ToString() != string.Empty)
                {
                    string OrderNum = tbxOrderNum.Text = Request.QueryString["OrderNum"].ToString();
                    List<OrderRegister> OrderRegisterList = registerserver.SelectOrdersDetail(OrderNum);

                    BindDictLab();///绑定分点
                    BindArea();
                    initBindDate();
                    //修改绑定头数据
                    OrderBindData(OrderNum);
                    //绑定列表数据
                    BindGridTest(OrderRegisterList);
                }
            }
        }
        #region 省市区绑定及下拉选择事件
        private void BindArea()
        {
            DropProvinceBinder(dpProvince);
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        //选择省份
        protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropCityBinder(dpProvince, dpCity);
            DropCountyBinder(dpCity, dpCounty);
        }
        //选择市
        protected void dpCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropCountyBinder(dpCity, dpCounty);
        }

        #endregion

        /// <summary>页面加载数据初始绑定 ，加载
        /// 页面加载数据初始绑定 ，加载
        /// </summary>
        private void initBindDate()
        {
            if (DropDictLab.SelectedValue != null)
            {
                BindDictTest(DropDictLab.SelectedValue);///组合项目
            }
            BindDictProduct(DropCustomer.SelectedValue);
            DropSex.Items.RemoveAt(0);//删除第一项(请选择)            
        }


        /// <summary>绑定Grid列表
        /// 绑定Grid列表
        /// </summary>
        /// <param name="OrderRegisterList">数据源LIST集合</param>
        private void BindGridTest(List<OrderRegister> OrderRegisterList)
        {
            ViewState["GridTest"] = OrderRegisterList;
            GridTest.Rows.Clear();
            GridTest.DataSource = OrderRegisterList;
            try
            {
                GridTest.DataBind();
            }
            catch (Exception) { }
        }
        ///文本改变
        protected void dateBirthday_TextChanged(object sender, EventArgs e)
        {
            GetAge();
        }

        ///年龄反算日期
        protected void tbxAge_TextChanged(object sender, EventArgs e)
        {
            double year = 0, month = 0, day = 0;
            try
            {
                year = tbxAge.Text == string.Empty ? 0 : Convert.ToDouble(tbxAge.Text);
                month = tbxMonth.Text == string.Empty ? 0 : Convert.ToDouble(tbxMonth.Text);
                day = tbxDay.Text == string.Empty ? 0 : Convert.ToDouble(tbxDay.Text);
            }
            catch (Exception)
            {
                MessageBoxShow("输入的[岁-月-日]只能为数字!"); return;
            }

            double days = 0 - (day + month * 30 + year * 365);
            dateBirthday.SelectedDate = DateTime.Now.AddDays(days);

        }

        #region >>>> zhouy 保存
        ///保存
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            GridRowCollection GridRow = GridTest.Rows;

            double? memberid = null;
            if (tbxmemberID.Text != string.Empty) { memberid = Convert.ToDouble(tbxmemberID.Text); }

            //获取系统时间时间
            DateTime? date = loginservice.GetServerTime();
            DateTime? datebirthday = null;
            int year = 0, month = 0, day = 0, hour = 0;//年月日时
            double customerid;
            //验证
            string msg = SaveCheck(date, out datebirthday, out year, out month, out day, out hour, out customerid);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }

            List<Dicttestitem> grouptestList = new List<Dicttestitem>(); //订单中组合集合            
            List<Dicttestitem> productList = new List<Dicttestitem>();//订单中套餐集合
            ///实验室分点
            double labid = Convert.ToDouble(DropDictLab.SelectedValue);
            List<Dicttestgroupdetail> TestGroupDetailList = loginservice.GetLoginDicttestgroupdetail();//组合项目字典
            //获取集合
            //List<OrderRegister> l = ViewState["GridTest"] as List<OrderRegister>;
            List<OrderRegister> _gridtestList = GetGridTest(true);
            //MessageBoxShow(l.Count.ToString() + "\t" + GridRow.Count.ToString());

            //return;

            //已接收的条码号
            string ReceivedBarcode = string.Empty;

            #region >>>> zhouy 获取订单中 组合,套餐

            for (int i = 0; i < _gridtestList.Count; i++)
            {
                OrderRegister item = _gridtestList[i];
                if (item.IsProduct)///套餐 去套餐ID和套餐名
                {
                    ///---------------------添加套餐-------------------------------------
                    ///不包含此套餐才添加
                    if (productList.Where(c => c.Dicttestitemid == item.Productid).Count() <= 0)
                    {
                        Dicttestitem _product = registerserver.SelectsTestItemListById(item.Productid);
                        productList.Add(_product);
                    }
                    ///----------------------end添加套餐-------------------------------------
                }
                else
                {
                    Dicttestitem _product = registerserver.SelectsTestItemListById(item.Id);
                    productList.Add(_product);
                }

                if ((Convert.ToInt32(item.Status)) >= ((int)daan.service.common.ParamStatus.OrderbarcodeStatus.Received))
                {
                    ReceivedBarcode += item.Barcode + ',';
                    //continue;
                }

                string str = registerserver.checkSex(item.Id, DropSex.SelectedValue);
                if (str != string.Empty) { MessageBoxShow(str); return; }

                Dicttestitem _groupitem;
                ///添加组合|项目
                _groupitem = new Dicttestitem();
                _groupitem = registerserver.SelectsTestItemListById(item.Id);
                _groupitem.IsActive = item.Isactive;//是否停止测试
                _groupitem.Isadd = item.Isadd;///是否追加 
                _groupitem.Billed = item.Billed;
                _groupitem.Sendbilled = item.Sendbilled;
                _groupitem.Adduserid = item.Adduserid;///追加人ID
                _groupitem.Sendoutcustomerid = item.Sendoutcustomerid;
                _groupitem.Tubegroup = item.Tubegroup;
                _groupitem.Barcode = item.Barcode;
                _groupitem.Productid = item.Productid;
                _groupitem.Productname = item.Productname;///套餐名

                grouptestList.Add(_groupitem);
            }

            #endregion

            #region >>>> zhouy 会员信息

            Dictmember _member = new Dictmember();

            _member.Realname = tbxName.Text.Trim();
            _member.Idnumber = tbxIDNumber.Text == string.Empty ? "" : tbxIDNumber.Text;
            //检查会员
            string errstr = registerserver.checkmember(memberid, ref _member);
            if (errstr != string.Empty) { MessageBoxShow(errstr); return; }

            _member.Nickname = tbxName.Text;
            _member.Sex = DropSex.SelectedValue;
            _member.Birthday = datebirthday;
            _member.Addres = tbxAddres.Text;
            _member.Phone = tbxPhone.Text;
            _member.Mobile = tbxMobile.Text;
            _member.Email = tbxEMail.Text;
            _member.Islock = "F";///是否锁定

            #endregion


            #region >>>> zhouy insert Orders

            Orders order = registerserver.SelectOrderInfo(tbxOrderNum.Text);

            Orders _orders = order.Copy<Orders>();
            _orders.Ordernum = tbxOrderNum.Text;///体检流水号
            _orders.Remarks = tbxRemark.Text;///备注
            _orders.Dictmemberid = _member.Dictmemberid;  ///会员ID
            _orders.Dictcustomerid = customerid;///所属客户ID
            _orders.Realname = _member.Nickname;
            _orders.Sex = _member.Sex; ///性别 对应INITBASIC表
            _orders.Caculatedage = AgeToHour(year, month, day, hour);///计算后的年龄（小时为单位）
            _orders.Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, hour); ;///年龄字符串拼接 年月日时
            _orders.Ordertestlst = tbxItemTest.Text;///项目清单（冗余字段）
            _orders.Dictlabid = labid;///实验室分点
            _orders.Ordersource = "1";
            _orders.Ismarried = radlIsMarried.SelectedValue;///婚否
            _orders.Section = tbxSection.Text;///部门
            _orders.Lastupdatedate = date;//最后更新时间
            //省市区
            _orders.Province = dpProvince.SelectedValue=="-1"?"":dpProvince.SelectedText;
            _orders.City = dpCity.SelectedValue == "-1" ? "" : dpCity.SelectedText;
            _orders.County = dpCounty.SelectedValue == "-1" ? "" : dpCounty.SelectedText;
            //邮寄信息
            _orders.ContactNumber = txtCONTACTNUMBER.Text.Trim();
            _orders.Recipient = txtRECIPIENT.Text.Trim();
            _orders.PostAddress = txtPostAddress.Text.Trim();
            //营业区
            _orders.Area = txtArea.Text.Trim();
            _orders.AccountManager = tbxAccountmanager.Text.Trim();
            //采样日期
            DateTime? spdate = null;
            if(dtSampleDate.Text!="")
                spdate = Convert.ToDateTime(dtSampleDate.Text);
            _orders.SamplingDate = spdate;
            #endregion

            string Content = string.Empty;
            ContrastObject(ref Content, _orders, order, _member);

            //是否有条码被物流接收
            bool isreceived = ReceivedBarcode.TrimEnd(',') != string.Empty;
            string error = "";
            bool b = registerserver.insertUpdateOrders("订单修改", Content, false, productList, grouptestList, _member, _orders, ReceivedBarcode.TrimEnd(','), ref error);
            if (b)
            {
                if (isreceived && (Content.Contains("年龄") || Content.Contains("性别")))//物流接受 则需要提示通知
                {
                    MessageBoxShow("该订单标本已被实验室接收，修改了年龄或者性别，请通知实验室");
                }
                PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
            }
            else { MessageBoxShow("保存失败:" + error); }
        }

        /// <summary>
        /// 对比是否有修改
        /// </summary>
        /// <param name="content"></param>
        /// <param name="_order"></param>
        /// <param name="order"></param>
        /// <param name="_member"></param>
        private static void ContrastObject(ref string content, Orders order, Orders _order, Dictmember member)
        {
            Dictmember _member = memberservice.GetMemberById(_order.Dictmemberid);

            if (_member.Realname != member.Realname) { content += string.Format("姓名:[{0}]更改为[{1}],", _member.Realname, member.Realname); }
            if (_member.Birthday != member.Birthday) { content += string.Format("出生日期:[{0}]更改为[{1}],", _member.Birthday, member.Birthday); }
            if (_member.SexName != member.SexName) { content += string.Format("性别:[{0}]更改为[{1}],", _member.SexName, member.SexName); }
            if (_member.Phone != member.Phone) { content += string.Format("电话:[{0}]更改为[{1}],", _member.Phone, member.Phone); }
            if (_member.Mobile != member.Mobile) { content += string.Format("手机:[{0}]更改为[{1}],", _member.Mobile, member.Mobile); }
            if (_member.Addres != member.Addres) { content += string.Format("地址:[{0}]更改为[{1}],", _member.Addres, member.Addres); }
            if (_member.Email != member.Email) { content += string.Format("Email:[{0}]更改为[{1}],", _member.Email, member.Email); }
            if (_member.Idnumber != member.Idnumber) { content += string.Format("身份证号:[{0}]更改为[{1}],", _member.Idnumber, member.Idnumber); }

            if (_order.Province != order.Province) { content += string.Format("省份：[{0}]更改为[{1}]", _order.Province, order.Province); }
            if (_order.City != order.City) { content += string.Format("城市：[{0}]更改为[{1}]", _order.City, order.City); }
            if (_order.County != order.County) { content += string.Format("地区：[{0}]更改为[{1}]", _order.County, order.County); }
            if (_order.Area != order.Area) { content += string.Format("营业区：[{0}]更改为[{1}]", _order.Area, order.Area); }
            if (_order.AccountManager != order.AccountManager) { content += string.Format("客户经理：[{0}]更改为[{1}]", _order.AccountManager, order.AccountManager); }

            if (_order.Age != order.Age) { content += string.Format("年龄:[{0}]更改为[{1}],", _order.Age, order.Age); }
            if (_order.Remarks != order.Remarks) { content += string.Format("备注:[{0}]更改为[{1}],", _order.Remarks, order.Remarks); }
            if (_order.Dictcustomerid != order.Dictcustomerid) { content += string.Format("客户ID:[{0}]更改为[{1}],", _order.Dictcustomerid, order.Dictcustomerid); }
            if (_order.Section != order.Section) { content += string.Format("部门:[{0}]更改为[{1}],", _order.Section, order.Section); }
            if (_order.Dictlabid != order.Dictlabid) { content += string.Format("分点ID:[{0}]更改为[{1}],", _order.Dictlabid, order.Dictlabid); }
            if (_order.Ismarried != order.Ismarried) { content += string.Format("婚否:[{0}]更改为[{1}],", _order.Ismarried, order.Ismarried); }
            if (_order.Ordertestlst != order.Ordertestlst) { content += string.Format("项目清单:[{0}]更改为[{1}],", _order.Ordertestlst, order.Ordertestlst); }

            if (_order.PostAddress != order.PostAddress) { content += string.Format("邮寄地址：[{0}]更改为[{1}]",_order.PostAddress,order.PostAddress); }
            if (_order.Recipient != order.Recipient) { content += string.Format("收件人：[{0}]更改为[{1}]", _order.Recipient, order.Recipient); }
            if (_order.ContactNumber != order.ContactNumber) { content += string.Format("联系电话：[{0}]更改为[{1}]", _order.ContactNumber, order.ContactNumber); }
        }

        /// <summary>保存前验证
        /// 保存前验证
        /// </summary>
        /// <param name="date">系统时间</param>
        /// <param name="datebirthday">生日</param>
        /// <param name="year">岁</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <returns></returns>
        private string SaveCheck(DateTime? date, out DateTime? datebirthday, out int year, out int month, out int day, out int hour, out double customerid)
        {
            datebirthday = null;
            year = month = day = hour = 0;            
            string strmsg = "";

            //单位
            customerid = Convert.ToDouble(DropCustomer.SelectedValue);
           

            if (DropDictLab.SelectedValue == null) { strmsg = "请选择实验室分点！"; return strmsg; }
            if (tbxName.Text == string.Empty) { strmsg = "姓名不能为空！"; return strmsg; }
           

            tbxMobile.Text = tbxMobile.Text.Trim();
            tbxPhone.Text = tbxPhone.Text.Trim();
            //if (tbxPhone.Text == string.Empty && tbxMobile.Text == string.Empty) { strmsg = "电话号码与手机号码不能同时为空！"; return strmsg; }
            //if (tbxPhone.Text != string.Empty && (tbxPhone.Text.Length < 8 || tbxPhone.Text.Length > 13)) { strmsg = "请填写正确的电话号码！"; return strmsg; }

            if (tbxEMail.Text.Trim() != string.Empty)
            {
                Regex reg = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
                if (!reg.IsMatch(tbxEMail.Text))
                {
                    strmsg = "请检查您填写的Email地址！"; return strmsg;
                }
            }
            if (GridTest.Rows.Count <= 0) { strmsg = "没有需要保存的订单！"; return strmsg; }
            

            if (tbxAge.Text == string.Empty) { tbxAge.Text = "0"; }
            if (tbxMonth.Text == string.Empty) { tbxMonth.Text = "0"; }
            if (tbxDay.Text == string.Empty) { tbxDay.Text = "0"; }
            if (tbxHour.Text == string.Empty) { tbxHour.Text = "0"; }
            try
            {
                year = Convert.ToInt32(tbxAge.Text);
                month = Convert.ToInt32(tbxMonth.Text);
                day = Convert.ToInt32(tbxDay.Text);
                hour = Convert.ToInt32(tbxHour.Text);
            }
            catch (Exception) { strmsg = "[岁月日时]格式只能为数字！"; return strmsg; }

            try
            {
                datebirthday = Convert.ToDateTime(dateBirthday.Text);
                TimeSpan ts = (TimeSpan)(date - datebirthday);
                if (ts.Days < 0) { strmsg = "输入生日不能在今天日期之后!"; return strmsg; }
            }
            catch (Exception)
            {
                strmsg = "输入生日格式不对！标准格式:2012-1-1或者2012-01-01!"; return strmsg;
            }
            return strmsg;
        }

        #endregion

        #region >>>> zhouy  行绑定，删除行，停做项目事件
        ///删除，停做组合|项目
        protected void GridTest_RowCommand(object sender, ExtAspNet.GridCommandEventArgs e)
        {
            List<OrderRegister> _gridtestList = GetGridTest(true);

            GridRow row = GridTest.Rows[e.RowIndex];
            OrderRegister _register = _gridtestList.Where<OrderRegister>(c => c.Id == Convert.ToDouble(row.Values[0])).First<OrderRegister>();

            if (e.CommandName == "Delete")
            {
                if (!_register.Isdelete) { MessageBoxShow("此项目账单已出，不允许删除，请停止测试"); return; }

                var deletename = "[" + _register.Code + "]" + _register.Name;
                var strname = "";
                if (_register.IsProduct)
                {
                    deletename = _register.Productname;
                    for (var i = 0; i < _gridtestList.Count; i++)
                    {
                        if (i == e.RowIndex) { continue; }

                        OrderRegister temp = _gridtestList[i];
                        if (temp.IsProduct && temp.Productname == deletename)
                        {
                            strname += "[" + temp.Code + "]" + temp.Name + ",";
                            temp.Productid = null;
                            temp.Productname = string.Empty;
                        }
                    }
                }
                deletename += ",";
                tbxItemTest.Text = tbxItemTest.Text.Replace(deletename, strname).Replace(",,", ",");
                _gridtestList.Remove(_register);
            }
            else if (e.CommandName == "Stop")
            {
                if (_register.Isactive == "1")
                {
                    row.Values[4] = "停止测试";
                    _register.Isactive = "0";
                }
                else
                {
                    GridTest.Rows[e.RowIndex].Values[4] = "正常";
                    _register.Isactive = "1";
                }
            }
            BindGridTest(_gridtestList);

        }

        //行绑定  
        protected void GridTest_RowDataBound(object sender, GridRowEventArgs e)
        {
            OrderRegister _register = e.DataItem as OrderRegister;
            System.Web.UI.WebControls.DropDownList ddlcustomer = (System.Web.UI.WebControls.DropDownList)GridTest.Rows[e.RowIndex].FindControl("DropSendCustomer");
            //绑定数据
            BindDropSendCustomer(ddlcustomer);
            ddlcustomer.SelectedValue = _register.Sendoutcustomerid.ToString();
            e.Values[3] = _register.IsGroup ? "组合" : "项目";
            e.Values[4] = _register.Isactive == "1" ? "正常" : "已停测试";
            if (_register.Sendbilled == "1") { ddlcustomer.Enabled = false; }
        }

        //绑定外包医院下拉框
        private void BindDropSendCustomer(System.Web.UI.WebControls.DropDownList ddlcustomer)
        {
            if (ViewState["SendCustomer"] == null) { ViewState["SendCustomer"] = dictCustomerService.GetDictCustomerListByType("1").Where<Dictcustomer>(c => c.Dictlabid == Convert.ToDouble(DropDictLab.SelectedValue)).ToList<Dictcustomer>(); }
            ddlcustomer.DataTextField = "Customername";
            ddlcustomer.DataValueField = "Dictcustomerid";
            ddlcustomer.DataSource = ViewState["SendCustomer"] as IList<Dictcustomer>;
            ddlcustomer.DataBind();
            ddlcustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("请选择", "-1"));
        }


        /// <summary>设置集合中的外包医院并返回集合
        /// 设置集合中的外包医院并返回集合
        /// </summary>
        /// <param name="isSendCustomer">是否加算外包医院</param>
        /// <returns></returns>
        private List<OrderRegister> GetGridTest(bool isSendCustomer)
        {
            List<OrderRegister> _gridtestList = ViewState["GridTest"] as List<OrderRegister>;
            if (isSendCustomer)
            {
                for (int i = 0; i < GridTest.Rows.Count; i++)
                {
                    System.Web.UI.WebControls.DropDownList ddloutcustomer = (System.Web.UI.WebControls.DropDownList)GridTest.Rows[i].FindControl("DropSendCustomer");
                    int sendcustomerid = Convert.ToInt32(ddloutcustomer.SelectedValue);
                    if (sendcustomerid != -1)
                    {
                        _gridtestList[i].Sendoutcustomerid = sendcustomerid;
                    }
                    else
                    {
                        _gridtestList[i].Sendoutcustomerid = null;
                    }
                }
            }
            //还未添加项目则new一个
            if (_gridtestList == null) { _gridtestList = new List<OrderRegister>(); }

            return _gridtestList;
        }
        #endregion

        #region >>>> zhouy 组合项目，套餐绑定以及选择添加
        ///绑定组合项目
        private void BindDictTest(object labid)
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetGroupTestByLabId(labid);
            BindDropdownList(DropTest,TestItemList);
        }

        // 绑定套餐
        private void BindDictProduct(object customerid)
        {            
            List<Dicttestitem> TestItemList = dicttestservice.GetProduct(customerid);
            BindDropdownList(DropItem,TestItemList);
        }
        //下拉列表绑定复合列
        private void BindDropdownList(ExtAspNet.DropDownList dp, List<Dicttestitem> list)
        {
            if (list == null || list.Count == 0)
                return;
            List<CustomClass> myList = new List<CustomClass>();
            foreach (Dicttestitem item in list)
            {
                myList.Add(new CustomClass(item.Dicttestitemid, "[" + item.Testcode + "]" + item.Testname));
            }
            dp.DataSource = myList;
            dp.DataTextField = "Name";
            dp.DataValueField = "ID";
            dp.DataBind();
            dp.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }
        ///选择组合项目添加
        protected void DropTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropTest.SelectedIndex == 0) { return; }
            ExtAspNet.ListItem li = DropTest.SelectedItem;

            //  Dicttestitem itemtest = registerserver.SelectsTestItemListById(Convert.ToDouble(li.Value));

            List<OrderRegister> _gridtestList = GetGridTest(false);
            string msg = registerserver.AddGroupTest(ref _gridtestList, DropSex.SelectedValue, null, Convert.ToDouble(li.Value), false, Userinfo, null,null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            ////校验性别是否符合
            //string str = registerserver.checkSex(itemtest.Dicttestitemid, DropSex.SelectedValue);
            //if (str != string.Empty) { MessageBoxShow(str); return; }

            //bool isNewBarcode = true;
            //string barccode = "";
            //foreach (OrderRegister item in _gridtestList)
            //{
            //    string msg = registerserver.checkInsert(item, itemtest, string.Empty);
            //    //此处判断组合存在，或者项目报告ID不同就不需要允许添加
            //    if (msg != string.Empty) { MessageBoxShow(msg); return; }

            //    //分管原则相同时 没有标本接受时使用以前条码
            //    if ((Convert.ToInt32(item.Status)) < ((int)daan.service.common.ParamStatus.OrderbarcodeStatus.Received) && item.Tubegroup == itemtest.Tubegroup)
            //    {
            //        isNewBarcode = false;
            //        barccode = item.Barcode;
            //    }
            //}
            ////判断使用原条码还是新条码
            //if (isNewBarcode) { barccode = registerserver.GetBarCode(); }

            //OrderRegister newOrderRegister = new OrderRegister();
            //newOrderRegister.Productid = null;
            //newOrderRegister.Productname = null;
            //newOrderRegister.Id = itemtest.Dicttestitemid;
            //newOrderRegister.Code = itemtest.Testcode;
            //newOrderRegister.Name = itemtest.Testname;
            //newOrderRegister.Type = itemtest.Testtype;
            //newOrderRegister.Isadd = "0";
            //newOrderRegister.Isactive = "1";
            //newOrderRegister.Billed = "0";
            //newOrderRegister.Sendbilled = "0";
            //newOrderRegister.Sendoutcustomerid = null;
            //newOrderRegister.Tubegroup = itemtest.Tubegroup;
            //newOrderRegister.Barcode = barccode;

            // _gridtestList.Add(newOrderRegister);
            BindGridTest(_gridtestList);//绑定数据

            DropTest.SelectedItem.Value = "-1";
            tbxItemTest.Text += li.Text + ",";
        }

        ///选择套餐添加
        protected void DropItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropItem.SelectedIndex == 0) { return; }
            ExtAspNet.ListItem li = DropItem.SelectedItem;

            string productname = "";
            List<OrderRegister> _gridtestList = GetGridTest(false);
            string msg = registerserver.AddProduct(ref _gridtestList, DropSex.SelectedValue, Convert.ToDouble(li.Value), false, Userinfo, ref productname,null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            tbxItemTest.Text += productname + ",";
            BindGridTest(_gridtestList);//绑定数据

            DropItem.SelectedItem.Value = "-1";
        }
        #endregion

        #region >>>> zhouy 绑定分点，以及选分点筛选单位
        /// 绑定分点
        private void BindDictLab()
        {
            DDLDictLabBinder(DropDictLab, true);
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToDouble(DropDictLab.SelectedValue));
            }
        }

        //绑定单位
        private void BindCustomer(double? labid)
        {
            DropDictcustomerBinder(DropCustomer, labid.ToString(), false);
        }

        ///选择分点事件 绑定单位
        protected void DropDictLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDictLab.SelectedValue != null)
            {
                BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));
                BindDictTest(DropDictLab.SelectedValue);
            }
            BindDictProduct(DropCustomer.SelectedValue);
        }

        ///选择单位 绑定套餐
        protected void DropCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDictProduct(DropCustomer.SelectedValue);
        }
        #endregion

        #region >>>> zhouy 生日|年龄计算
        /// 根据生日计算年龄
        private void GetAge()
        {
            DateTime birthday;
            bool b = DateTime.TryParse(dateBirthday.Text, out birthday);
            if (b)
            {
                TimeSpan ts = DateTime.Now.Date - birthday;
                if (ts.Days < 0) { dateBirthday.Text = DateTime.Today.ToShortDateString(); MessageBoxShow("生日日期不能在今天之后！"); return; }
                tbxAge.Text = (Math.Truncate((double)(ts.Days / 365))).ToString();
                tbxMonth.Text = ((ts.Days % 365) / 30).ToString();
                tbxDay.Text = ((ts.Days % 365) % 30).ToString();
            }
            else
            {
                tbxAge.Text = "0"; tbxMonth.Text = "0"; tbxDay.Text = "0"; tbxHour.Text = "0";
            }
        }

        /// <summary>计算年龄
        public int AgeToHour(int year, int month, int day, int hour)
        {
            return year * 12 * 30 * 24 + month * 30 * 24 + day * 24 + hour;

        }
        #endregion

        #region >>>> zhouy 文本框赋值

        /// <summary>修改时绑定会员数据和订单表数据
        /// 修改时绑定会员数据和订单表数据
        /// </summary>
        /// <param name="ordernum">订单号</param>
        private void OrderBindData(string ordernum)
        {
            Orders order = registerserver.SelectOrderInfo(ordernum);
            Dictmember member = memberservice.GetMemberById(order.Dictmemberid);
            //会员赋值
            SetMemberInfo(member);

            //订单
            tbxRemark.Text = order.Remarks;
            tbxItemTest.Label = tbxItemTest.Text = order.Ordertestlst;
            string[] strage = order.Age.Split('岁');
            tbxAge.Text = strage[0];
            string[] strmoneth = strage[1].Split('月');
            tbxMonth.Text = strmoneth[0];
            string[] strday = strmoneth[1].Split('日');
            tbxDay.Text = strday[0];
            string[] strhour = strday[1].Split('时');
            tbxHour.Text = strhour[0];            
            radlIsMarried.SelectedValue = order.Ismarried;
            DropDictLab.SelectedValue = order.Dictlabid.ToString();
            BindCustomer(order.Dictlabid);
            DropCustomer.SelectedValue = order.Dictcustomerid.ToString();
            tbxSection.Text = order.Section;
            txtPostAddress.Text = order.PostAddress;
            txtRECIPIENT.Text = order.Recipient;
            txtCONTACTNUMBER.Text = order.ContactNumber;
            dtSampleDate.Text = order.SamplingDate == null ? "" : order.SamplingDate.Value.ToString("yyyy-MM-dd");
            txtArea.Text = order.Area ?? "";
            tbxAccountmanager.Text = order.AccountManager ?? "";
            if (order.Province==null||order.Province=="")
            {
                dpProvince.SelectedIndex = 0;
            }
            else
            {
                dpProvince.SelectedValue = order.Province;
            }
            if (order.City==null||order.City=="")
            {
                dpCity.SelectedIndex = 0;
            }
            else
            {
                DropCityBinder(dpProvince, dpCity);
                dpCity.SelectedValue = order.City;
            }
            if (order.County==null||order.County=="")
            {
                dpCounty.SelectedIndex = 0;
            }
            else
            {
                DropCountyBinder(dpCity, dpCounty);
                dpCounty.SelectedValue = order.County;
            }
            //条码已打印则不能修改分点以及单位
            if ((Convert.ToInt32(order.Status)) >= ((int)daan.service.common.ParamStatus.OrdersStatus.BarCodePrint))
            {
                this.DropDictLab.Readonly = true;             
            }
        }

        /// <summary>会员资料赋值
        /// 会员资料赋值
        /// </summary>
        /// <param name="member"></param>
        private void SetMemberInfo(Dictmember member)
        {
            tbxPhone.Text = member.Phone;
            tbxMobile.Text = member.Mobile;
            tbxEMail.Text = member.Email;
            dateBirthday.Text = member.Birthday == null ? "" : member.Birthday.Value.ToString("yyyy-MM-dd");
            tbxAddres.Text = member.Addres;
            tbxIDNumber.Text = member.Idnumber;
            tbxmemberID.Text = member.Dictmemberid.ToString();
            tbxName.Text = member.Realname;
            DropSex.SelectedValue = member.Sex;
        }

        #endregion

        //关闭窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        //清空列表
        private void clearList()
        {
            this.tbxItemTest.Text = string.Empty;
            this.GridTest.Rows.Clear();
            ViewState["GridTest"] = null;
        }

        #region CustomClass
        public class CustomClass
        {
            private double? _id;
            public double? ID
            {
                get { return _id; }
                set { _id = value; }
            }
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public CustomClass(double? id, string name)
            {
                _id = id;
                _name = name;
            }
        }
        #endregion
    }
}



