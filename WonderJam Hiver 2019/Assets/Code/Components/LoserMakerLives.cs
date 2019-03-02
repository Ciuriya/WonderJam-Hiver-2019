using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoserMakerLives : MonoBehaviour
{
    [Tooltip("Event that's raised when the value is smaller or equal to zero")]
    public GameEvent m_DeathEvent;

    [Tooltip("Curent value")]
    public IntVariable m_Value;



    public void GameOver()
    {
        if (m_Value.Value <= 0)
        {
            m_DeathEvent.Raise();
        }
    }
}
