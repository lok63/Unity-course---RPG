using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    protected float stateTimer;
    protected bool triggerCalled;
    
    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }
    
    public virtual void Enter()
    {
        //we call this every time the state changes
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        anim.SetFloat(GlobalStringsConfig.Animations.yVelocity, rb.linearVelocity.y);
        
        if(input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public virtual void Exit()
    {
        // this will be called every time we exit state and change to a new one
        anim.SetBool(animBoolName, false);

    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    private bool CanDash()
    {
        if (player.wallDetected )
            return false;
        if (stateMachine.currentState == player.dashState)
            return false;
        return true;
    }
}
