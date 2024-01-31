using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State currentState;
    
    private void Update()
    {
        currentState?.Update();
    }

    private void LateUpdate()
    {
        currentState?.LateUpdate();
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState.Enter();
    }
}