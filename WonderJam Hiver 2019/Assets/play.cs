using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play : MonoBehaviour
{
    public bool m_play;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem pS = GetComponent<ParticleSystem>();
        pS.enableEmission = m_play;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
