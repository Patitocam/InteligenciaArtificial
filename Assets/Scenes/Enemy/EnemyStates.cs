using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EnemyStates: State<EnemyStatesEnum>
{
    public EnemyStates(GenericStateMachine<EnemyStatesEnum> stateMachine) : base(stateMachine)
    {

    }
}

