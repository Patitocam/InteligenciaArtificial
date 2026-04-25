using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PatrolStateEnemy: EnemyStates
{
    private EnemySM sm;
    EnemyController owner;
    float speed;
    Transform[] wayPoints;
    int currentWaypoint;

    public PatrolStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, EnemyController owner, float speed, Transform[] wayPoints) : base(stateMachine) 
    {
        this.sm = sm;
        this.owner = owner;
        this.speed = speed / 2;
        this.wayPoints = wayPoints;
        currentWaypoint = 0;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Patrol();
    }
    private void Patrol()
    {
        if (wayPoints.Length == 0) return;

        Transform target = wayPoints[currentWaypoint];

        Vector3 direction = (target.position - owner.transform.position).normalized;
        owner.Move(direction, speed);
        owner.transform.LookAt(target.position);

        if ((target.position - owner.transform.position).magnitude < 1f)
        {
            currentWaypoint = (currentWaypoint + 1) % wayPoints.Length;
        }
    }
}

