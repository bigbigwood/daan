using System;

namespace TimeHelper.Logging
{
	/// <summary>
	/// 日志接口
	/// </summary>
	
	public interface ILog
	{
		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="message">错误信息</param>
		void Debug(object message);

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常</param>
		void Debug(object message, Exception t);

		/// <summary>
		/// 信息
		/// </summary>
		/// <param name="message">错误信息</param>
		void Info(object message);

		/// <summary>
		/// 信息
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常</param>
		void Info(object message, Exception t);


		/// <summary>
		/// 警告
		/// </summary>
		/// <param name="message">错误信息</param>
		void Warn(object message);

		/// <summary>
		/// 警告
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常</param>
		void Warn(object message, Exception t);


		/// <summary>
		/// 错误
		/// </summary>
		/// <param name="message">错误信息</param>
		void Error(object message);

		/// <summary>
		/// 错误
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常</param>
		void Error(object message, Exception t);


		/// <summary>
		/// 致命错误
		/// </summary>
		/// <param name="message">错误信息</param>
		void Fatal(object message);

		/// <summary>
		/// 致命错误
		/// </summary>
		/// <param name="message">错误信息</param>
		/// <param name="t">异常</param>
		void Fatal(object message, Exception t);
	}
}
