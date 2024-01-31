public abstract class State
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void LateUpdate();
    public abstract void Exit();
    public abstract void EvaluateTransition();
}