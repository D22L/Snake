using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ISnake
{
    public ReactiveProperty<bool> IsDead { get; }    
    public IReadOnlyList<SnakeTailView> TailParts { get; }

    
}
