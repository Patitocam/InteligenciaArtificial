
using UnityEngine; 

public class ChaseStateEnemy : EnemyStates
{
    private EnemySM sm;

    public ChaseStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target) : base(stateMachine)
    {
        this.sm = sm;
    }
    public override void Tick()
    {
        base.Tick();
        Chase();
    }
    private void Chase()
    {
        Debug.Log("Im Chasing");
    }

}
