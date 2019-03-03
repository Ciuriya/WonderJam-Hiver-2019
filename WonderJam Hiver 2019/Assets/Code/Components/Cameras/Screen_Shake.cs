using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen_Shake : MonoBehaviour
{
    [Tooltip("Length of the screen tremors")]
    public float m_shakeDuration;

    [Tooltip("Strenght of the screen tremors")]
    public float m_magnitude;

    Vector3 m_initPosition;
    Vector3 m_newPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_initPosition = gameObject.transform.localPosition;
        m_newPosition = m_initPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_shakeDuration > 0)
        {
            m_newPosition.x += Random.insideUnitSphere.x * m_magnitude;
            m_shakeDuration -= Time.deltaTime;
            gameObject.transform.localPosition = m_newPosition;
        }

        else
        {
            gameObject.transform.localPosition = m_initPosition;
            Destroy(this);
        }
    }
}

