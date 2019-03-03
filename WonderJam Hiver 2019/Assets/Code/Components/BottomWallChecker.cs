using UnityEngine;

public class BottomWallChecker : MonoBehaviour
{
    [Tooltip("Event that is raised when an enemy reaches the bottom of the screen")]
    public GameEvent m_OnEnemyPassed;

    public ValueManager m_pressureManager;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            m_pressureManager.UpdateValue(enemy.m_pressure);

            if (enemy.m_pressure > 0)
                m_OnEnemyPassed.Raise();

            Destroy(collision.gameObject);
        }
    }
}
