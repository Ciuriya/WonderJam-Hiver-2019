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
        System.IO.StreamReader Reader;
        string Path = Application.dataPath + "/Data/OnlineLeaderboard.JSON";
        if (!System.IO.File.Exists(Path))
        {
            return "";
        }
        Reader = new System.IO.StreamReader(Path);

        string full = "";

		for(string json = Reader.ReadLine(); json != null; json = Reader.ReadLine())
		{
			full += json;
		}

		return full;
	}

	public void Upload(string p_jsonData, int p_retries) 
	{ 
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("104.236.227.95"), 1234);

		socket.Connect(endpoint);

		string data = "JSONHIGHSCORE---" + p_jsonData;
		socket.Send(Encoding.UTF8.GetBytes(data));

		byte[] buff = new byte[1024];
		socket.Receive(buff);

		socket.Disconnect(false);
		socket.Close();

		string response = Encoding.UTF8.GetString(buff);
		Debug.Log("Received: " + response);

		if(!response.StartsWith("OK") && p_retries < 10)
			Upload(p_jsonData, p_retries + 1);
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
