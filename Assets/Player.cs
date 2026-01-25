using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;
    private StateMachine stateMachine;
    
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState  { get; private set; }

    public Vector2 moveInput { get; private set; }
    private void Awake()
    {
        stateMachine = new StateMachine();
        input = new PlayerInputSet();
        
        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        
    }

    private void OnEnable()
    {
        // input.Player.Movement.started -> input just began/ when we first press the button
        // input.Player.Movement.performed -> when we hold the button 
        // input.Player.Movement.canceled -> when we release the button
        input.Enable();
        
        input.Player.Movement.performed +=ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled +=ctx => moveInput = Vector2.zero;
        
    }

    private void OnDisable()
    {
        // when player is dead we want to disable the input
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Init(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
