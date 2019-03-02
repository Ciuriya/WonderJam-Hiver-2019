using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Manager/ValueManager")]
public class ValueManager : ScriptableObject
{
    public IntReference m_value;

    public IntReference m_minValue;
    public IntReference m_maxValue;

    public GameEvent m_updateEvent;

    public void UpdateValue(int delta)
    {
        m_value.Value = Mathf.Max(Mathf.Min(m_value + delta, m_maxValue), m_minValue);
        m_updateEvent.Raise();
    }
}
