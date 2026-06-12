using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PfNode : MonoBehaviour
{
    [SerializeField] List<PfNode> neighbors = new();
    [SerializeField] private int x, y;
    [SerializeField] private float cost = 1;
    private Renderer rend;
    private BoxCollider coll;

    [SerializeField] private bool reacheable = true;
    public List<PfNode> Neighbors => neighbors;
    public bool Reacheable => reacheable;
    public float Cost => cost;

    public int X => x;
    public int Y => y;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        coll = GetComponent<BoxCollider>();
        reacheable = Check();
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

    bool Check()
    {
        Collider[] collisions = Physics.OverlapBox(transform.position, coll.size, transform.rotation, LayerMask.GetMask("Walls"));

        foreach (Collider col in collisions) 
        {

            if (LayerMask.LayerToName(col.gameObject.layer) == "Walls")
            {
                neighbors = new List<PfNode>();
                return false;
            }
            else return true;
        }
        return true;
    }
}
