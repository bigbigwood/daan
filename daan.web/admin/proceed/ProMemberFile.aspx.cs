using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.domain;
using daan.service.login;
using hlis.service.common;
using ExtAspNet;
using daan.service.proceed;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;

namespace daan.web.admin.proceed
{
    public partial class ProMemberFile : PageBase
    {
        static DictmemberService memberservice = new DictmemberService();
        static DictMEDHistoryService MEDHistory = new DictMEDHistoryService();
        private string _mid;
        public string Mid
        {
            get { return _mid; }
            set { _mid = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                dateBirthday.MaxDate = DateTime.Now;
            }
        }

        private void InitData()
        {
            object MidQueryString = Request.QueryString["Mid"];
            if (MidQueryString != null)
            {
                Mid = MidQueryString.ToString();
                Dictmember Member = memberservice.GetMemberById(Convert.ToDouble(Mid));
                if (Member == null)
                {
                    MessageBoxShow(string.Format("不存在ID为[{0}]的会员", Mid));
                    PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
                    return;
                }
                Dictmember member = Member;
                ViewState["oldmember"] = member;
                //不可修改
                lblConsumer.Text = member.Consumer.ToString();
                lblCreatedate.Text = member.Createdate.ToString();
                lblscores.Text = member.Scores.ToString();
                if (member.Logintime != null)
                {
                    lblLogintime.Text = member.Logintime.ToString();
                }
                else
                {
                    lblLogintime.Text = "";
                }
                lblLoginnum.Text = member.Loginnum.ToString();

                lblLoginip.Text = member.Loginip;
                lblId.Text = member.Dictmemberid.ToString();
                lblLoginname.Text = member.Loginname;

                //可修改
                tbxRealname.Text = member.Realname;
                tbxNickname.Text = member.Nickname;
                tbxIdnumber.Text = member.Idnumber;
                DropSex.SelectedValue = member.Sex;
                if (member.Birthday != null)
                {
                    dateBirthday.Text = member.Birthday.Value.ToString("yyyy-MM-dd");
                }

                tbxEmail.Text = member.Email;
                tbxQq.Text = member.Qq;
                tbxMsn.Text = member.Msn;
                tbxUrl.Text = member.Url;
                ckbIsactive.Checked = member.Active == "T" ? true : false;
                ckbIslock.Checked = member.Islock == "T" ? true : false;
                tbxPhone.Text = member.Phone;
                tbxMobile.Text = member.Mobile;
                tbxAddres.Text = member.Addres;

                lblMemberID.Label = Mid.ToString();
                BindData();
            }
            return;
        }

        //绑定数据
        private void BindData()
        {
            DataTable dt=MEDHistory.GetHealthRecordsDataList(lblMemberID.Label);
            GridMedHistory.DataSource = dt;
            GridMedHistory.DataBind();
        }

        //保存会员信息
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string strmsg = "";
            //判断
            Dictmember member = new Dictmember();
            member.Dictmemberid = Convert.ToDouble(lblId.Text.Trim());
            member.Realname = tbxRealname.Text.Trim();
            member.Nickname = tbxNickname.Text.Trim();
            string idnumber = tbxIdnumber.Text.Trim();
            
            member.Idnumber = idnumber;
            member.Sex = DropSex.SelectedValue;
            if (dateBirthday.Text.Trim() != "")
                member.Birthday = Convert.ToDateTime(dateBirthday.Text.Trim());
            if (tbxEmail.Text.Trim() != string.Empty)
            {
                Regex reg = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
                if (!reg.IsMatch(tbxEmail.Text.Trim()))
                {
                    strmsg = "请检查您填写的Email地址！";
                    MessageBoxShow(strmsg);
                    return;
                }
                else
                {
                    member.Email = tbxEmail.Text.Trim();
                }
            }

            member.Qq = tbxQq.Text.Trim();
            member.Msn = tbxMsn.Text.Trim();
            member.Url = tbxUrl.Text.Trim();
            member.Active = ckbIsactive.Checked ? "T" : "F";
            member.Islock = ckbIslock.Checked ? "T" : "F";
            member.Phone = tbxPhone.Text.Trim();
            member.Addres = tbxAddres.Text.Trim();
            member.Mobile = this.tbxMobile.Text.Trim();
            Dictmember oldmember = ViewState["oldmember"] as Dictmember;
            if (!new ProRegisterService().UpdateMemberInfo(oldmember, member))
            {
                MessageBoxShow("保存失败，请刷新页面重试");
                return;
            }
            else
            {
                MessageBoxShow("保存成功！");
                // PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            }
            oldmember = member;
        }

