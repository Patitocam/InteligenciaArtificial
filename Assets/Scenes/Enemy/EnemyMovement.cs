using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement
{
    private Transform owner;
    private Rigidbody ownerRB;
    private LayerMask obstacleLayer;

    // Parámetros del avoidance
    private float detectionRadius;
    private float personalArea;
    private Collider[] colliders;

    // El constructor de la clase recibe el transform del enemigo, la capa de obstáculos, el radio de detección para detectar obstáculos, el área personal para evitar que el enemigo se acerque demasiado a los obstáculos y el número máximo de obstáculos a detectar.
    public EnemyMovement(Transform transformOwner, LayerMask obs, float detectionRadius = 7f, float personalArea = 1f, int maxObstacles = 5)
    {
        owner = transformOwner;
        ownerRB = transformOwner.GetComponent<Rigidbody>();
        obstacleLayer = obs;
        this.detectionRadius = detectionRadius;
        this.personalArea = personalArea;
        colliders = new Collider[maxObstacles];
    }

    // El enemigo se mueve en la dirección deseada, pero ajusta su dirección para evitar obstáculos detectados en su camino.
    public void Move(Vector3 desiredDirection, float speed, float delta)
    {
        Vector3 finalDir = GetAvoidanceDir(desiredDirection);
        finalDir.y = 0;
        finalDir = finalDir.normalized;

        if (finalDir != Vector3.zero)
            owner.rotation = Quaternion.LookRotation(finalDir);

        ownerRB.MovePosition(ownerRB.position + finalDir * speed * delta);
    }

    // El enemigo detecta obstáculos en un radio determinado y calcula una dirección de avoidance para esquivarlos, dando prioridad a los obstáculos más cercanos. Cuanto más cerca esté el enemigo del obstáculo, más fuerte será la dirección de avoidance.
    private Vector3 GetAvoidanceDir(Vector3 currentDir)
    {
        int count = Physics.OverlapSphereNonAlloc(owner.position, detectionRadius, colliders, obstacleLayer); //Creamos una esfera y contamos sus colisiones

        Collider nearestObs = null;
        float nearestDistance = 0;
        Vector3 nearestClosestPoint = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Vector3 closestPoint = colliders[i].ClosestPoint(owner.position); //agarramos el punto mas cercano del obstaculo al owner
            closestPoint.y = owner.position.y; //cancelamos la Y

            Vector3 dirToObs = closestPoint - owner.position; //Dirección al obstaculo
            float distance = dirToObs.magnitude; //Distancia al obstaculo

            if (nearestObs == null || distance < nearestDistance) //Seleccionamos el obstáculo mas cercano 
            {
                nearestObs = colliders[i];
                nearestDistance = distance;
                nearestClosestPoint = closestPoint;
            }
        }

        // Sin obstáculos, seguir dirección deseada
        if (nearestObs == null) return currentDir;

        //Analiza si el obstaculo debe ser esquivado a la izquierda o a la derecha
        Vector3 relativePos = owner.InverseTransformPoint(nearestClosestPoint);
        Vector3 dirToClosetPoint = (nearestClosestPoint - owner.position).normalized;
        Vector3 newDir;
        if (relativePos.x < 0)
        {
            newDir = Vector3.Cross(Vector3.up, dirToClosetPoint);
        }
        else
        {
            newDir = -Vector3.Cross(Vector3.up, dirToClosetPoint);
        }

        //Devuelve un punto entre la dirección deseada y la dirección para esquivar un obstáculo, según qué tan cerca del obstáculo esté
        return Vector3.Lerp(currentDir, newDir, (detectionRadius - Mathf.Clamp(nearestDistance - personalArea, 0, detectionRadius)) / detectionRadius);
    }
}