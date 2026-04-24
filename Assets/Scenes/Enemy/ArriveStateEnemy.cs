using System.Buffers;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArriveStateEnemy: EnemyStates
{
    private EnemySM sm;
    private GameObject target;
    private EnemyController owner;
    private float speed = 5f;
    float slowingDistance = 5f;
    float arriveDistance = 2f;

    public ArriveStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, Rigidbody playerRb, EnemyController owner) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Arrive(deltaTime);
    }
    private void Arrive(float delta)
    {
        var dir = (target.transform.position - owner.transform.position);
        var distance = dir.magnitude;

        
        float speedT = 5;
        if (distance < slowingDistance) speedT = speed * 0.3f;
        if (distance < arriveDistance) speedT = 0;

        owner.transform.Translate(dir.normalized * speedT * delta, Space.World);
    }
}

