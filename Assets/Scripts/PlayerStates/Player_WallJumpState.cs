using UnityEngine;

public class Player_WallJumpState : EntityState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // we have to multiply by the facing directions to ensure the player jumps on the opposite direction of the wall
        var xVelocity = player.wallJumpForce.x * -player.facingDirection;
        player.SetVelocity(xVelocity, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();
        
        if(rb.linearVelocity.y  < 0)
            stateMachine.ChangeState(player.fallState);
        
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
        
    }
}
