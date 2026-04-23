
using UnityEngine; 

public class ChaseStateEnemy : EnemyStates
{
    private EnemySM sm;
    private GameObject target;
    private EnemyController owner;
    private Rigidbody playerRB;
    private float speed = 5f;

    public ChaseStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, Rigidbody playerRb, EnemyController owner) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.playerRB = playerRb;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Chase(deltaTime);
    }
    private void Chase(float deltaTime)
    {
        /*Vector3 currentSpeed;
        var future_position = target.transform.position + playerRB.Speed * timePrediction;

        var desired_velocity = (future_position - transform.position).NoY().normalized * max_speed;
        var steering = desired_velocity - currentSpeed;

        currentSpeed += steering * deltaTime;
        owner.transform.position += currentSpeed * deltaTime;*/
    }

}
