using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour 
{
	[Tooltip("Whether or not this entity's health is local or shared")]
	public bool m_usingLocalHealth;

	[Tooltip("The entity's current health amount")]
	[ConditionalField("m_usingLocalHealth", "false")]
	public IntReference m_sharedHealth;

	[Tooltip("The entity's current health amount")]
	[ConditionalField("m_usingLocalHealth", "true")]
	public int m_localHealth;

	[Tooltip("The entity's maximum health")]
	public IntReference m_maxHealth;

	[Tooltip("The time after taking damage where the unit is immune to damage")]
	[Range(0, 2)] public float m_immunityWindow;
	private float m_lastHit;

	[Tooltip("Event called when the entity takes damage")]
	public UnityEvent m_damageEvent;

	[HideInInspector] public Entity m_entity;

	public void Init(Entity p_entity) 
	{
		m_entity = p_entity;
        if (m_usingLocalHealth)
        {
            m_localHealth = m_maxHealth.Value;
        }
	}

	public int GetHealth() 
	{
		if(m_usingLocalHealth) return m_localHealth;
		else return m_sharedHealth.Value;
	}

	public int GetMaxHealth() 
	{ 
		return m_maxHealth.Value;
	}

	private void SetHealth(int p_value) 
	{
		int value = p_value;

		if(value > GetMaxHealth()) value = GetMaxHealth();
		else if(value < 0) value = 0;

		if(m_usingLocalHealth) m_localHealth = value;
		else m_sharedHealth.Value = value;
	}

	public bool IsImmune() 
	{
		return Time.time * 1000 < m_lastHit + m_immunityWindow * 1000;
	}

	public void Damage(int p_amount, bool p_bypassImmunityWindow) 
	{
		if(!p_bypassImmunityWindow && IsImmune()) return;

		SetHealth(GetHealth() - p_amount);

		m_lastHit = Time.time * 1000;
		m_damageEvent.Invoke();

		if(GetHealth() <= 0) m_entity.Kill();
	}
}
