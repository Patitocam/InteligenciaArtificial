
using System.Collections.Generic;
using UnityEngine;

public class FlockingPatrol: EnemyStates
{
    private EnemySM sm;
    EntityController owner;
    Transform[] wayPoints;
    float speed;
    int currentWaypoint;

    private Vector3 velocity;
    private float maxForce = 10f;
    private float maxSpeed;

    private float separationWeight = 3f;
    private float cohesionWeight = 1f;
    private float allignmentWeight = 1f;

    float separationRadius = 1.5f;
    float cohesionRadius = 6f;

    public FlockingPatrol(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, EntityController owner, float speed, Transform[] wayPoints) : base(stateMachine)
    {
        this.sm = sm;
        this.owner = owner;
        this.speed = speed / 2;
        this.maxSpeed = speed / 2;
        this.wayPoints = wayPoints;
        currentWaypoint = 0;
    }

    public override void Enter()
    {

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
       AddForce(Separation() * separationWeight + Cohesion() * cohesionWeight + Allignment() * allignmentWeight + SeekWaypoint() * 2);
    }

    private Vector3 Separation() 
    {
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, separationRadius, LayerMask.GetMask("Boids"));
        Vector3 totalForce = Vector3.zero;
        int cont = 0;

        foreach (var boidTaken in boidsInRange) 
        {
            if (boidTaken == owner.GetComponent<Collider>()) continue;

            var direction = owner.transform.position - boidTaken.transform.position;
            var force = direction.normalized / direction.magnitude / separationRadius;

            totalForce += force;
            cont++;
        }
        if (cont == 0) return Vector3.zero;

        totalForce /= cont;

        return CalculateSteering(totalForce * maxSpeed);
    }
    private Vector3 Cohesion() 
    {
        var averagePosition = Vector3.zero;
        int cont = 0;
        var boidsInRange = Physics.OverlapSphere(owner.transform.position, cohesionRadius, LayerMask.GetMask("Boids"));

        foreach( var boidTaken in boidsInRange)
        {
            if (boidTaken == owner.GetComponent<Collider>()) continue;

            averagePosition += boidTaken.transform.position;
            cont++;
        }
        if (cont == 0) return Vector3.zero;

        averagePosition /= cont;
        return Seek(averagePosition);
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

    private Vector3 SeekWaypoint()
    {
        if(wayPoints.Length == 0) return Vector3.zero;

        Transform target = wayPoints[currentWaypoint];

        float distance = Vector3.Distance(owner.transform.position, target.position);
        if (distance < 0.4f)
        {
            currentWaypoint = (currentWaypoint + 1) % wayPoints.Length;
            target = wayPoints[currentWaypoint];
        }
        return Seek(target.position);
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - owner.transform.position).normalized;
        return CalculateSteering(desired);
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - velocity;
        return Vector3.ClampMagnitude(steering, maxForce * Time.deltaTime);
    }

    private void AddForce(Vector3 force)
    {
        velocity = Vector3.ClampMagnitude(velocity + force, maxSpeed);
    }

}

