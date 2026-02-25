using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if(enemy.PlayerDetection())
            stateMachine.ChangeState(enemy.battleState);
    }
}
