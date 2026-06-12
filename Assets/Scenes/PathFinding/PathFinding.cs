using UnityEngine;
using System.Collections.Generic;

public class PathFinding
{
    public static List<PfNode> Astar(PfNode start, PfNode end)
    {
        var frontier = new PriorityQueue<PfNode>();
        frontier.Enqueue(start, 0);
        var cameFrom = new Dictionary<PfNode, PfNode>();
        cameFrom[start] = null;
        var costSoFar = new Dictionary<PfNode, float>();
        costSoFar[start] = 0;

        while (!frontier.IsEmpty)
        {
            PfNode current = frontier.Dequeue();

            if (current == end)
            {
                PfNode newCurrent = end;
                var path = new List<PfNode>();
                while (newCurrent != null)
                {
                    path.Add(newCurrent);
                    newCurrent = cameFrom[newCurrent];
                }
                path.Reverse();
                return path;
            }

            foreach (var next in current.Neighbors)
            {
                if (!next.Reacheable) continue;

                var newCost = costSoFar[current] + next.Cost;
                if (!cameFrom.ContainsKey(next) || newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next] = newCost + ManhattanHeuristic(next, end);
                    cameFrom[next] = current;
                }
            }
        }
        return new List<PfNode>();
    }

    private static int ManhattanHeuristic(PfNode a, PfNode b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }
}

