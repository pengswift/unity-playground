using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Strawberry;
using Strawberry.Protocols;

public class Chat : MonoBehaviour {

	public InputField chatInputField;


	void Start() {
		// 注册handler
		GatewayClient.gatewayManager.AddHandler (FrameType.SAYHI, OnChat); 

	}


	public void SendChat() {
		string chatMsg = chatInputField.text;
		Debug.Log ("send message! : " + chatMsg);

		Message message = new Message ();
		message.SetID (FrameType.SAYHI);

		byte[] chatMsgByte = System.Text.Encoding.Default.GetBytes (chatMsg);
		message.SetBody (chatMsgByte);

		// 发送给自身
		//	GatewayClient.gatewayManager.AddMessage (message);

		// 发送给网络玩家
		GatewayClient.gatewayManager.Send (message);


	

		//		FlatBufferBuilder fbb = new FlatBufferBuilder (1);
		//		SayHiRequest.StartSayHiRequest (fbb);
		//		var str = fbb.CreateString ("Hi");
		//		SayHiRequest.AddMsg (fbb, str);
		//		var offset = SayHiRequest.EndSayHiRequest (fbb);
		//
		//		var ms = new MemoryStream(fbb.DataBuffer.Data, fbb.DataBuffer.Position, fbb.Offset);
		//
		//
		//
		//		Message message = new Message ();
		//		message.SetID (FrameType.SAYHI);
		//		message.SetBody (ms.ToArray());
		//
		//
		//		clientPeer.Send (message);
	
	}

	public void OnChat(Message message) {
		Debug.Log ("............OnChat..............");
	}
}
