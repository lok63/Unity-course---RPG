using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;

    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }
    
    public virtual void Enter()
    {
        //we call this every time the state changes
        Debug.Log("I entered " + stateName);
    }

    public virtual void Update()
    {
        // Run the logic of the state
        Debug.Log("I updated " + stateName);
    }

    public virtual void Exit()
    {
        // this will be called every time we exit state and change to a new one
        Debug.Log("I exited " + stateName);
    }
}
