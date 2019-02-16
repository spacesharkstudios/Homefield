using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    public float moveSpeed;
    private bool facingRight = true;
    public float VI;
    private Vector3 m_velocity = Vector3.zero;
    public float gravity = .5f;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = 10;
        VI = 3;
    }

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalMovement * moveSpeed, -(rb2d.gravityScale) * gravity);
        //rb2d.AddForce(movement * moveSpeed);
        rb2d.velocity = movement;
        if (horizontalMovement == -1)
        {
            facingRight = false;
        }
        if (horizontalMovement == 1)
        {
            facingRight = true;
        }
        if (!facingRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
