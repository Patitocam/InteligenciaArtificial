using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

namespace Assets.Scenes.Enemy
{
    public class RunAwayStateEnemy : EnemyStates
    {
        private EnemySM sm;
        GameObject target;
        EnemyController owner;
        float speed;

        public RunAwayStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, EnemyController owner, float speed) : base(stateMachine)
        {
            this.sm = sm;
            this.target = Target;
            this.owner = owner;
            this.speed = speed; 
        }
        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            Move();
        }

        private void Move()
        {
            owner.Move(RunAway(), speed);
            owner.transform.LookAt(target.transform.position);
        }

        private Vector3 RunAway()
        {
            Vector3 toTarget = target.transform.position - owner.transform.position;
            float distance = toTarget.magnitude;

            Vector3 desiredDir = ((toTarget - owner.transform.position).normalized) * -1;

            return desiredDir;
        }
    }
}
