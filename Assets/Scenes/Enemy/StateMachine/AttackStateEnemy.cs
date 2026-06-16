using UnityEngine;
using UnityEngine.SceneManagement;
public class AttackStateEnemy : EnemyStates
{
    private EnemySM sm;
    EntityController owner;
    Collider[] collisions;

    public AttackStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, EntityController owner) : base(stateMachine)
    {
        this.sm = sm;
        this.owner = owner;
        collisions = new Collider[5];
    }
 
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Attack();
    }

    // Si el jugador está en un radio de 1.5 unidades alrededor del enemigo, se recarga la escena
    private void Attack()
    {
        var coll =Physics.OverlapSphere(owner.transform.position, 1.5f);

        foreach (Collider collider in coll)
        {
            if (collider.CompareTag("Player"))
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

}