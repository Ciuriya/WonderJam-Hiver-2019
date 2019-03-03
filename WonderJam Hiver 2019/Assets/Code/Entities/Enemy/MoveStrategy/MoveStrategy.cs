using UnityEngine;

[System.Serializable]
public abstract class MoveStrategy : ScriptableObject
{   
    public abstract Vector2 GetMovementVector(Enemy enemy);
}