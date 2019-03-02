using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBuff : MonoBehaviour
{
    public bool changeBuff;
    public Shooter m_shooter;

    // Start is called before the first frame update
    void Start()
    {
        m_shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeBuff)
        {
            
        }
    }
}
