using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using daan.service.dict;
using daan.domain;
using System.Data;
using System.Collections;
using ExtAspNet;
using System.Text;
using daan.service.login;
using hlis.service.common;
using daan.service.common;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using daan.web.code;
using daan.util.Common;
using daan.util.Web;

namespace daan.web.admin.dict
{
    public partial class DictRuleFormular : PageBase
    {
        #region Loading

        LoginService loginservice = new LoginService();
        DictRuleFormularService service = new DictRuleFormularService();
        private string strCompile;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExtAspNet.PageContext.RegisterStartupScript("(Ext.getCmp('" + ddldiagnosisname.ClientID + "')).listWidth=250;");  
                btnDel.OnClientClick = gdFormularName.GetNoSelectionAlertReference("请选择需要删除的规则公式！", "提示");
                btnDel.ConfirmText = "确实要删除该规则公式？";
                BindddlDictdiagnosis();
                BindLabDept();
                BindInitBase();
                BindDictLab();
            }
        }

        /// <summary>绑定分点
        /// 
        /// </summary>
        private void BindDictLab()
        {
            //查询区分点绑定
            DDLDictLabBinder(ddlDictlabSearch, true);
            ddlDictlabSearch.Items.Insert(0, new ExtAspNet.ListItem("----通用公式----", "0"));
            ddlDictlabSearch.SelectedValue = "0";

            //编辑区分点绑定
            DDLDictLabBinder(ddlDictlab, true);
            ddlDictlab.Items.Insert(0, new ExtAspNet.ListItem("----通用公式----", "0"));
            ddlDictlab.SelectedValue = "0";
        }

        /// <summary>绑定诊断名称
        /// 
        /// </summary>
        public void BindddlDictdiagnosis()
        {
            List<Dictdiagnosis> dictdiagnosisLst = new DictDiagnosisService().SelectDictdiagnosisLst().OrderByDescending(aa => aa.Diagnosisname).ToList<Dictdiagnosis>();     
            ddldiagnosisname.DataSource = dictdiagnosisLst;
            ddldiagnosisname.DataTextField = "Diagnosisname";
            ddldiagnosisname.DataValueField = "Dictdiagnosisid";
            ddldiagnosisname.DataBind();
        }

        /// <summary>绑定基础信息
        /// 
        /// </summary>
        public void BindInitBase()
        {
            //年龄单位
            DDLInitbasicBinder(ddlageunit, "AGEUNIT");

            //性别
            DDLInitbasicBinder(ddlsex, "REFSEX");
            //婚姻
            DDLInitbasicBinder(ddlmarry, "ISMARRY");
        }        

        #region >>>规则公式及分页
        private void BindGrid(string dictlibraryid)
        {
            //分页查询条件
            PageUtil pageUtil = new PageUtil(gdFormularName.PageIndex, gdFormularName.PageSize);
            Hashtable ht1 = new Hashtable();

            ht1.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
            ht1.Add("pageStart", pageUtil.GetPageStartNum());
            ht1.Add("pageEnd", pageUtil.GetPageEndNum());
            ht1.Add("Dictlabdeptid", dictlibraryid);
            ht1.Add("Dictlabid", ddlDictlabSearch.SelectedValue);
            //设置总项数
            gdFormularName.RecordCount = service.GetDictruleformularPageLstCount(ht1);
            List<Dictruleformular> list = service.GetDictruleformularPageLst(ht1);
            gdFormularName.DataSource = list;
            gdFormularName.DataBind();

            AddDictruleformular();
        }

        #endregion

        /// <summary>绑定物理组
        /// 
        /// </summary>
        public void BindLabDept()
        {
            DictlabdeptService service = new DictlabdeptService();
            ddlPhysicalGourp1.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            List<Dictlabdept> listlabdept = loginservice.GetLoginDictlabdeptList();
            foreach (Dictlabdept lab in listlabdept)
            {
                ddlgoupLibrary.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
                ddlPhysicalGourp1.Items.Add(new ExtAspNet.ListItem(lab.Labdeptname, lab.Dictlabdeptid.ToString()));
            }
        }

        //物理组
        protected void ddlgoupLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearchData();
        }

        #endregion

        

        #region 模糊查询
        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            gdFormularName.PageIndex = 0;
            AddDictruleformular();
            BindSearchData();
        }
        private void BindSearchData()
        {
            if (ddlgoupLibrary.SelectedValue!="-1")
            {
                BindGrid(ddlgoupLibrary.SelectedValue);
            }
            else
            {
                BindGrid(null);
            }           
        }        
        #endregion

        #region 显示详细信息
        protected void gdFormularName_RowClick(object sender, GridRowClickEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    object[] keys = gdFormularName.DataKeys[e.RowIndex];
                    Dictruleformular ruleformularitem = new Dictruleformular();
                    ruleformularitem = service.SelectDictruleformularByruleformularid(Convert.ToDouble(keys[0]));                   
                    hdRuleFormularID.Value = ruleformularitem.Dictruleformularid.ToString();
                    txtFormularName.Text = ruleformularitem.Formularname;//诊断公式名称
                    ddlPhysicalGourp1.SelectedValue = ruleformularitem.Dictlabdeptid.ToString();//物理组
                    BindPhysicalGourp();//显示增加项目
                    
                    ddldiagnosisname.SelectedValue = ruleformularitem.Dictdiagnosisid.ToString(); ;//诊断名称
                    ddlsex.SelectedValue = ruleformularitem.Sex;//性别
                    ddlageunit.SelectedValue = ruleformularitem.Ageunit;  //绑定年龄

                   
                    txtdisplayorder.Text = ruleformularitem.Displayorder.ToString();//次序
                    txtagelow.Text = ruleformularitem.Agelow.ToString();//起始年龄
                    txtagehight.Text = ruleformularitem.Agehight.ToString();//结束年龄
                    txtruleformulardescription.Text = ruleformularitem.Formulardesc;//诊断公式描述
                    txtruleformularcontent.Text = ruleformularitem.Formular;//诊断公式内容                        
                    txtdictrulecode.Text = ruleformularitem.Dictrulecode;//规则公式
                    ddlmarry.SelectedValue = ruleformularitem.Ismarry;//婚否
                    ddlDictlab.SelectedValue = ruleformularitem.Dictlabid.ToString();//分点
                }
                else
                {
                    AddDictruleformular();
                }
            }
            catch (Exception)
            {
                MessageBoxShow("数据显示出错！");
            }
        }
        #endregion

        #region 校验
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            bool b = isVerify();
            if (b)
            {
                MessageBoxShow("校验成功！");
            }
        }
        #endregion

        #region 清空
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtruleformularcontent.Text = null;
        }
        #endregion

        #region 新增
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtFormularName.Focus();
            SimpleFormEdit.Title = "当前状态-新增";
            AddDictruleformular();
        }

        private void AddDictruleformular()
        {
            gdFormularName.SelectedRowIndexArray = new int[] { };           
            txtFormularName.Text = null;
            ddlPhysicalGourp1.SelectedIndex = -1;

            BindddlDictdiagnosis();
            ddlDictlab.SelectedValue = "0";
            txtdisplayorder.Text = null;
            txtagelow.Text = null;
            txtagehight.Text = null;

            txtruleformulardescription.Text = null;
            txtruleformularcontent.Text = null;           
            txtdictrulecode.Text = null;
        }
        #endregion

        #region 保存
        //保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Check();
                if (s != null)
                {
                    MessageBoxShow(s);
                    return;
                }

                bool bCheck = this.isVerify();
                if (!bCheck)
                {
                    MessageBoxShow("公式校验不通过！");
                    return;
                }
                else
                {
                    MessageBoxShow("公式校验通过！");
                }  
                if (string.IsNullOrEmpty(hdRuleFormularID.Value.Trim()))
                {

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("Formular",txtruleformularcontent.Text);
                    ht1.Add("Formularname", txtFormularName.Text);                   
                    Dictruleformular d = service.SelectDictruleformularByrule(ht1);
                    if (d!=null)
                    {
                        MessageBoxShow("该计算公式已存在！");
                        return;
                    }
                }
                Dictruleformular ruleformularitem = new Dictruleformular();
                Dictruleformular ruleformularOld = new Dictruleformular();//日志 旧值
                List<Dictruleformular> dictruleformularList = new List<Dictruleformular>();
 
                if (gdFormularName.SelectedRowIndexArray.Length > 0)
                {
                    object[] keys = gdFormularName.DataKeys[gdFormularName.SelectedRowIndexArray[0]];
                    double? id = Convert.ToDouble(keys[0].ToString());
                    ruleformularitem = service.SelectDictruleformularByruleformularid(id);
                    ruleformularOld = ruleformularitem;
                    Hashtable ht = new Hashtable();
                    ht.Add("Dictrulecode", txtdictrulecode.Text.Trim());
                    ht.Add("Dictruleformularid", id);
                    ht.Add("Dictlabid", ddlDictlab.SelectedValue);
                    dictruleformularList = new DictRuleFormularService().SelectDictruleformularByCode(ht);                    
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("Dictrulecode", txtdictrulecode.Text.Trim());
                    ht.Add("Dictlabid", ddlDictlab.SelectedValue);
                    dictruleformularList = new DictRuleFormularService().SelectDictruleformularByCode(ht);
                }
                if (dictruleformularList.Count > 0)
                {
                    MessageBoxShow("已存在相同的规则代码");
                    return;
                }
                ruleformularitem.Dictlabid = Convert.ToDouble(ddlDictlab.SelectedValue);
                ruleformularitem.Formularname = txtFormularName.Text;
                ruleformularitem.Dictlabdeptid = Convert.ToDouble(ddlPhysicalGourp1.SelectedValue);
                ruleformularitem.Dictdiagnosisid = Convert.ToDouble(ddldiagnosisname.SelectedValue);
                ruleformularitem.Displayorder = Convert.ToDouble(txtdisplayorder.Text);
                ruleformularitem.Agelow = Convert.ToDouble(txtagelow.Text);
                ruleformularitem.Agehight = Convert.ToDouble(txtagehight.Text);
                ruleformularitem.Formulardesc = txtruleformulardescription.Text;
                ruleformularitem.Formular = txtruleformularcontent.Text;                
                ruleformularitem.Sourcecode = strCompile;

                ruleformularitem.Ageunit = ddlageunit.SelectedValue;
                //计算小时
                ruleformularitem.Caculatedagelow = calcAge(ruleformularitem.Agelow.Value);
                ruleformularitem.Caculatedagehigh = calcAge(ruleformularitem.Agehight.Value);

                ruleformularitem.Sex = ddlsex.SelectedValue;//性别
                ruleformularitem.Ismarry = ddlmarry.SelectedValue;//婚姻状况
                ruleformularitem.Dictrulecode = txtdictrulecode.Text;//规则代码
                if (ViewState["ProductIDs"] != null)
                    ruleformularitem.Formulartests = ViewState["ProductIDs"].ToString();
                            
                double? f = service.SaveFuleFormular(ruleformularitem, ruleformularOld);
                if (f > 0)
                {
                    ruleformularitem.Dictruleformularid = f;  
                    MessageBoxShow("新增成功！");
                    ttbSearch_Trigger2Click(null,null);
                }
                else if (f == 0)
                {                    
                    MessageBoxShow("修改成功！");
                    ttbSearch_Trigger2Click(null, null);
                }
                else
                {
                    MessageBoxShow("操作失败！");
                }

            }
            catch (Exception)
            {

                MessageBoxShow("保存失败！");
            }
        }
        #endregion

        #region 验证
        private string Check()
        {
            string str = null;
            if (string.IsNullOrEmpty(txtFormularName.Text.Trim()))
            {

                str = "请填写诊断公式名称！";
                return str;
            }
            if (string.IsNullOrEmpty(txtdictrulecode.Text.Trim()))
            {

                str = "请填写规则代码！";
                return str;
            }
            if (ddlPhysicalGourp1.SelectedIndex == 0)
            {
                str = "请选择物理类型！";
                return str;
            }
            //年龄低值
            try
            {

                int.Parse(txtagelow.Text.Trim());
            }
            catch
            {
                str = "请填写年龄低值！";
                return str;
            }
            //年龄高值
            try
            {

                int.Parse(txtagehight.Text.Trim());
            }
            catch
            {
                str = "请填写年龄高值！";
                return str;
            }
            //次序
            try
            {

                int.Parse(txtdisplayorder.Text.Trim());
            }
            catch
            {
                str = "次序请填写整数！";
                return str;
            }


            if (string.IsNullOrEmpty(txtruleformularcontent.Text.Trim()))
            {
                str = "请填写诊断公式内容！";
                return str;
            }

            return str;
        }
        #endregion

        #region 校验
        private bool isVerify()
        {
            string strErr = null;
            string cFormular = this.txtruleformularcontent.Text.Trim();
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            for (int i = 0; i < cFormular.Length; i++)
            {
                if (cFormular[i].ToString() == "[")
                    count1++;
                if (cFormular[i].ToString() == "]")
                    count2++;
                if (cFormular[i].ToString() == "(")
                    count3++;
                if (cFormular[i].ToString() == ")")
                    count4++;
            }
            if (count1 != count2 || count1 == 0)
            {
                MessageBoxShow("提示：变量请用[]括起来或[]个数不匹配。");
                return false;
            }
            if (count3 != count4)
            {
                MessageBoxShow("提示：变量请用()括起来或()个数不匹配。");
                return false;
            }

            //储存项目位置
            int length = 0;
            ArrayList listLeft = new ArrayList();
            for (int i = 0; i < (cFormular.Length - length); i++)
            {
                int index = cFormular.IndexOf("[", (length));
                length = index + 1;
                if (index >= 0 && length > 0)
                {
                    listLeft.Add(index.ToString());
                }
                else break;
            }

            int length2 = 0;
            ArrayList listRight = new ArrayList();
            for (int i = 0; i < (cFormular.Length - length2); i++)
            {
                int index = cFormular.IndexOf("]", (length2));
                length2 = index + 1;
                if (index >= 0 && length2 > 0)
                {
                    listRight.Add(index.ToString());
                }
                else break;
            }

            //存储项目
            ArrayList paramItem = new ArrayList();
            for (int i = 0; i < count1; i++)
            {
                paramItem.Add(cFormular.Substring(Convert.ToInt32(listLeft[i]), Convert.ToInt32(listRight[i]) - Convert.ToInt32(listLeft[i]) + 1));
            }

            //将项目替换成变量
            string strReplace = cFormular;
            for (int i = 0; i < paramItem.Count; i++)
            {
                //由于生成的动态编译的语法的参数类型为double型，所以若项目对应运算的常量是字符串时，要进行转换
                int pos = 0;
                if (i + 1 == paramItem.Count)
                    pos = cFormular.IndexOf("\"", Convert.ToInt32(listRight[i]), cFormular.Length - Convert.ToInt32(listRight[i]) - 1);
                else
                    pos = cFormular.IndexOf("\"", Convert.ToInt32(listRight[i]), Convert.ToInt32(listLeft[i + 1]) - Convert.ToInt32(listRight[i]) - 1);

                if (pos >= 0)            
                    strReplace = strReplace.Replace(paramItem[i].ToString(), "a" + i.ToString() + ".ToString()");            
                else
                    //strReplace = strReplace.Replace(paramItem[i].ToString(),"Convert.ToDouble(a"+ i.ToString()+")");
                    strReplace = strReplace.Replace(paramItem[i].ToString(), "a" + i.ToString());
            }

            //开始调用动态编译类
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            //获取描述方法   
            ArrayList arrList;
            GetProductDesc(out arrList);

            CompilerResults cr;
            //规则公式   
            strCompile = Efficacy(paramItem.Count, strReplace, arrList);
            cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, strCompile);

            if (cr.Errors.HasErrors)
            {
                foreach (CompilerError err in cr.Errors)
                {

                    MessageBoxShow("公式有误，请仔细检查。");
                }
                //编译语法  是否查看编译器动态生成的校验语法?
                strErr = strCompile;
                return false;
            }
            return true;
        }
        #endregion

        #region 匹配公式对应的项目名称
        protected void GetProductDesc(out ArrayList arrType)
        {

            string Formular = this.txtruleformularcontent.Text;
            arrType = new ArrayList();
            if (string.IsNullOrEmpty(Formular))
            {
                this.txtruleformulardescription.Text = string.Empty;
                return;
            }

            string str = string.Empty;
            string cPattern = @"\[(\d+)\]";
            Regex regex = new Regex(cPattern);

            StringBuilder sb = new StringBuilder();
            foreach (Match ma in regex.Matches(Formular))
            {
                if (ma.Success)
                {
                    string id = ma.Groups[1].Value;
                    string testname = string.Empty;
                    //项目
                    List<Dicttestitem> dicttestitemall = loginservice.GetLoginDicttestitemList();
                    List<Dicttestitem> tempList = (from Dicttestitem in dicttestitemall where Dicttestitem.Dicttestitemid == Convert.ToDouble(id) select Dicttestitem).ToList<Dicttestitem>();


                    if (tempList != null)
                    {
                        Dicttestitem testitem = tempList[0];
                        testname = testitem.Testname;
                        Formular = Formular.Replace(ma.Groups[0].Value, "[" + testname + "]");
                        sb.AppendFormat("{0},", testitem.Dicttestitemid);
                        //arrType.Add(testitem.Testtype);
                        //udp by lee
                        arrType.Add(testitem.Resulttype);
                    }
                }
            }
            ViewState["ProductIDs"] = sb.ToString();//获取选择的项目编号
            this.txtruleformulardescription.Text = Formular;

        }
        #endregion

        #region 规则公式动态编译类
        static string Efficacy(int count, string strReplace, ArrayList arrType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public object OutPut(");
            for (int i = 0; i < count; i++)
            {
                if (arrType != null)
                {
                    if (Convert.ToChar(arrType[i]) == '0')
                    {
                        sb.Append("double ");
                    }
                    else
                    {
                        sb.Append("string ");
                    }
                }
                else
                {
                    sb.Append("double ");
                }
                sb.Append("a" + i.ToString());
                if (i != count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            sb.Append("             if(");
            sb.Append(strReplace);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("     return true;");
            sb.Append(Environment.NewLine);
            sb.Append("   else");
            sb.Append(Environment.NewLine);
            sb.Append("     return false;");
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }
        #endregion

        #region 编辑区物理组
        protected void ddlPhysicalGourp1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPhysicalGourp();
        }

        private void BindPhysicalGourp()
        {
            List<Dicttestitem> dicttestitemall = loginservice.GetLoginDicttestitemList();
            double labdeptid = Convert.ToDouble(ddlPhysicalGourp1.SelectedValue);
            List<Dicttestitem> testitems = (from Dicttestitem in dicttestitemall where Dicttestitem.Dictlabdeptid == labdeptid && Dicttestitem.Active=="1" select Dicttestitem).ToList<Dicttestitem>();
            ddlTestitem.Items.Clear();
            //ddlTestitem.Items.Add(new System.Web.UI.WebControls.ListItem("请选择", "-1"));
            //foreach (Dicttestitem testitem in testitems)
            //{
            //    ddlTestitem.Items.Add(new System.Web.UI.WebControls.ListItem(testitem.Testname, testitem.Dicttestitemid.ToString()));
            //}

            //更改下拉框增加空项，处理当只有一个项目时选择不了的问题 fhp
            ddlTestitem.Items.Add(new ExtAspNet.ListItem("请选择", "-1"));
            foreach (Dicttestitem testitem in testitems)
            {
                ddlTestitem.Items.Add(new ExtAspNet.ListItem(testitem.Testname, testitem.Dicttestitemid.ToString()));
            }
            //ddlTestitem.DataSource = testitems;
            ddlTestitem.DataTextField = "Testname";
            ddlTestitem.DataValueField = "Dicttestitemid";
            ddlTestitem.DataBind();
        }
        #endregion

        #region 导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdFormularName.Rows.Count == 0)
                {
                    MessageBoxShow("导出没有数据！");
                    return;
                }

                Hashtable ht = new Hashtable();
               
                if (ddlgoupLibrary.SelectedValue=="-1")
                {
                    ht.Add("Dictlabdeptid", null);
                }
                else
                {
                    ht.Add("Dictlabdeptid", ddlgoupLibrary.SelectedValue);
                }
                ht.Add("strKey", TextUtility.ReplaceText(ttbSearch.Text.Trim()) == "" ? null : TextUtility.ReplaceText(ttbSearch.Text.Trim()));
                List<Dictruleformular> ruleformularlist =loginservice.GetLoginDictruleformularresultList();
             
                String sheetname = DateTime.Now.ToString("yyyy-MM-dd");
                String filename = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                SortedList sortlist = new SortedList(new MySort());
                sortlist.Add("Dictrulecode", "规则代码");
                sortlist.Add("Formularname", "公式名称");                
                ExcelOperation<Dictruleformular>.ExportListToExcel(ruleformularlist, sortlist, filename, sheetname);
               
            }
            catch (Exception)
            {
                MessageBoxShow("导出出错，请联系管理员！");
            }
        }
        #endregion

        #region 删除
        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {               
                Dictruleformular ruleformularitem = new Dictruleformular();
                if (gdFormularName.SelectedRowIndexArray.Length > 0)
                {
                    object[] keys = gdFormularName.DataKeys[gdFormularName.SelectedRowIndexArray[0]];
                    double? id = Convert.ToDouble(keys[0].ToString());
                    ruleformularitem = service.SelectDictruleformularByruleformularid(id);                    
                }
                int result = service.DelDictRuleFormularByID(ruleformularitem);
                if (result > 0)
                {           
                    MessageBoxShow("删除成功！");
                    ttbSearch_Trigger2Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }
        #endregion

        #region 分页
        protected void gdFormularName_PageIndexChange(object sender, GridPageEventArgs e)
        {
            gdFormularName.PageIndex = e.NewPageIndex;
            BindSearchData();
        }
        #endregion

        #region 计算年龄
        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="head"></param>
        private double? calcAge(double age)
        {
            //换算成小时后的年龄
            double? agehour = 0;//小时
            //岁月天
            string ageunit = ddlageunit.SelectedItem.Text;//年龄单位
            if (ageunit == "岁")
            {
                agehour = Convert.ToDouble(age * 12 * 30 * 24);
            }
            if (ageunit == "月")
            {
                agehour = Convert.ToDouble(age * 30 * 24);
            }
            if (ageunit == "日")
            {
                agehour = Convert.ToDouble(age * 24);
            }
            if (ageunit == "小时")
            {
                agehour = age;
            }
            return agehour;
        }
        #endregion

        protected void ddlTestitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTestitem.SelectedValue.ToString() == "-1")
            {
                return;
            }
            PageContext.RegisterStartupScript("GetCup('"+ddlTestitem.SelectedValue+"',1,1)");

        }
    }
}