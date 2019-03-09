using UnityEngine;
using System.Collections;

public class Player : Entity
{
	[HideInInspector] public int m_playerId; // starts at 1
	[HideInInspector] public InputUser m_input;
	[HideInInspector] public PlayerController m_playerController;

    [Tooltip("The time for which the player is invincible, in seconds")]
    [Range(0, 60)] public float m_InvincibilityTime = 6f;
    [Range(0, 60)] public float m_RespawnInvulnerability = 1f;

    [Tooltip("The time for which the player have a speed boost, in seconds")]
    [Range(0, 60)] public float m_SpeedBoostTime = 3f;

    [Tooltip("The speed boost added to the speed of the player")]
    [Range(0, 60)] public float m_SpeedBoost = 3f;

	[Tooltip("The spawn delay between deaths")]
	[Range(0, 2)] public float m_spawnDelay = 0.25f;

    public ValueManager m_lifeManager;

    private Flashing flashing;

    public override void Start()
	{
		base.Start();

		m_playerController = GetComponent<PlayerController>();
		m_playerController.m_player = this;

        flashing = GetComponent<Flashing>();

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

        if (m_lifeManager.m_value > 0)
        {
            m_lifeManager.UpdateValue(-1);
            StartCoroutine(Invincibility(m_RespawnInvulnerability));

            if (flashing) flashing.Flash();

            return true;
        }
        else
            return false;
	}

    //Override pour eviter que le player devienne Dead (On ne peut plus le tuer s'il est dead)
    public override void Kill()
    {
		if(m_canDie) 
		{
			Explosion();
			Die();
		}
    }

    protected override void Die()
    {
		m_deathSoundEvent.Play();

		if (m_lifeManager.m_value.Value > 0)
		{
			m_shooter.StopShooting();
            StartCoroutine(RespawnDelay());
		} 
		else
            Game.m_players.RemovePlayer(m_input, true);
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

    public IEnumerator Invincibility(float Time)
    {
        m_canDie = false;
        yield return new WaitForSeconds(Time);
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

	private IEnumerator RespawnDelay() 
	{ 
		gameObject.transform.position = new Vector3(-50000, -50000, -50000);

		yield return new WaitForSeconds(m_spawnDelay);
		Spawn();
	}

    public void AddLife()
    {
        m_lifeManager.UpdateValue(+1);
    }
}
