using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, GlobalStringsConfig.Animations.Idle);
        moveState = new Enemy_MoveState(this, stateMachine, GlobalStringsConfig.Animations.Move);
        attackState = new Enemy_AttackState(this, stateMachine, GlobalStringsConfig.Animations.Attack);
       battleState = new Enemy_BattleState(this, stateMachine, GlobalStringsConfig.Animations.Battle);
    }

    override protected void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }
}
