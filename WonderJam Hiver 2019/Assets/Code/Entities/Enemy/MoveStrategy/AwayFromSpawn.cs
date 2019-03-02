using UnityEngine;

[CreateAssetMenu(menuName = "MoveStrategy/AwayFromSpawn")]
public class AwayFromSpawn : MoveStrategy
{
    public float speed = .1f;
    public float maxDistance = .3f;

    public override Vector2 GetMovementVector(Enemy enemy)
    {
        Vector3 distance = new Vector2(enemy.transform.position.x - enemy.m_spawnFromPosition.x, 0);

        return (distance.magnitude < maxDistance) ? distance.normalized * speed : new Vector3(0, 0);
    }
} 