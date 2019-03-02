using UnityEngine;

public class ValueAjuster : MonoBehaviour
{
    [Tooltip("Value that needs to be changed")]
    public IntReference m_Value;

    [Tooltip("Event that causes the UI to update")]
    public GameEvent m_OnValueChanged;

    [Tooltip("By how much does the value need to be changed")]
    public int m_ChangeValueBy = 1;

    public void updateValue()
    {
        m_Value.Value += m_ChangeValueBy;
        m_OnValueChanged.Raise();
    }
}
