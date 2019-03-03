using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AI/Actions/ShootAction")]
public class ShootAction : Action
{
    public override void Execute(StateController p_controller)
    {
        p_controller.m_entity.m_shooter.Shoot(p_controller.m_entity.m_shooter.m_patternToShoot);
        p_controller.m_entity.m_shooter.SetPatternInfo(p_controller.m_entity.m_shooter.m_patternToShoot, "forcedTarget", new Vector2(p_controller.m_entity.transform.position.x, -10));
    }
}
