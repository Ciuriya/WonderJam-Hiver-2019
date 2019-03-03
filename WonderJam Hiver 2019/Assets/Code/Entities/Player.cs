using UnityEngine;
using System.Collections;

public class Player : Entity
{
	public GameEvent m_OnPlayerDeath;

	[HideInInspector] public int m_playerId; // starts at 1
	[HideInInspector] public InputUser m_input;
	[HideInInspector] public PlayerController m_playerController;

    [Tooltip("The time for which the player is invincible, in seconds")]
    [Range(0, 60)] public float m_InvincibilityTime = 3f;

    [Tooltip("The time for which the player have a speed boost, in seconds")]
    [Range(0, 60)] public float m_SpeedBoostTime = 3f;

    [Tooltip("The speed boost added to the speed of the player")]
    [Range(0, 60)] public float m_SpeedBoost = 3f;

    public ValueManager m_lifeManager;

    public override void Start()
	{
		base.Start();

		m_playerController = GetComponent<PlayerController>();
		m_playerController.m_player = this;
	}

	void Update() 
	{
		string fireButton = "Primary Fire";

		if(m_input.m_controllerId > 0) fireButton += m_input.m_controllerId;

		bool fire = Game.m_keybinds.GetButton(fireButton, m_input);

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
        if(m_canDie == true && collision.collider.tag == "Enemy" )
        {
            Kill();
        }
        else if(m_canDie == false && collision.collider.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<Entity>().Kill();
        }
    }

	public bool Spawn() 
	{
		GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnP" + m_playerId);

		gameObject.transform.position = spawnPoint.transform.position;

		return true;
	}

	public void Despawn() 
	{ 
		Destroy(gameObject);
	}

    //Override pour eviter que le player devienne Dead (On ne peut plus le tuer s'il est dead)
    public override void Kill()
    {
        Explosion();
        m_lifeManager.UpdateValue(-1);
        Die();
    }

    protected override void Die()
    {
        Spawn();
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

    public void AddLife()
    {
        m_lifeManager.UpdateValue(+1);
    }
}
