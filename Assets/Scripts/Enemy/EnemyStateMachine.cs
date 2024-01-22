using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyStateMachine
{
    private EnemyState currentState;

    public EnemyStateMachine(EnemyState initialState, GameObject gameObject)
    {
        currentState = initialState;
        currentState.OnEnter(gameObject);
    }

    // Update is called once per frame
    public void UpdateState()
    {
        currentState.OnUpdate();
    }

    public void ChangeState(EnemyState newState, GameObject gameObject)
    {
        currentState.OnExit();

        currentState = newState;
        currentState.OnEnter(gameObject);
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }
}
