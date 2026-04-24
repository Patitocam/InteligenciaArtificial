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
    [SerializeField] public Rigidbody Rb;

    private EnemySM enemySm;

    private QuestionNode root;
    ActionNode seeing;
    ActionNode notSeeing;
    ActionNode nearPlayer;
    QuestionNode seeQuestion;
    QuestionNode closeQuestion;
    Rigidbody playerRb;

    void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);
        seeing = new ActionNode(SeeingPlayer);
        notSeeing = new ActionNode(NotSeeingPlayer);
        nearPlayer = new ActionNode(InRange);
        closeQuestion = new QuestionNode(IsClose, nearPlayer, seeing);
        seeQuestion = new QuestionNode(IsSeeing, closeQuestion, notSeeing);
        root = seeQuestion;
        playerRb = Target.GetComponent<Rigidbody>();
        enemySm = new EnemySM(Target, playerRb, this);
    }

    void Update()
    {
        root.Execute();
        enemySm.Tick(Time.deltaTime);
        Debug.Log(enemySm.fsm.CurrentState);
    }

    private bool IsSeeing() => LOS.Sight();
    private bool IsClose() => (Target.transform.position - this.transform.position).magnitude < 10;

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
}
 