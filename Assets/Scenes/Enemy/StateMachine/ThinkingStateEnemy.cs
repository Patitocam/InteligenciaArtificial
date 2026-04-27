using UnityEngine;

public class ThinkingStateEnemy : EnemyStates
{
    private EnemySM sm;
    private EnemyStatesEnum nextState;
    private float thinkTime;
    private float timer;

    public ThinkingStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, float thinkTime = 1f) : base(stateMachine)
    {
        this.sm = sm;
        this.thinkTime = thinkTime;
    }
    public void SetNextState(EnemyStatesEnum next)
    {
        nextState = next;
    }

    public override void Enter()
    {
        timer = 0f;
        Debug.Log("[Thinking] Hmm...");
    }

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
