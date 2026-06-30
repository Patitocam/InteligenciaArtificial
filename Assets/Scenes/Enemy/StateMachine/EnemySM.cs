using Assets.Scenes.Enemy;
using UnityEngine;

public enum EnemyStatesEnum
{
    Idle,
    Patrolling,
    Chasing,
    RunAway,
    Attack,
    Arrive,
    Thinking,
    Alerted,
}

public class EnemySM
{
    public GenericStateMachine<EnemyStatesEnum> fsm;
    public ThinkingStateEnemy ThinkingState { get; private set; }

    // Constructor Chaser: con A* patrol y sistema de alertas
    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner,
        float arriveDistance, float attackRange, float speed,
        EnemyMovement movement, Transform[] wayPoints, GridGenerator grid)
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();

        var idle = new IdleStateEnemy(this, fsm);
        ThinkingState = new ThinkingStateEnemy(this, fsm, thinkTime: 1f);

        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new PatrolStateEnemy(this, fsm, owner, speed, wayPoints, grid), EnemyStatesEnum.Patrolling);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(new ChaseStateEnemy(this, fsm, Target, rb, owner, speed), EnemyStatesEnum.Chasing);
        fsm.AddState(new AttackStateEnemy(this, fsm, Target, owner), EnemyStatesEnum.Attack);
        fsm.AddState(new ArriveStateEnemy(this, fsm, Target, owner, arriveDistance, attackRange, speed), EnemyStatesEnum.Arrive);
        fsm.AddState(ThinkingState, EnemyStatesEnum.Thinking);
        fsm.AddState(new AlertedState(this, fsm, Vector3.zero, owner, arriveDistance, attackRange, speed, grid), EnemyStatesEnum.Alerted);
        fsm.SetCurrent(idle);
    }

    // Constructor Grouper: con flocking
    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner,
        float arriveDistance, float attackRange, float speed,
        EnemyMovement movement, Transform[] wayPoints, bool useFlocking)
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();

        var idle = new IdleStateEnemy(this, fsm);
        ThinkingState = new ThinkingStateEnemy(this, fsm, thinkTime: 1f);

        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new FlockingPatrol(this, fsm, owner, speed, wayPoints), EnemyStatesEnum.Patrolling);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(new FlockingChase(this, fsm, Target, rb, owner, speed), EnemyStatesEnum.Chasing);
        fsm.AddState(new AttackStateEnemy(this, fsm, Target, owner), EnemyStatesEnum.Attack);
        fsm.AddState(new ArriveStateEnemy(this, fsm, Target, owner, arriveDistance, attackRange, speed), EnemyStatesEnum.Arrive);
        fsm.AddState(ThinkingState, EnemyStatesEnum.Thinking);

        fsm.SetCurrent(idle);
    }

    // Constructor Runner: sin patrol ni A*
    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner,
        float speed, EnemyMovement movement)
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();

        var idle = new IdleStateEnemy(this, fsm);
        ThinkingState = new ThinkingStateEnemy(this, fsm, thinkTime: 1f);

        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(ThinkingState, EnemyStatesEnum.Thinking);

        fsm.SetCurrent(idle);
    }

    public void Tick(float deltaTime) => fsm.Update(deltaTime);
    public void SwitchState(EnemyStatesEnum newState) => fsm.ChangeState(newState);

    public void SwitchStateWithThinking(EnemyStatesEnum nextState)
    {
        ThinkingState.SetNextState(nextState);
        fsm.ChangeState(EnemyStatesEnum.Thinking);
    }
}