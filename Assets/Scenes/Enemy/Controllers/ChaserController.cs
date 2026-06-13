
using UnityEngine;

public class ChaserController : EntityController
{
    [SerializeField] protected float arriveDistance;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform[] wayPoints;

    // Arrastrá el GridGenerator de la escena aquí desde el Inspector
    [SerializeField] protected GridGenerator grid;

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
        enemySm = new EnemySM(Target, playerRb, this, arriveDistance, attackRange, speed, movement, wayPoints, grid);

        seeingNode = new ActionNode(SeeingPlayer);
        notSeeingNode = new ActionNode(NotSeeingPlayer);
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
        if (enemySm.fsm.CurrentState is ChaseStateEnemy || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        enemySm.SwitchState(EnemyStatesEnum.Chasing);
        LOS.ModifyLosAngle();
    }
    void NotSeeingPlayer()
    {
        if (enemySm.fsm.CurrentState is PatrolStateEnemy || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        LOS.ResetLosAngle(viewAngle);
        enemySm.SwitchStateWithThinking(EnemyStatesEnum.Patrolling);
    }
    void InRange() { enemySm.SwitchState(EnemyStatesEnum.Arrive); }
    void Attack() { enemySm.SwitchState(EnemyStatesEnum.Attack); }
}