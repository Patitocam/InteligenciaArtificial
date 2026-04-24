using UnityEngine;


public class LineOfSight 
{
    private GameObject Target;
    private float viewAngle;
    private float viewLenght;
    private LayerMask wallsAndObs;

    private float realAngle;
    private Transform transform;

    public LineOfSight(GameObject Target, float viewAngle, float viewLenght, LayerMask wallsAndObs, Transform transform)
    {
        this.Target = Target;
        this.viewAngle = viewAngle;
        this.viewLenght = viewLenght;
        this.wallsAndObs = wallsAndObs;
        realAngle = viewAngle / 2;
        this.transform = transform;
    }

    public bool Sight()
    {
        var dir = Target.transform.position - transform.position;
        if (dir.magnitude > viewLenght) return false;
        if (Vector3.Angle(transform.forward, dir) > realAngle) return false;
        if (Physics.Raycast(transform.position, dir.normalized, dir.magnitude, wallsAndObs)) return false;
        return true;
    }
}

