using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float atackVelocityTimer;
    
    private const int FistComboIndex = 1; // We start combo index with no 1, this param is used in the animator
    private int comboIndex=1;
    private int comboIndexLimit=3;

    private float lastTimeAttacked;
    
    
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        // TODO: Change this to become a fixed list of combos
        // rename the attackVelocity to become attackStepForwardVelocities
        if (comboIndexLimit != player.attackVelocity.Length)
            comboIndexLimit = player.attackVelocity.Length;
    }

    public override void Enter()
    {
        base.Enter();
        ResetComboIndex();
        atackVelocityTimer = player.attackVelocityDuration;
        anim.SetInteger(GlobalStringsConfig.Animations.BasicAttackIndex, comboIndex);
        StepForward();
    }


    public override void Update()
    {
        base.Update();
        StopMovingForward();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        // remember time when we last attacked
        lastTimeAttacked = Time.time;
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
        Vector2 attackVelocity = player.attackVelocity[comboIndex -1];
        player.SetVelocity(attackVelocity.x * player.facingDirection, attackVelocity.y);
    }
    
    private void ResetComboIndex()
    {
        // reset combo index based on the combo window
        bool comboTimeExpired = Time.time > lastTimeAttacked + player.comboResetTimeWindow;
        if(comboIndex>comboIndexLimit || comboTimeExpired)
            comboIndex = FistComboIndex;
    }
}
