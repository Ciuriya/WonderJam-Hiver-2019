using UnityEngine;

public static class Vector2Extension 
{
	public static Vector2 Rotate(this Vector2 p_vector, float p_degrees) 
	{
		float radians = p_degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);

		float tx = p_vector.x;
		float ty = p_vector.y;

		return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
	}

	public static Vector2 Center(this Vector2 p_first, Vector2 p_second)
	{ 
		Vector2 diff = new Vector2(p_first.x - p_second.x, p_first.y - p_second.y);
		Vector2 lowest = p_first.x <= p_second.x && p_first.y <= p_second.y ? p_first : p_second;

		return new Vector2(lowest.x + diff.x / 2, lowest.y + diff.y / 2);
	}
}