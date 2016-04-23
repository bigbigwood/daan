using System;
using System.Collections.Generic;
using System.Linq;

namespace daan.domain
{
    public static class ErrorCode
    {
        #region >>>> 登录验证
        /// <summary>
        /// 找不到WebService站点
        /// </summary>
        public const string Login_1001 = "MSG0001";

        /// <summary>
        /// 远程调用WebService错误
        /// </summary>
        public const string Login_1002 = "MSG0002";

        /// <summary>
        /// 用户名或者密码不能为空
        /// </summary>
        public const string Login_1003 = "MSG0003";

        /// <summary>
        /// 没有权限或密码错误 可能原因：密码不需要加密,用户名或密码不正确
        /// </summary>
        public const string Login_1004 = "MSG0004";

        /// <summary>
        /// 未登录或者登录过期失效或SID不正确
        /// </summary>
        public const string Login_1005 = "MSG0005";

        /// <summary>
        /// 预留选项
        /// </summary>
        public const string Login_1006 = "MSG0006";

        /// <summary>
        /// 预留选项
        /// </summary>
        public const string Login_1007 = "MSG0007";
        #endregion

        #region 体检系统与新易感基因系统对接

        #region >>>> 接收数据
        /// <summary>
        /// 登录失败或超时或SID不正确***********【作废】
        /// </summary>
        //public const string Rec_1001 = "MSG0008";

        /// <summary>
        /// XML格式不正确
        /// </summary>
        public const string Rec_1002 = "MSG0008";

        /// <summary>
        /// XML数据节点异常【没数据节点，或者数据节点数量不对】
        /// </summary>
        public const string Rec_1003 = "MSG0009";

        /// <summary>
        /// XML数据节点数量不对***********【作废】
        /// </summary>
        //public const string Rec_1004 = "MSG0011";

        /// <summary>
        /// 有必填项未填
        /// </summary>
        public const string Rec_1005 = "MSG0010";

        /// <summary>
        /// 生日和年龄必须填写一项或者两项均填写错误
        /// </summary>
        public const string Rec_1006 = "MSG0011";

        /// <summary>
        /// 性别必须为M或者F或者U***********【作废】
        /// </summary>
        //public const string Rec_1007 = "MSG0014";

        /// <summary>
        /// 套餐代码无匹配项，性别不匹配或者无该套餐
        /// </summary>
        public const string Rec_1008 = "MSG0012";

        /// <summary>
        /// 存在多个此套餐代码的套餐
        /// </summary>
        public const string Rec_1009 = "MSG0013";

        /// <summary>
        /// 条码号必须为12位数字
        /// </summary>
        public const string Rec_1010 = "MSG0014";

        /// <summary>
        /// 条码号必须以00结尾
        /// </summary>
        public const string Rec_1011 = "MSG0015";

        /// <summary>
        /// 条码号已在系统中生成
        /// </summary>
        public const string Rec_1012 = "MSG0016";

        /// <summary>
        /// 出生日期或采样日期不能转换成时间格式
        /// </summary>
        public const string Rec_1013 = "MSG0017";

        /// <summary>
        /// 客户代码在体检系统中未维护
        /// </summary>
        public const string Rec_1014 = "MSG0018";

        /// <summary>
        /// 性别与套餐中项目不合
        /// </summary>
        public const string Rec_1015 = "MSG0019";

        /// <summary>
        /// 没有找到套餐下组合
        /// </summary>
        public const string Rec_1016 = "MSG0020";

        /// <summary>
        ///添加套餐失败。可能原因【所属科室未维护、标本类型未维护、分管原则相同，但[所属科室]不同、分管原则相同，但[标本类型]不同】 
        /// </summary>
        public const string Rec_1017 = "MSG0021";

        ///// <summary>
        ///// 标本类型 未维护
        ///// </summary>
        //public const string Rec_1018 = "MSG0022";

        ///// <summary>
        ///// 分管原则相同，但[所属科室]不同
        ///// </summary>
        //public const string Rec_1019 = "MSG0023";

        ///// <summary>
        ///// 分管原则相同，但[标本类型]不同
        ///// </summary>
        //public const string Rec_1020 = "MSG0024";

        /// <summary>
        /// 其他原因   
        /// </summary>
        public const string Rec_1018 = "MSG0022";
        #endregion

        #endregion

        #region 对接医护平台（微信端）
        #region 1 >>>> 报告状态查询
        /// <summary>
        /// 条码号|姓名|手机号为空
        /// </summary>
        public const string Query_0001 = "MSG0008";

        /// <summary>
        /// 条码号在体检系统中不存在
        /// </summary>
        public const string Query_0002 = "MSG0009";

        /// <summary>
        /// 姓名不匹配
        /// </summary>
        public const string Query_0003 = "MSG0010";

        /// <summary>
        /// 手机号码不匹配
        /// </summary>
        public const string Query_0004 = "MSG0011";

