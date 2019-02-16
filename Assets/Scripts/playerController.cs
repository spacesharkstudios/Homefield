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
        Vector2 movement = new Vector2(horizontalMovement, 0);
        rb2d.AddForce(movement * moveSpeed);
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
        if (horizontalMovement == 0 && facingRight)
        {
            movement.Set(-2 * moveSpeed, 0);
        }
        if (horizontalMovement == 0 && !facingRight)
        {
            movement.Set(2 * moveSpeed, 0);
        }
    }
}
