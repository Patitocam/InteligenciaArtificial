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

    [SerializeField] private bool reacheable = true;
    public List<PfNode> Neighbors => neighbors;
    public bool Reacheable => reacheable;
    public float Cost => cost;

    public int X => x;
    public int Y => y;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

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
                Debug.Log("Obs");
                neighbors = new List<PfNode>();
                return false;
            }
        }
        return true;
    }
}
