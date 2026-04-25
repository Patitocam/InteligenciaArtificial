using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

namespace Assets.Scenes.Enemy
{
    public class RunAwayStateEnemy : EnemyStates
    {
        private EnemySM sm;
        GameObject target;
        EntityController owner;
        float speed;
        float speedUpDistance = 10;

        public RunAwayStateEnemy(EnemySM sm, GenericStateMachine<EnemyStatesEnum> stateMachine, GameObject Target, EntityController owner, float speed) : base(stateMachine)
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
            var dir = RunAway();
            owner.Move(dir.Item1, dir.Item2);
            owner.transform.LookAt(owner.transform.position + dir.Item1);
        }

        private (Vector3, float) RunAway()
        {
            Vector3 toTarget = target.transform.position - owner.transform.position;
            float distance = toTarget.magnitude;
            float speedT = speed + 2 * (speedUpDistance / distance);

            Vector3 desiredDir = ((toTarget).normalized) * -1;

            return (desiredDir, speedT);
        }
    }
}
