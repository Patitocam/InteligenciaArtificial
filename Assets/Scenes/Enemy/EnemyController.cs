using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private LineOfSight LOS;

    [SerializeField] private GameObject Target;
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewLenght;
    [SerializeField] private LayerMask wallsAndObs;
    [SerializeField] private LayerMask obs;
    [SerializeField] public Rigidbody Rb;
    [SerializeField] private float arriveDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float speed;

    private EnemySM enemySm;
    private EnemyMovement movement;

    private QuestionNode root;
    ActionNode seeingNode;
    ActionNode notSeeingNode;
    ActionNode nearPlayerNode;
    ActionNode attackNode;
    QuestionNode seeQuestionNode;
    QuestionNode attackQuestionNode;
    QuestionNode closeQuestionNode;

    Rigidbody playerRb;

    void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);

        seeingNode = new ActionNode(SeeingPlayer);
        notSeeingNode = new ActionNode(NotSeeingPlayer);
        nearPlayerNode = new ActionNode(InRange);
        attackNode = new ActionNode(Attack);

        attackQuestionNode = new QuestionNode(IsInAttackRange, attackNode, nearPlayerNode);
        closeQuestionNode = new QuestionNode(IsClose, attackQuestionNode, seeingNode);
        seeQuestionNode = new QuestionNode(IsSeeing, closeQuestionNode, notSeeingNode);
        root = seeQuestionNode;

        playerRb = Target.GetComponent<Rigidbody>();
        enemySm = new EnemySM(Target, playerRb, this,arriveDistance, attackRange, speed, movement);
        movement = new EnemyMovement(transform, obs);
    }

    void Update()
    {
        root.Execute();
        enemySm.Tick(Time.deltaTime);
        Debug.Log(enemySm.fsm.CurrentState);
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
        enemySm.SwitchState(EnemyStatesEnum.Idle);
    }
    void InRange()
    {
        enemySm.SwitchState(EnemyStatesEnum.Arrive);
    }
    void Attack()
    {
        enemySm.SwitchState(EnemyStatesEnum.Attack);
    }

    public void Move(Vector3 target, float speed)
    {
        movement.Move(target, Time.deltaTime, speed);
    }
}
 