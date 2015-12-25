using System;
using System.Net;
using System.Net.Sockets;
using Strawberry.Protocols;
using UnityEngine;

namespace Strawberry
{
	public class TCPClient
	{
		public Socket socket;
		private NetworkManager networkMgr;

		public TCPClient ( NetworkManager netMgr)
		{
			networkMgr = netMgr;
		}

		public void Connect(string ip, int port) {
			IPEndPoint ipe = new IPEndPoint (IPAddress.Parse (ip), port);
			try {
				
			    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.BeginConnect(ipe, new System.AsyncCallback(ConnectionCallback), socket);

			} catch (SocketException e) {

				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});

			}
		}

		void ConnectionCallback(System.IAsyncResult ar) {
			Socket socket = (Socket)ar.AsyncState;
			try {

				socket.EndConnect(ar);
				socket.SendTimeout = 3000;
				socket.ReceiveTimeout = 3000;

				// 通知已经成功连接到服务器
				AddInternalMessage(FrameType.CONNECTED, socket);

				// 开始接受服务器信息
				Message message = new Message();
				message.socket = socket;
				// 每次读1024, 减少读开销
				socket.BeginReceive(message.buffer, 0, Constants.BUFFER_SIZE, SocketFlags.None, new System.AsyncCallback(ReceiveHeader), message);


			} catch (SocketException e) {

				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});

				// 通知连接失败
				AddInternalMessage (FrameType.CONNECT_FAILED, socket);

			}
		}

		// 接受消息长度4字节
		private void ReceiveHeader(System.IAsyncResult ar) {
			Message message = (Message)ar.AsyncState;
			try {
				int readLen = message.socket.EndReceive(ar);
				if (readLen < 1) {
					// 丢失连接
					AddInternalMessage(FrameType.LOST, message.socket);
					return;
				}

				message.readLength += readLen;
				// 消息头必须读满4个字节
				if (message.readLength < Constants.HEADER_SIZE) {
					message.socket.BeginReceive(message.buffer,
						message.readLength,
						Constants.HEADER_SIZE - message.readLength,
						SocketFlags.None,
						new System.AsyncCallback(ReceiveHeader),
						message);
				} else {
					message.DecodeHeader();
					message.readLength = 0;

					// 开始读消息
					message.socket.BeginReceive(message.buffer,
						Constants.HEADER_SIZE, 
						message.messageLength,
						SocketFlags.None,
						new System.AsyncCallback(ReceiveMessage),
						message);
				}


			} catch (SocketException e) {

				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});

			}
		}

		// 接受消息 
		private void ReceiveMessage(System.IAsyncResult ar) {
			Message message = (Message)ar.AsyncState;

			try {

				int readLen = message.socket.EndReceive(ar);
				if (readLen < 1) {
					// 丢失连接
					AddInternalMessage(FrameType.LOST, message.socket);
					return ;
				}
				message.readLength += readLen;

				if (message.readLength < message.messageLength) {
					message.socket.BeginReceive(message.buffer,
						Constants.HEADER_SIZE + message.readLength,
						message.messageLength - message.readLength,
						SocketFlags.None,
						new System.AsyncCallback(ReceiveMessage),
						message);
				} else {

					message.DecodeMessage();
					networkMgr.AddMessage(message);

					// 下一个读取
					message.Reset();
					message.socket.BeginReceive(message.buffer,
						0,
						Constants.HEADER_SIZE, 
						SocketFlags.None,
						new System.AsyncCallback(ReceiveHeader),
						message);
				}
					
			} catch (SocketException e) {

				Debug.LogErrorFormat ("{0} Error code:{1}.", new object[]{e.Message, e.ErrorCode});

			}
		}

		// 向服务器发送消息
		public void Send(Socket sk, Message message) {
			// 发送前处理数据
			message.EncodeMessage ();

			NetworkStream ns;
			lock (sk) {
				ns = new NetworkStream (sk);
				if (ns.CanWrite) {
					try {

						ns.BeginWrite(message.buffer, 0, message.dataLength, new System.AsyncCallback(SendCallback), ns);

					} catch (System.Exception e) {

						Debug.LogError (e.Message);

					}
				}
			}
		}

		// 发送回调
		private void SendCallback(System.IAsyncResult ar) {
			NetworkStream ns = (NetworkStream)ar.AsyncState;
			try {
				ns.EndWrite(ar);
				ns.Flush();
				ns.Close();

			} catch (System.Exception e) {

				Debug.LogError (e.Message);

			}

		}

		// 添加内部消息
		private void AddInternalMessage(FrameType frameType, Socket sk) {
			Message message = new Message ();
			message.socket = sk;
			message.SetID (frameType);
			networkMgr.AddMessage (message);
		}
	}
}

