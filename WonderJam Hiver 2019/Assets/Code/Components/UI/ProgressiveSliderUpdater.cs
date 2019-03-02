using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveSliderUpdater : MonoBehaviour
{
    [Tooltip("The slider to update using the values given below")]
    public Slider m_slider;

    [Tooltip("The main value's reference")]
    public IntVariable m_value;

    [Tooltip("The displayed value's reference")]
    public IntVariable m_displayedValue;

    [Tooltip("The maximum value possible, if null, only the value will be shown")]
    public IntVariable m_maxValue;

    [Tooltip("The update speed")]
    public int m_updateSpeed = 10;

    public void OnEnable()
    {
        StartCoroutine(UpdateFillProgressive(m_maxValue.Value));
    }

    public void UpdateFill()
    {
        StartCoroutine(UpdateFillProgressive(m_updateSpeed));
    }

    IEnumerator UpdateFillProgressive(int speed)
    {
        if (speed == 0) speed = 1;

        do
        {
            m_displayedValue.Value += (m_value.Value - m_displayedValue.Value > speed) ? 
                speed : (m_value.Value - m_displayedValue.Value < -speed) ? -speed : m_value.Value - m_displayedValue.Value;
            m_slider.value = (m_displayedValue.Value < m_maxValue.Value ? m_displayedValue.Value : m_maxValue.Value);

            yield return null;

        } while (m_displayedValue.Value != m_value.Value);
    }
}