using System.Collections.Generic;
using UnityEngine;


public class GenericStateMachine<T>
{
    private IState currentState;
    private Dictionary<T, IState> states = new();

    public IState CurrentState => currentState;

    public void SetCurrent(IState newCurrent)
    {
        currentState = newCurrent;
    }

    public void AddState(IState state, T stateValue)
    {
        states.Add(stateValue, state);
    }

    public void Update()
    {
        currentState.Tick();
    }

    public void ChangeState(T newState)
    {
        if (states.TryGetValue(newState, out IState state))
        {
            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }
        else Debug.Log("No state corresponding");
    }
}

