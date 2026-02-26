using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;
    
    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;
    
    [Header("Collision Detection")] 
    [SerializeField] protected LayerMask groundType;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float wallCheckDistance; 
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    
    public float coyoteTimer { get; private set; }
    

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        stateMachine = new StateMachine();
        
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }
    
    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        // if moving right and not facing right then want to flip transform
        // if moving left and facing right then we need to flip transform
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight == true)
            Flip();
    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        // We draw a gizmo when we try to detect walls and this line has to flip as well 
        facingDirection *= -1;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundType);
        coyoteTimer = groundDetected ? coyoteTime : coyoteTimer - Time.deltaTime;
        var direction = Vector2.right * facingDirection; // ensure we change direction when we flip

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, direction, wallCheckDistance, groundType)
                           && Physics2D.Raycast(secondaryWallCheck.position, direction, wallCheckDistance, groundType);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, direction, wallCheckDistance, groundType);
    }
    protected virtual void OnDrawGizmos()
    {
        var  startPoint = groundCheck.position;
        var groundEndPoint = groundCheck.position + new Vector3(0, -groundCheckDistance, 0);
        
        Gizmos.DrawLine(startPoint,groundEndPoint );
        
        // we multiply with facing direction to make sure we flip the line whenever the character flips
        var wallEndPointPrimary = primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0, 0);
        Gizmos.DrawLine(primaryWallCheck.position, wallEndPointPrimary);
        
        if (secondaryWallCheck != null)
        {
            var wallEndPointSecondary = secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0, 0);
            Gizmos.DrawLine(secondaryWallCheck.position, wallEndPointSecondary);
        }
    }
}
