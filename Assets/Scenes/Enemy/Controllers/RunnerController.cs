
public class RunnerController: EntityController
{
    ActionNode seeingNode;
    ActionNode notSeeingNode;
    QuestionNode seeQuestionNode;

    // El runner sólo tiene dos estados: correr o quedarse quieto. Si ve al jugador corre, sino se queda quieto.
    public override void Start()
    {
        // Inicializa el EnemySM con los parámetros necesarios para sus estados
        base.Start();
        enemySm = new EnemySM(Target, playerRb, this, speed, movement);
        seeingNode = new ActionNode(SeeingPlayer);
        notSeeingNode = new ActionNode(NotSeeingPlayer);

        // Construye el tree de comportamiento
        seeQuestionNode = new QuestionNode(IsSeeing, seeingNode, notSeeingNode);
        root = seeQuestionNode;
    }

    // Pregunta si ve al jugador
    private bool IsSeeing() => LOS.Sight();

    // Si ve al jugador corre
    void SeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.RunAway);
    }

    // Si no ve al jugador se queda quieto
    void NotSeeingPlayer()
    {
        enemySm.SwitchState(EnemyStatesEnum.Idle);
    }

}

