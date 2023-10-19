using System;
using UnityEngine;

public interface IFood : ICollected
{    
    public FoodView FoodView { get; }
    void Init(Action<IFood> onRespawnAction);
    void SetPosition(Vector3 position);
    void SetActive(bool state);    
    void Respawn();

}
