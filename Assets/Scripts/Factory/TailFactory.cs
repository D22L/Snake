using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailFactory : ABaseFactory<SnakeTailView>
{
    public override SnakeTailView Create(object arg = null)
    {
        if (arg is SnakeTailView tailPfb)
        {
            return MonoBehaviour.Instantiate(tailPfb);
        }
        else return null;
    }
}
