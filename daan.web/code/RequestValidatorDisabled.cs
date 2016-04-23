using System;
using System.Web;
using System.Web.Util;
/// <summary>
/// 因系统自身会检查是否有敏感字符输入
/// 此类配合webconfig中<httpRuntime requestValidationType="RequestValidatorDisabled" />避免强制验证
/// </summary>
class RequestValidatorDisabled : System.Web.Util.RequestValidator 
{ 
    protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex) 
    { 
        validationFailureIndex = -1; 
        return true; 
    } 
}