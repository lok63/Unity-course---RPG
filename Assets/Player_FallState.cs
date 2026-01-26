using UnityEngine;

public class Player_FallState : EntityState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        // if player detecting the ground below, go to idle state
        if(player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
