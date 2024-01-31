public class FallState : FirstPersonState
{
    public FallState(FirstPersonStateMachine sm) : base(sm) {}

    public override void Enter()
    {
        base.Enter();
        
        sm.Velocity.y = 0f;
    }

    public override void Update()
    {
        ApplyGravity();
        CalculateMoveDirection();
        Move();
        
        if(sm.CharacterController.isGrounded)
            sm.ChangeState(new MoveState(sm));
    }

    public override void Exit() {}


    public override void EvaluateTransition()
    {
        
    }
}