using System;
using System.Net.Sockets;
using Strawberry.Protocols;
using UnityEngine;


namespace Strawberry
{
	/*
	 * 
	 *  data = 4 字节长度 ＋ 2字节frameType + body 组成
	 *   
	 *  message = frameType + body
	 * 
	 *  网络传输  采用 big endian
	*/
	public class Protocol {
		// 发送message
		public static Int32 SendResponse(Socket sk, byte[] message) {
			Int32 n = 0;

			try {
				// 写4字节长度
				byte[] dataLen = BitConverter.GetBytes(message.Length);
				// little -> big
				if (BitConverter.IsLittleEndian) {
					Array.Reverse(dataLen);
				}
				// 发送length
				sk.Send(dataLen);
			} catch (SocketException e) {
				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});
				return 0;
			}

			try {
				// 发送message
				n = sk.Send(message);
			} catch (SocketException e) {
				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});
				return 0;
			}

			return n + Constants.HEADER_SIZE;
		}

		// 发送 指定 frameType 的 message
		public static Int32 SendFramedResponse (Socket sk, FrameType frameType, byte[] body) {
			Int32 n = 0;

			try {
				byte[] dataLen = BitConverter.GetBytes(body.Length + Constants.HEADER_SIZE);
				if (BitConverter.IsLittleEndian) {
					Array.Reverse(dataLen);
				}
				// 发送长度
				sk.Send(dataLen);
			} catch (SocketException e) {
				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});
				return 0;
			}

			try {
				// 提取2字节frameType
				byte[] frameTypeArray = BitConverter.GetBytes((Int16)frameType);
				if (BitConverter.IsLittleEndian) {
					Array.Reverse(frameTypeArray);
				}
				// 发送frameType
				n = sk.Send(frameTypeArray);
			} catch (SocketException e) {
				Debug.LogErrorFormat ("{0} Error code:{1}.", e.Message, e.ErrorCode);
				return 0;
			}

			try {	
				// 发送body
				n = sk.Send(body);
			} catch (SocketException e) {
				Debug.LogErrorFormat ("{0} Error code:{1}.", e.Message, e.ErrorCode);
				return 0;
			}

			return n + Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE;
		}
			
	}
}

