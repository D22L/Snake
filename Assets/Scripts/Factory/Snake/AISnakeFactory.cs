using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeFactory : ABaseFactory<AISnake>
{
    public override AISnake Create(object arg = null)
    {
        AISnakeFactoryArg createData = (AISnakeFactoryArg)arg;

         var view = MonoBehaviour.Instantiate(createData.Pfb);
        view.transform.position = createData.StartPosition + createData.Normal*0.5f;
        view.transform.up = createData.Normal;

        AISnake snake = new AISnake(view, createData.Settings);

        return snake;
    }
}
