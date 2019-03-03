using UnityEngine.UI;
using UnityEngine;

public class ActivePowerUP : MonoBehaviour
{
    public int m_playerID;

    [HideInInspector] public Image m_Image;

    [HideInInspector] public Color m_color;

    public Sprite m_UIMask;

    private Activeplayers m_activeplayers;

    private Shooter m_Shooter;

    private PowerUp m_powerUp;

    private Sprite m_sprite;

    private float m_TimeLeft;

    // Start is called before the first frame update
    private void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {


        if (Game.m_players.m_players.Exists(p => p.m_playerId == m_playerID))
        {
            Player player = Game.m_players.m_players.Find(p => p.m_playerId == m_playerID);
            

            m_Image = GetComponent<Image>();
            m_Image.sprite = m_UIMask;


            if (player.m_shooter.m_spPowerUps.Last != null)
            {
                m_Image.sprite = player.m_shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_icon.sprite;
            }

            if (player.m_shooter.m_spPowerUps.Last != null)
            {
                m_TimeLeft = ((player.m_shooter.m_spPowerUps.Last.Value.time)
               + (player.m_shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_duration * 1000) - (Time.time * 1000)) / 1000;
            }

            if (m_TimeLeft <= 0)
            {
                m_Image.sprite = m_UIMask;
            }

        }
                   
    }
}
