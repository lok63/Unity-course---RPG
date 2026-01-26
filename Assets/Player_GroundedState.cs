using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        // When character drops we need to display the falling animation
        if (rb.linearVelocity.y < -0.1f && player.coyoteTimer <= 0f)
            stateMachine.ChangeState(player.fallState);
        
        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);
    }
}
