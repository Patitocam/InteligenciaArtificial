using UnityEngine;


public class IdleStateEnemy: EnemyStates
{
    private EnemySM sm;

    // El enemigo no hace nada, simplemente espera. Es el estado inicial del enemigo.
    public IdleStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine) : base(stateMachine)
    {
        this.sm = sm;
    }

    // En este estado el enemigo no hace nada, simplemente espera. Es el estado inicial del enemigo.
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Idle();
    }
    private void Idle()
    {
    }

}

