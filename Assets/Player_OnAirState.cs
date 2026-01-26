using UnityEngine;

public class Player_OnAirState : EntityState
{
    public Player_OnAirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        // allow the character to move while they are on air or after jumping
        var xVelocity = player.moveInput.x * (player.moveSpeed * player.inAirMultiplier);
        var yVelocity = rb.linearVelocity.y;
        if(player.moveInput.x != 0 )
            player.SetVelocity(xVelocity, yVelocity);
    }
}
