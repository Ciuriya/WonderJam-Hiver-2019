using UnityEngine;

[System.Serializable]
public abstract class EnnemyBehavior
{
    public abstract void Move(Transform own_transform);

    public abstract void Attack();
}
