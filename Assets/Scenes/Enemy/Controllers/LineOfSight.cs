using UnityEngine;


public class LineOfSight 
{
    private GameObject Target;
    private float viewAngle;
    private float viewLenght;
    private LayerMask wallsAndObs;

    private float realAngle;
    private Transform transform;

    // Recibe el objetivo a detectar, el ángulo de visión, la distancia de visión, las capas que bloquean la visión y el transform del enemigo
    public LineOfSight(GameObject Target, float viewAngle, float viewLenght, LayerMask wallsAndObs, Transform transform)
    {
        this.Target = Target;
        this.viewAngle = viewAngle;
        this.viewLenght = viewLenght;
        this.wallsAndObs = wallsAndObs;
        realAngle = viewAngle / 2;
        this.transform = transform;
    }

    // Devuelve true si el objetivo está dentro del ángulo de visión, dentro de la distancia de visión y no hay obstáculos bloqueando 
    public bool Sight()
    {
        var dir = Target.transform.position - transform.position;
        if (dir.magnitude > viewLenght) return false;
        if (Vector3.Angle(transform.forward, dir) > realAngle) return false;
        if (Physics.Raycast(transform.position, dir.normalized, dir.magnitude, wallsAndObs)) return false;
        return true;
    }

    // Modifica el ángulo de visión a 360 grados (para perseguir al jugador)
    public void ModifyLosAngle()
    {
        realAngle = 360;
    }

    // Resetea el ángulo de visión al valor original (para patrullar)
    public void ResetLosAngle(float viewAngle)
    {
        realAngle = viewAngle / 2;
    }
}

