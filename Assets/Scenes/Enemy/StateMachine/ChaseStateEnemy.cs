
using UnityEngine; 

public class ChaseStateEnemy : EnemyStates
{
    private EnemySM sm;
    private GameObject target;
    private EntityController owner;
    private Rigidbody playerRB;
    private float speed;

    public ChaseStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, Rigidbody playerRb, EntityController owner, float speed) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.playerRB = playerRb;
        this.speed = speed;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move();
    }

    private void Move()
    {
        owner.Move(Chase(), speed);
        owner.transform.LookAt(target.transform.position);
    }
    private Vector3 Chase()
    {
        Vector3 toTarget = target.transform.position - owner.transform.position;
        float distance = toTarget.magnitude;

        Vector3 futurePosition = target.transform.position;

        if (playerRB.velocity.magnitude > 0.001f)
        {
            float predictor = Mathf.Clamp(distance / speed, 0f, 1f);
            futurePosition = target.transform.position + playerRB.velocity * predictor;
        }

        Vector3 desiredDir = (futurePosition - owner.transform.position).normalized;
        return desiredDir;
    }

}
