using UnityEngine;

public class Player : Entity
{
	[HideInInspector] public PlayerController m_playerController;
    public GameEvent m_OnPlayerDeath;

    public ValueManager m_lifeManager;


    public override void Start()
	{
		base.Start();

		m_playerController = GetComponent<PlayerController>();
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Kill();
        }
    }

    //Override pour eviter que le player devienne Dead (On ne peut plus le tuer s'il est dead)
    public override void Kill()
    {
        m_lifeManager.UpdateValue(-1);
        Die();
    }

    protected override void Die()
    {
        //On teleporte le joueur a une position de spawn (Setuper arbitrairement pour le moment)
        gameObject.transform.position = new Vector3(0,-2,0);

        m_OnPlayerDeath.Raise();
        Debug.Log("U got murdered");
    }
}
