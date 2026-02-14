using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float atackVelocityTimer;
    
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        atackVelocityTimer = player.attackVelocityDuration;
        StepForward();
    }

    public override void Update()
    {
        base.Update();
        StopMovingForward();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void StopMovingForward()
    {
        atackVelocityTimer -= Time.deltaTime;
        // Stop moving after the timer expires
        if (atackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void StepForward()
    {
        player.SetVelocity(player.attackVelocity.x * player.facingDirection, player.attackVelocity.y);
    }
}
