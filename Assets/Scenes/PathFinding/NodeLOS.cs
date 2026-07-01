
using System.Collections.Generic;
using UnityEngine;

public class NodeLOS
{
    private List<PfNode> neighbors;
    private Dictionary<PfNode, PfNode> nodes;
    private float viewLenght;
    private LayerMask wallsAndObs;

    private float realAngle;
    private Transform transform;


    // Recibe el objetivo a detectar, el ángulo de visión, la distancia de visión, las capas que bloquean la visión y el transform del enemigo
    public NodeLOS(float viewLenght, LayerMask wallsAndObs, Transform transform, List<PfNode> neigs)
    {
        this.viewLenght = 300;
        this.wallsAndObs = wallsAndObs;
        realAngle = 360;
        this.transform = transform;
        neighbors = neigs;
        nodes = new Dictionary<PfNode, PfNode>();
        foreach (var N in neighbors) {nodes.Add(N, N);}
    }

    // Devuelve true si el objetivo está dentro del ángulo de visión, dentro de la distancia de visión y no hay obstáculos bloqueando 
    public bool Sight(PfNode target)
    {
        var dir = target.transform.position - transform.position;

        if (dir.magnitude > viewLenght)
            return false;

        if (Physics.Raycast(transform.position, dir.normalized, dir.magnitude, wallsAndObs))
            return false;

        return true;
    }
}

