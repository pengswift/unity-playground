using System;
using System.Collections;
using System.Collections.Generic;
using Strawberry.Protocols;
using UnityEngine;


namespace Strawberry
{
	public class NetworkManager
	{
		// 代理回调函数
		public delegate void OnReceive( Message message);

		// 每个消息对应一个OnReceive函数
		public Dictionary<FrameType, OnReceive> handlers;

		// 存储消息的队列
		private Queue messages = new System.Collections.Queue();

		public NetworkManager ()
		{
			handlers = new Dictionary<FrameType, OnReceive> ();

			// 注册网络消息
			AddHandler (FrameType.ACCETPED, OnAccepted);
			AddHandler (FrameType.CONNECTED, OnConnected);
			AddHandler (FrameType.CONNECT_FAILED, OnConnectFailed);
			AddHandler (FrameType.LOST, OnLost);
		}

		// 注册消息
		public void AddHandler(FrameType frameType, OnReceive handler) {
			handlers.Add (frameType, handler);
		}

		// 消息入队
		public void AddMessage(Message message) {
			lock (messages) {
				messages.Enqueue (message);
			}
		}
		 
		// 消息出队
		public Message GetMessage() {
			lock (messages) {
				if (messages.Count == 0)
					return null;
				return (Message)messages.Dequeue ();
			}
		}

		public void Update() {
			Message message = null;
			for (message = GetMessage (); message != null;) {
				OnReceive handler = null;
				// 通过消息id 取得 相应的 OnReceive代理函数
				if (handlers.TryGetValue (message.GetID(), out handler)) {
					if (handler != null) {
						handler (message);
					}
				}
				message = null;
			}
		}

		// 处理服务器接受客户端的连接
		protected virtual void OnAccepted(Message message) {

		}

		// 处理客户端取得与服务器的连接
		protected virtual void OnConnected(Message message) {

		}

		// 处理客户端取得与服务器连接失败
		protected virtual void OnConnectFailed(Message message) {

		}

		// 处理丢失连接
		protected virtual void OnLost(Message message) {
			
		}
	}
}

