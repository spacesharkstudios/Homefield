using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerController_V2 : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight;
    public bool grounded;
    const float groundedRadius = 0.2f;
    public float gravityScale;
    [SerializeField] public float movementSpeed = 400f;
    [SerializeField] public float jumpForce = 150f;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public Transform ceilingCheck;

    public UnityEvent onLandEvent;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScale = rb2d.gravityScale;

        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }

    // Update is called once per physics calculation
    private void FixedUpdate()
    {
        bool wasGouunded = grounded;
        grounded = false;

        // The player is "grounded" once the a circle cast overlaps anything dubbed as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGouunded)
                    onLandEvent.Invoke();
            }
        }

        Move(Input.GetAxis("Horizontal"), .75f, false, Input.GetKey(KeyCode.Space));
    }

    private void Move(float move, float airMove, bool launched, bool jump)
    {
        if (grounded && !launched)
        {
            rb2d.velocity = new Vector2(move * movementSpeed * Time.fixedDeltaTime, rb2d.velocity.y);

        }
        if (!grounded && !launched)
        {
            //rb2d.AddForce(new Vector2(move * movementSpeed * airMove * Time.fixedDeltaTime, 0));
            //rb2d.velocity = new Vector2((move * movementSpeed * airMove * Time.fixedDeltaTime) + (rb2d.velocity.x * Time.fixedDeltaTime * .5f), rb2d.velocity.y);
            if(rb2d.velocity.y < 0)
            {
                rb2d.gravityScale = gravityScale * 2f;
            }
            if (rb2d.velocity.y >= 0)
            {
                rb2d.gravityScale = gravityScale;
            }

        }
        if(jump && grounded)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }

        if (rb2d.velocity.x < 0)
            isFacingRight = true;
        if (rb2d.velocity.x > 0)
            isFacingRight = false;
        
        spriteRenderer.flipX = isFacingRight;
    }
}
