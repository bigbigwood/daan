using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.domain;
using daan.util.Web;
using System.Collections;
using daan.service.dict;
using ExtAspNet;
using System.Text;
using daan.util.Common;
using Daan.Authority.Handler;
using Daan.Authority.Handler.AuthorityServiceReference;
using daan.web.code;

namespace daan.web.admin.dict
{
    public partial class DictUserInfoList : PageBase
    {
        //用户Id
        public double DictUserId
        {
            get { return Convert.ToDouble(Convert.ToInt32(ViewState["DictUserId"]) == 0 ? 0 : ViewState["DictUserId"]); }
            set { ViewState["DictUserId"] = value; }
        }
        Dictuser dictuser = new Dictuser();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGrid();//绑定用户
                BindGridLabTo();//绑定分点列表
                BindGridDep();//绑定实验室
            }
          
        }

        /// <summary>
        /// 绑定用户列表
        /// </summary>
        private void BindGrid()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gvList.PageIndex, gvList.PageSize);
                Hashtable ht1 = new Hashtable();
                ht1.Add("strKey", TextUtility.ReplaceText(btSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(btSearch.Text.Trim()));
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                //设置总项数
                gvList.RecordCount = new DictuserService().GetDictuserPageLstCount(ht1);
                gvList.DataSource = new DictuserService().GetDictuserPageLst(ht1);
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定分点列表
        /// </summary>
        private void BindGridLabTo() //分点列表
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gdLabItem.PageIndex, gdLabItem.PageSize);
                Hashtable ht1 = new Hashtable();
                //ht1.Add("Userid", DictUserId);
                ht1.Add("pageStart", pageUtil.GetPageStartNum());//pageUtil.GetPageStartNum()
                ht1.Add("pageEnd", pageUtil.GetPageEndNum()); // pageUtil.GetPageEndNum()
                //设置总项数
                gdLabItem.RecordCount = new DictlabService().GetDictlabPageLstCount(ht1);
                gdLabItem.DataSource = new DictlabService().GetDictlabPageLst(ht1);

                List<Dictlabdept> dictlabdep = new DictlabdeptService().GetDictlabdeptPageLstUser(ht1).ToList();
                //double userId = 0;
                //if (gvList.SelectedRowIndexArray.Length > 0)
                //{
                //    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                //    userId = TypeParse.StrToDouble(objValue[0], 0);
                //}

                //全部列表
                List<Dictlab> labLst = new DictlabService().GetDictlabPageLst(ht1).ToList<Dictlab>();
                Hashtable ht2 = new Hashtable();
                ht2.Add("Userid", DictUserId);
                ht2.Add("pageStart", pageUtil.GetPageStartNum());
                ht2.Add("pageEnd", pageUtil.GetPageEndNum());
                IList<Dictuserandlab> dictlabList = new DictuserlabService().GetDictuserandlabPageLst(ht2);
                gdLabItem.DataBind();
                int[] labArray = new int[dictlabList.Count];
                if (dictlabList.Count != 0)
                {
                    int index = 0;
                    foreach (Dictuserandlab item in dictlabList)
                    {
                        labArray[index] = labLst.IndexOf(labLst.Find(lab => lab.Dictlabid == item.Dictlabid));
                        index++;
                    }
                    gdLabItem.SelectedRowIndexArray = labArray;
                }
                else
                {
                    gdLabItem.SelectedRowIndexArray = new int[] { };

                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 绑定实验室
        /// </summary>
        private void BindGridDep()
        {
            try
            {
                //分页查询条件
                PageUtil pageUtil = new PageUtil(gdDep.PageIndex, gdDep.PageSize);
                Hashtable ht1 = new Hashtable();
                //ht1.Add("Userid", DictUserId);
                ht1.Add("pageStart", pageUtil.GetPageStartNum());
                ht1.Add("pageEnd", pageUtil.GetPageEndNum());
                //设置总项数
                gdDep.RecordCount = new DictlabdeptService().GetDictlabdeptPageLstCount(ht1);
                gdDep.DataSource = new DictlabdeptService().GetDictlabdeptPageLst(ht1);

                //查询全部
                List<Dictlabdept> dictlabdep = new DictlabdeptService().GetDictlabdeptPageLstUser(ht1).ToList();
                //double userId = 0;
                //if (gvList.SelectedRowIndexArray.Length > 0)
                //{
                //    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                //    userId = TypeParse.StrToDouble(objValue[0], 0);
                //}
                Hashtable ht2 = new Hashtable();
                ht2.Add("Userid", DictUserId);
                ht2.Add("pageStart", pageUtil.GetPageStartNum());
                ht2.Add("pageEnd", pageUtil.GetPageEndNum());
                IList<Dictuserandlabdept> dictlabIlst = new DictuserandlabdeptService().GetDictuserandlabdeptPageLst(ht2);
                gdDep.DataBind();
                int[] labArray = new int[dictlabIlst.Count];
                if (dictlabIlst.Count != 0)
                {
                    int index = 0;
                    foreach (Dictuserandlabdept item in dictlabIlst)
                    {
                        labArray[index] = dictlabdep.IndexOf(dictlabdep.Find(lab => lab.Dictlabdeptid == item.Dictlabdeptid));
                        index++;
                    }
                    gdDep.SelectedRowIndexArray = labArray;
                }
                else
                {
                    gdDep.SelectedRowIndexArray = new int[] { };
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        //分页
        protected void gvList_PageIndexChange(object sender, ExtAspNet.GridPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        // 执行清空动作
        protected void btSearch_Trigger1Click(object sender, EventArgs e)
        {
            btSearch.Text = "";
            btSearch.ShowTrigger1 = false;
        }
        // 执行搜索动作   
        protected void btSearch_Trigger2Click(object sender, EventArgs e)
        {
            BindGrid();
            btSearch.ShowTrigger1 = true;
        }

        #region 绑定项目详细信息
        protected void gvList_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int gridRowID = e.RowIndex;
                    object[] keys = gvList.DataKeys[e.RowIndex];
                    //根据选中的行得到当前选中的实例
                    if (Convert.ToInt32(keys[0]) != 0)
                    {
                        dictuser.Dictuserid = Convert.ToInt32(keys[0]);
                        DictUserId = Convert.ToInt32(dictuser.Dictuserid);
                        BindGridLabTo();  //已选分点
                        BindGridDep();   //已选物理组
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// 行点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "edit") //编缉
            {
                int id = Convert.ToInt32(gvList.DataKeys[e.RowIndex][0].ToString());//获取Grid1中第e.RowIndex+1行的第一个DateKeyName值
                string UserName = gvList.DataKeys[e.RowIndex][1].ToString();
                //编辑绑定数据 
                if (id != 0)
                {
                    //WinLibraryEdit.Hidden = false;
                    string URL = "DictUser_Window.aspx?id=" + id;
                    string title = "用户资料维护 - " + UserName;
                    PageContext.RegisterStartupScript(WinLibraryEdit.GetShowReference(URL, title));
                }
            }
        }


        /// <summary>
        /// 分点添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                SortedList SQLlist = new SortedList(new MySort());
                //double userId = 0;
                //if (gvList.SelectedRowIndexArray.Length > 0)
                //{
                //    object[] objValue = gvList.DataKeys[gvList.SelectedRowIndexArray[0]];
                //    userId = TypeParse.StrToDouble(objValue[0], 0);
                //}
                if (DictUserId != 0)
                {
                    if (gdLabItem.SelectedRowIndexArray.Length > 0)
                    {
                        var library = new DictuserlabService();
                        Dictuserandlab userlab = new Dictuserandlab();
                        userlab.Dictuserid = DictUserId;
                        Hashtable hts = new Hashtable();
                        hts.Add("value", userlab.Dictuserid);
                        SQLlist.Add(new Hashtable() { { "DELETE", "Dict.DeleteDictuserandlabByUserId" } }, userlab.Dictuserid);
                        foreach (int row in gdLabItem.SelectedRowIndexArray)
                        {
                            Dictuserandlab dictuserandlabList = new Dictuserandlab();
                            dictuserandlabList.Dictuserandlabid = library.getSeqID("SEQ_DICTUSERANDLAB");
                            dictuserandlabList.Dictlabid = Convert.ToDouble(gdLabItem.DataKeys[row][0].ToString());
                            dictuserandlabList.Dictuserid = Convert.ToInt32(DictUserId);
                            dictuserandlabList.Createdate = DateTime.Now;
                            SQLlist.Add(new Hashtable() { { "INSERT", "Dict.InsertDictuserandlab" } }, dictuserandlabList);
                        }
                        if (library.ExecuteSqlTran(SQLlist))
                        {
                            BindGridLabTo();
                            MessageBoxShow("所选分点已成功保存,需重新登陆数据才能生效！", MessageBoxIcon.Information);
                           
                        }
                        else
                        {
                            MessageBoxShow("所选分点添加有误！", MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBoxShow("您未选择分点！", MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBoxShow("您还未选择用户！", MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加实验室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDepSave_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                SortedList SQLlist = new SortedList(new MySort());
                if (DictUserId != 0)
                {
                    if (gdDep.SelectedRowIndexArray.Length != 0)
                    {
                        var library = new DictuserandlabdeptService();
                        Dictuserandlabdept userdep = new Dictuserandlabdept();
                        userdep.Dictuserid = DictUserId;
                        Hashtable hts = new Hashtable();
                        hts.Add("value", userdep.Dictuserid);
                        SQLlist.Add(new Hashtable() { { "DELETE", "Dict.DeleteDictuserandlabdeptByUserId" } }, userdep.Dictuserid);
                        foreach (int row in gdDep.SelectedRowIndexArray)
                        {
                            Dictuserandlabdept dictuserandlabdep = new Dictuserandlabdept();
                            dictuserandlabdep.Dictuserandlabdeptid = library.getSeqID("SEQ_DICTUSERANDLABDEPT");
                            dictuserandlabdep.Dictlabdeptid = Convert.ToDouble(gdDep.DataKeys[row][0].ToString());
                            dictuserandlabdep.Dictuserid = Convert.ToInt32(DictUserId);
                            dictuserandlabdep.Createdate = DateTime.Now;
                            SQLlist.Add(new Hashtable() { { "INSERT", "Dict.InsertDictuserandlabdept" } }, dictuserandlabdep);
                        }

                        if (library.ExecuteSqlTran(SQLlist))
                        {
                            BindGridDep();
                            MessageBoxShow("所选实验室已成功保存,需重新登陆数据才能生效！",  MessageBoxIcon.Information); 
                        }
                        else
                        {
                            MessageBoxShow("所选实验室添加有误！",  MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBoxShow("您未选择实验室！", MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBoxShow("您还未选择用户！", MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 同步用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSynchronous_Click(object sender, EventArgs e)
        {
            //同步系统用户获取系统下所有用户信息
            List<UserInUserInfo> userinfo = SecurityHandler.LoginOn(Request.Cookies.Get("authorizationcode").Value.ToString()).GetUserInfoList();
            //本地用户
            List<Dictuser> allUser = new DictuserService().GetDictuser().ToList();
            Dictionary<string, Dictuser> map = new Dictionary<string, Dictuser>();
            for (int i = 0; i < allUser.Count; i++)
            {
                map.Add(allUser[i].Usercode, allUser[i]);
            }
            #region 用户数据更新
            foreach (UserInUserInfo sysu in userinfo)
            {
                if (map.ContainsKey(sysu.USERNAME))
                {
                    Dictuser newUser = map[sysu.USERNAME];
                    if (sysu.ECHONAME != null)
                        newUser.Username = sysu.ECHONAME;
                    if (sysu.PASSWORD != null)
                        newUser.Password = sysu.PASSWORD;
                    try
                    {
                        new DictuserService().SaveDictlab(newUser);
                    }
                    catch (Exception ex)
                    {
                        MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        Dictuser newUser = new Dictuser();
                        newUser.Username = sysu.ECHONAME;
                        newUser.Usercode = sysu.USERNAME;
                        newUser.Password = sysu.PASSWORD;
                        newUser.Dictlabid = 0;
                        newUser.Dictlabdeptid = 0;
                        newUser.Active = "1";
                        new DictuserService().SaveDictlab(newUser);
                    }
                    catch (Exception ex)
                    {
                        MessageBoxShow(ex.Message, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            MessageBoxShow("用户已同步！", MessageBoxIcon.Information);
            BindGrid();
            #endregion

        }
    }
}