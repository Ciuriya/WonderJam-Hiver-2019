using UnityEngine;

public class LoserMaker : MonoBehaviour
{
    [Tooltip("Event that's raised when the value is greater or equal to the max value")]
    public GameEvent m_DeathEvent;

    [Tooltip("Curent value")]
    public IntVariable m_Value;

    [Tooltip("Max value")]
    public IntVariable m_MaxValue;


    public void GameOver()
    {
        if (m_Value.Value >= m_MaxValue.Value)
        {
            m_DeathEvent.Raise();
        }
    }
}
