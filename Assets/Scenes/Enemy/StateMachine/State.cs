using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


// implementa Istate y tiene una referencia a la maquina de estados
public class State<T> : IState
{
    protected GenericStateMachine<T> stateMachine;

    public State(GenericStateMachine<T> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }
    public virtual void Tick(float deltaTime)
    {

    }
    public virtual void Exit() 
    {

    }
}

