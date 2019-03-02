using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour 
{
	[Tooltip("The text to update using the values given below")]
	public Text m_text;

	[Tooltip("The main value's reference")]
	public IntVariable m_value;

	[Tooltip("The maximum value possible, if null, only the value will be shown")]
	public IntVariable m_maxValue;

    [Tooltip("The number of zero padding the integer")]
    public int m_padding = 0;

	public void OnEnable() 
	{
		UpdateText();
	}

	public void UpdateText() 
	{
        m_text.text = (m_value.Value < m_maxValue.Value ? m_value.Value : m_maxValue.Value).ToString().PadLeft(m_padding, '0');
	}
}