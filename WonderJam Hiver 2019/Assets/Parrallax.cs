using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrallax : MonoBehaviour
{
    public float m_ParrallaxSpeed;

    public Transform[] backgrounds;//array of bakground that has to be parrallax
    public float m_Smoothing = 1;//how smooth parralax going to be

    public Transform m_BackGroundGameObject;
    private Vector3 m_PreviousBackGroundGameObjectPos;

    
    void Start()
    {
        m_PreviousBackGroundGameObjectPos = m_BackGroundGameObject.position;


        m_ParrallaxSpeed = 2;

    }

    void Update()
    {
        





    }



}
