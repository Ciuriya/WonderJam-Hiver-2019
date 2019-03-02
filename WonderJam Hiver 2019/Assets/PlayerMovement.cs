using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;
    private Vector3 change;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float yAxis = Input.GetAxisRaw("Vertical");

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");

        if (yAxis >= 0)
            change.y = yAxis;

        else
            change.y = 0;

        Debug.Log(change);

       
            change.y += -0.4f;
            MoveCharacter();
      
    }

    void MoveCharacter()
    {
        rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        rb.angularVelocity = 0;
        if (col.gameObject.CompareTag("Enemy"))
        Destroy(gameObject);
        Debug.Log("OnCollisionEnter2D");
    }
}
