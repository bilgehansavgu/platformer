using UnityEngine;

public class SquareAttackState : MonoBehaviour, IPlayerState
{
    private Animator animator;
    private Rigidbody2D rb;
    public PlayerStateMachine stateMachine;
    public PlayerStateInputs inputHandler;

    private string groundedAttackAnimation = "chain_punch_R_animation";
    private string aerialAttackAnimation = "jump_and_ground_slam_R_animation";

    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer; // Layer mask for enemies
    
    [SerializeField] private float attackWidth = 4f;
    [SerializeField] private float attackHeight = 2f;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float knockbackForce = 10f;
    
    public bool isAttacking = false; 
    private bool hasDealtDamage = false; // Flag to track if damage has been dealt

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();
        inputHandler = GetComponent<PlayerStateInputs>();
    }

    public void EnterState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Debug.Log("SquareAttackState");
            
            if (IsGrounded())
            {
                animator.Play(groundedAttackAnimation);
            }
            else
            {
                animator.Play(aerialAttackAnimation);
                rb.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
            }
        }
    }

    public void UpdateState()
    {
        CheckForEnemyCollision();
    }

    public void ExitState()
    {
        attackCollider.enabled = false;
        isAttacking = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
    public void OnAnimationFinished()
    {
        Debug.Log("Square Attack finished.");
        isAttacking = false;
        stateMachine.SetState(GetComponent<IdleState>());
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null && !hit.collider.isTrigger;
    }
    private void CheckForEnemyCollision()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(attackWidth, attackHeight), 0, Vector2.zero, 0, enemyLayer);
        foreach (RaycastHit2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(0);
            }
        }
    }
}