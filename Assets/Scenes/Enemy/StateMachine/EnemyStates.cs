
public class EnemyStates: State<EnemyStatesEnum> //Clase padre de los estados del enemigo basada en el Enum de estados
{
    public EnemyStates(GenericStateMachine<EnemyStatesEnum> stateMachine) : base(stateMachine)
    {

    }
}

