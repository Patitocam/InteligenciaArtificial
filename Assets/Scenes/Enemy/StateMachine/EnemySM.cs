using Assets.Scenes.Enemy;
using UnityEngine;

public enum EnemyStatesEnum
{
    Idle,
    Patrolling,
    Chasing,
    RunAway,
    Attack,
    Arrive
}

public class EnemySM 
{
    public GenericStateMachine<EnemyStatesEnum> fsm;

    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner, float arriveDistance, float attackRange, float speed, EnemyMovement movement, Transform[] wayPoints)
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();
        var idle = new IdleStateEnemy(this, fsm);
        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new PatrolStateEnemy(this, fsm, owner, speed, wayPoints), EnemyStatesEnum.Patrolling);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.AddState(new ChaseStateEnemy(this, fsm, Target, rb, owner, speed), EnemyStatesEnum.Chasing);
        fsm.AddState(new AttackStateEnemy(this, fsm, Target), EnemyStatesEnum.Attack);
        fsm.AddState(new ArriveStateEnemy(this, fsm, Target, owner, arriveDistance, attackRange, speed), EnemyStatesEnum.Arrive);
        fsm.SetCurrent(idle);
    }
    public EnemySM(GameObject Target, Rigidbody rb, EntityController owner, float speed, EnemyMovement movement)
    {
        fsm = new GenericStateMachine<EnemyStatesEnum>();
        var idle = new IdleStateEnemy(this, fsm);
        fsm.AddState(idle, EnemyStatesEnum.Idle);
        fsm.AddState(new RunAwayStateEnemy(this, fsm, Target, owner, speed), EnemyStatesEnum.RunAway);
        fsm.SetCurrent(idle);
    }

    public void Tick(float deltaTime)
    {
        fsm.Update(deltaTime);
    }

    public void SwitchState(EnemyStatesEnum newState) 
    {
        fsm.ChangeState(newState);
    }
}

