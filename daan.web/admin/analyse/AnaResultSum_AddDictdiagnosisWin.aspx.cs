using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.login;
using daan.domain;
using ExtAspNet;
using System.Collections;
using daan.service.order;
using daan.util.Web;
using System.Data;

namespace daan.web.admin.analyse
{
    public partial class AnaResultSum_AddDictdiagnosisWin : PageBase
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                if (Request.QueryString["ordernum"]!=null)
                {
                    ViewState.Add("ordernum", Request.QueryString["ordernum"]);
                    
                    //ViewState.Add("orderbarcode", Request.QueryString["orderbarcode"]);
                    ViewState.Add("dictdiagnosisid", null);
                    BindDiagnosistype();
                    BindDiagnosis(GetDiagnosisAll());
                }
                

            }

        }

        /// <summary>
        /// 添加诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gdDiagnosis.SelectedRowIndexArray.Length==0)
            {
                MessageBoxShow("必须选择一项诊断建议");
                return;
            }
           
            if (ViewState["ordernum"] != null)
            {
                 int index = gdDiagnosis.SelectedRowIndexArray[0];
                //判断诊断建议是否重复
                OrderdiagnosisService orderdiagnosisService = new OrderdiagnosisService();
                Hashtable ht2 = new Hashtable();
                ht2.Add("ordernum",ViewState["ordernum"].ToString());
                ht2.Add("dictdiagnosisid",gdDiagnosis.DataKeys[index][0].ToString());
                DataTable dt = orderdiagnosisService.CountDiagnosis(ht2);
               
                if (dt.Rows[0][0].ToString()!="0")
                {
                    MessageBoxShow("诊断建议已存在");
                    return;
                }
               
                
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", ViewState["ordernum"].ToString());
                ht.Add("dictdiagnosisid", gdDiagnosis.DataKeys[index][0].ToString());

                if (orderdiagnosisService.AddOrderdiagnosis(ht))
                {
                    orderdiagnosisService.AddOperationLog(ViewState["ordernum"].ToString(), null, "总检", "添加诊断信息", "修改留痕", "无");
                }
                else
                {
                    MessageBoxShow("添加失败!请刷新重试");
                }
               
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        /// <summary>
        /// 根据缓存获取全部诊断建议字典
        /// </summary>
        /// <returns></returns>
        public List<Dictdiagnosis> GetDiagnosisAll()
        {
            LoginService loginservice = new LoginService();
            List<Dictdiagnosis> diagnosisAll = loginservice.GetLoginDictdiagnosisresultList();
            return diagnosisAll;
        }

        /// <summary>
        /// 绑定诊断建议字典
        /// </summary>
        public void BindDiagnosis(List<Dictdiagnosis> diagnosisAll)
        {
             PageUtil pageUtil = new PageUtil(gdDiagnosis.PageIndex, gdDiagnosis.PageSize);
            int start = pageUtil.GetPageStartNum();
            int end = pageUtil.GetPageEndNum();
            gdDiagnosis.RecordCount = diagnosisAll.Count;
            gdDiagnosis.DataSource =diagnosisAll.Skip(start-1).Take(end - start+1);
            gdDiagnosis.DataBind();
            
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       protected void gdDiagnosis_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdDiagnosis.PageIndex = e.NewPageIndex;
            ttbSearch_Trigger2Click(null, null);
        }



        /// <summary>
        /// 绑定疾病类型
        /// </summary>
        public void BindDiagnosistype()
        {
            LoginService loginservice = new LoginService();
            //疾病类型
            ddldiagnosistype.DataSource = loginservice.GetLoginInitbasicList().FindAll(c => c.Basictype == "DIAGNOSISTYPE");
            ddldiagnosistype.DataTextField = "Basicname";
            ddldiagnosistype.DataValueField = "BASICVALUE";
            ddldiagnosistype.DataBind();
        }

        #region 查找 根据诊断名称
        /// <summary>
        /// 查找 根据诊断名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();
            string diagnosisStr = ttbSearch.Text;
            List<Dictdiagnosis> newslist = new List<Dictdiagnosis>();
            foreach (Dictdiagnosis diagnosis in diagnosisAll)
            {
                if ((diagnosis.Diagnosisname != null && diagnosis.Diagnosisname.ToLower().Contains(diagnosisStr.ToLower())))
                {
                    newslist.Add(diagnosis);
                }
            }
            BindDiagnosis(newslist);
          
        }
        #endregion
        #region 显示详细信息
        //显示详细信息
        protected void gdDiagnosis_RowClick(object sender, GridRowClickEventArgs e)
        {
            Dictdiagnosis dictdiagnosis = new Dictdiagnosis();
            object[] keys = gdDiagnosis.DataKeys[e.RowIndex];
            List<Dictdiagnosis> diagnosisAll = GetDiagnosisAll();
            dictdiagnosis = (from Dictdiagnosis in diagnosisAll where Dictdiagnosis.Dictdiagnosisid == Convert.ToDouble(keys[0]) select Dictdiagnosis).ToList<Dictdiagnosis>()[0];
            txtdiagnosisname.Text = dictdiagnosis.Diagnosisname;
            ddldiagnosistype.SelectedValue = dictdiagnosis.Diagnosistype;
            if (dictdiagnosis.Isdisease == "1")
            {
                chbisdisease.Checked = true;
            }
            else
            {
                chbisdisease.Checked = false;
            }
            tadiseasedescription.Text = dictdiagnosis.Diseasedescription;
            tadiseasecause.Text = dictdiagnosis.Diseasecause;
            tasuggestion.Text = dictdiagnosis.Suggestion;
            taengdiseasedescription.Text = dictdiagnosis.Engdiseasedescription;
            taengdiseasecause.Text = dictdiagnosis.Engdiseasecause;
            taengsuggestion.Text = dictdiagnosis.Engsuggestion;
            txtdiagnosiscode.Text = dictdiagnosis.Diagnosiscode;
        }
        #endregion

    }
}