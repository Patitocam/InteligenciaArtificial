using UnityEngine;

public class ChaserController : EntityController
{
    [SerializeField] protected float arriveDistance;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform[] wayPoints;
    [SerializeField] protected GridGenerator grid;
    [SerializeField] private GameObject Alerted;

    ActionNode seeingNode;
    ActionNode notSeeingNode;
    ActionNode nearPlayerNode;
    ActionNode attackNode;
    QuestionNode seeQuestionNode;
    QuestionNode attackQuestionNode;
    QuestionNode closeQuestionNode;

    private bool wasSeeing = false;

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

        Alerted.SetActive(false);
    }

    private bool IsSeeing() => LOS.Sight();
    private bool IsClose() => (Target.transform.position - transform.position).magnitude < arriveDistance;
    private bool IsInAttackRange() => (Target.transform.position - transform.position).magnitude < attackRange;

    void SeeingPlayer()
    {
        if (!wasSeeing)
        {
            wasSeeing = true;
            AlertedState.AlertAll(transform.position);
            if (enemySm.fsm.CurrentState is not ChaseStateEnemy)
                enemySm.SwitchState(EnemyStatesEnum.Chasing);
            Alerted.SetActive(true);
        }
    }

    void NotSeeingPlayer()
    {
        if (wasSeeing)
        {
            wasSeeing = false;
            LOS.ResetLosAngle(viewAngle);
            if (enemySm.fsm.CurrentState is not PatrolStateEnemy &&
                enemySm.fsm.CurrentState is not ThinkingStateEnemy)
                enemySm.SwitchStateWithThinking(EnemyStatesEnum.Patrolling);
            Alerted.SetActive(false);
        }
        else if (enemySm.fsm.CurrentState is not PatrolStateEnemy && enemySm.fsm.CurrentState is not AlertedState)
        {
            enemySm.SwitchState(EnemyStatesEnum.Patrolling);
            Alerted.SetActive(false);
        }
    }

    void InRange() { enemySm.SwitchState(EnemyStatesEnum.Arrive); }
    void Attack() { enemySm.SwitchState(EnemyStatesEnum.Attack); }
}