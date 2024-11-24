using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IGameState currentState;
    private Stack<IGameState> stateHistory = new Stack<IGameState>();
    public IGameState CurrentState => currentState; 

    public void SetState(IGameState newState)
    {
        if (currentState != null)
        {
            stateHistory.Push(currentState); 
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void RevertToPreviousState()
    {
        if (stateHistory.Count > 0)
        {
            currentState.Exit();
            currentState = stateHistory.Pop(); 
            currentState.Enter();
        }
        else
        {
            Debug.LogWarning("StateMachine: No previous state to revert to.");
        }
    }

    public void Update()
    {
        currentState?.Update();
    }
}
