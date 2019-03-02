using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Vitesse de déplacement du joueur.")]
    public float m_speed;

    [Tooltip("Force de la gravité qui attire le joueur vers le bas. Doit être négatif.")]
    public float m_downwardVelocity;

    private Rigidbody2D m_rigidBody2D;
    private Vector3 m_input_Direction;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float m_yAxisInput = Game.m_keybinds.GetAxis("MoveY") * 1.5f;

        m_input_Direction = Vector3.zero;
        m_input_Direction.x = Game.m_keybinds.GetAxis("MoveX");

        //Met le deplacement vertical à 0 si le joueur essaie de se déplacer vers le bas
        if (m_yAxisInput < 0)
            m_input_Direction.y = 0;

        else
            m_input_Direction.y = m_yAxisInput;

        MoveCharacter();
    }

    void MoveCharacter()
    {
        //Applique une vélocité vers le bas constante
        m_input_Direction.y += m_downwardVelocity;

        //Déplacement du joueur selon la direction et la vitesse demandée
        m_rigidBody2D.MovePosition(transform.position + m_input_Direction * m_speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D p_collider)
    {
        //Tue l'objet joueur s'il entre en contact avect un objet ayant le tag Enemy
        if (p_collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
