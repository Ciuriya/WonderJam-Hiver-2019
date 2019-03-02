using UnityEngine;

public class PlayerController : MonoBehaviour {

	public CharController m_controller;

	void FixedUpdate() {
		float moveY = Game.m_keybinds.GetAxis("MoveY");

		if(moveY < 0) moveY = 0;

		Vector2 move = new Vector2(Game.m_keybinds.GetAxis("MoveX") * Time.fixedDeltaTime,
								   moveY * Time.fixedDeltaTime);

		m_controller.Move(move);
	}
}
