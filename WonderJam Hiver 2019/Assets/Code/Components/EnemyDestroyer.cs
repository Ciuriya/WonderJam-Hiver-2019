using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    [Tooltip("Event that is raised when an enemy reaches the bottom of the screen")]
    public GameEvent m_OnEnemyPassed;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            m_OnEnemyPassed.Raise();
            Destroy(collision.gameObject);
        }
    }
}
