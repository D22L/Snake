using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanCollect<T> where T : ICollected
{
    void Collect(T item);
}
