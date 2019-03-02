/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pouvoir overide lability, mais garder le cooldown de l'autre.



public class ActivePowerUP : MonoBehaviour
{
    
    public Shooter m_Shooter;






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float TimeLeft = m_Shooter.m_spPowerUps.Last.Value.time
            + m_Shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_duration * 1000 - (Time.time * 1000);

        float fill = TimeLeft / m_Shooter.m_spPowerUps.Last.Value.shotPatternPowerUp.m_duration;

        //checker le temps restant au power up

        //(temps + duration - temps courant )/duration


        //defill le radius 360






    }
}*/
