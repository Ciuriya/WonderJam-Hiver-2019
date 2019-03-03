using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Entity : MonoBehaviour 
{
	[Tooltip("The entity's movement controller")]
	public CharController m_controller;

	[Tooltip("If this entity can die")]
	public bool m_canDie;
	[HideInInspector] public bool m_isDead;

	[Tooltip("The entity's viewing variables, only set if this entity uses AI, otherwise leave it empty")]
	public Look m_look;

	[Tooltip("All runtime sets this entity is a part of")]
	public List<EntityRuntimeSet> m_runtimeSets;

	[Tooltip("Event called when the entity dies")]
	public UnityEvent m_deathEvent;

	[Tooltip("Particle Effect to be used when the entity dies")]
    public GameObject m_particleSystem;

    public SimpleAudioEvent m_pickupAudioEvent;
    public SimpleAudioEvent m_deathSoundEvent;

    [HideInInspector] public AudioSource m_audioSource;
    [HideInInspector] public SpriteRenderer m_renderer;
	[HideInInspector] public Shooter m_shooter;
	[HideInInspector] public CollisionRelay m_collisionRelay;
	[HideInInspector] public UnitHealth m_health;
	[HideInInspector] public StateController m_ai;
	[HideInInspector] public GameObject m_currentParticleSystem;

    public virtual void Start()
    {
        m_health = GetComponent<UnitHealth>();
        m_shooter = GetComponent<Shooter>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_audioSource = gameObject.AddComponent<AudioSource>();

        Game.m_audio.AddAudioSource(m_audioSource, AudioCategories.SFX);

        if (m_shooter) m_shooter.Init(this);
	}

	public virtual void OnEnable() 
	{ 
		foreach(EntityRuntimeSet set in m_runtimeSets)
			set.Add(this);
	}

	void OnDisable() 
	{
		foreach(EntityRuntimeSet set in m_runtimeSets)
			set.Remove(this);
	}

	public void Damage(Entity p_damager, int p_damage, bool p_bypassImmunityWindow)
	{
		if(m_health) 
		{
			int finalDamage = p_damage; // calculate defense, resistances, etc. and compile it into this variable

			if(finalDamage > 0) 
			{
				m_health.Damage(finalDamage, p_bypassImmunityWindow);
			}
		}

		// make sure the AI starts targeting its last damager
		if(m_ai && p_damager) m_ai.m_target = p_damager;

        OnDamage();
    }

    public virtual void OnDamage() {}


	public virtual void Kill() 
	{
		if(m_canDie && !m_isDead) 
		{
		    Explosion();
			m_isDead = true;
			Die();
		}
	}

	protected virtual void Die() 
	{
		m_deathSoundEvent.Play();
		m_deathEvent.Invoke();
		Destroy(m_currentParticleSystem);
        Destroy(gameObject);
	}

	//Play an explosion particle effect for the entity
    protected virtual void Explosion()
    {
        if(m_particleSystem != null)
			m_currentParticleSystem = Instantiate(m_particleSystem, transform.position, transform.rotation);
    }
}

