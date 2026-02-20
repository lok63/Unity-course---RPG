using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float atackVelocityTimer;
    
    private const int FistComboIndex = 1; // We start combo index with no 1, this param is used in the animator
    private int attackDir;
    private int comboIndex=1;
    private int comboIndexLimit=3;

    private float lastTimeAttacked;
    private bool comboAttackQueued;
    
    
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        // TODO: Change this to become a fixed list of combos
        // rename the attackVelocity to become attackStepForwardVelocities
        if (comboIndexLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Adjusted combo limit to match attack velocity array!");
            comboIndexLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        comboAttackQueued = false;
        atackVelocityTimer = player.attackVelocityDuration;

        ResetComboIndex();
        SetAttackDirection();
        
        anim.SetInteger(GlobalStringsConfig.Animations.BasicAttackIndex, comboIndex);
        StepForward();
    }

    private void SetAttackDirection()
    {
        if (player.moveInput.x != 0)
            attackDir = (int)player.moveInput.x;
        else
            attackDir = player.facingDirection;
    }


    public override void Update()
    {
        base.Update();
        StopMovingForward();

        if (input.Player.Attack.WasPressedThisFrame())
            // If I decide to use this instead of QueueNextAttack() this won't add the idle anim after we finish the combo
            // and it will allow th user to spam the combo. 
            // comboAttackQueed = true; 
            QueueNextAttack(); // only queue if there are more attacks left in the combo, this will make sure the idle is played when attack 
        if (triggerCalled)
            HandleStateExit();
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            // When we trigger ChangeState it calls Exit() and Enter() on the same frame
            // so we want to make the anim bool on this frame 
            anim.SetBool(animBoolName, false);
            // After we call the coroutine it will call ChangeState again so there
            // it will set back the animBoolName to true
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        // remember time when we last attacked
        lastTimeAttacked = Time.time;
    }

    /**
    * this will make sure the idle is played when attack combo list is completed
    */
    private void QueueNextAttack()
    {
        //Chain the next attack if there is another combo in the index and we are not in the last attack
        if (comboIndex < comboIndexLimit)
            comboAttackQueued = true;
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
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
    
    private void ResetComboIndex()
    {
        // reset combo index based on the combo window
        bool comboTimeExpired = Time.time > lastTimeAttacked + player.comboResetTimeWindow;
        if(comboIndex>comboIndexLimit || comboTimeExpired)
            comboIndex = FistComboIndex;
    }
}
