using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
public interface IJoystick
{
    IReactiveCommand<Vector2> OnDragAction { get; }

    UniTask Init();

}
