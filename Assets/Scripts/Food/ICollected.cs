using System;
using UniRx;
public interface ICollected
{
    bool IsCollected { get; }
    ICollected Collect();
}
