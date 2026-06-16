using UnityEngine;

public class FlockingChase : EnemyStates
{
    private EnemySM sm;
    private GameObject target;
    private EntityController owner;
    private Rigidbody playerRB;
    private float speed;

    private Vector3 velocity;
    private float maxForce = 5f;
    private float maxSpeed;

    private float separationWeight = 3f;
    private float cohesionWeight = 0.5f;
    private float allignmentWeight = 1.5f;

    float separationRadius = 1.5f;
    float cohesionRadius = 6f;

    public FlockingChase(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, Rigidbody playerRb, EntityController owner, float speed) : base(stateMachine)
    {
        this.sm = sm;
        this.target = Target;
        this.owner = owner;
        this.playerRB = playerRb;
        this.speed = speed;
        this.maxSpeed = speed;
    }

    public override void Enter()
    {
        velocity = Vector3.zero;

    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move();
    }

    private void Move()
    {
        Flocking();
        owner.Move(velocity, speed);
    }

    private void Flocking()
    {
        AddForce(Separation() * separationWeight + Cohesion() * cohesionWeight + Allignment() * allignmentWeight + SeekTarget() * 3f
        );
    }

    private Vector3 SeekTarget()
    {
        // Calcula el centroide del grupo
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, cohesionRadius * 3f, LayerMask.GetMask("Boids"));
        Vector3 groupCenter = owner.transform.position;
        int cont = 1;
        foreach (var b in boidsInRange)
        {
            if (b.transform == owner.transform) continue;
            groupCenter += b.transform.position;
            cont++;
        }
        groupCenter /= cont;

        // Predice la posición futura del jugador
        Vector3 toTarget = target.transform.position - groupCenter;
        float distance = toTarget.magnitude;
        Vector3 futurePosition = target.transform.position;
        if (playerRB.velocity.magnitude > 0.001f)
        {
            float predictor = Mathf.Clamp(distance / speed, 0f, 1f);
            futurePosition = target.transform.position + playerRB.velocity * predictor;
        }

        // Cada boid busca el punto desplazado por su offset respecto al centro
        Vector3 offset = owner.transform.position - groupCenter;
        return Seek(futurePosition + offset);
    }

    private Vector3 Separation()
    {
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, separationRadius, LayerMask.GetMask("Boids"));
        Vector3 totalForce = Vector3.zero;
        int cont = 0;

        foreach (var boidTaken in boidsInRange)
        {
            if (boidTaken == owner.GetComponent<Collider>()) continue;

            Vector3 direction = owner.transform.position - boidTaken.transform.position;
            float distance = direction.magnitude;

            if (distance < 0.001f) continue;

            float strenght = 1f - Mathf.Clamp01(distance/separationRadius);
            totalForce += direction.normalized * strenght;
            cont++;
        }
        if (cont == 0) return Vector3.zero;
        totalForce /= cont;
        return CalculateSteering(totalForce * maxSpeed);
    }

    private Vector3 Cohesion()
    {
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, cohesionRadius, LayerMask.GetMask("Boids"));
        Vector3 averagePosition = Vector3.zero;
        int cont = 0;
        foreach (var boidTaken in boidsInRange)
        {
            if (boidTaken == owner.GetComponent<Collider>()) continue;
            averagePosition += boidTaken.transform.position;
            cont++;
        }
        if (cont == 0) return Vector3.zero;
        return Seek(averagePosition / cont);
    }

    private Vector3 Allignment()
    {
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, cohesionRadius, LayerMask.GetMask("Boids"));
        Vector3 averageVelocity = Vector3.zero;
        int cont = 0;
        foreach (var boidTaken in boidsInRange)
        {
            if (boidTaken.transform == owner.transform) continue;
            averageVelocity += boidTaken.transform.forward;
            cont++;
        }
        if (cont == 0) return Vector3.zero;
        return CalculateSteering(averageVelocity.normalized * maxSpeed);
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - owner.transform.position).normalized;
        return CalculateSteering(desired);
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - velocity;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    private void AddForce(Vector3 force)
    {
        velocity = Vector3.ClampMagnitude(velocity + force * Time.fixedDeltaTime, maxSpeed);
    }
}

