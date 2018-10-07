using System;
using Util;
/// <summary>
/// 消息处理接口.
/// </summary>
interface IAction
{
	/// <summary>
	/// 处理消息.
	/// </summary>
	/// <param name='msgId'>
	/// 消息ID.
	/// </param>
	/// <param name='readBytes'>
	/// 消息内容.
	/// </param>
	 void ProcessMessage(ushort msgId,ByteBuffer readBytes);
}


