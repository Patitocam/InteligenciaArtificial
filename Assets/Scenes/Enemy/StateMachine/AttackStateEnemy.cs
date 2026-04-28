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

    // Si el jugador está en un radio de 1.5 unidades alrededor del enemigo, se recarga la escena (simulando que el jugador muere)
    private void Attack()
    {
        Physics.OverlapSphereNonAlloc(owner.transform.position, 1.5f, collisions);
        foreach (Collider collider in collisions)
        {
            if (collider.CompareTag("Player"))
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

}