using UnityEngine;
using System.Collections.Generic;

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

		scores.Sort(new System.Comparison<LeaderboardScore>((LeaderboardScore first, LeaderboardScore second) => { return second.Score - first.Score; }));

		m_highscoreValue.Value = scores[0].Score;
		m_highscoreChangeEvent.Raise();
	}

	private List<LeaderboardScore> GetScores(bool p_local) 
	{
		List<LeaderboardScore> scores = new List<LeaderboardScore>();
		System.IO.StreamReader Reader = new System.IO.StreamReader(Application.dataPath + "/Data/" + (p_local ? "Local" : "Online") + "Leaderboard.JSON");

		for (string json = Reader.ReadLine(); json != null; json = Reader.ReadLine())
		{
			scores.Add(JsonUtility.FromJson<LeaderboardScore>(json));
		}

		Reader.Close();

		return scores;
	}
}
