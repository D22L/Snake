using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseFactory <T> where T : class
{
    public abstract T Create( object arg = null);

}
