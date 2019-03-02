using UnityEngine;
using System.Collections;

public class Player : Entity
{
	[HideInInspector] public PlayerController m_playerController;
    public GameEvent m_OnPlayerDeath;
    [Range(0, 60)] public float m_InvincibilityTime = 3f;
    [Range(0, 60)] public float m_SpeedBoostTime = 3f;
    [Range(0, 60)] public float m_SpeedBoost = 3f;

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
        if(collision.collider.tag == "Enemy" && m_canDie == true)
        {
            Kill();
        }
        else if(collision.collider.tag == "Enemy" && m_canDie == false)
        {
            Destroy(collision.collider.gameObject);
        }
    }

    //Override pour eviter que le player devienne Dead (On ne peut plus le tuer s'il est dead)
    public override void Kill()
    {
        Die();
    }

    protected override void Die()
    {
            //On teleporte le joueur a une position de spawn (Setuper arbitrairement pour le moment)
            gameObject.transform.position = new Vector3(0, -2, 0);
            m_OnPlayerDeath.Raise();
            Debug.Log("U got murdered");

    }

    public void AddInvincibility()
    {
        StartCoroutine(Invincibility());
    }

    public IEnumerator Invincibility()
    {
        m_canDie = false;
        yield return new WaitForSeconds(m_InvincibilityTime);
        m_canDie = true;
    }

    public void AddSpeed()
    {
        StartCoroutine(Speed());
    }

    public IEnumerator Speed()
    {
        m_controller.m_speed += m_SpeedBoost;
        yield return new WaitForSeconds(m_SpeedBoostTime);
        m_controller.m_speed -= m_SpeedBoost;
    }
}
