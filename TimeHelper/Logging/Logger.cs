using System;

//导入配置文件
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace TimeHelper.Logging
{
	/// <summary>
	/// 日志实现
	///	使用log4net做为日志实现
	/// </summary>
	public class Logger: ILog 
	{

		private log4net.ILog log;

	    /// <summary>
		/// 构造器
		/// </summary>
		/// <param name="name">日志器的名称</param>
		public Logger(String name) 
		{
			log = log4net.LogManager.GetLogger(name);
		}

		#region ILog 成员

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="message">调试信息</param>
		public void Debug(object message) 
		{
			Console.WriteLine(message);
			log.Debug(message);
		}

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常类</param>
		public void Debug(object message, Exception t) 
		{   
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Debug(message, t);
		}

		/// <summary>
		/// 信息
		/// </summary>
		/// <param name="message">错误信息</param>
		public void Info(object message) 
		{
			Console.WriteLine(message);
			log.Info(message);
		}

		/// <summary>
		/// 信息
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常类</param>
		public void Info(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Info(message, t);
		}

		/// <summary>
		/// 警告
		/// </summary>
		/// <param name="message">警告信息</param>
		public void Warn(object message) 
		{
			Console.WriteLine(message);
			log.Warn(message);
		}

		/// <summary>
		/// 警告
		/// </summary>
		/// <param name="message">警告信息</param>
		/// <param name="t">异常类</param>
		public void Warn(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Warn(message, t);
		}

		/// <summary>
		/// 错误
		/// </summary>
		/// <param name="message">错误信息</param>
		public void Error(object message) 
		{
			Console.WriteLine(message);
			log.Error(message);
		}

		/// <summary>
		/// 错误
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常类</param>
		public void Error(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Error(message, t);
		}

		/// <summary>
		/// 致命错误
		/// </summary>
		/// <param name="message">错误信息</param>
		public void Fatal(object message) 
		{
			Console.WriteLine(message);
			log.Fatal(message);
		}

		/// <summary>
		/// 致命错误
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常类</param>
		public void Fatal(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Fatal(message, t);
		}

		#endregion
	}
}
