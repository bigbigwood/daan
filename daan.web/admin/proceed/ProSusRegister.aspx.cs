using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using ExtAspNet;
using daan.domain;
using daan.service.proceed;
using daan.service.login;
using daan.service.dict;
using System.Text.RegularExpressions;
using daan.service.order;

namespace daan.web.admin.proceed
{
    public partial class ProSusRegister : PageBase
    {
        static ProRegisterService registerserver = new ProRegisterService();
        static LoginService loginservice = new LoginService();
        static DictmemberService memberservice = new DictmemberService();
        static DictCustomerService dictCustomerService = new DictCustomerService();
        static DicttestitemService dicttestservice = new DicttestitemService();
        static HpvtestingService hpvService = new HpvtestingService();
        static OrderbarcodeService barcodeservice = new OrderbarcodeService();
        protected void Page_Load(object sender, EventArgs e)
        {
            ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + DropCustomer.ClientID + "')).listWidth=250;");
            //会员选择 回发
            if (Request.Form["__EVENTTARGET"] == tbxmember.ClientID && Request.Form["__EVENTARGUMENT"] == "specialkey") { SelectMember(); }

            if (!IsPostBack)
            {
                BindDictLab();///绑定分点
                if (DropDictLab.SelectedValue != null)
                {
                    BindDictTest(DropDictLab.SelectedValue);///组合项目
                }
                BindDictProduct(DropCustomer.SelectedValue);//套餐 
                BindDictSex();
                //BindArea();
                tbxOrderNum.Text = registerserver.GetOrderNum();
                dateBirthday.MaxDate = DateTime.Now;
            }
        }
        #region 省市区绑定及下拉选择事件
        //private void BindArea()
        //{
        //    BindProvince();
        //    BindCity();
        //    BindCounty();
        //}
        ////省
        //private void BindProvince()
        //{
        //    dpProvince.DataSource = memberservice.GetProveice();
        //    dpProvince.DataTextField = "provincename";
        //    dpProvince.DataValueField = "dictprovinceid";
        //    dpProvince.DataBind();
        //    dpProvince.Items.Insert(0, new ListItem("选择省份", "-1"));
        //}
        ////市
        //private void BindCity()
        //{
        //    string pro = dpProvince.SelectedValue;
        //    if (pro != "-1")
        //    {
        //        dpCity.DataSource = memberservice.GetCity(pro);
        //        dpCity.DataTextField = "cityname";
        //        dpCity.DataValueField = "dictcityid";
        //        dpCity.DataBind();
        //    }
        //    dpCity.Items.Insert(0, new ListItem("选择市", "-1"));
        //}
        ////县/区
        //private void BindCounty()
        //{
        //    string city = dpCity.SelectedValue;
        //    if (city != "-1")
        //    {
        //        dpCounty.DataSource = memberservice.GetCounty(city);
        //        dpCounty.DataTextField = "countyname";
        //        dpCounty.DataValueField = "dictcountyid";
        //        dpCounty.DataBind();
        //    }
        //    dpCounty.Items.Insert(0, new ListItem("选择县/区", "-1"));
        //}
        ////选择省份
        //protected void dpProvince_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dpCity.Items.Clear();
        //    BindCity();

        //    dpCounty.Items.Clear();
        //    BindCounty();
        //}
        ////选择市
        //protected void dpCity_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dpCounty.Items.Clear();
        //    BindCounty();
        //}
        #endregion
        /// <summary> 
        ///  输入会员名查找会员 
        /// </summary>
        private void SelectMember()
        {
            Dictmember m = new Dictmember();
            m.Realname = tbxmember.Text.Trim();
            List<Dictmember> memberlist = memberservice.GetDictmemberList(m);
            if (memberlist.Count == 0)
            {
                MessageBoxShow("没有搜索到匹配的会员！");
                tbxmember.Text = string.Empty;
                tbxmember.Focus();
                return;
            }
            else if (memberlist.Count == 1)
            {
                SetMemberInfo(memberlist[0]);
                GetAge();
                tbxmember.Text = string.Empty;
            }
            else
            {
                GridMember.DataSource = memberlist;
                GridMember.DataBind();
                winMemberSelect.Hidden = false;
            }
        }

