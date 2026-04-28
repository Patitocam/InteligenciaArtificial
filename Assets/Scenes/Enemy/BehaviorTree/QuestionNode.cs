using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class QuestionNode: ITreeNode
{
    ITreeNode trueNode;
    ITreeNode falseNode;
    private Func<bool> question;

    //recibe una Func que devuelve un bool (la pregunta a evaluar) y dos nodos (el nodo verdadero y el nodo falso)
    public QuestionNode(Func<bool> question, ITreeNode trueNode, ITreeNode falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;
        this.question = question;
    }

    //evalua la pregunta, si es verdadera ejecuta el nodo verdadero, sino el nodo falso
    public void Execute()
    {
        if (question.Invoke()) trueNode.Execute();
        else falseNode.Execute();
    }
}