        /// <summary>
        /// 其他原因
        /// </summary>
        public const string Query_0005 = "MSG0012";
        #endregion

        #region 2 >>>> 查询并下载报告
        /// <summary>
        /// 条码号|姓名|手机号为空
        /// </summary>
        public const string Down_0001 = "MSG0014";

        /// <summary>
        /// 条码号在体检系统中不存在
        /// </summary>
        public const string Down_0002 = "MSG0015";

        /// <summary>
        /// 姓名不匹配
        /// </summary>
        public const string Down_0003 = "MSG0016";

        /// <summary>
        /// 手机号码不匹配
        /// </summary>
        public const string Down_0004 = "MSG0017";

        /// <summary>
        /// 订单状态完成总检后才能下载报告单
        /// </summary>
        public const string Down_0005 = "MSG0018";

        /// <summary>
        /// 报告文件不存在
        /// </summary>
        public const string Down_0006 = "MSG0019";

        /// <summary>
        /// 其他原因
        /// </summary>
        public const string Down_0007 = "MSG0020";
        #endregion

        #region 3 >>>> 易感基因系统向体检系统上传订单
        /// <summary>
        /// XML格式错误
        /// </summary>
        public const string Up_0001 = "MSG0027";
        /// <summary>
        /// XML数据节点异常【没数据节点，或者数据节点数量不对】
        /// </summary>
        public const string Up_0002 = "MSG0028";
        /// <summary>
        /// 有必填项为空
        /// </summary>
        public const string Up_0003 = "MSG0029";
        /// <summary>
        /// 生日和年龄必填一项或者两者均填写错误
        /// </summary>
        public const string Up_0004 = "MSG0030";
        /// <summary>
        /// 套餐代码无匹配项，性别不匹配或者无该套餐
        /// </summary>
        public const string Up_0005 = "MSG0031";
        /// <summary>
        /// 存在多个此套餐代码的套餐
        /// </summary>
        public const string Up_0006 = "MSG0032";
        /// <summary>
        /// 条码号必须为12位数字
        /// </summary>
        public const string Up_0007 = "MSG0033";
        /// <summary>
        /// 条码号必须以00结尾
        /// </summary>
        public const string Up_0008 = "MSG0034";
        /// <summary>
        /// 条码号已在体检系统中生成
        /// </summary>
        public const string Up_0009 = "MSG0035";
        /// <summary>
        /// 出生日期或采样日期填写了但是转换成日期格式失败
        /// </summary>
        public const string Up_0010 = "MSG0036";
        /// <summary>
        /// 客户代码在体检系统中未维护或维护存在多个相同客户代码的客户
        /// </summary>
        public const string Up_0011 = "MSG0037";
        /// <summary>
        /// 性别与套餐中项目不合
        /// </summary>
        public const string Up_0012 = "MSG0038";
        /// <summary>
        /// 未找到套餐下组合
        /// </summary>
        public const string Up_0013 = "MSG0039";
        /// <summary>
        /// 添加套餐失败。可能原因【分管原则、所属科室、标本类型未维护；分管原则相同，但[标本类型]不同】
        /// </summary>
        public const string Up_0014 = "MSG0040";
        /// <summary>
        /// 其他原因
        /// </summary>
        public const string Up_0015 = "MSG0041";
        #endregion

        #region 4 >>>> 同步体检系统单位信息到易感基因系统和大众健康平台
        /// <summary>
        /// 对接系统标志有误
        /// </summary>
        public const string Sync_0001 = "MSG0050";
        /// <summary>
        /// 没有未同步的单位信息
        /// </summary>
        public const string Sync_0002 = "MSG0051";
        /// <summary>
        /// 转化XML出错
        /// </summary>
        public const string Sync_0003 = "MSG0052";
        /// <summary>
        /// 未查询到单位信息
        /// </summary>
        public const string Sync_0004 = "MSG0053";
        /// <summary>
        /// 预留信息
        /// </summary>
        public const string Sync_0005 = "MSG0054";
        /// <summary>
        /// 其他原因
        /// </summary>
        public const string Sync_0006 = "MSG0055";
        #endregion

        #region 5 >>>> 大众健康平台修改受检者信息/删除订单
        /// <summary>
        /// 条码号为空
        /// </summary>
        public const string Update_0001 = "MSG0060";
        /// <summary>
        /// 条码号格式不正确
        /// </summary>
        public const string Update_0002 = "MSG0061";
        /// <summary>
        /// 条码号在体检系统不存在
        /// </summary>
        public const string Update_0003 = "MSG0062";
        /// <summary>
        /// 条码不是来自大众平台，无法修改	
        /// </summary>
        public const string Update_0004 = "MSG0063";
        /// <summary>
        /// 检测结果已出，无法修改
        /// </summary>
        public const string Update_0005 = "MSG0064";
        /// <summary>
        /// 其他原因
        /// </summary>
        public const string Update_0007 = "MSG0065";
        #endregion

        #endregion
    }
}
