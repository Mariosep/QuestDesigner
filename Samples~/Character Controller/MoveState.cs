using UnityEngine;

public class MoveState : FirstPersonState
{
    
    public MoveState(FirstPersonStateMachine stateMachine) : base(stateMachine) {}
    
    public override void Enter()
    {
        base.Enter();
        
        sm.Velocity.y = Physics.gravity.y;
        
        sm.inputManager.OnJumpPerformed += SwitchToJumpState;
    }

    public override void Update()
    {
        if (!sm.CharacterController.isGrounded)
        {
            sm.ChangeState(new FallState(sm));
        }
        
        CalculateMoveDirection();
        Move();
    }

    public override void Exit()
    {
        sm.inputManager.OnJumpPerformed -= SwitchToJumpState;
    }

    private void SwitchToJumpState()
    {
        sm.ChangeState(new JumpState(sm));    
    }
    
    public override void EvaluateTransition()
    {
        
    }
}