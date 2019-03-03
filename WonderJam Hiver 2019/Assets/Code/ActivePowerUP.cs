using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActivePowerUP : MonoBehaviour
{
    [HideInInspector] public GameObject[] m_players;

    [HideInInspector] public Image m_Image;

    [HideInInspector] public Color m_color;

    public Sprite m_UIMask;

    private float m_TimeLeft;

    private Shooter m_Shooter;

    private PowerUp m_powerUp;

    private Sprite m_sprite;

   
    void Awake()
    {
        m_players = GameObject.FindGameObjectsWithTag("Player");
        m_Shooter = m_players[0].GetComponent<Shooter>();
        m_Image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start() { m_Image.sprite = m_UIMask; }

    // Update is called once per frame
    void Update()
    {
        if (m_Shooter.m_spPowerUps.Last!=null)
        {
            m_Image.sprite = m_Shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_icon.sprite;
        }
       
        if(m_Shooter.m_spPowerUps.Last != null)
        {
             m_TimeLeft = ((m_Shooter.m_spPowerUps.Last.Value.time)
            + (m_Shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_duration * 1000) - (Time.time * 1000)) / 1000;
        }

        if (m_TimeLeft<=0)
        {
            m_Image.sprite = m_UIMask;
        }
    }
}
