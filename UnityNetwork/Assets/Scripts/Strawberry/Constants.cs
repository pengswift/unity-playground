using System;

namespace Strawberry
{
	public class Constants
	{

		/*
		 * 
	 	 *  data = 4 字节长度 ＋ 2字节frameType + body 组成
		 *   
		 *  message = frameType + body
	 	 * 
	 	 *  网络传输 采用 big endian
		*/
		public const Int32 HEADER_SIZE = 4;      // 消息头长度
		public const Int32 FRAME_TYPE_SIZE = 2;  // 消息类型
		public const Int32 BUFFER_SIZE = 1024;   // buffer 大小
	}
}

