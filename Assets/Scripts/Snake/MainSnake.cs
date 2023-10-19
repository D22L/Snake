using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class MainSnake : ABaseSnake
{
    public SnakeView View => view;
    private IJoystick _joystick;    
    public MainSnake(SnakeView viewValue, IJoystick joystick) : base(viewValue)
    {
        _joystick = joystick;

        Init();
    }

    private void Init()
    {  
        _joystick.OnDragAction.Subscribe(direction =>
        {            
            Rotate(direction);

        }).AddTo(view);

        Observable.EveryUpdate().Subscribe(_ =>
        {
            Move();
        });
    }

}
