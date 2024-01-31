using UnityEngine;

public class JumpState : FirstPersonState
{
    public JumpState(FirstPersonStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        sm.Velocity = new Vector3(sm.Velocity.x, sm.JumpForce, sm.Velocity.z);
    }

    public override void Update()
    {
        ApplyGravity();
        
        if(sm.Velocity.y <= 0f)
            sm.ChangeState(new FallState(sm));
        
        CalculateMoveDirection();
        Move();
    }

    public override void Exit() {}

    public override void EvaluateTransition()
    {
        
    }
}