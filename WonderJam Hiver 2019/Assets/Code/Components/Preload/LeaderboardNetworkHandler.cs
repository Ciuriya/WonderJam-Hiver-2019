using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LeaderboardNetworkHandler : MonoBehaviour
{
	public void Awake()
	{
		FetchBlocking();
		ConvertFileToJSON();
	}

	public string ConvertFileToJSON() 
	{
		System.IO.StreamReader Reader = new System.IO.StreamReader(Application.dataPath + "/Data/OnlineLeaderboard.JSON");

		string full = "";

		for(string json = Reader.ReadLine(); json != null; json = Reader.ReadLine())
		{
			full += json;
		}

		return full;
	}

	public void Upload(string p_jsonData) 
	{ 
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("104.236.227.95"), 1234);

		socket.Connect(endpoint);

		string data = "JSONHIGHSCORE---" + p_jsonData;
		socket.Send(Encoding.UTF8.GetBytes(data));

		socket.Disconnect(false);
		socket.Close();
	}

	public IEnumerator UploadV2(string p_jsonData) 
	{
		TcpClient client = new TcpClient();
		client.Connect(new IPEndPoint(IPAddress.Parse("104.236.227.95"), 1234));

		NetworkStream stream = client.GetStream();

		if(stream.CanWrite) 
		{
			string data = "JSONHIGHSCORE---" + p_jsonData;
			byte[] buff = Encoding.UTF8.GetBytes(data);
			stream.Write(buff, 0, buff.Length);
		}

		yield return new WaitForSeconds(0.2f);

		client.Close();
	}

	public string FetchBlocking() 
	{ 
		UnityWebRequest download = UnityWebRequest.Post("http://smcmax.com/highscores/OnlineLeaderboard.JSON", "");

		StartCoroutine(Send(download));

		while(!download.isDone) { }

		if(!download.isNetworkError && !download.isHttpError) return download.downloadHandler.text;
		else return "";
	}

	private IEnumerator Send(UnityWebRequest p_request) 
	{
		yield return p_request.SendWebRequest();
	}
}
