using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isChasing;
    [SerializeField] private Animator animator;

    [Header("Enemy Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private int facingDirection = 1;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCoolDown = 1f;
    [SerializeField] private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(player.position,transform.position);

        if(distance <= attackRange)
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isChasing", false);
            if (!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
            return;
        }

        if(isChasing == true)
        {
            if(player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
            {
                Flip();
            }
        }

        if(isChasing == true)
        {
            Chase();
        }
    }

    void Chase()
    {
        animator.SetBool("isChasing", true);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        
        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position,transform.position);
            if(distance > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCoolDown);
        }
        isAttacking = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach(Collider2D player in hitPlayer)
        {
            Debug.Log("Attacking");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isChasing", false);
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
