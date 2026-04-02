using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private int facingDirection = 1;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    [Header("Crouch Settings")]
    [SerializeField] private Vector2 normalColliderSize;
    [SerializeField] private Vector2 crouchColliderSize;
    [SerializeField] private bool isCrouching;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Input Settings")]
    [SerializeField] private float moveInput;

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        Jump();

        if(moveInput > 0 && transform.localScale.x < 0 || moveInput < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        HandleAnimations();
    }

    private void FixedUpdate()
    {
        Duck();

        DuckUp();

        Move();

        Attack();

        CrouchAttack();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Duck()
    {
        if(Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            isCrouching = true;
            capsuleCollider.size = crouchColliderSize;
            animator.SetBool("isCrouching", true);
        }
    }

    void DuckUp()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
            capsuleCollider.size = normalColliderSize;
            animator.SetBool("isCrouching", false);
        }
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach(Collider2D enemy in hitEnemies)
            {
                // Implement damage to enemy here
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    void CrouchAttack()
    {
        if(Input.GetKeyDown(KeyCode.K) && isCrouching)
        {
            animator.SetTrigger("CrouchAttack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach(Collider2D enemy in hitEnemies)
            {
                // Implement damage to enemy here
                Debug.Log("Crouch Attack Hit " + enemy.name);
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void HandleAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isJumping", rb.linearVelocity.y > 0.1f);
    }
}