using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Speed++")]
public class SpeedPowerUp : PowerUp
{
    [HideInInspector] public Player player;
    // Start is called before the first frame update
    public override void Use(Shooter p_shooter)
    {
        m_pickupAudioEvent.Play(p_shooter.m_entity.m_audioSource);
        player = p_shooter.GetComponentInParent<Player>();
        player.AddSpeed();
    }

    // Update is called once per frame
    public override void End(Shooter p_shooter)
    {

    }
}
