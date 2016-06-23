using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.service.common
{
    public static class ParamStatus
    {

        // 各业务表状态常量类，要求每个表单独CLASS，状态名称要求英文表达状态意思，状态值对应INITBASIC表BASICVALUE


        /// <summary>
        /// Orderbarcode表状态
        /// </summary>
        public enum OrderbarcodeStatus
        {
            //[EnumDescription("已注册")]
            [EnumDescription("已登记")]
            Registed = 5,

            [EnumDescription("已采集")]
            Collected = 10,

            [EnumDescription("已接收")]
            Received = 15,

        }

        /// <summary>
        /// Orders表状态
        /// </summary>
        public enum OrdersStatus
        {    
            [EnumDescription("已登记")]
            Register = 5,

            [EnumDescription("条码已打印")]
            BarCodePrint = 10,

            [EnumDescription("待总检")]
            WaitCheck = 15,

            [EnumDescription("初步总检")]
            FirstCheck = 20,

            [EnumDescription("完成总检")]
            FinishCheck = 25,

            [EnumDescription("报告已打印")]
            FinishPrint = 30,
        }

        /// <summary>
        /// 报告财务审核状态
        /// </summary>
        public enum FinanceAuditStatus
        {
            [EnumDescription("已审核")]
            Audit=1,
            [EnumDescription("未审核")]
            UnAudit=0,
        }

        /// <summary>
        /// Billhead表状态
        /// </summary>
        public enum BillheadStatus
        {
            [EnumDescription("预出账")]
            PrepareOut = 0,

            [EnumDescription("已接收")]
            Receive = 1,

            [EnumDescription("已作废")]
            Invalid = 9,

            [EnumDescription("已退款")]
            Refundment = 5,
        }
        /// <summary>
        /// Billdetail表状态
        /// </summary>
        public enum BilldetailStatus
        {
            [EnumDescription("正常")]
            Normal = 1,
            [EnumDescription("降价")]
            reduce = 2,
        }

        /// <summary>
        /// ORDERLABDEPTRESULT科室小结表状态
        /// </summary>
        public enum Orderlabdepstatus
        {
            [EnumDescription("结果待查")]
            WaitCheck= 5,
            [EnumDescription("已小结")]
            Summary = 10,
            [EnumDescription("已审核")]
            Audited= 15,
            
        }
        /// <summary>
        /// REPORTTYPE状态
        /// </summary>
        public enum ReportTypeStatus
        {
            [EnumDescription("常规报告")]
            Normal = 5,
            [EnumDescription("HPV报告")]
            HPV = 10,
            [EnumDescription("TM15报告")]
            TM15 = 15,
            [EnumDescription("财务账单")]
            Financial = 20,
            [EnumDescription("体检指引单")]
            ChcekOrder = 25,
            [EnumDescription("收费收据")]
            MoneyOrder = 30,
            [EnumDescription("团检报告")]
            GroupOrder = 35,
            [EnumDescription("C14报告")]
            C14=40,
            [EnumDescription("C13报告")]
            C13 = 45,
        }

        /// <summary>
        /// 个人客户编号
        /// </summary>
        public enum PersonalCustomerID
        { 
             [EnumDescription("个人客户编号")]
            SingleCustomerCode = 999999,
        }

        /// <summary>
        /// 疾病类型[OrderDiagnosis表diagnosistype字段]
        /// </summary>
        public enum OrderDiagnosisType
        {
            [EnumDescription("重要问题")]
            ImportantIssue = 1,

            [EnumDescription("主要问题")]
            MajorProblem = 2,

            [EnumDescription("其它问题")]
             OtherProblems = 3,
        }

        /// <summary>
        /// 科室类型[initbasic表BASICTYPE=LABDEPTTYPE字段]
        /// </summary>
        public enum LabdeptType
        {
            [EnumDescription("检查科室")]
            CheckTheDepartment = 1,

            [EnumDescription("检验科室")]
            InspectionDepartment = 2,

            [EnumDescription("功能科室")]
            FunctionDepartment = 3,
        }


        /// <summary>
        /// LIS订单状态
        /// </summary>
        //public enum LisOrderState
        //{
        //    [EnumDescription("迟发")]
        //    //DELAY = "DELAY",

        //    [EnumDescription("退回待分发")]
        //    //BACKTOOUTPOST = "BACKTOOUTPOST",

        //    [EnumDescription("退单")]
        //    //CANCEL = "CANCEL",

        //    [EnumDescription("修改病人信息")]
        //    //MODIFYSPECINFO = "MODIFYSPECINFO",

        //    [EnumDescription("取消审核")]
        //    //CANCELAUDITPOST = "CANCELAUDITPOST",
        //}
    }
}
