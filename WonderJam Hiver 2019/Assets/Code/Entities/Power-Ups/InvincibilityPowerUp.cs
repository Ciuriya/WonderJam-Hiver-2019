using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Invincibility")]
public class InvincibilityPowerUp : PowerUp
{

    [HideInInspector] public Player player;
    // Start is called before the first frame update
    public override void Use(Shooter p_shooter)
    {
        m_pickupAudioEvent.Play(p_shooter.m_audioSource);
        player = p_shooter.GetComponentInParent<Player>();
        player.AddInvincibility();
    }

    // Update is called once per frame
    public override void End(Shooter p_shooter)
    {

    }
    
}
