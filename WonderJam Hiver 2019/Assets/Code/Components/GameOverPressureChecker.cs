using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPressureChecker : MonoBehaviour
{
    public IntReference m_currentValue;
    public IntReference m_maxValue;
    public GameEvent m_event;

    public void Update()
    {
        if (m_currentValue >= m_maxValue)
            m_event.Raise();
    }
}
