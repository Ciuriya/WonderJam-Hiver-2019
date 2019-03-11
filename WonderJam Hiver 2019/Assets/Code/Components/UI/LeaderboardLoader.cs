using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class LeaderboardLoader : MonoBehaviour
{
	[Tooltip("The leaderboard spot's prefab")]
	public GameObject m_spotPrefab;

	[Tooltip("The leaderboard spot's editable prefab")]
	public GameObject m_editableSpotPrefab;

	[Tooltip("Should leaderboards be pulled locally or from the server?")]
	public bool m_local;

	[Tooltip("The amount of scores to load in the leaderboard (maximum)")]
	public int m_loadedAmount;

	private List<GameObject> m_instantiatedObjects;

	void Start()
	{
		Load();
	}

	public void Load()
	{
		if(m_instantiatedObjects != null && m_instantiatedObjects.Count > 0)
			return;

		m_instantiatedObjects = new List<GameObject>();

		List<LeaderboardScore> scores = m_local ? GetLocalScores() : GetOnlineScores();
		LeaderboardScore pending = GetPendingScore();

		if(pending != null) scores.Add(pending);

		if(scores.Count > 0)
			scores.Sort(CompareScores);

		for(int i = 1; i <= m_loadedAmount; i++) 
		{ 
			if(scores.Count < i) break;

			GameObject spotObj;

			if(scores[i - 1].Name.Length == 0) 
				spotObj = Instantiate(m_editableSpotPrefab, transform);
			else spotObj = Instantiate(m_spotPrefab, transform);

			m_instantiatedObjects.Add(spotObj);
			LeaderboardSpot spot = spotObj.GetComponent<LeaderboardSpot>();

			spot.m_local = m_local;
			spot.m_loader = this;
			spot.Set(i, scores[i - 1]);
		}
	}

	public List<LeaderboardScore> GetLocalScores() 
	{
		List<LeaderboardScore> scores = new List<LeaderboardScore>();
        string path = Application.dataPath + "/Data/LocalLeaderboard.JSON";

        if(!CheckPath(path)) return scores;

        StreamReader reader = new StreamReader(path);

		for(string json = reader.ReadLine(); json != null; json = reader.ReadLine())
		{
			scores.Add(JsonUtility.FromJson<LeaderboardScore>(json));
		}

		reader.Close();

		return scores;
	}

	public List<LeaderboardScore> GetOnlineScores() 
	{
		List<LeaderboardScore> scores = new List<LeaderboardScore>();

		string json = Game.m_leaderNetHandler.FetchBlocking();
		string fixedJson = "";

		if(json.Length > 0) 
		{ 
			string[] individualScores = json.Split('{');

			for(int i = 1; i < individualScores.Length; i++) 
			{ 
				string indivScore = individualScores[i].Replace("\n", "");

				if(!indivScore.Contains("{")) indivScore = "{" + indivScore;
				if(!indivScore.Contains("}")) indivScore = indivScore + "}";

				scores.Add(JsonUtility.FromJson<LeaderboardScore>(indivScore + "\n"));
				fixedJson += indivScore + "\n";
			}
		}

		OverwriteOnlineLeaderboards(fixedJson);

		return scores;
	}

	public void AddScore(LeaderboardScore p_score) 
	{
		List<LeaderboardScore> online = GetOnlineScores();
		List<LeaderboardScore> local = GetLocalScores();
		bool onlineChange = false;

		online.Sort(CompareScores);
		local.Sort(CompareScores);

        string onlinePath = Application.dataPath + "/Data/OnlineLeaderboard.JSON";
		string localPath = Application.dataPath + "/Data/LocalLeaderboard.JSON";

		CheckPath(onlinePath);
		CheckPath(localPath);

		StreamWriter onlineWriter = new StreamWriter(onlinePath, true);
        StreamWriter localWriter = new StreamWriter(localPath, true);

		if(online.Count < m_loadedAmount || online[m_loadedAmount - 1].Score < p_score.Score) // if we make it in top x
		{
			onlineWriter.Write((online.Count > 0 ? "\n" : "") + JsonUtility.ToJson(p_score));
			onlineChange = true;
		}

		if(local.Count < m_loadedAmount || local[m_loadedAmount - 1].Score < p_score.Score) // if we make it in top x
			localWriter.Write((local.Count > 0 ? "\n" : "") + JsonUtility.ToJson(p_score));

		onlineWriter.Close();
		localWriter.Close();

		DeletePendingScore();

		if(onlineChange) 
			Game.m_leaderNetHandler.Upload(Game.m_leaderNetHandler.ConvertFileToJSON(), 1);
	}

	public void OverwriteOnlineLeaderboards(string p_jsonData) 
	{
        string path = Application.dataPath + "/Data/OnlineLeaderboard.JSON";

		if(!CheckPath(path)) return;

        StreamWriter writer = new StreamWriter(path, false);

		writer.Write(p_jsonData);
		writer.Close();
	}

	public LeaderboardScore GetPendingScore() 
	{
        string path = Application.dataPath + "/Data/PendingScores.JSON";

        if(!CheckPath(path)) return null;

		StreamReader reader = new StreamReader(path);
		LeaderboardScore score = JsonUtility.FromJson<LeaderboardScore>(reader.ReadLine());

		reader.Close();
		return score;
	}

	public void DeletePendingScore() 
	{
        string path = Application.dataPath + "/Data/PendingScores.JSON";

		if(!CheckPath(path)) return;

        StreamWriter writer = new StreamWriter(path, false);

		writer.Write("");
		writer.Close();
	}

	public static int CompareScores(LeaderboardScore first, LeaderboardScore second) {
		try 
		{
			return second.Score - first.Score;
		} 
		catch(NullReferenceException) 
		{ 
			if(first == null) return second.Score;
			else return first.Score;
		}
	}

	public static bool CheckPath(string path) 	
	{ 
		if(!File.Exists(path))
		{ 
			try 
			{ 
				File.Create(path).Dispose();
				return true;
			} 
			catch(DirectoryNotFoundException) 
			{ 
				if(path.Contains("/")) 
				{
					string[] split = path.Split('/');
					string folder = path.Replace("/" + split[split.Length - 1], "");

					try 
					{
						Directory.CreateDirectory(folder);
						File.Create(path).Dispose();

						return true;
					} catch(Exception) { return false; }
				}
			}
		} else return true;

		return false;
	}
}

[System.Serializable]
public class LeaderboardScore
{
	public string Name;
	public int Score;

	public LeaderboardScore(string p_name, int p_score) 
	{ 
		Name = p_name;
		Score = p_score;
	}
}