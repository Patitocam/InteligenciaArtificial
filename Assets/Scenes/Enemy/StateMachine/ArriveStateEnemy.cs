using UnityEngine;

public class ArriveStateEnemy: EnemyStates
{
    private EnemySM sm;
    private GameObject target;
    private EntityController owner;
    private float speed;
    float arriveDistance;
    float attackRange;

    public ArriveStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, EntityController owner, float arriveDistance, float attackRange, float speed) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.speed = speed;
        this.arriveDistance = arriveDistance;
        this.attackRange = attackRange;
    }

    // velocidad normal si el enemigo está lejos, velocidad reducida si el enemigo está cerca y velocidad 0 si el enemigo está en rango de ataque
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move();
    }

    public void Move()
    {
        var (dir, spd) = Arrive();
        owner.Move(dir, spd);
    }
    private (Vector3, float) Arrive()
    {
        var dir = (target.transform.position - owner.transform.position);
        var distance = dir.magnitude;

        
        float speedT = speed;
        if (distance < arriveDistance) speedT = distance;
        if (distance < attackRange) speedT = 0;

        var destiny = dir.normalized;
        return (destiny, speedT);
    }
}

