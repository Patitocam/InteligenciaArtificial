using UnityEngine;

public class EnemyMovement
{
    private Transform owner;
    private Rigidbody ownerRB;
    private LayerMask obstacleLayer;

    private float detectionRadius;
    private float personalArea;
    private float avoidanceAngle;
    private Collider[] colliders;


    public EnemyMovement(Transform transformOwner, LayerMask obs,
        float detectionRadius = 7f, float personalArea = 1f, float avoidanceAngle = 70f, int maxObstacles = 5)
    {
        owner = transformOwner;
        ownerRB = transformOwner.GetComponent<Rigidbody>();
        obstacleLayer = obs;
        this.detectionRadius = detectionRadius;
        this.personalArea = personalArea;
        this.avoidanceAngle = avoidanceAngle;
        colliders = new Collider[maxObstacles];
    }

    // Movimiento normal dirección deseada + obstacle avoidance (Chase, RunAway, etc.)
    public void Move(Vector3 desiredDirection, float speed, float delta)
    {
        Vector3 finalDir = GetAvoidanceDir(desiredDirection);
        finalDir.y = 0;
        finalDir = finalDir.normalized;
        if (finalDir != Vector3.zero)
            owner.rotation = Quaternion.LookRotation(finalDir);
        ownerRB.MovePosition(ownerRB.position + finalDir * speed * delta);
    }

    // Movimiento sin avoidance para patrol con A*
    public void MoveRaw(Vector3 desiredDirection, float speed, float delta)
    {
        Vector3 dir = desiredDirection;
        dir.y = 0;
        dir = dir.normalized;
        if (dir != Vector3.zero)
            owner.rotation = Quaternion.LookRotation(dir);
        ownerRB.MovePosition(ownerRB.position + dir * speed * delta);
    }

    private Vector3 GetAvoidanceDir(Vector3 currentDir)
    {
        int count = Physics.OverlapSphereNonAlloc(owner.position, detectionRadius, colliders, obstacleLayer);

        Collider nearestObs = null;
        float nearestDistance = 0;
        Vector3 nearestClosestPoint = Vector3.zero;



        for (int i = 0; i < count; i++)
        {

            Vector3 closestPoint = colliders[i].ClosestPoint(owner.position);
            closestPoint.y = owner.position.y;

            Vector3 dirToObs = closestPoint - owner.position;
            float distance = dirToObs.magnitude;
            float angle = Vector3.Angle(currentDir, dirToObs);

            if (angle > avoidanceAngle) continue;

            if (nearestObs == null || distance < nearestDistance)
            {
                nearestObs = colliders[i];
                nearestDistance = distance;
                nearestClosestPoint = closestPoint;
            }
        }

        if (nearestObs == null) return currentDir;

        Vector3 relativePos = owner.InverseTransformPoint(nearestClosestPoint);
        Vector3 dirToClosestPoint = (nearestClosestPoint - owner.position).normalized;
        Vector3 newDir = relativePos.x < 0
            ? Vector3.Cross(Vector3.up, dirToClosestPoint)
            : -Vector3.Cross(Vector3.up, dirToClosestPoint);

        float weight = (detectionRadius - Mathf.Clamp(nearestDistance - personalArea, 0, detectionRadius)) / detectionRadius;
        return Vector3.Lerp(currentDir, newDir, weight);
    }
}