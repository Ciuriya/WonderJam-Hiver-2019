using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveTextUpdater : MonoBehaviour
{ 
    [Tooltip("The text to update using the values given below")]
    public Text m_text;

    [Tooltip("The main value's reference")]
    public IntVariable m_value;

    [Tooltip("The displayed value's reference")]
    public IntVariable m_displayedValue;

    [Tooltip("The maximum value possible, if null, only the value will be shown")]
    public IntVariable m_maxValue;

    [Tooltip("The number of zero padding the integer")]
    public int m_padding = 0;

    [Tooltip("The update speed")]
    public int m_updateSpeed = 10;

    public void OnEnable()
    {
        StartCoroutine(UpdateTextProgressive(m_maxValue.Value));
    }

    public void UpdateText()
    {
        StartCoroutine(UpdateTextProgressive(m_updateSpeed));
    }

    IEnumerator UpdateTextProgressive(int speed)
    {
        if (speed == 0) speed = 1;

        while (m_displayedValue.Value <= m_value.Value)
        {
            m_displayedValue.Value += (m_value.Value - m_displayedValue.Value > speed) ? speed : m_value.Value - m_displayedValue.Value;
            m_text.text = (m_displayedValue.Value < m_maxValue.Value ? m_displayedValue.Value : m_maxValue.Value).ToString().PadLeft(m_padding, '0');

            if (m_displayedValue.Value == m_value.Value) break;

            yield return null;
        }
    }
}