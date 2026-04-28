using System;


public class ActionNode: ITreeNode
{
    Action action;
    //Recibe una accion para ejecutar
    public ActionNode(Action action)
    {
        this.action = action;
    }

    //Ejecuta la accion que se le dio al nodo
    public void Execute()
    {
        action.Invoke();
    }
}

