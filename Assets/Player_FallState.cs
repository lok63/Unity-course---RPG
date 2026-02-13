using UnityEngine;

public class Player_FallState : Player_OnAirState
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
        
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
