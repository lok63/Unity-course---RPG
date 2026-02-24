using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState  { get; private set; }
    public Player_JumpState jumpState  { get; private set; }
    public Player_FallState fallState  { get; private set; }
    public Player_WallSlideState wallSlideState  { get; private set; }
    public Player_WallJumpState wallJumpState  { get; private set; }
    public Player_DashState dashState  { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    
    
    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration=.1f;
    public float comboResetTimeWindow = 1;
    private Coroutine queuedAttackCo;
    
    [Header("Movement Details")]
    public float moveSpeed= 8;
    public float jumpForce = 12;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMultiplier = 0.65f; // should be from 0-1
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.3f;
    public Vector2 moveInput { get; private set; }
    [Space]
    public float dashDuration = 0.15f;
    public float dashSpeed = 25;

    protected override void Awake()
    {
        base.Awake();
        
        input = new PlayerInputSet();
        
        idleState = new Player_IdleState(this, stateMachine, GlobalStringsConfig.Animations.Idle);
        moveState = new Player_MoveState(this, stateMachine, GlobalStringsConfig.Animations.Move);
        jumpState = new Player_JumpState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        fallState = new Player_FallState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        wallSlideState = new Player_WallSlideState(this, stateMachine, GlobalStringsConfig.Animations.WallSlide);
        wallJumpState = new Player_WallJumpState(this, stateMachine, GlobalStringsConfig.Animations.JumpFall);
        dashState = new Player_DashState(this, stateMachine, GlobalStringsConfig.Animations.Dash);
        basicAttackState = new Player_BasicAttackState(this, stateMachine, GlobalStringsConfig.Animations.BasicAttack);
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, GlobalStringsConfig.Animations.JumpAttack);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }
    
    public void EnterAttackStateWithDelay()
    {
        // Make sure if a coroutine is active to stop the previous one if any
        // this avoid multiple attack states happening on the same time if the players queues them together
        // and only one is been executed
        if(queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayedCo());

    }
    private IEnumerator EnterAttackStateWithDelayedCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
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

}