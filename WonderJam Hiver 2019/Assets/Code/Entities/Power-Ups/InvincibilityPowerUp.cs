using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Invincibility")]
public class InvincibilityPowerUp : PowerUp
{

    [HideInInspector] public Player player;
    public override void Use(Shooter p_shooter)
    {
        m_pickupAudioEvent.Play(p_shooter.m_entity.m_audioSource);
        player = p_shooter.GetComponentInParent<Player>();
        player.AddInvincibility();
    }

    public override void End(Shooter p_shooter)
    {

    }
    
}
