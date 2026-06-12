using UnityEngine;

public class ThinkingStateEnemy : EnemyStates
{
    private EnemySM sm;
    private EnemyStatesEnum nextState;
    private float thinkTime;
    private float timer;

    // el enemeigo se detiene por un momento antes de pasar a la siguenta accion
    public ThinkingStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, float thinkTime = 1f) : base(stateMachine)
    {
        this.sm = sm;
        this.thinkTime = thinkTime;
    }

    // Permite configurar el estado al que se dirigirá el enemigo después de pensar.
    public void SetNextState(EnemyStatesEnum next)
    {
        nextState = next;
    }
    
    public override void Enter()
    {
        timer = 0f;
    }

    // El enemigo permanece en este estado durante un tiempo determinado (thinkTime) antes de cambiar al siguiente estado configurado.
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        timer += deltaTime;
        if (timer >= thinkTime)
        {
            sm.SwitchState(nextState);
        }
    }

    public override void Exit()
    {
        timer = 0f;
    }
}
