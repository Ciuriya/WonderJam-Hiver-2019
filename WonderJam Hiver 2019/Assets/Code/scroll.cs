using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    public float m_scrollSpeed = -5f;

    public float m_offset = 10f;

    Vector2 m_startpos;

    // Start is called before the first frame update
    void Start()
    {
        m_startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * m_scrollSpeed, m_offset);
        transform.position = m_startpos + Vector2.down * newPos;
    }
}
