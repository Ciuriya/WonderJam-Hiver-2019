using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
	[Tooltip("The power to give when the player collides with this object")]
	public PowerUp m_powerUpToGive;

	void OnTriggerEnter2D(Collider2D p_collider)
	{
		if(p_collider.tag == "Player")
		{
			m_powerUpToGive.Use(p_collider.gameObject.GetComponent<Shooter>());
			Destroy(gameObject);
		}
	}
}
