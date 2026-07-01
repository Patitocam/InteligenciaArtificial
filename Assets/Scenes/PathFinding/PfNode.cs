using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PfNode : MonoBehaviour
{
    [SerializeField] List<PfNode> neighbors = new();
    [SerializeField] private int x, y;
    [SerializeField] private float cost = 1;

    [SerializeField] private float viewLenght;
    [SerializeField] private LayerMask layer;
    private Renderer rend;

    [SerializeField] private bool reacheable = true;
    public List<PfNode> Neighbors => neighbors;
    public bool Reacheable => reacheable;
    public float Cost => cost;

    public int X => x;
    public int Y => y;

    private NodeLOS LOS;

    public List<PfNode> visibleNeighbors = new();

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        reacheable = Check();

        LOS = new NodeLOS(viewLenght, layer, transform, neighbors);
        SetVisibleNeighbors();
    }

    public void SetIndexes(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetNeighbors(List<PfNode> neighbors)
    {
        this.neighbors = neighbors;
    }

    public bool HasNextNodeInSight(PfNode target)
    {
        return LOS.Sight(target);
    }

    private void SetVisibleNeighbors()
    {
        foreach (PfNode node in neighbors) 
        { 
            if (HasNextNodeInSight(node)) visibleNeighbors.Add(node);
        }
    }

    bool Check()
    {
        Collider[] collisions = Physics.OverlapBox(transform.position, new Vector3(1,1,1), transform.rotation);

        foreach (Collider col in collisions) 
        {

            if (LayerMask.LayerToName(col.gameObject.layer) == "Walls" )
            {
                neighbors = new List<PfNode>();
                return false;
            }
            if (LayerMask.LayerToName(col.gameObject.layer) == "Obstacles")
            {
                neighbors = new List<PfNode>();
                return false;
            }
        }
        return true;
    }
}
