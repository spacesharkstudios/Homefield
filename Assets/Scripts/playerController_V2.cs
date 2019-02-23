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
    private float drag;
    public bool grounded;
    const float groundedRadius = 0.2f;
    public float gravityScale;
    public float maxAirSpeed = 100f;
    public float maxGroundSpeed;
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
        drag = rb2d.drag;

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
            animator.SetBool("isJumping", false);

            rb2d.velocity = new Vector2(move * movementSpeed * Time.fixedDeltaTime, rb2d.velocity.y);

            if (Mathf.Abs(rb2d.velocity.x) > 0)
                animator.SetBool("isRunning", true);
            if (rb2d.velocity.x == 0)
                animator.SetBool("isRunning", false);
        }
        if (!grounded && !launched)
        {
            //rb2d.AddForce(new Vector2(move * movementSpeed * airMove * Time.fixedDeltaTime, 0));
            if (Mathf.Abs(rb2d.velocity.x) <= maxAirSpeed)
            {
                rb2d.AddForce(new Vector2(move * (movementSpeed * 2f) * Time.fixedDeltaTime, 0));
            }
            else
            {
                rb2d.velocity = new Vector2(maxAirSpeed * (Mathf.Abs(rb2d.velocity.x) / rb2d.velocity.x), rb2d.velocity.y);
            }
            if(rb2d.velocity.y < 0)
            {
                rb2d.gravityScale = gravityScale * 2f;
            }
            if (rb2d.velocity.y >= 0)
            {
                rb2d.gravityScale = gravityScale;
            }

            animator.SetBool("isJumping", true);
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
