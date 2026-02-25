using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        
        rb = enemy.rb;
        anim = enemy.anim;
    }

    public override void Update()
    {
        base.Update();
        
        if(Input.GetKeyDown(KeyCode.F))
            stateMachine.ChangeState(enemy.attackState);
        
        anim.SetFloat(GlobalStringsConfig.Animations.moveAnimSpeedMultiplier, enemy.moveAnimSpeedMultiplier);
        anim.SetFloat(GlobalStringsConfig.Animations.xVelocity,rb.linearVelocity.x);
    }
}
