using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class MainSnake : ABaseSnake
{
    public SnakeView View => view;
    private IJoystick _joystick;    
    public MainSnake(SnakeView viewValue, SnakeSettings settings, IJoystick joystick) : base(viewValue, settings)
    {
        _joystick = joystick;

        Init();
    }

    private void Init()
    {  
        _joystick.OnDragAction.Subscribe(direction =>
        {
            if (IsDead.Value) return;
            Rotate(direction);

        }).AddTo(view);

        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (IsDead.Value) return;
            Move();
        });
    }

}