        /// <summary>
        /// 会员资料赋值
        /// </summary>
        /// <param name="member"></param>
        private void SetMemberInfo(Dictmember member)
        {
            tbxMobile.Text = member.Mobile;
            tbxEMail.Text = member.Email;
            if (member.Birthday != null)
            {
                dateBirthday.Text = member.Birthday.Value.ToString("yyyy-MM-dd");
            }
            tbxAddres.Text = member.Addres;
            tbxIDNumber.Text = member.Idnumber;
            tbxmemberID.Text = member.Dictmemberid.ToString();
            tbxName.Text = member.Realname;
            DropSex.SelectedValue = member.Sex;
            radlIsMarried.SelectedValue = member.Ismarried == null ? "2" : member.Ismarried;
        }

        /// <summary>
        /// 绑定Grid列表
        /// </summary>
        /// <param name="OrderRegisterList">数据源LIST集合</param>
        private void BindGridTest(List<OrderRegister> OrderRegisterList)
        {
            ViewState["GridTest"] = OrderRegisterList;
            GridTest.DataSource = OrderRegisterList;
            GridTest.DataBind();
        }

        /// 添加 清空数据
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            clearData(PageIsInsert.Readonly);
        }

        /// 档案
        protected void btnArchive_Click(object sender, EventArgs e)
        {
            if (tbxmemberID.Text == string.Empty || tbxName.Text == string.Empty)
            {
                MessageBoxShow("请先选择会员");
                return;
            }
            string URL = "ProMemberFile.aspx?Mid=" + Convert.ToDouble(tbxmemberID.Text);
            PageContext.RegisterStartupScript(winArchive.GetShowReference(URL));
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
            if (barcodeservice.CheckBarCode(tbxbarcode1.Text.Trim()))//条码号存在
            {
                MessageBoxShow(string.Format("此条码号[{0}]已在本系统内生成，请更改条码号！", this.tbxbarcode1.Text.Trim()));
                return;
            }
            double? memberid = null;
            if (tbxmemberID.Text != string.Empty) { memberid = Convert.ToDouble(tbxmemberID.Text); }

            ///获取系统时间时间
            DateTime? date = loginservice.GetServerTime();
            DateTime? datebirthday = null;
            int year = 0, month = 0, day = 0, hour = 0;//年月日时
            double customerid;
            //验证会员信息
            string msg = SaveCheck(date, out datebirthday, out year, out month, out day, out hour, out customerid);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }

            List<Dicttestitem> grouptestList = new List<Dicttestitem>(); //订单中组合集合            
            List<Dicttestitem> productList = new List<Dicttestitem>();//订单中套餐集合
            ///实验室分点
            double labid = Convert.ToDouble(DropDictLab.SelectedValue);
            List<Dicttestgroupdetail> TestGroupDetailList = loginservice.GetLoginDicttestgroupdetail();//组合项目字典
            //获取集合
            List<OrderRegister> _gridtestList = GetGridTest(true);

            #region >>>> zhouy 获取订单中 组合,套餐

            for (int i = 0; i < _gridtestList.Count; i++)
            {
                OrderRegister item = _gridtestList[i];
                //判断性别是否符合
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
                if (item.IsProduct)///套餐 去套餐ID和套餐名
                {

                    _groupitem.Productid = item.Productid;
                    _groupitem.Productname = item.Productname;///套餐名
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

                grouptestList.Add(_groupitem);
            }

            #endregion

            #region >>>> zhouy 会员信息

            Dictmember member = new Dictmember();
            member.Realname = tbxName.Text;
            member.Idnumber = tbxIDNumber.Text;
            //检查会员
            string errstr = registerserver.checkmember(memberid, ref member);
            if (errstr != string.Empty) { MessageBoxShow(errstr); return; }

            member.Nickname = tbxName.Text;
            member.Sex = DropSex.SelectedValue;
            member.Birthday = datebirthday;
            member.Addres = tbxAddres.Text;
            member.Mobile = tbxMobile.Text;
            member.Email = tbxEMail.Text;
            member.Islock = "F";///是否锁定

            #endregion

            #region >>>> zhouy insert Orders

            Orders _orders = new Orders();
            _orders.Ordernum = tbxOrderNum.Text;///体检流水号
            _orders.Remarks = tbxRemark.Text;///备注
            _orders.Dictmemberid = member.Dictmemberid;  ///会员ID
            _orders.Dictcustomerid = customerid;///所属客户ID
            _orders.Realname = member.Nickname;
            _orders.Sex = member.Sex; ///性别 对应INITBASIC表
            _orders.Caculatedage = AgeToHour(year, month, day, hour);///计算后的年龄（小时为单位）
            _orders.Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, hour); ;///年龄字符串拼接 年月日时
            _orders.Enterby = Userinfo.userName;///录入人
            _orders.Ordertestlst = tbxItemTest.Text;///项目清单（冗余字段）
            _orders.Dictlabid = labid;///实验室分点
            //_orders.Ordersource = radlCustomerType.SelectedValue;
            _orders.Ordersource = "1";//默认单位
            _orders.Ismarried = radlIsMarried.SelectedValue;///婚否
            _orders.Section = tbxSection.Text;///部门
            _orders.Createdate = date;

            //_orders.Provincename = dpProvince.SelectedValue == "-1" ? "" : dpProvince.SelectedText;
            //_orders.Cityname = dpCity.SelectedValue == "-1" ? "" : dpCity.SelectedText;
            //_orders.Countyname = dpCounty.SelectedValue == "-1" ? "" : dpCounty.SelectedText;
            #endregion

            string error = "";
            bool b = registerserver.insertUpdateOrders("体检登记", "", true, productList, grouptestList, member, _orders, "", ref error);
            if (b) { clearData(true); } else { MessageBoxShow("保存失败:" + error); }
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

            //单位必选
            customerid = Convert.ToDouble(DropCustomer.SelectedValue);

            if (DropDictLab.SelectedValue == null) { strmsg = "请选择实验室分点！"; return strmsg; }
            if (tbxName.Text.Trim() == string.Empty) { tbxName.Text = ""; strmsg = "姓名不能为空！"; return strmsg; }

            string idnumber = tbxIDNumber.Text.Trim();
            
            tbxMobile.Text = tbxMobile.Text.Trim();
            //tbxPhone.Text = tbxPhone.Text.Trim();
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

            if (tbxAge.Text.Trim() == string.Empty) { tbxAge.Text = "0"; }
            if (tbxMonth.Text.Trim() == string.Empty) { tbxMonth.Text = "0"; }
            if (tbxDay.Text.Trim() == string.Empty) { tbxDay.Text = "0"; }
            if (tbxHour.Text.Trim() == string.Empty) { tbxHour.Text = "0"; }
            try
            {
                year = Convert.ToInt32(tbxAge.Text);
                month = Convert.ToInt32(tbxMonth.Text);
                day = Convert.ToInt32(tbxDay.Text);
                hour = Convert.ToInt32(tbxHour.Text);
            }
            catch (Exception)
            {
                strmsg = "[岁月日时]格式只能为数字！"; return strmsg;
            }

            try
            {
                DateTime dataStart = Convert.ToDateTime("1900-1-1");
                datebirthday = Convert.ToDateTime(dateBirthday.Text);
                TimeSpan ts = (TimeSpan)(date.Value.Date - datebirthday);
                if (ts.Days < 0 || datebirthday <= dataStart) { strmsg = "日期范围[1900-01-01]-[" + date.Value.Date + "]!"; return strmsg; }
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
                var deletename = "[" + _register.Code + "]" + _register.Name;//////////
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
                            strname += "[" + temp.Code + "]" + temp.Name + ",";//////////
                            temp.Productid = null;
                            temp.Productname = string.Empty;
                        }
                    }
                }
                deletename += ",";
                tbxItemTest.Text = tbxItemTest.Text.Replace(deletename, strname).Replace(",,", ",");
                _gridtestList.Remove(_register);
            }
            //else if (e.CommandName == "Stop")
            //{
            //    if (_register.Isactive == "1")
            //    {
            //        row.Values[4] = "停止测试";
            //        _register.Isactive = "0";
            //    }
            //    else
            //    {
            //        GridTest.Rows[e.RowIndex].Values[4] = "正常";
            //        _register.Isactive = "1";
            //    }
            //}
            BindGridTest(_gridtestList);

        }

        //行绑定  
        protected void GridTest_RowDataBound(object sender, GridRowEventArgs e)
        {
            OrderRegister _register = e.DataItem as OrderRegister;
            System.Web.UI.WebControls.DropDownList ddlcustomer = (System.Web.UI.WebControls.DropDownList)GridTest.Rows[e.RowIndex].FindControl("DropSendCustomer");
            //绑定数据
            BindDropSendCustomer(ddlcustomer);
            ddlcustomer.SelectedValue = (_register.Sendoutcustomerid ?? -1).ToString();
            e.Values[3] = _register.IsGroup ? "组合" : "项目";
            //e.Values[4] = _register.Isactive == "1" ? "正常" : "已停测试";
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

        #region >>>> zhouy 选择会员窗口(选择&关闭 事件)
        ///选择会员
        protected void btnSelectMember_Click(object sender, EventArgs e)
        {
            int[] selectrow = GridMember.SelectedRowIndexArray;

            if (selectrow.Length <= 0) { MessageBoxShow("您还没有选择会员"); return; }

            selectMemberByRowIndex(selectrow[0]);
        }

        private void selectMemberByRowIndex(int index)
        {
            int memberid = Convert.ToInt32(GridMember.DataKeys[index][0]);

            Dictmember member = memberservice.GetMemberById(memberid);
            if (member != null)
            {
                SetMemberInfo(member);
                winMemberSelect.Hidden = true;
                GetAge();
                tbxmember.Text = string.Empty;
            }
        }

        ///关闭会员选择窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            winMemberSelect.Hidden = true;
        }

        protected void GridMember_RowClick(object sender, ExtAspNet.GridRowClickEventArgs e)
        {
            selectMemberByRowIndex(e.RowIndex);
        }
        #endregion

        #region >>>> zhouy 组合项目，套餐绑定以及选择添加
        ///绑定组合项目
        private void BindDictTest(object labid)
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetGroupTestByLabId(labid);
            BindDropdownList(DropTest, TestItemList);
        }

        // 绑定套餐
        private void BindDictProduct(object customerid)
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetProduct(customerid);
            BindDropdownList(DropItem, TestItemList);
            clearList();
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
            List<OrderRegister> _gridtestList = GetGridTest(false);

            Dicttestitem dictestitem = new DicttestitemService().SelectDicttestitemByDicttestitemid(Convert.ToDouble(li.Value.ToString()));
            if (dictestitem != null)
            {
                if (dictestitem.Tubegroup == "" || dictestitem.Dictlabdeptid.ToString() == "" || dictestitem.Dictspecimentypeid.ToString() == "")
                {
                    if (dictestitem.Tubegroup == "")
                    {
                        MessageBoxShow("" + dictestitem.Testname + "分管原则不能为空，请重新维护后再添加！");
                        return;
                    }
                    else if (dictestitem.Dictlabdeptid.ToString() == "")
                    {
                        MessageBoxShow("" + dictestitem.Testname + "所属科室不能为空，请重新维护后再添加！");
                        return;
                    }
                    else if (dictestitem.Dictspecimentypeid.ToString() == "")
                    {
                        MessageBoxShow("" + dictestitem.Testname + "标本类型不能为空，请重新维护后再添加！");
                        return;
                    }
                }
            }
            string msg = registerserver.AddGroupTest(ref _gridtestList, DropSex.SelectedValue, null, Convert.ToDouble(li.Value), false, Userinfo, null, null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            //Dicttestitem itemtest = registerserver.SelectsTestItemListById(Convert.ToDouble(li.Value));
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
            //    if (item.Tubegroup == itemtest.Tubegroup && (Convert.ToInt32(item.Status)) < ((int)daan.service.common.ParamStatus.OrderbarcodeStatus.Received))
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
            //newOrderRegister.Isadd = PageIsInsert.Readonly ? "1" : "0";
            //if (PageIsInsert.Readonly)
            //{
            //    newOrderRegister.Adduserid = Userinfo.userId;///追加人 取当前用户名,ID
            //    newOrderRegister.Addusername = Userinfo.userName;
            //}
            //newOrderRegister.Isactive = "1";
            //newOrderRegister.Billed = "0";
            //newOrderRegister.Sendbilled = "0";
            //newOrderRegister.Sendoutcustomerid = null;
            //newOrderRegister.Tubegroup = itemtest.Tubegroup;
            //newOrderRegister.Barcode = barccode;
            //_gridtestList.Add(newOrderRegister);
            BindGridTest(_gridtestList);//绑定数据

            tbxItemTest.Text += li.Text + ",";

            //DropTest.SelectedItem.Value = "-1";
            // DropTest.SelectedValue = "-1";
        }

        ///选择套餐添加
        protected void DropItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropItem.SelectedIndex == 0) { return; }
            ExtAspNet.ListItem li = DropItem.SelectedItem;

            CacheHelper.RemoveAllCache("daan.GetDictproductdetail");
            List<Dictproductdetail> dictproduct = loginservice.GetLoginDictproductdetail();
            List<Dictproductdetail> dictprodu = (from Dictproductdetail in dictproduct where (Dictproductdetail.Productid == Convert.ToDouble(li.Value)) select Dictproductdetail).ToList<Dictproductdetail>();
            if (dictprodu.Count > 0)
            {
                for (int i = 0; i < dictprodu.Count; i++)
                {
                    Dicttestitem dictestitem = new DicttestitemService().SelectDicttestitemByDicttestitemid(Convert.ToDouble(dictprodu[i].Testgroupid));
                    if (dictestitem != null)
                    {
                        if (dictestitem.Tubegroup == "" || dictestitem.Dictlabdeptid.ToString() == "" || dictestitem.Dictspecimentypeid.ToString() == "")
                        {
                            if (dictestitem.Tubegroup == "")
                            {
                                MessageBoxShow("" + dictestitem.Testname + "分管原则不能为空，请重新维护后再添加！");
                                return;
                            }
                            else if (dictestitem.Dictlabdeptid.ToString() == "")
                            {
                                MessageBoxShow("" + dictestitem.Testname + "所属科室不能为空，请重新维护后再添加！");
                                return;
                            }
                            else if (dictestitem.Dictspecimentypeid.ToString() == "")
                            {
                                MessageBoxShow("" + dictestitem.Testname + "标本类型不能为空，请重新维护后再添加！");
                                return;
                            }
                        }
                    }
                }

            }

            string productname = "";
            List<OrderRegister> _gridtestList = GetGridTest(false);
            string msg = registerserver.AddProduct(ref _gridtestList, DropSex.SelectedValue, Convert.ToDouble(li.Value), false, Userinfo, ref productname, null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            tbxItemTest.Text += productname + ",";
            BindGridTest(_gridtestList);//绑定数据
            //DropItem.SelectedItem.Value = "-1";
        }
        #endregion

        #region >>>> zhouy 绑定分点，以及选分点筛选单位
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
            List<Dictcustomer> CustomerList = loginservice.GetDictcustomer();

            var customer = CustomerList.Where<Dictcustomer>(c => c.Dictlabid == labid && c.Customertype == "0" && c.Active == "1");//

            DropCustomer.DataSource = customer;
            DropCustomer.DataValueField = "Dictcustomerid";
            DropCustomer.DataTextField = "Customername";
            DropCustomer.DataBind();



            //if (radlCustomerType.SelectedValue == "0")
            //{
            //    DropCustomer.Items.Insert(0, new ExtAspNet.ListItem("个人客户", "-1"));
            //    DropCustomer.SelectedValue = "-1";
            //}
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

            ViewState["SendCustomer"] = null;
        }

        ///选择单位 绑定套餐
        protected void DropCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDictProduct(DropCustomer.SelectedValue);
        }

        //绑定姓别
        private void BindDictSex()
        {
            DDLInitbasicBinder(DropSex, "SEX");
            DropSex.SelectedValue = "F";///设置默认选项
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

        /// <summary>清空数据
        /// 清空数据
        /// <param name="isOrdernum">是否重新生成订单号</param>
        /// </summary>
        private void clearData(bool isOrdernum)
        {
            if (isOrdernum)
            {
                tbxOrderNum.Text = registerserver.GetOrderNum();
            }
            tbxbarcode1.Text = string.Empty;
            tbxmember.Text = string.Empty;
            tbxmemberID.Text = string.Empty;
            tbxAddres.Text = string.Empty;
            tbxAge.Text = string.Empty;
            tbxMonth.Text = string.Empty;
            tbxHour.Text = string.Empty;
            tbxDay.Text = string.Empty;
            tbxEMail.Text = string.Empty;
            tbxIDNumber.Text = string.Empty;
            tbxItemTest.Text = string.Empty;
            tbxMobile.Text = string.Empty;
            //tbxPhone.Text = string.Empty;
            tbxRemark.Text = string.Empty;
            tbxSection.Text = string.Empty;
            tbxName.Text = string.Empty;
            radlIsMarried.SelectedIndex = 2;///默认选中未知 
            //  radlCustomerType.SelectedIndex = 0;///默认选中个人  单位选择禁用
            // DropCustomer.SelectedIndex = 0;
            //DropCustomer.Readonly = true;
            GridTest.Rows.Clear();///清空集合数据           
            DropSex.SelectedValue = "F";///设置默认选项
            dateBirthday.Text = string.Empty;
            // DropTest.SelectedItem.Value = "-1";
            //DropItem.SelectedItem.Value = "-1";
            //this.DropItem.Enabled = true;
            //this.DropTest.Enabled = true;
            ViewState["GridTest"] = null;//订单的组合项目
            ViewState["SendCustomer"] = null;//外包客户
        }

        //清空列表
        private void clearList()
        {
            this.tbxItemTest.Text = string.Empty;
            this.GridTest.Rows.Clear();
            ViewState["GridTest"] = null;
        }

        /// <summary>
        /// 易感基因条码扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxbarcode1_TriggerClick(object sender, EventArgs e)
        {
            if (tbxbarcode1.Text.Trim() == "")
            {
                MessageBoxShow("标本条码为空！");
                clearList();
                return;
            }
            Hashtable ht1 = new Hashtable();
            ht1.Add("Barcode", tbxbarcode1.Text);
            List<Hpvinstruments> hpvinstrmentsList = hpvService.GetHpvinstrumentsByWhere(ht1).ToList();
            if (hpvinstrmentsList.Count == 0)
            {
                this.tbxbarcode1.Text = string.Empty;
                MessageBoxShow("没有找到该标本条码");
                clearList();
                return;
            }
            Dictcustomer dictcustomer = dictCustomerService.GetDictCustomerById(Convert.ToDouble(hpvinstrmentsList[0].Dictcustomerid.ToString()));
            if (dictcustomer != null)
            {
                Dictlab dictlab = new DictlabService().GetDictlabById(Convert.ToDouble(dictcustomer.Dictlabid));
                if (dictlab != null)
                {
                    DropDictLab.SelectedValue = dictlab.Dictlabid.ToString();
                    if (DropDictLab.SelectedValue != null)
                    {
                        BindCustomer(Convert.ToInt32(DropDictLab.SelectedValue));//绑定单位
                        DropCustomer.SelectedValue = hpvinstrmentsList[0].Dictcustomerid.ToString();
                        BindDictTest(DropDictLab.SelectedValue);//绑定组合项目
                    }
                }
            }
            ViewState["SendCustomer"] = null;
            BindDictProduct(DropCustomer.SelectedValue);//绑定套餐
            string productname = "";
            List<OrderRegister> _gridtestList = GetGridTest(false);

            string msg = registerserver.AddProduct
                (
                ref _gridtestList,
                DropSex.SelectedValue,
                Convert.ToDouble(hpvinstrmentsList[0].Dicttestitemid),
                false,
                Userinfo,
                ref productname,
                tbxbarcode1.Text,
                true
                );
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            tbxItemTest.Text += productname + ",";
            BindGridTest(_gridtestList);
            this.tbxbarcode1.Text = string.Empty;
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



