using UnityEngine;

public class GameOver : MonoBehaviour
{
	public IntReference m_score;

    public void DoPressureGameOverStuff()
    {
        Debug.Log("You lost you dumb fuck (PRESSURE)");
		SaveScoreToJSON();
	}

    public void DoOutOfLivesGameOverStuff()
    {
        Debug.Log("You ran out of lives you dumb fuck (LIVES)");
		SaveScoreToJSON();
    }

	public void SaveScoreToJSON() 
	{
		LeaderboardScore score = new LeaderboardScore("", m_score.Value);
		WriteToJSON(score, Application.dataPath + "/Data/PendingLocalScores.JSON");
		WriteToJSON(score, Application.dataPath + "/Data/PendingOnlineScores.JSON");
	}

	private void WriteToJSON(LeaderboardScore p_score, string p_fileName) 
	{
		System.IO.StreamWriter Writer = new System.IO.StreamWriter(p_fileName, false);

		Writer.Write(JsonUtility.ToJson(p_score));
		Writer.Close();
	}
}
