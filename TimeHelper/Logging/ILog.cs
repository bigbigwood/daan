using System;

namespace TimeHelper.Logging
{
	/// <summary>
	/// ��־�ӿ�
	/// </summary>
	
	public interface ILog
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		void Debug(object message);

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣</param>
		void Debug(object message, Exception t);

		/// <summary>
		/// ��Ϣ
		/// </summary>
		/// <param name="message">������Ϣ</param>
		void Info(object message);

		/// <summary>
		/// ��Ϣ
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣</param>
		void Info(object message, Exception t);


		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		void Warn(object message);

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣</param>
		void Warn(object message, Exception t);


		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		void Error(object message);

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣</param>
		void Error(object message, Exception t);


		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="message">������Ϣ</param>
		void Fatal(object message);

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="message">������Ϣ</param>
		/// <param name="t">�쳣</param>
		void Fatal(object message, Exception t);
	}
}
