using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using System;

public class FoodCollector : ICanCollect<IFood>
{
    private List<IFood> _collectedFood = new List<IFood>();
    private Collider _collectCollider;

    public ReactiveProperty<int> OnCollect { get; private set; }
    public FoodCollector(Collider collectPoint)
    {
        OnCollect = new ReactiveProperty<int>();
        _collectCollider = collectPoint;
        _collectCollider.OnTriggerEnterAsObservable().Subscribe(_=> {
            if (_.TryGetComponent(out ICollected collectedItem))
            {
                if (!collectedItem.IsCollected)
                {
                    var item = collectedItem.Collect();
                    if (item is IFood food) Collect(food);
                }
            }

        }).AddTo(collectPoint.gameObject);
    }

    public void Collect(IFood food)
    {
        
        food.FoodView.transform.DOScale(Vector3.zero,1f);
        food.FoodView.transform
            .DOMove(_collectCollider.transform.position, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(()=> food.Respawn());

        _collectedFood.Add(food);
        OnCollect.Value++;
    }
}
