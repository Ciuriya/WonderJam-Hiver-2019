using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSpot : MonoBehaviour
{
	[Tooltip("The UI text displaying the spot's ranking")]
	public Text m_rankText;

	[Tooltip("The UI text displaying the spot's owner")]
	public Text m_nameText;

	[Tooltip("The UI text displaying the spot's score")]
	public Text m_scoreText;

	[Tooltip("Can the name be modified?")]
	public bool m_editable;

	[HideInInspector] public LeaderboardScore m_score;
	[HideInInspector] public bool m_local;
	[HideInInspector] public LeaderboardLoader m_loader;

	public void Set(int p_rank, LeaderboardScore p_score)
	{
		m_score = p_score;
		m_rankText.text = p_rank.ToString();
		m_nameText.text = p_score.Name;
		m_scoreText.text = p_score.Score.ToString();

		if(m_editable)
		{
			InputField field = m_nameText.GetComponent<InputField>();
			field.Select();
			field.ActivateInputField();
		}
	}

	public void FinishEditing() 
	{
		m_score.Name = m_nameText.text;

		if(m_score.Name.Length > 0) 
		{
			m_loader.AddScore(m_score);
		}
	}
}
