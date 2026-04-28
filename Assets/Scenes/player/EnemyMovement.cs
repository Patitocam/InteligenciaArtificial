using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement
{
    private Transform owner;
    private Rigidbody ownerRB;
    private LayerMask obstacleLayer;

    // Parámetros del avoidance
    private float detectionRadius;
    private float personalArea;
    private Collider[] colliders;

    public EnemyMovement(Transform transformOwner, LayerMask obs, float detectionRadius = 7f, float personalArea = 1f, int maxObstacles = 5)
    {
        owner = transformOwner;
        ownerRB = transformOwner.GetComponent<Rigidbody>();
        obstacleLayer = obs;
        this.detectionRadius = detectionRadius;
        this.personalArea = personalArea;
        colliders = new Collider[maxObstacles];
    }

    public void Move(Vector3 desiredDirection, float speed, float delta)
    {
        Vector3 finalDir = GetAvoidanceDir(desiredDirection);
        finalDir.y = 0;
        finalDir = finalDir.normalized;
        if (finalDir != Vector3.zero)
            owner.rotation = Quaternion.LookRotation(finalDir);
        ownerRB.MovePosition(ownerRB.position + finalDir * speed * delta);
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

            if (nearestObs == null || distance < nearestDistance)
            {
                nearestObs = colliders[i];
                nearestDistance = distance;
                nearestClosestPoint = closestPoint;
            }
        }

        // Sin obstáculos, seguir dirección deseada
        if (nearestObs == null) return currentDir;

        Vector3 relativePos = owner.InverseTransformPoint(nearestClosestPoint);
        Vector3 dirToClosetPoint = (nearestClosestPoint - owner.position).normalized;
        Vector3 newDir;
        if (relativePos.x < 0)
        {
            newDir = Vector3.Cross(Vector3.up, dirToClosetPoint);
        }
        else
        {
            newDir = -Vector3.Cross(Vector3.up, dirToClosetPoint);
        }

        return Vector3.Lerp(currentDir, newDir, (detectionRadius - Mathf.Clamp(nearestDistance - personalArea, 0, detectionRadius)) / detectionRadius);
    }
}