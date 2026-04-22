using UnityEngine;
public class AttackStateEnemy : EnemyStates
{
    private EnemySM sm;

    public AttackStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target) : base(stateMachine)
    {
        this.sm = sm;
    }
    public override void Tick()
    {
        base.Tick();
        Attack();
    }
    private void Attack()
    {
        Debug.Log("Im Attacking");
    }

}