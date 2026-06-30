using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class AlertedState: EnemyStates
{
    private EnemySM sm;
    private Vector3 target;
    private EntityController owner;
    private float speed;
    float arriveDistance;
    float attackRange;
    GridGenerator grid;

    public AlertedState(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, Vector3 Target, EntityController owner, float arriveDistance, float attackRange, float speed, GridGenerator grid) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.speed = speed;
        this.arriveDistance = arriveDistance;
        this.attackRange = attackRange;
        this.grid = grid;
    }

    // Si un enemigo ve al player debe avisar a todos los enemigos que no lo hayan visto que vayan al punto donde lo vio
    //Si otro enemigo ve al player actualiza a los enemigos que no lo vieron 
    // Despues de un tiempo se olvidan y vuelven a patrullar
    //Si el target esta en su linea de vision van recto, si hay obstaculos en medio usan a*
}

