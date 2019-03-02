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
            m_pressureManager.UpdateValue(collision.gameObject.GetComponent<Enemy>().m_pressure);

            m_OnEnemyPassed.Raise();
            Destroy(collision.gameObject);
        }
    }
}
