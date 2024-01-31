using UnityEngine;

public abstract class FirstPersonState : State
{
    protected readonly FirstPersonStateMachine sm;

    protected FirstPersonState(FirstPersonStateMachine sm)
    {
        this.sm = sm;
    }

    public override void Enter()
    {
        HideCursor();
    }

    public override void LateUpdate()
    {
        CalculateCameraRotation();
    }

    protected void CalculateMoveDirection()
    {
        Vector3 cameraForward = new Vector3(sm.MainCamera.forward.x, 0, sm.MainCamera.forward.z);
        Vector3 cameraRight = new Vector3(sm.MainCamera.right.x, 0, sm.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * sm.inputManager.Movement.y +
                                cameraRight.normalized * sm.inputManager.Movement.x;

        sm.Velocity.x = moveDirection.x * sm.MovementSpeed;
        sm.Velocity.z = moveDirection.z * sm.MovementSpeed;
    }

    protected void CalculateCameraRotation()
    {
        Vector2 mouseDelta = sm.inputManager.MouseDelta;
        Vector2 rawFrameRotation = mouseDelta * sm.CameraSensitivity;
        sm.frameRotation = Vector2.Lerp(sm.frameRotation, rawFrameRotation, 1/ 1.5f);

        sm.rotation += sm.frameRotation;
        sm.rotation.y = Mathf.Clamp(sm.rotation.y, -sm.xClamp, sm.xClamp);
        
        sm.transform.localRotation = Quaternion.AngleAxis(sm.rotation.x, Vector3.up);
        sm.MainCamera.localRotation = Quaternion.AngleAxis(-sm.rotation.y, Vector3.right);
    }
    
    protected void ApplyGravity()
    {
        if (sm.Velocity.y > Physics.gravity.y)
        {
            sm.Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected void Move()
    {
        sm.CharacterController.Move(sm.Velocity * Time.deltaTime);
    }
    
    protected void Rotate()
    {
        sm.CharacterController.Move(sm.Velocity * Time.deltaTime);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}