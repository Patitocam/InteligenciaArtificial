using UnityEngine;

public class EntityController : MonoBehaviour
{
    protected LineOfSight LOS;

    [SerializeField] protected GameObject Target;
    [SerializeField] protected float viewAngle;
    [SerializeField] protected float viewLenght;
    [SerializeField] protected LayerMask wallsAndObs;
    [SerializeField] protected LayerMask obs;
    [SerializeField] protected float speed;

    public Rigidbody Rb;

    protected EnemySM enemySm;
    protected EnemyMovement movement;
    protected QuestionNode root;
    protected Rigidbody playerRb;

    virtual public void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);
        playerRb = Target.GetComponent<Rigidbody>();
        movement = new EnemyMovement(transform, obs);
    }

    void Update()
    {
        root.Execute();
    }

    private void FixedUpdate()
    {
        enemySm.Tick(Time.fixedDeltaTime);
    }

    // Con avoidance, para Chase, RunAway, Arrive
    public void Move(Vector3 direction, float speed)
    {
        movement.Move(direction, speed, Time.fixedDeltaTime);
    }

    // Sin avoidance, para Patrol con A* (el path ya esquiva obstáculos)
    public void MoveRaw(Vector3 direction, float speed)
    {
        movement.MoveRaw(direction, speed, Time.fixedDeltaTime);
    }
}