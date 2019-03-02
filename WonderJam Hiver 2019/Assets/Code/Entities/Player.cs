using UnityEngine;

public class Player : Entity
{
	void Start() 
	{ 
		Game.m_keybinds.m_entity = this;
	}

	void Update() 
	{
		bool fire = Game.m_keybinds.GetButton("Primary Fire");

		if(fire) 
		{
			ShotPattern toFire = m_shooter.GetCurrentPattern();

			if(toFire == null) return;

			//m_shooter.SetPatternInfo(toFire, "forcedTarget", (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition));
			m_shooter.Shoot(toFire);
		}
	}
}
