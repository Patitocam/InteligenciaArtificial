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

    // Inicializa el LineOfSight, el EnemySM y el EnemyMovement con los parámetros necesarios
    virtual public void Start()
    {
        LOS = new LineOfSight(Target, viewAngle, viewLenght, wallsAndObs, this.transform);
        playerRb = Target.GetComponent<Rigidbody>();
        movement = new EnemyMovement(transform, obs);
    }

    // Ejecuta el tree de comportamiento y hace tick al EnemySM
    void Update()
    {
        root.Execute();
        Debug.Log(enemySm.fsm.CurrentState);
    }

    // Hace tick al EnemySM en FixedUpdate para utilizar físicas correctamente
    private void FixedUpdate()
    {
        enemySm.Tick(Time.fixedDeltaTime);
    }

    // Llama al método Move del EnemyMovement para mover al enemigo hacia el objetivo con la velocidad dada
    public void Move(Vector3 target, float speed)
    {
        movement.Move(target, speed, Time.fixedDeltaTime);
    }
}
 