using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : Entity
{
    [System.Serializable]
    public struct Spawn
    {
        public int Weight;
        public GameObject Object;
        public MoveStrategy Strategy;
    }

    public int m_pressure = 0;
    public int m_points = 0;

    public ValueManager m_scoreManager;

    public MoveStrategy m_strategy;

    [Header("Spawn data")]

    public int m_spawnCount = 0;
    public Spawn[] m_spawnList;

    [HideInInspector] public Vector3 m_spawnFromPosition;

    private int totalWeight = 0;


    public void OnEnable()
    {
        foreach(var spawn in m_spawnList)
            totalWeight += spawn.Weight;
    }

    public void FixedUpdate()
    {
        if (m_strategy)
            m_controller.Move(m_strategy.GetMovementVector(this));
    }

    public override void Kill()
    {
        if (m_canDie && !m_isDead)
        {
            m_isDead = true;
            Explosion();
            m_scoreManager.UpdateValue(m_points);
            Die();
        }
    }

    protected override void Die()
    {
        for (int i = 0; i < m_spawnCount; ++i)
            SpawnChild();

        Destroy(gameObject);
    }

    private void SpawnChild()
    {
        int rand = Random.Range(0, totalWeight);

        foreach (var spawn in m_spawnList)
        {
            rand -= spawn.Weight;

            float donutInternRadius = .15f;
            float donutThickness = .15f;

            Vector3 positionTweaker = Random.insideUnitCircle * donutThickness;

            positionTweaker += positionTweaker.normalized * donutInternRadius;

            if (rand <= 0)
            {
                GameObject go = Instantiate(spawn.Object, gameObject.transform.position + positionTweaker, gameObject.transform.rotation);         
                Enemy enemy = go.GetComponent<Enemy>();

                enemy.m_spawnFromPosition = transform.position;
                enemy.m_strategy = spawn.Strategy;             
            }
        }
    }
}