using UnityEngine;

public class UIState : State
{
    public override void Enter()
    {
        ShowCursor();    
    }

    public override void Update() {}
    public override void LateUpdate() {}
    public override void Exit() {}
    public override void EvaluateTransition() {}
    
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}