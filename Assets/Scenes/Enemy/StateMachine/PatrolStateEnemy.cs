using UnityEngine;


public class PatrolStateEnemy: EnemyStates
{
    private EnemySM sm;
    EntityController owner;
    float speed;
    Transform[] wayPoints;
    int currentWaypoint;

    // El enemigo se mueve entre los waypoints en orden, rotando para mirar hacia el siguiente waypoint. Si el enemigo llega a un waypoint, se dirige al siguiente.
    public PatrolStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, EntityController owner, float speed, Transform[] wayPoints) : base(stateMachine) 
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
        Move();
    }
    public void Move()
    {
        owner.Move(Patrol(), speed);
    }
    private Vector3 Patrol()
    {
        if (wayPoints.Length == 0) return Vector3.zero;

        Transform target = wayPoints[currentWaypoint];

        Vector3 direction = (target.position - owner.transform.position).normalized;
        
        owner.transform.LookAt(target.position);

        if ((target.position - owner.transform.position).magnitude < 1f)
        {
            currentWaypoint = (currentWaypoint + 1) % wayPoints.Length;
        }
        return direction;
    }
}

