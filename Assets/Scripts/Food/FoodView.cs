using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodView : MonoBehaviour, ICollected
{
    private ICollected _collected;

    public bool IsCollected => _collected.IsCollected;

    public void Init(ICollected collected)
    {
        _collected = collected;
    }
    public ICollected Collect()
    {
        return _collected;
    }
}
