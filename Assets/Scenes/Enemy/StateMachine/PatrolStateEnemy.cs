using System.Collections.Generic;
using UnityEngine;

public class PatrolStateEnemy : EnemyStates
{
    private EnemySM sm;
    EntityController owner;
    float speed;
    Transform[] wayPoints;
    int currentWaypoint;

    GridGenerator grid;
    List<PfNode> currentPath = new();
    int pathIndex = 0;

    public PatrolStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, EntityController owner, float speed, Transform[] wayPoints, GridGenerator grid) : base(stateMachine)
    {
        this.sm = sm;
        this.owner = owner;
        this.speed = speed / 2;
        this.wayPoints = wayPoints;
        this.grid = grid;
        currentWaypoint = 0;
    }

    public override void Enter()
    {
        RecalculatePath();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move();
    }

    private void Move()
    {
        if (currentPath == null || currentPath.Count == 0) return;

        if (pathIndex >= currentPath.Count)
        {
            currentWaypoint = (currentWaypoint + 1) % wayPoints.Length;
            RecalculatePath();
            return;
        }

        Vector3 originalPos = currentPath[pathIndex].transform.position;
        Vector3 nextPos = currentPath[Mathf.Clamp(pathIndex + 1, 0, currentPath.Count - 1)].transform.position;
        Vector3 targetPos = Vector3.Lerp(nextPos, originalPos, Mathf.Clamp01((owner.transform.position - originalPos).magnitude));
        Vector3 ownerXZ = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
        Vector3 targetXZ = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 direction = (targetXZ - ownerXZ).normalized;

        owner.MoveRaw(direction, speed);

        if ((targetXZ - ownerXZ).magnitude < 0.4f)
        {
            pathIndex++;
            if (pathIndex >= currentPath.Count)
            {
                currentWaypoint = (currentWaypoint + 1) % wayPoints.Length;
                RecalculatePath();
            }
        }
    }

    private void RecalculatePath()
    {
        PfNode startNode = GetClosestNode(owner.transform.position);
        PfNode endNode = GetClosestNode(wayPoints[currentWaypoint].position);
        if (startNode == null || endNode == null) return;
        currentPath = PathFinding.Astar(startNode, endNode);
        pathIndex = 0;
    }

    private PfNode GetClosestNode(Vector3 worldPos)
    {
        PfNode closest = null;
        float minDist = float.MaxValue;
        foreach (PfNode node in grid.nodeGrid)
        {
            if (!node.Reacheable) continue;
            float dist = Vector3.Distance(worldPos, node.transform.position);
            if (dist < minDist) { minDist = dist; closest = node; }
        }
        return closest;
    }
}