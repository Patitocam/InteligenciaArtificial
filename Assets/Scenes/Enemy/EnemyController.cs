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

    private EnemySM enemySm;

    private QuestionNode root;
    ActionNode seeing;
    ActionNode notSeeing;
    QuestionNode seeQuestion;

    void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);
        seeing = new ActionNode(SeeingPlayer);
        notSeeing = new ActionNode(NotSeeingPlayer);
        seeQuestion = new QuestionNode(IsSeeng, seeing, notSeeing);
        root = seeQuestion;
        enemySm = new EnemySM(Target);
    }

    void Update()
    {
        root.Execute();
        enemySm.Tick();
    }

    private bool IsSeeng() => LOS.Sight();
    void SeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Patrolling);
    }
    void NotSeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Idle);
    }
}
 