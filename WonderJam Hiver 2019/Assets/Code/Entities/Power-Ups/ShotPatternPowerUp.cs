﻿using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Shot Pattern")]
public class ShotPatternPowerUp : PowerUp
{
	[Tooltip("The pattern to give to the player")]
    public ShotPattern m_powerUpPattern;

	[Tooltip("The length of time during which this power-up is active, can be overriden by other power-ups or refreshed")]
    [Range(0, 60)] public float m_duration;

    public override void Use(Shooter p_shooter)
    {
		m_pickupAudioEvent.Play(p_shooter.m_entity.m_audioSource);

		p_shooter.StopShooting(p_shooter.GetCurrentPattern());
        p_shooter.AddShotPatternPowerUp(this);
    }

    public override void End(Shooter p_shooter) {
		p_shooter.StopShooting(m_powerUpPattern);
	}
}
