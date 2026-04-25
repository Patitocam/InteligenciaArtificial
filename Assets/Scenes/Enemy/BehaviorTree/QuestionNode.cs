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

    public QuestionNode(Func<bool> question, ITreeNode trueNode, ITreeNode falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;
        this.question = question;
    }

    public void Execute()
    {
        if (question.Invoke()) trueNode.Execute();
        else falseNode.Execute();
    }
}
