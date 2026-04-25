using UnityEngine;

public class EnemyMovement
{
    private Transform owner;
    private Rigidbody ownerRB;
    private LayerMask obstacleLayer;

    // Parámetros del avoidance
    private float detectionRadius;
    private float detectionAngle;
    private float personalArea;
    private Collider[] colliders;

    public EnemyMovement(Transform transformOwner, LayerMask obs,
        float detectionRadius = 6f, float detectionAngle = 120f,
        float personalArea = 0.5f, int maxObstacles = 5)
    {
        owner = transformOwner;
        ownerRB = transformOwner.GetComponent<Rigidbody>();
        obstacleLayer = obs;
        this.detectionRadius = detectionRadius;
        this.detectionAngle = detectionAngle;
        this.personalArea = personalArea;
        colliders = new Collider[maxObstacles];
    }

    public void Move(Vector3 desiredDirection, float speed, float delta)
    {
        Vector3 finalDir = GetAvoidanceDir(desiredDirection);
        finalDir.y = 0;
        finalDir = finalDir.normalized;
        Debug.DrawLine(owner.position, owner.position + finalDir, Color.magenta);
        ownerRB.MovePosition(ownerRB.position + finalDir * speed * delta);
    }

    private Vector3 GetAvoidanceDir(Vector3 currentDir)
    {
        int count = Physics.OverlapSphereNonAlloc(owner.position, detectionRadius, colliders, obstacleLayer);

        Collider nearestColl = null;
        float nearestDistance = float.MaxValue;
        Vector3 nearestClosestPoint = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Vector3 closestPoint = colliders[i].ClosestPoint(owner.position);
            closestPoint.y = owner.position.y;

            Vector3 dirToColl = closestPoint - owner.position;
            float distance = dirToColl.magnitude;
            float angle = Vector3.Angle(dirToColl, currentDir);

            // Ignorar obstáculos fuera del ángulo de visión
            if (angle > detectionAngle / 2) continue;

            if (distance < nearestDistance)
            {
                nearestColl = colliders[i];
                nearestDistance = distance;
                nearestClosestPoint = closestPoint;
            }
        }

        // Sin obstáculos, seguir dirección deseada
        if (nearestColl == null) return currentDir;

        // Determinar hacia qué lado esquivar usando espacio local
        Vector3 relativePos = owner.InverseTransformPoint(nearestClosestPoint);
        Vector3 dirToObstacle = (nearestClosestPoint - owner.position).normalized;

        Vector3 avoidDir = relativePos.x < 0
            ? Vector3.Cross(owner.up, dirToObstacle)   // obstáculo a la izquierda → girar derecha
            : -Vector3.Cross(owner.up, dirToObstacle); // obstáculo a la derecha → girar izquierda

        // Lerp: cuanto más cerca el obstáculo, más fuerte el avoidance
        float weight = (detectionRadius - Mathf.Clamp(nearestDistance - personalArea, 0, detectionRadius)) / detectionRadius;
        return Vector3.Lerp(currentDir, avoidDir, weight);
    }
}