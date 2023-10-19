using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ABaseFood : IFood
{
    public FoodView FoodView { get; protected set; }
    public bool IsCollected {get; private set;}

    private Action<IFood> _onRespawnAction;
    public ABaseFood(FoodView view)
    {        
        FoodView = view;
        FoodView.Init(this);
    }

    public void SetActive(bool state)
    {
        FoodView.gameObject.SetActive(state);
    }

    public void SetPosition(Vector3 position)
    {
        FoodView.transform.position = position;
    }

    public ICollected Collect()
    {
        IsCollected = true;
        return this;
    }

    public void Respawn()
    {
        IsCollected = false;
        FoodView.transform.localScale = Vector3.one;
        _onRespawnAction?.Invoke(this);
    }

    public void Init(Action<IFood> onRespawnAction)
    {
        _onRespawnAction = onRespawnAction;
    }
}
