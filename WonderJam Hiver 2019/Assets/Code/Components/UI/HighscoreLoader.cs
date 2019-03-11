﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class HighscoreLoader : MonoBehaviour
{
	[Tooltip("The value being updated with the highscore in leaderboards")]
	public IntReference m_highscoreValue;

	[Tooltip("The event called when the highscore is changed, used to update the UI")]
	public GameEvent m_highscoreChangeEvent;

	void Start()
	{
		LoadJSON();
	}

	public void LoadJSON() 
	{
		List<LeaderboardScore> scores = GetScores(false);
		scores.AddRange(GetScores(true));

		scores.Sort(LeaderboardLoader.CompareScores);

		if(scores.Count == 0) m_highscoreValue.Value = 0;
		else m_highscoreValue.Value = scores[0].Score;

		m_highscoreChangeEvent.Raise();
	}

	private List<LeaderboardScore> GetScores(bool p_local) 
	{
		List<LeaderboardScore> scores = new List<LeaderboardScore>();
        string path = Application.dataPath + "/Data/" + (p_local ? "Local" : "Online") + "Leaderboard.JSON";

        if(!LeaderboardLoader.CheckPath(path)) return scores;
        
        StreamReader reader = new StreamReader(path);

		for(string json = reader.ReadLine(); json != null && json.Length > 0; json = reader.ReadLine())
		{
			scores.Add(JsonUtility.FromJson<LeaderboardScore>(json));
		}

		reader.Close();

		return scores;
	}
}
