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
    QuestionNode seeQuestion;
    Rigidbody playerRb;

    void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);
        seeing = new ActionNode(SeeingPlayer);
        notSeeing = new ActionNode(NotSeeingPlayer);
        seeQuestion = new QuestionNode(IsSeeing, seeing, notSeeing);
        root = seeQuestion;
        playerRb = Target.GetComponent<Rigidbody>();
        enemySm = new EnemySM(Target, playerRb, this);
    }

    void Update()
    {
        root.Execute();
        enemySm.Tick(Time.deltaTime);
    }

    private bool IsSeeing() => LOS.Sight();
    void SeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Chasing);
    }
    void NotSeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Idle);
    }
}
 