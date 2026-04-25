using UnityEngine;

public class EnemyMovement
{
    private Transform owner;
    private Rigidbody ownerRB;

    private float avoidanceRayLength = 2.5f;
    private float avoidanceStrength = 15;
    private float avoidanceRayAngle = 30;
    private LayerMask obstacleLayer;

    public EnemyMovement(Transform transformOwner, LayerMask obs)
    {
        owner = transformOwner;
        ownerRB = transformOwner.GetComponent<Rigidbody>();
        obstacleLayer = obs;
    }

    public void Move(Vector3 target, float speed, float delta)
    {
        Vector3 finalDir = ApplyObstacleAvoidance(target);
        finalDir.y = 0;
        Debug.DrawLine(owner.position, owner.position + finalDir, Color.magenta);
        ownerRB.MovePosition(ownerRB.position + finalDir * speed * delta);
    }

    private Vector3 ApplyObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 avoidance = GetAvoidanceForce(desiredDirection);

        if (avoidance == Vector3.zero)
            return desiredDirection;

        Vector3 finalDirection = (desiredDirection + avoidance * avoidanceStrength).normalized;
        return finalDirection;
    }

    private Vector3 GetAvoidanceForce(Vector2 desiredDirection)
    {
        Vector3 avoidance = Vector3.zero;


        if (Physics.Raycast(owner.position, desiredDirection, out RaycastHit hitCenter, avoidanceRayLength, obstacleLayer))
        {

            Vector3 avoid = Vector3.Cross(hitCenter.normal, Vector3.up);

 
            if (Vector3.Dot(avoid, owner.right) < 0)
                avoid = -avoid;

            avoidance += avoid;
        }


        Vector3 leftDir = Quaternion.Euler(0, -avoidanceRayAngle, 0) * desiredDirection;
        if (Physics.Raycast(owner.position, leftDir, out RaycastHit hitLeft, avoidanceRayLength, obstacleLayer))
        {
            Vector3 avoid = Vector3.Cross(hitLeft.normal, Vector3.up);
            if (Vector3.Dot(avoid, owner.right) < 0)
                avoid = -avoid;

            avoidance += avoid;
        }


        Vector3 rightDir = Quaternion.Euler(0, avoidanceRayAngle, 0) * desiredDirection;
        if (Physics.Raycast(owner.position, rightDir, out RaycastHit hitRight, avoidanceRayLength, obstacleLayer))
        {
            Vector3 avoid = Vector3.Cross(hitRight.normal, Vector3.up);
            if (Vector3.Dot(avoid, owner.right) < 0)
                avoid = -avoid;

            avoidance += avoid;
        }

        return avoidance.normalized;
    }
}

