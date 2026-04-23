using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PatrolStateEnemy: EnemyStates
{
    private EnemySM sm;

    public PatrolStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine) : base(stateMachine) 
    {
        this.sm = sm;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Patrol();
    }
    private void Patrol()
    {
        Debug.Log("Im patrolling");
    }
}

