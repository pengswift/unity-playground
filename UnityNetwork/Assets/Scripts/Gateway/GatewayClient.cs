using System;
using UnityEngine;
using Strawberry;
using Strawberry.Protocols;
using FlatBuffers;
using System.IO;

public class GatewayClient : MonoBehaviour {

	// 暴露gatewayManager， 提供注册handler 方法
	public static GatewayManager gatewayManager;

	public string serverIP = "127.0.0.1";
	public Int32 port = 10001;

	// gamewayManager 无法自身启动， 需子类重载并绑定到gameobject对象上

	void Awake() {
		gatewayManager = new GatewayManager ();
		gatewayManager.Start (serverIP, port);
	}

	void Update() {
		// 启动自身 逻辑
		gatewayManager.Update ();
	}
}


