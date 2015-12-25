using System;
using FlatBuffers;
using System.Net.Sockets;
using Strawberry.Protocols;
using UnityEngine;

namespace Strawberry
{
	// 消息结构体
	public class Message
	{
		private Int16 ID;
		private byte[] Body;

		public byte[] buffer = new byte[Constants.BUFFER_SIZE];
		public Int32 readLength;


		// 消息长度
		public Int32 messageLength;
		// 数据长度, 添加4字节header长度
		public Int32 dataLength;

		public Socket socket;

		public Message() {
			readLength = 0;
			messageLength = 0;
			dataLength = 0;
		}

		public Message(Int16 id, byte[] body) {
			ID = id;
			Body = body;
		}

		public void SetID(FrameType frameType) {
			ID = (Int16)frameType;
		}
		public FrameType GetID() {
			return (FrameType)ID;
		}

		public void SetBody(byte[] body) {
			Body = body;
		}

		public void Reset() {
			readLength = 0;
			messageLength = 0;
			dataLength = 0;

			ID = 0;
			Body = null;
		}

		// 由buffer数组最前面的4个字节得到数据的长度
		public void DecodeHeader() {
			// 处理 big-endian 数据
			byte[] messageLenArray = new byte[Constants.HEADER_SIZE];
			Array.Copy (buffer, 0, messageLenArray, 0, Constants.HEADER_SIZE);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse(messageLenArray);
			}

			messageLength = System.BitConverter.ToInt32 (messageLenArray, 0);
		}

		// 将 id, body 合并成buffer
		public void EncodeMessage() {
			if (Body == null) {
				Debug.LogAssertion ("body is null !!!");
				return;
			}

			// 将 header + frameType + body 写入 buffer中
			// 计算数据长度
			dataLength = Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE + Body.Length;

			if (dataLength > Constants.BUFFER_SIZE) {
				Debug.LogAssertion ("message body is too bigger !!!");
				return;
			}

			byte[] dataLenArray = BitConverter.GetBytes(dataLength);

			byte[] frameTypeArray = new byte[Constants.FRAME_TYPE_SIZE];
			frameTypeArray = BitConverter.GetBytes (ID);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse (dataLenArray);
				Array.Reverse (frameTypeArray);
			}

			dataLenArray.CopyTo (buffer, 0);
			frameTypeArray.CopyTo (buffer, Constants.HEADER_SIZE);
			Body.CopyTo (buffer, Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE);

		}

		// 解析message 内容
		public void DecodeMessage() {
			if (buffer.Length < Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE ) {
				return;
			}
			// 提取frameType
			byte[] frameTypeArray = new byte[Constants.FRAME_TYPE_SIZE];
			Array.Copy (buffer, Constants.HEADER_SIZE, frameTypeArray, 0, Constants.FRAME_TYPE_SIZE);

			if (BitConverter.IsLittleEndian) {
				Array.Reverse (frameTypeArray);
			}

			ID = System.BitConverter.ToInt16 (frameTypeArray, 0);

			// 提取body
			if (buffer.Length > Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE) {
				Body = new byte[messageLength - Constants.FRAME_TYPE_SIZE];
				Array.Copy (buffer, Constants.HEADER_SIZE + Constants.FRAME_TYPE_SIZE, Body, 0, Body.Length);
			}
		}


		// 加密
		public byte[] Encrypt() {
			return null;
		}

		// 解密
		public byte[] Decrypt() {
			return null;
		}
			
	}
}

