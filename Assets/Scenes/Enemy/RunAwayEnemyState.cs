using UnityEngine;

namespace Assets.Scenes.Enemy
{
    public class RunAwayStateEnemy : EnemyStates
    {
        private EnemySM sm;

        public RunAwayStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target) : base(stateMachine)
        {
            this.sm = sm;
        }
        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            RunAway();
        }
        private void RunAway()
        {
            Debug.Log("Im Running away");
        }
    }
}
