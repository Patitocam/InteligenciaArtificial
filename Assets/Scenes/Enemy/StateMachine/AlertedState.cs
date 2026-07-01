using System;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : EnemyStates
{
    private EnemySM sm;
    private Vector3 target;
    private EntityController owner;
    private float speed;
    float arriveDistance;
    float attackRange;
    GridGenerator grid;

    // Evento estático: cualquier enemigo puede suscribirse y será alertado
    // con la última posición conocida del jugador
    public static event Action<Vector3> OnPlayerSpotted;

    // Dispara el evento — lo llama el enemigo que vio al jugador
    public static void AlertAll(Vector3 lastKnownPosition)
    {
        OnPlayerSpotted?.Invoke(lastKnownPosition);
        Debug.Log("HOLAAAAAAAAAAAAAAAAAAA");
    }

    // Ruta A* hacia la última posición conocida
    private List<PfNode> currentPath = new();
    private int pathIndex = 0;
    private bool hasTarget = false;

    // Timer para olvidarse
    private float forgetTime = 5f;
    private float timer = 0f;

    public AlertedState(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, Vector3 Target, EntityController owner, float arriveDistance, float attackRange, float speed, GridGenerator grid) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.speed = speed;
        this.arriveDistance = arriveDistance;
        this.attackRange = attackRange;
        this.grid = grid;

        // Suscribirse al evento al crear el estado
        OnPlayerSpotted += OnAlerted;
    }

    // Cuando llega una alerta: actualizar destino y cambiar a este estado
    private void OnAlerted(Vector3 lastKnownPosition)
    {
        // Si ya está persiguiendo o atacando, ignorar la alerta
        var current = sm.fsm.CurrentState;
        if (current is ChaseStateEnemy || current is AttackStateEnemy || current is ArriveStateEnemy) return;

        target = lastKnownPosition;
        hasTarget = true;
        sm.SwitchState(EnemyStatesEnum.Alerted);
        Debug.Log("RECIBIIIIII");
    }

    public override void Enter()
    {
        timer = 0f;
        OnPlayerSpotted += OnAlerted;
        Debug.Log("ENTRE");
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        timer += deltaTime;

        if (timer >= forgetTime)
        {
            sm.SwitchState(EnemyStatesEnum.Patrolling);
            timer = 0f;
            return;
        }

        if (!hasTarget) return;
        MoveToTarget();
    }

    public override void Exit()
    {
        timer = 0f;
        hasTarget = false;
        currentPath.Clear();
        // Desuscribirse al salir para no acumular suscripciones
        OnPlayerSpotted -= OnAlerted;
    }

    private void MoveToTarget()
    {
        owner.Move(target, speed);
    }
}