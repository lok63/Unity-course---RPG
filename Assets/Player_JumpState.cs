using Unity.VisualScripting;
using UnityEngine;

public class Player_JumpState : Player_OnAirState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if(rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}
