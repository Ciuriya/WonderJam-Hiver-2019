using UnityEngine;

[CreateAssetMenu(menuName="MoveStrategy/StraightDown")]
public class StraightDown : MoveStrategy
{
    public override Vector2 GetMovementVector(Enemy enemy)
    {
        return new Vector2(0, 0);
    }
}
