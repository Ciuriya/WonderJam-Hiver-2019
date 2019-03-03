using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activeplayers : MonoBehaviour
{
    [HideInInspector] public bool m_Player1Active = true;


    public GameObject m_player1;
    public GameObject m_player2;
    public GameObject m_player3;
    public GameObject m_player4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( !(Game.m_players.m_players.Exists(p => p.m_playerId == 1)) )
        {
            m_player1.SetActive(false);
        }
        else
        {
            m_player1.SetActive(true);
        }

        if(!(Game.m_players.m_players.Exists(p => p.m_playerId == 2)))
        {
            m_player2.SetActive(false);
        }
        else
        {
            m_player2.SetActive(true);
        }

        if(!(Game.m_players.m_players.Exists(p => p.m_playerId == 3)))
        {
            m_player3.SetActive(false);
        }
        else
        {
            m_player3.SetActive(true);
        }

        if(!(Game.m_players.m_players.Exists(p => p.m_playerId == 4)))
        {
            m_player4.SetActive(false);
        }
        else
        {
            m_player4.SetActive(true);
        }
    }
}
