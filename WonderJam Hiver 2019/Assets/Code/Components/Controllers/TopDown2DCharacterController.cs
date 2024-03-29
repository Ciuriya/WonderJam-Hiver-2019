﻿using UnityEngine;

public class TopDown2DCharacterController : CharController 
{
	protected override void OnAwake() 
	{
		m_rigidbody2D.gravityScale = 0f;
	}

	public override void Move(Vector2 p_move) {
		float targetX = p_move.x * (m_speed / Time.fixedDeltaTime);
		float targetY = p_move.y * (m_speed / Time.fixedDeltaTime);
		float totalSpeed = Mathf.Abs(targetX) + Mathf.Abs(targetY);

		// scale back target X and Y speeds to fit within the speed limit
		if (totalSpeed > m_speed) {
			float scaledDiagonalSpeed = (m_speed * 2) * m_diagonalSpeedPercentage;
			float scaledX = scaledDiagonalSpeed * (Mathf.Abs(targetX) / totalSpeed);
			float scaledY = scaledDiagonalSpeed * (Mathf.Abs(targetY) / totalSpeed);

			// make sure it keeps the orientation after the math is done
			if(targetX < 0) scaledX *= -1;
			if(targetY < 0) scaledY *= -1;

			targetX = scaledX;
			targetY = scaledY;
		}

		Vector3 targetVelocity = new Vector2(targetX + m_rigidbody2D.velocity.x / 2, targetY + m_rigidbody2D.velocity.y / 2) - new Vector2(0, m_downwardVelocity);

		m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetVelocity, ref m_velocity, m_smoothTime);
	}
}
