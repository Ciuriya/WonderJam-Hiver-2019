using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Tooltip("The variable that will be decremented every second")]
    public IntReference m_TimeLeft;

    [Tooltip("How much time will we be waiting")]
    public IntReference m_MaxTime;

    [Tooltip("Event to update de UI with the time value")]
    public GameEvent m_UpdateEvent;

    [Tooltip("Event to raise at the end of the timer")]
    public GameEvent m_GameEvent;

    [Tooltip("Put the level value change here to show a nice cute feedback")]
    public GameEvent m_GameJamCode;

    public void StartTimer()
    {
        m_TimeLeft.Value = m_MaxTime.Value;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (m_TimeLeft > 1)
        {
            m_TimeLeft.Value--;
            m_UpdateEvent.Raise();
            yield return new WaitForSecondsRealtime(1f);
        }
        m_GameJamCode.Raise();
        m_GameEvent.Raise();
    }
}
