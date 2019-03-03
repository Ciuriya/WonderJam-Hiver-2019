using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="MoveStrategy/Wavy")]
public class Wavy : MoveStrategy
{
    public float speed;
    public float frequency;
    public float magnitude;

    public override Vector2 GetMovementVector(Enemy enemy)
    {
        Vector3 pos = enemy.transform.position;
        float angle = Mathf.Sin((Time.time - enemy.m_spawnTime) * frequency + Mathf.PI / 2);

        enemy.transform.position += Vector3.down * speed * Time.deltaTime + Vector3.right * angle * magnitude;

        return pos;
    }
}
