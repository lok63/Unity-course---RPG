using Unity.VisualScripting;
using UnityEngine;

public class Player_DashState : EntityState
{
    private float originalGravityScale;
    private int dashDirection;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        SetDashDirection();
        
        stateTimer = player.dashDuration;
        // if we don't cache the original gravity scale and reset it on the exit
        // we want the character to have the correct gravity scale so as soon as it ends dashing
        // to drop to the ground if mid air
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }
    
    private void SetDashDirection()
    {
        if (player.moveInput.x != 0)
            dashDirection = (int)player.moveInput.x;
        else
            dashDirection = player.facingDirection;
    }


    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSpeed * dashDirection, 0);
        
        if (stateTimer < 0)
            if (player.groundDetected) 
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
            
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }
    
    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if(player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }   
    }
}
