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
}

public class EnemySM
{
    public GenericStateMachine<EnemyStatesEnum> fsm; //Se crea una state machine 

    public ThinkingStateEnemy ThinkingState { get; private set; } //Thinking state que controla pausas entre estados

    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner, float arriveDistance, float attackRange, float speed, EnemyMovement movement, Transform[] wayPoints) //Constructor para el enemigo Chaser
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();

        var idle = new IdleStateEnemy(this, fsm);
        ThinkingState = new ThinkingStateEnemy(this, fsm, thinkTime: 1f);

        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new PatrolStateEnemy(this, fsm, owner, speed, wayPoints), EnemyStatesEnum.Patrolling);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(new ChaseStateEnemy(this, fsm, Target, rb, owner, speed), EnemyStatesEnum.Chasing);
        fsm.AddState(new AttackStateEnemy(this, fsm, Target, owner), EnemyStatesEnum.Attack);
        fsm.AddState(new ArriveStateEnemy(this, fsm, Target, owner, arriveDistance, attackRange, speed), EnemyStatesEnum.Arrive);
        fsm.AddState(ThinkingState, EnemyStatesEnum.Thinking);

        fsm.SetCurrent(idle);
    }

    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner, float speed, EnemyMovement movement) //Constructor para el enemigo Runner
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();

        var idle = new IdleStateEnemy(this, fsm);
        ThinkingState = new ThinkingStateEnemy(this, fsm, thinkTime: 1f);

        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(ThinkingState, EnemyStatesEnum.Thinking);
        fsm.SetCurrent(idle);
    }

    //LLamados a las funciones de la state machine real
    public void Tick(float deltaTime)
    {
        fsm.Update(deltaTime);
    }

    public void SwitchState(EnemyStatesEnum newState)
    {
        fsm.ChangeState(newState);
    }

    public void SwitchStateWithThinking(EnemyStatesEnum nextState)
    {
        ThinkingState.SetNextState(nextState);
        fsm.ChangeState(EnemyStatesEnum.Thinking);
    }
}