        //关闭档案窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }

        //添加
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string url = "../dict/DictHealthRecords_Window.aspx?mid="+lblMemberID.Label;
            PageContext.RegisterStartupScript(win1.GetShowReference(url));
        }

        //添加既往病史
        //protected void btnMedHistoryAdd_Click(object sender, EventArgs e)
        //{
        //    if (tbxDiseaseName.Text.Trim() == string.Empty) { MessageBoxShow("请填写患病名称！"); return; }
        //    try
        //    {
        //        Convert.ToInt32(tbxDiseaseDate.Text);
        //        if (tbxDiseaseDate.Text.Length > 4 || tbxDiseaseDate.Text.Length == 0) { throw new Exception(); }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBoxShow("请输入指定格式年份！如：1949"); return;
        //    }
        //    Dictmedhistory _medhistory = new Dictmedhistory();
        //    _medhistory.Dictmemberid = Convert.ToDouble(lblMemberID.Label);
        //    _medhistory.Diseasedate = Convert.ToDateTime(DateTime.Today.ToString().Replace(DateTime.Today.Year.ToString(), tbxDiseaseDate.Text));
        //    _medhistory.Diseasename = tbxDiseaseName.Text;
        //    _medhistory.Istakemedicine = radlIsTakemedicine.SelectedValue;
        //    _medhistory.Istreat = radlIsTreat.SelectedValue;
        //    _medhistory.Medname = tbxMedName.Text;
        //    bool b = MEDHistory.InsertmedHistory(_medhistory);
        //    if (!b)
        //    {
        //        MessageBoxShow("添加失败，请重试!"); return;

        //    }
        //    //刷数据.
        //    BindMedHistoryData();
        //    //记录日志
        //    List<LogInfo> logLst = DictMEDHistoryService.getLogInfo<Dictmedhistory>(new Dictmedhistory(), _medhistory);
        //    Dictmember _member = ViewState["oldmember"] as Dictmember;
        //    MEDHistory.AddMaintenanceLog("dictmedhistory", _medhistory.Dictmedhistoryid, logLst, "添加", _member == null ? "" : _member.Realname, _member == null ? "" : _member.Loginname, "会员档案");

        //    tbxDiseaseName.Text = string.Empty;
        //    tbxMedName.Text = string.Empty;
        //    tbxDiseaseDate.Text = string.Empty;
        //    radlIsTakemedicine.SelectedIndex = 0;
        //    radlIsTreat.SelectedIndex = 0;
        //}

        //删除
        protected void btnDel_Click(object sender, EventArgs e)
        {
            int[] selectstr = GridMedHistory.SelectedRowIndexArray;
            if (selectstr.Length != 1)
            {
                MessageBoxShow("请选中一条既往病史记录删除!"); return;
            }
            string id = GridMedHistory.DataKeys[selectstr[0]][0].ToString();
            if (!MEDHistory.DeleteHealthRecords(id))
            {
                MessageBoxShow("删除失败，请重试!"); 
                return;
            }
            BindData();
        //    string[] str = GridMedHistory.Rows[selectstr[0]].Values;
        //    //获取要删除的对象 fhp
        //    Dictmedhistory _medhistory = new Dictmedhistory();
        //    if (str != null && Request.QueryString["Mid"] != null)
        //    {
        //        //获取会员所有家族病史 fhp 
        //        IList<Dictmedhistory> medhistoryList = MEDHistory.GetDictmedHistoryDataList(Request.QueryString["Mid"].ToString());
        //        //取出选中的病史 fhp
        //        _medhistory = (from Dictmedhistory in medhistoryList where Dictmedhistory.Dictmedhistoryid == Convert.ToDouble(str[0].ToString()) select Dictmedhistory).ToList<Dictmedhistory>()[0];
        //    }
        //    if (!MEDHistory.DeletemedHistory(str[0]))
        //    {
        //        MessageBoxShow("删除失败，请重试!"); return;
        //    }
        //    //刷数据.
        //    BindMedHistoryData();
            //记录日志
            //List<LogInfo> logLst = DictMEDHistoryService.getLogInfo<Dictmedhistory>(_medhistory, new Dictmedhistory());
            //Dictmember _member = ViewState["oldmember"] as Dictmember;
            //MEDHistory.AddMaintenanceLog("dictmedhistory", Convert.ToInt32(id), logLst, "删除", _member == null ? "" : _member.Realname, _member == null ? "" : _member.Loginname, "会员档案");

        }
    }
}