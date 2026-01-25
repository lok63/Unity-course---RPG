using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    
    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        
        anim = player.anim;
        rb = player.rb;
    }
    
    public virtual void Enter()
    {
        //we call this every time the state changes
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        // Run the logic of the state
        Debug.Log("I updated " + animBoolName);
    }

    public virtual void Exit()
    {
        // this will be called every time we exit state and change to a new one
        anim.SetBool(animBoolName, false);

    }
}
