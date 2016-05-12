﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.ui.PrintingApplication
{
    public class ConstString
    {
        public const String ALL = "全部";
        public const String OrdersStatus_Register = "已登记";
        public const String OrdersStatus_BarCodePrint = "条码已打印";
        public const String OrdersStatus_WaitCheck = "待总检";
        public const String OrdersStatus_FirstCheck = "初步总检";
        public const String OrdersStatus_FinishCheck = "完成总检";
        public const String OrdersStatus_FinishPrint = "报告已打印";

        public const String ReportStatus_Normal = "正常";
        public const String ReportStatus_Delay = "迟发";
        public const String ReportStatus_Refund = "退单";

        public const String DateFormat = "yyyy-MM-dd";
    }
}