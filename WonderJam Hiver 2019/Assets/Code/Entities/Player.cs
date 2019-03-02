using UnityEngine;

public class Player : Entity
{
	[HideInInspector] public PlayerController m_playerController;

	public override void Awake()
	{
		base.Awake();

		m_playerController = GetComponent<PlayerController>();
	}

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

			m_shooter.SetPatternInfo(toFire, "forcedTarget", new Vector2(transform.position.x, transform.position.y + 1));
			m_shooter.Shoot(toFire);
		}
	}
}
