
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
        // Inicializa el EnemySM con los parámetros necesarios para sus estados
        base.Start();
        enemySm = new EnemySM(Target, playerRb, this, arriveDistance, attackRange, speed, movement, wayPoints);
        seeingNode = new ActionNode(SeeingPlayer);
        notSeeingNode = new ActionNode(NotSeeingPlayer);
        nearPlayerNode = new ActionNode(InRange);
        attackNode = new ActionNode(Attack);

        // Construye el tree de comportamiento
        attackQuestionNode = new QuestionNode(IsInAttackRange, attackNode, nearPlayerNode);
        closeQuestionNode = new QuestionNode(IsClose, attackQuestionNode, seeingNode);
        seeQuestionNode = new QuestionNode(IsSeeing, closeQuestionNode, notSeeingNode);
        root = seeQuestionNode;
    }

    // Pregunta si ve al jugador
    private bool IsSeeing() => LOS.Sight(); 
    private bool IsClose() => (Target.transform.position - this.transform.position).magnitude < arriveDistance;
    private bool IsInAttackRange() => (Target.transform.position - this.transform.position).magnitude < attackRange;

    
    void SeeingPlayer()
    {
        // Sólo transiciona a perseguir si NO está ya persiguiendo o pensando
        if (enemySm.fsm.CurrentState is ChaseStateEnemy || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        enemySm.SwitchState(EnemyStatesEnum.Chasing);
        LOS.ModifyLosAngle();
    }

    // 
    void NotSeeingPlayer()
    {
        // Sólo transiciona a patrullar si NO está ya patrullando o pensando
        if (enemySm.fsm.CurrentState is PatrolStateEnemy || enemySm.fsm.CurrentState is ThinkingStateEnemy) return;
        LOS.ResetLosAngle(viewAngle);
        enemySm.SwitchStateWithThinking(EnemyStatesEnum.Patrolling); // <-- pausa antes de patrullar 
    }

    // Cambia a perseguir
    void InRange()
    {
        enemySm.SwitchState(EnemyStatesEnum.Arrive);
    }

    //Cambia a atacar
    void Attack()
    {
        enemySm.SwitchState(EnemyStatesEnum.Attack);
    }
}