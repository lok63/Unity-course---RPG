using Unity.VisualScripting;
using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Reset velocity when we enter the idle state
        // this will prevent the character from sliding after we finished jumping
        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        //stop running if we hit the wall and transition
        if (player.moveInput.x == player.facingDirection && player.wallDetected)
            return;
        
        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
    }
}
