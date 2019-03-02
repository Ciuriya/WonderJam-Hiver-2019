using UnityEngine;

[CreateAssetMenu(menuName = "Power-Up/Nuke")]
public class NukePowerUp : PowerUp
{

    [HideInInspector] public GameObject[] enemies;
    // Start is called before the first frame update
    public override void Use(Shooter p_shooter)
    {
        m_pickupAudioEvent.Play(p_shooter.m_audioSource);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Entity>().Kill(); 
        }
    }

    // Update is called once per frame
    public override void End(Shooter p_shooter)
    {

    }
}
