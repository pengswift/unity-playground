using System;
using Strawberry;
using UnityEngine;

public class GatewayManager : NetworkManager {
	private TCPClient client;

	public void Start(string serverIp, Int32 port) {
		// 连接服务器
		client = new TCPClient (this);
		client.Connect (serverIp, port);
	}

	public void Send(Message message) {	
		client.Send (client.socket, message);
	}

	protected override void OnLost(Message message) {
		Debug.Log ("丢失与服务器的连接");
	}

	protected override void OnConnected(Message message) {
		Debug.Log("成功连接到服务器");
	}

	protected override void OnConnectFailed(Message message) {
		Debug.Log("连接服务器失败，请退出");
	}
		
}


