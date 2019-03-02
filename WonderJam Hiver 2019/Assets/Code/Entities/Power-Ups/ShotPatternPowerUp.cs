using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Shot Pattern")]
public class ShotPatternPowerUp : PowerUp
{
    public ShotPattern m_powerUpPattern;
    public float m_duration;

    public override void Use(Shooter p_shooter)
    {
        p_shooter.AddShotPatternPowerUp(this);
    }

    public override void End(Shooter p_shooter)
    {
        Debug.Log("How did you even get here");
    }
}
