using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    public bool cheat;
    [HideInInspector]public Shooter m_shooter;
    public ShotPatternPowerUp newShotPatternPowerUp;
    public float delayBetweenCheatCodes;

    // Start is called before the first frame update
    void Start()
    {
        m_shooter = GetComponent<Shooter>();
        if (cheat)
        {
            StartCoroutine(CheatCodeRefresher());
        }
    }

    IEnumerator CheatCodeRefresher()
    {
        yield return new WaitForSeconds(delayBetweenCheatCodes);
        m_shooter.AddShotPatternPowerUp(newShotPatternPowerUp);
        StartCoroutine(CheatCodeRefresher());
    }
}
