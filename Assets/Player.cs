using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    public PlayerInputSet input { get; private set; }
    private StateMachine stateMachine;
    
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState  { get; private set; }
    public Player_JumpState jumpState  { get; private set; }
    public Player_FallState fallState  { get; private set; }
    public Player_WallSlideState wallSlideState  { get; private set; }
    public Player_WallJumpState wallJumpState  { get; private set; }
    public Player_DashState dashState  { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    
    [Header("Attack Details")]
    public Vector2 attackVelocity;
    public float attackVelocityDuration=.1f;
    
    
    [Header("Movement Details")]
    public float moveSpeed= 8;
    public float jumpForce = 12;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMultiplier = 0.65f; // should be from 0-1
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.3f;
    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;
    public Vector2 moveInput { get; private set; }
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;

    [Header("Collision Detection")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundType;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float wallCheckDistance; 
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    
    public float coyoteTimer { get; private set; }
    


    private void SetRBValues()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearDamping = 0.1f;
        rb.gravityScale = 3.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        // SetRBValues();
        
        stateMachine = new StateMachine();
        input = new PlayerInputSet();
        
        idleState = new Player_IdleState(this, stateMachine, GlobalStringsConfig.Animations.Idle);
        moveState = new Player_MoveState(this, stateMachine, GlobalStringsConfig.Animations.Move);
        jumpState = new Player_JumpState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        fallState = new Player_FallState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        wallSlideState = new Player_WallSlideState(this, stateMachine, GlobalStringsConfig.Animations.WallSlide);
        wallJumpState = new Player_WallJumpState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        dashState = new Player_DashState(this, stateMachine, GlobalStringsConfig.Animations.Dash);
        basicAttackState = new Player_BasicAttackState(this, stateMachine, GlobalStringsConfig.Animations.BasicAttack);
    }

    private void OnEnable()
    {
        input.Enable();
        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Init(idleState);
    }

    private void Update()
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

    private void HandleFlip(float xVelocity)
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
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundType);
        coyoteTimer = groundDetected ? coyoteTime : coyoteTimer - Time.deltaTime;
        
        var direction = Vector2.right * facingDirection; // ensure we change direction when we flip
        wallDetected = Physics2D.Raycast(transform.position, direction, wallCheckDistance, groundType);
    }
    private void OnDrawGizmos()
    {
        var  startPoint = transform.position;
        var groundEndPoint = transform.position + new Vector3(0, -groundCheckDistance, 0);
        
        Gizmos.DrawLine(startPoint,groundEndPoint );
        
        // we multiply with facing direction to make sure we flip the line whenever the character flips
        var wallEndPoint = transform.position + new Vector3(wallCheckDistance * facingDirection, 0, 0);
        Gizmos.DrawLine(startPoint, wallEndPoint);
    }
}