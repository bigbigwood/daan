using System;

//���������ļ�
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace TimeHelper.Logging
{
	/// <summary>
	/// ��־ʵ��
	///	ʹ��log4net��Ϊ��־ʵ��
	/// </summary>
	public class Logger: ILog 
	{

		private log4net.ILog log;

	    /// <summary>
		/// ������
		/// </summary>
		/// <param name="name">��־��������</param>
		public Logger(String name) 
		{
			log = log4net.LogManager.GetLogger(name);
		}

		#region ILog ��Ա

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		public void Debug(object message) 
		{
			Console.WriteLine(message);
			log.Debug(message);
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣��</param>
		public void Debug(object message, Exception t) 
		{   
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Debug(message, t);
		}

		/// <summary>
		/// ��Ϣ
		/// </summary>
		/// <param name="message">������Ϣ</param>
		public void Info(object message) 
		{
			Console.WriteLine(message);
			log.Info(message);
		}

		/// <summary>
		/// ��Ϣ
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣��</param>
		public void Info(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Info(message, t);
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		public void Warn(object message) 
		{
			Console.WriteLine(message);
			log.Warn(message);
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣��</param>
		public void Warn(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Warn(message, t);
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		public void Error(object message) 
		{
			Console.WriteLine(message);
			log.Error(message);
		}

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣��</param>
		public void Error(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Error(message, t);
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="message">������Ϣ</param>
		public void Fatal(object message) 
		{
			Console.WriteLine(message);
			log.Fatal(message);
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣��</param>
		public void Fatal(object message, Exception t) 
		{
			Console.WriteLine("message={0}, ToString()={1}", message, t);
			log.Fatal(message, t);
		}

		#endregion
	}
}
