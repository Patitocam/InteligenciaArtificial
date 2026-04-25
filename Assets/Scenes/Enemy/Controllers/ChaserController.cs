
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class ChaserController : EntityController
{
    [SerializeField] protected float arriveDistance;
    [SerializeField] protected float attackRange;
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
        enemySm = new EnemySM(Target, playerRb, this, arriveDistance, attackRange, speed, movement, wayPoints);
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
    private bool IsClose() => (Target.transform.position - this.transform.position).magnitude < arriveDistance;
    private bool IsInAttackRange() => (Target.transform.position - this.transform.position).magnitude < attackRange;

    void SeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Chasing);
    }
    void NotSeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Patrolling);
    }
    void InRange()
    {
        enemySm.SwitchState(EnemyStatesEnum.Arrive);
    }
    void Attack()
    {
        enemySm.SwitchState(EnemyStatesEnum.Attack);
    }

}

