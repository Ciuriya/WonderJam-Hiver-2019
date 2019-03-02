using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodcells : MonoBehaviour
{
    public GameObject BloodCells;
    public float m_velocityX = 0;
    public float m_velocityY = 2;
    [HideInInspector] public int[] m_scale;
    public float m_spawntime;
    public Transform[] spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating ("Spawn", m_spawntime, m_spawntime);
    }

    void Spawn()
    {
       
        int spawnPointIndex = Random.Range(0, spawnpoints.Length);

     //   BloodCells.transform.localScale += new Vector3(Random.Range(0.1f, m_scale.Length), Random.Range(0.1f, m_scale.Length), Random.Range(min: 0.1f, m_scale.Length));

        GameObject TempBloodCell = Instantiate(BloodCells, spawnpoints[spawnPointIndex].position, spawnpoints[spawnPointIndex].rotation);

        TempBloodCell.transform.localScale += new Vector3(Random.Range(-0.5f, m_scale.Length), Random.Range(-0.5f, m_scale.Length), Random.Range(min: -0.5f, m_scale.Length));

        


    }
}
