using System.Collections.Generic;
using UnityEngine;


public class GenericStateMachine<T>
{
    private IState currentState; //Referencia al estado actual
    private Dictionary<T, IState> states = new(); //Se crea un diccionario que asocia el tipo de la máquina de estados (Enum) y un Istate

    public IState CurrentState => currentState;

    public void SetCurrent(IState newCurrent) //Fuerza el estado current
    {
        currentState = newCurrent;
    }

    public void AddState(IState state, T stateValue) //Agrega al diccionario
    {
        states.Add(stateValue, state);
    }

    public void Update(float deltaTime) //Actualiza el estado
    {
        currentState.Tick(deltaTime);
    }

    public void ChangeState(T newState) //Transiciona entre estados
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

