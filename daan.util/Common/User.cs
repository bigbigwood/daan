/**********************************************
 * �����ã�   �û�ʵ����
 * �����ˣ�   daan
 * ����ʱ�䣺 2012-03-28 
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

namespace daan.util.Common
{
    /// <summary>
    /// �û�ʵ���࣬�Զ��崰�������֤ʱ����ʹ�á�
    /// </summary>
    public sealed class UserUtil
    {
        /// <summary>
        /// �û���¼����
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="roles">�û���ɫ</param>
        /// <param name="isPersistent">�Ƿ�־�cookie</param>
        public static void Login(string username, string roles, bool isPersistent)
        {
            DateTime dt = isPersistent ? DateTime.Now.AddMinutes(99999) : DateTime.Now.AddMinutes(60);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                                                1, // Ʊ�ݰ汾��
                                                                                username, // Ʊ�ݳ�����
                                                                                DateTime.Now, //����Ʊ�ݵ�ʱ��
                                                                                dt, // ʧЧʱ��
                                                                                isPersistent, // ��Ҫ�û��� cookie 
                                                                                roles, // �û����ݣ�������ʵ�����û��Ľ�ɫ
                                                                                FormsAuthentication.FormsCookiePath);//cookie��Ч·��

            //ʹ�û�����machine key����cookie��Ϊ�˰�ȫ����
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash); //����֮���cookie

            //��cookie��ʧЧʱ������Ϊ��Ʊ��tikets��ʧЧʱ��һ�� 
            HttpCookie u_cookie = new HttpCookie("username", username);
            if (ticket.IsPersistent)
            {
                u_cookie.Expires = ticket.Expiration;
                cookie.Expires = ticket.Expiration;
            }

            //���cookie��ҳ��������Ӧ��
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Response.Cookies.Add(u_cookie);
        }

        /// <summary>
        /// �û��˳�����
        /// </summary>
        public static void Logout()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null)
            {
                cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            cookie.Expires = DateTime.Now.AddYears(-10);

            HttpCookie u_cookie = new HttpCookie("username", string.Empty);
            u_cookie.Expires = DateTime.Now.AddYears(-10);
            HttpContext.Current.Response.Cookies.Add(u_cookie);
        }
    }
}
