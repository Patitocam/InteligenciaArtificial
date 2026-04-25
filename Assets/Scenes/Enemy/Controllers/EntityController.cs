using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(enemySm.fsm.CurrentState);
    }
    private void FixedUpdate()
    {
        enemySm.Tick(Time.fixedDeltaTime);
    }

    public void Move(Vector3 target, float speed)
    {
        movement.Move(target, speed, Time.fixedDeltaTime);
    }
}
 