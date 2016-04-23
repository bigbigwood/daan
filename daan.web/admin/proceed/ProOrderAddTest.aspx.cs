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

namespace daan.web.admin.proceed
{
    public partial class ProOrderAddTest : PageBase
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
                    if (OrderNum == string.Empty) { MessageBoxShow("体检流水号为空,请重新打开窗口！"); return; }
                    List<OrderRegister> OrderRegisterList = registerserver.SelectOrdersDetail(OrderNum);
                    //修改绑定头数据
                    OrderBindData(OrderNum);
                    initBindDate();
                    //绑定列表数据
                    BindGridTest(OrderRegisterList);
                }
            }
        }

        /// <summary>页面加载数据初始绑定 ，加载
        /// 页面加载数据初始绑定 ，加载
        /// </summary>
        private void initBindDate()
        {
            if (tbxDictLab.Label != null && tbxDictLab.Label != "")
            {
                BindDictTest(tbxDictLab.Label);///组合项目
                BindDictProduct(tbxCustomer.Label);//套餐
            }
        }

        /// <summary>绑定Grid列表
        /// 绑定Grid列表
        /// </summary>
        /// <param name="OrderRegisterList">数据源LIST集合</param>
        private void BindGridTest(List<OrderRegister> OrderRegisterList)
        {
            ViewState["GridTest"] = OrderRegisterList;
            GridTest.DataSource = OrderRegisterList;
            try
            {
                GridTest.DataBind();
            }
            catch (Exception) { }
        }

        #region >>>> zhouy 保存
        ///保存
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            GridRowCollection GridRow = GridTest.Rows;

            double? memberid = null;
            if (tbxmemberID.Text != string.Empty) { memberid = Convert.ToDouble(tbxmemberID.Text); }

            ///获取系统时间时间
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
            double labid = Convert.ToDouble(tbxDictLab.Label);
            List<Dicttestgroupdetail> TestGroupDetailList = loginservice.GetLoginDicttestgroupdetail();//组合项目字典
            //获取集合
            List<OrderRegister> _gridtestList = GetGridTest(true);

            //已接收的条码号
            string ReceivedBarcode = string.Empty;

            #region >>>> zhouy 获取订单中 组合,套餐

            for (int i = 0; i < _gridtestList.Count; i++)
            {
                OrderRegister item = _gridtestList[i];
                if (item.IsProduct)///套餐 取套餐ID和套餐名
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
                   // continue;
                }
                string str = registerserver.checkSex(item.Id, tbxSex.Label);
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

                ////if (item.IsProduct)///套餐 取套餐ID和套餐名
                ////{
                _groupitem.Productid = item.Productid;
                _groupitem.Productname = item.Productname;///套餐名
                ////}

                grouptestList.Add(_groupitem);
            }

            #endregion

            #region >>>> zhouy insert Orders
            Orders order = registerserver.SelectOrderInfo(tbxOrderNum.Text);
            Orders _orders = order.Copy<Orders>();
            _orders.Ordernum = tbxOrderNum.Text;///体检流水号
            _orders.Remarks = tbxRemark.Text;///备注
            _orders.Dictmemberid = Convert.ToDouble(tbxmemberID.Text);  ///会员ID
            _orders.Dictcustomerid = customerid;///所属客户ID
            _orders.Realname = tbxName.Text;
            _orders.Sex = tbxSex.Label; ///性别 对应INITBASIC表
            _orders.Caculatedage = AgeToHour(year, month, day, hour);///计算后的年龄（小时为单位）
            _orders.Age = string.Format("{0}岁{1}月{2}日{3}时", year, month, day, hour); ;///年龄字符串拼接 年月日时
            //_orders.Enterby = Userinfo.userName;///录入人 修改时不变
            _orders.Ordertestlst = tbxItemTest.Text;///项目清单（冗余字段）
            _orders.Dictlabid = labid;///实验室分点
            _orders.Ordersource = "1";
            _orders.Ismarried = radlIsMarried.SelectedValue;///婚否
            _orders.Section = tbxSection.Text;///部门
            _orders.Lastupdatedate = date;
            // _orders.Createdate = date;
            #endregion
            string content = string.Empty;
            if (tbxItemTest.Label != tbxItemTest.Text) { content += string.Format("项目清单:[{0}]更改为[{1}],", tbxItemTest.Label, tbxItemTest.Text); }

            bool isreceived = ReceivedBarcode.TrimEnd(',') != string.Empty;
            string error = "";
            bool b = registerserver.insertUpdateOrders("追加项目", content, false, productList, grouptestList, null, _orders, ReceivedBarcode.TrimEnd(','), ref error);

            #region >>>> zhouy 执行结果 手续操作

            if (b)
            {
                PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
            }
            else { MessageBoxShow("保存失败:" + error); }

            #endregion

        }

        private static void ContrastObject(ref string content, Orders _order)
        {
            Orders order = registerserver.SelectOrderInfo(_order.Ordernum);


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
            customerid = Convert.ToDouble(tbxCustomer.Label);

            if (tbxDictLab.Label == string.Empty) { strmsg = "请选择实验室分点！"; return strmsg; }
            if (tbxName.Text == string.Empty) { strmsg = "姓名不能为空！"; return strmsg; }

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
            catch (Exception)
            {
                strmsg = "[岁月日时]格式只能为数字！"; return strmsg;
            }

            try
            {
                datebirthday = Convert.ToDateTime(tbxBirthday.Text);
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
            //List<OrderRegister> _gridtestList = GetGridTest(true);

            //GridRow row = GridTest.Rows[e.RowIndex];
            //OrderRegister _register = _gridtestList.Where<OrderRegister>(c => c.Id == Convert.ToDouble(row.Values[0])).First<OrderRegister>();

            //if (e.CommandName == "Delete")
            //{
            //    if (!_register.Isdelete) { MessageBoxShow("此项目账单已出，不允许删除"); return; }
            // //   if ((Convert.ToInt32(_register.Orderstatus)) >= ((int)daan.service.common.ParamStatus.OrdersStatus.BarCodePrint)) { MessageBoxShow("条码已打印，不允许删除"); return; }

            //    if ((Convert.ToInt32(_register.Status)) >= ((int)daan.service.common.ParamStatus.OrderbarcodeStatus.Received)) { MessageBoxShow("条码已接收，不允许删除"); return; }


            //    var deletename = _register.Name;
            //    var strname = "";
            //    if (_register.IsProduct)
            //    {
            //        deletename = _register.Productname;
            //        for (var i = 0; i < _gridtestList.Count; i++)
            //        {
            //            if (i == e.RowIndex) { continue; }

            //            OrderRegister temp = _gridtestList[i];
            //            if (temp.IsProduct && temp.Productname == deletename)
            //            {
            //                strname += temp.Name + ",";
            //                temp.Productid = null;
            //                temp.Productname = string.Empty;
            //            }
            //        }
            //    }
            //    deletename += ",";
            //    tbxItemTest.Text = tbxItemTest.Text.Replace(deletename, strname).Replace(",,", ",");
            //    _gridtestList.Remove(_register);
            //}
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
            // BindGridTest(_gridtestList);

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

            //if (_register.Isadd != "1")
            //{
            //    e.Values[7] = "";
            //}
            if (_register.Sendbilled == "1") { ddlcustomer.Enabled = false; }
        }

        //绑定外包医院下拉框
        private void BindDropSendCustomer(System.Web.UI.WebControls.DropDownList ddlcustomer)
        {
            if (ViewState["SendCustomer"] == null) { ViewState["SendCustomer"] = dictCustomerService.GetDictCustomerListByType("1").Where<Dictcustomer>(c => c.Dictlabid == Convert.ToDouble(tbxDictLab.Label)).ToList<Dictcustomer>(); }
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
            DropTest.DataSource = TestItemList;
            DropTest.DataTextField = "Testname";
            DropTest.DataValueField = "Dicttestitemid";
            DropTest.DataBind();
            DropTest.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }

        // 绑定套餐
        private void BindDictProduct(object customerid)
        {
            List<Dicttestitem> TestItemList = dicttestservice.GetProduct(customerid);
            DropItem.DataSource = TestItemList;
            DropItem.DataTextField = "Testname";
            DropItem.DataValueField = "Dicttestitemid";
            DropItem.DataBind();
            DropItem.Items.Insert(0, new ExtAspNet.ListItem("请选择", "-1"));
        }

        ///选择组合项目添加
        protected void DropTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropTest.SelectedIndex == 0) { return; }

            ExtAspNet.ListItem li = DropTest.SelectedItem;

            List<OrderRegister> _gridtestList = GetGridTest(false);
            string msg = registerserver.AddGroupTest(ref _gridtestList, tbxSex.Label, null, Convert.ToDouble(li.Value), true, Userinfo,null,null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            //Dicttestitem itemtest = registerserver.SelectsTestItemListById(Convert.ToDouble(li.Value));
            ////校验性别是否符合
            //string str = registerserver.checkSex(itemtest.Dicttestitemid, tbxSex.Label);
            //if (str != string.Empty) { MessageBoxShow(str); return; }
            //List<OrderRegister> _gridtestList = GetGridTest(false);

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
            //newOrderRegister.Isadd = "1";
            //newOrderRegister.Adduserid = Userinfo.userId;///追加人 取当前用户名,ID
            //newOrderRegister.Addusername = Userinfo.userName;
            //newOrderRegister.Isactive = "1";
            //newOrderRegister.Billed = "0";
            //newOrderRegister.Sendbilled = "0";
            //newOrderRegister.Sendoutcustomerid = null;
            //newOrderRegister.Tubegroup = itemtest.Tubegroup;
            //newOrderRegister.Barcode = barccode;

            //_gridtestList.Add(newOrderRegister);
            BindGridTest(_gridtestList);//绑定数据

            tbxItemTest.Text += li.Text + ",";
            DropTest.SelectedItem.Value = "-1";
        }

        ///选择套餐添加
        protected void DropItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropItem.SelectedIndex == 0) { return; }
            ExtAspNet.ListItem li = DropItem.SelectedItem;

            string productname = "";
            List<OrderRegister> _gridtestList = GetGridTest(false);
            string msg = registerserver.AddProduct(ref _gridtestList, tbxSex.Label, Convert.ToDouble(li.Value), true, Userinfo, ref productname,null);
            if (msg != string.Empty) { MessageBoxShow(msg); return; }
            tbxItemTest.Text += productname + ",";
            BindGridTest(_gridtestList);//绑定数据
            DropItem.SelectedItem.Value = "-1";
        }
        #endregion

        //关闭窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        #region >>>> zhouy 文本框赋值,清空，设置不可编辑

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
            //分点
            tbxDictLab.Label = order.Dictlabid.ToString();
            tbxDictLab.Text = loginservice.GetLoginDictlab().Where<Dictlab>(c => c.Dictlabid == order.Dictlabid).First<Dictlab>().Labname;

            tbxSection.Text = order.Section;
            //单位
            if (order.Ordersource == "1")
            {
                IEnumerable<Dictcustomer> IEcustomer = loginservice.GetDictcustomer().Where<Dictcustomer>(c => c.Dictcustomerid == order.Dictcustomerid);
                if (IEcustomer.Count() > 0)
                {
                    tbxCustomer.Label = IEcustomer.First<Dictcustomer>().Dictcustomerid.ToString();
                    tbxCustomer.Text = IEcustomer.First<Dictcustomer>().Customername;
                }
            }
            else
            {
                tbxCustomer.Text = "个人客户";
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
            tbxBirthday.Text = member.Birthday == null ? "" : member.Birthday.Value.ToString("yyyy-MM-dd");
            tbxAddres.Text = member.Addres;
            tbxIDNumber.Text = member.Idnumber;
            tbxmemberID.Text = member.Dictmemberid.ToString();
            tbxName.Text = member.Realname;
            tbxSex.Label = member.Sex;
            tbxSex.Text = member.SexName;
        }
        #endregion

        /// <summary>计算年龄
        public int AgeToHour(int year, int month, int day, int hour)
        {
            return year * 12 * 30 * 24 + month * 30 * 24 + day * 24 + hour;

        }
    }
}



