using UnityEngine;
using UnityEngine.Serialization;

public class JumpState : MonoBehaviour, IPlayerState
{
    private Animator animator;
    private Rigidbody2D rb;
    public PlayerStateMachine stateMachine;
    [FormerlySerializedAs("inputHandler")] public PlayerStateInputs_old inputOldHandler;
    private bool facingRight = true;
    private bool isGrounded = false;


    [SerializeField] private float _maxMovementVelocity = 5f;

    [SerializeField] private float jumpLoad = 3f;
    [SerializeField] private float fallLoad = 4f;

    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();
        inputOldHandler = GetComponent<PlayerStateInputs_old>();
    }

    public void EnterState()
    {
        isGrounded = false;
        animator.Play("jump_animation");
        rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }

    public void UpdateState()
    {
        if (isGrounded)
        {
            stateMachine.SetState(GetComponent<LandingState>());
        }
        
        if (rb.velocity.y <= 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * fallLoad * Time.deltaTime);
        }
        
        // // if rising and space hold down
        // else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        // {
        //     rb.velocity += Vector2.up * (Physics2D.gravity.y * _jumpRiseVelDec * Time.deltaTime);
        // }
        
        // if rising but space not hold down
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * (float)(Physics2D.gravity.y * jumpLoad * Time.deltaTime);
        }
  
        if (inputOldHandler.MoveInputValue.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (rb.velocity.x < _maxMovementVelocity)
            {
                float speedDifference = Mathf.Abs(_maxMovementVelocity - rb.velocity.x);
                rb.velocity += new Vector2(speedDifference, 0);
            }
        } else if (inputOldHandler.MoveInputValue.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            if (rb.velocity.x > -_maxMovementVelocity)
            {
                float speedDifference = Mathf.Abs(_maxMovementVelocity + rb.velocity.x);
                rb.velocity += new Vector2(-speedDifference, 0);
            }
        }
        if (inputOldHandler.attackSquareActionTriggered)
        {
            stateMachine.SetState(GetComponent<SquareAttackState>());
        }
        if (inputOldHandler.attackTriangleActionTriggered)
        {
            stateMachine.SetState(GetComponent<TriangleAttackState>());
        }
    }
    
    public void ExitState()
    {

    }
    private void FlipPlayer()
    {
        facingRight = !facingRight;

        Vector3 newRotation = transform.eulerAngles;
        
        newRotation.y += 180f;
        
        transform.eulerAngles = newRotation;
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}