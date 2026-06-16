
using UnityEngine;

public class GrouperController : EntityController
{
    [SerializeField] private float arriveDistance;
    [SerializeField] private float attackRange;
    [SerializeField] protected Transform[] wayPoints;

    ActionNode seeingNode;
    ActionNode notSeeingNode;
    ActionNode nearPlayerNode;
    ActionNode attackNode;
    QuestionNode seeQuestionNode;
    QuestionNode attackQuestionNode;
    QuestionNode closeQuestionNode;

    public override void Start()
    {
        base.Start();
        enemySm = new EnemySM(Target, playerRb, this, arriveDistance, attackRange, speed, movement, wayPoints, true);

        notSeeingNode = new ActionNode(NotSeeingPlayer);
        seeingNode = new ActionNode(SeeingPlayer);
        nearPlayerNode = new ActionNode(InRange);
        attackNode = new ActionNode(Attack);

        attackQuestionNode = new QuestionNode(IsInAttackRange, attackNode, nearPlayerNode);
        closeQuestionNode = new QuestionNode(IsClose, attackQuestionNode, seeingNode);
        seeQuestionNode = new QuestionNode(IsSeeing, closeQuestionNode, notSeeingNode);
        root = seeQuestionNode;
    }
  
    private bool IsSeeing() => LOS.Sight();
    private bool IsClose() => (Target.transform.position - transform.position).magnitude < arriveDistance;
    private bool IsInAttackRange() => (Target.transform.position - transform.position).magnitude < attackRange;

    void SeeingPlayer()
    {
        if (enemySm.fsm.CurrentState is FlockingChase || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        enemySm.SwitchState(EnemyStatesEnum.Chasing);
        LOS.ModifyLosAngle();
    }
    void NotSeeingPlayer()
    {
        if (enemySm.fsm.CurrentState is FlockingPatrol || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        LOS.ResetLosAngle(viewAngle);
        enemySm.SwitchState(EnemyStatesEnum.Patrolling);
    }
    void InRange() { enemySm.SwitchState(EnemyStatesEnum.Arrive); }
    void Attack() { enemySm.SwitchState(EnemyStatesEnum.Attack); }
}

