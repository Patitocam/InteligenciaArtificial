
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
        Move(deltaTime);
    }

    private void Move(float deltaTime)
    { 
        owner.transform.Translate(Chase() * speed * deltaTime, Space.World);
        owner.transform.LookAt(Chase());
    }
    private Vector3 Chase()
    {
        Vector3 toTarget = target.transform.position - owner.transform.position;
        float distance = toTarget.magnitude;

        Vector3 targetDir = playerRB.velocity.normalized;

        float movingAway = Vector3.Dot(toTarget.normalized, targetDir);

        float predictor = 0;

        if (movingAway > 0)
        {
            predictor = Mathf.Clamp(distance / speed, 0f, 1f);
        }

        Vector3 futurePosition = target.transform.position + playerRB.velocity * predictor;

        Vector3 desiredDir = (futurePosition - owner.transform.position).normalized;

        return desiredDir;
    }

}
