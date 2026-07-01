using UnityEngine;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;

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
                return CleanUp(path);
            }

            foreach (var next in current.Neighbors)
            {
                if (!next.Reacheable) continue;

                var newCost = costSoFar[current] + next.Cost;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;                                          
                    float priority = newCost + ManhattanHeuristic(next, end);         
                    frontier.Enqueue(next, priority);
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

    private static List<PfNode> CleanUp (List<PfNode> list)
    {
        if (list.Count <= 2) return list;

        List<PfNode> result = new();

        int current = 0;
        result.Add(list[current]);

        while (current < list.Count - 1)
        {
            int next = current + 1;

            for (int i =  list.Count- 1; i > current; i--)
            {
                if (list[current].HasNextNodeInSight(list[i]))
                {
                    next = i;
                    break;
                }
            }
            result.Add(list[next]);
            current = next;
        }
        return result;
    }
}

