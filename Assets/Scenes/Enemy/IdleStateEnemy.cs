using UnityEngine;


public class IdleStateEnemy: EnemyStates
{
    private EnemySM sm;

    public IdleStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine) : base(stateMachine)
    {
        this.sm = sm;
    }
    public override void Tick()
    {
        base.Tick();
        Idle();
    }
    private void Idle()
    {
        Debug.Log("Im idle");
    }



}

