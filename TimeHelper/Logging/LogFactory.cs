using System;

namespace TimeHelper.Logging
{
	/// <summary>
	/// 日志工厂
	/// </summary>
	public class LogFactory
	{
		private LogFactory(){}

		/// <summary>
		/// 获得日志器的实例
		/// </summary>
		/// <param name="t">实例Type</param>
		/// <returns>日志器的实例</returns>
		public static ILog GetLogger(Type t) 
		{
			return new Logger(t.FullName);
		}

		/// <summary>
		/// 获得日志器的实例
		/// </summary>
		/// <param name="name">实例名称</param>
		/// <returns>日志器的实例</returns>
		public static ILog GetLogger(string name) 
		{
			return new Logger(name);
		}
	}
}
