using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class MainSnake : ABaseSnake
{
    public SnakeView View => view;
    private IJoystick _joystick;
    private bool _isStoped;
    public MainSnake(SnakeView viewValue, SnakeSettings settings, IJoystick joystick) : base(viewValue, settings)
    {
        _joystick = joystick;

        Init();
    }

    protected override void Die()
    {
        Camera.main.transform.parent = null;
        base.Die();
       
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
            if (IsDead.Value || _isStoped) return;
            Move();
        });
    }

    public void Stop()
    {
        _isStoped = true;
    }
}
