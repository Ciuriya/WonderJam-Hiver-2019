using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public EnnemyBehavior m_behavior;


    public void Move()
    {
        m_behavior.Move(transform.position);
    }

    public void Attack()
    {
        m_behavior.Attack();
    }


    void Update()
    {
        Move();
    }
}
