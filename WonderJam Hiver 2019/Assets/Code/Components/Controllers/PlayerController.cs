using UnityEngine;

public class PlayerController : MonoBehaviour {

	public CharController m_controller;

	[HideInInspector] public Player m_player;

	void FixedUpdate() 
	{
		if(!m_player) return;

		float moveY = Game.m_keybinds.GetAxis("MoveY", m_player.m_input);

		if(moveY < 0) moveY = 0;

		Vector2 move = new Vector2(Game.m_keybinds.GetAxis("MoveX", m_player.m_input) * Time.fixedDeltaTime,
								   moveY * Time.fixedDeltaTime);

		m_controller.Move(move);
	}
}
