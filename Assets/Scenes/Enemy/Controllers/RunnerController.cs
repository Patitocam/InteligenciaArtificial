
public class RunnerController: EntityController
{
    ActionNode seeingNode;
    ActionNode notSeeingNode;
    QuestionNode seeQuestionNode;

    public override void Start()
    {
        base.Start();
        enemySm = new EnemySM(Target, playerRb, this, speed, movement);
        seeingNode = new ActionNode(SeeingPlayer);
        notSeeingNode = new ActionNode(NotSeeingPlayer);

        seeQuestionNode = new QuestionNode(IsSeeing, seeingNode, notSeeingNode);
        root = seeQuestionNode;
    }

    private bool IsSeeing() => LOS.Sight();

    void SeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.RunAway);
    }
    void NotSeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Idle);
    }

}

