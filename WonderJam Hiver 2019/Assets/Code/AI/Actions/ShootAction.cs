using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AI/Actions/ShootAction")]
public class ShootAction : Action
{
    public override void Execute(StateController p_controller)
    {
		ShotPattern pattern = p_controller.m_entity.m_shooter.m_patternToShoot;
		object active = p_controller.m_entity.m_shooter.GetPatternInfo(pattern, "active");

		if (active == null || !((bool) active)) 
		{
			p_controller.m_entity.m_shooter.Shoot(pattern);
			p_controller.m_entity.m_shooter.SetPatternInfo(p_controller.m_entity.m_shooter.m_patternToShoot, "forcedTarget", new Vector2(p_controller.m_entity.transform.position.x, -10));
		}
    }
}
