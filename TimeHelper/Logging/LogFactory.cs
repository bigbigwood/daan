using System;

namespace TimeHelper.Logging
{
	/// <summary>
	/// ��־����
	/// </summary>
	public class LogFactory
	{
		private LogFactory(){}

		/// <summary>
		/// �����־����ʵ��
		/// </summary>
		/// <param name="t">ʵ��Type</param>
		/// <returns>��־����ʵ��</returns>
		public static ILog GetLogger(Type t) 
		{
			return new Logger(t.FullName);
		}

		/// <summary>
		/// �����־����ʵ��
		/// </summary>
		/// <param name="name">ʵ������</param>
		/// <returns>��־����ʵ��</returns>
		public static ILog GetLogger(string name) 
		{
			return new Logger(name);
		}
	}
}
