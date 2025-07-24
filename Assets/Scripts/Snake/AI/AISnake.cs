using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class AISnake:ABaseSnake
{    
    private SnakeSettings _settinngs;
    private Vector3 _currentDirrection;
    private float _updateDirrectionTime = 5f;
    private float _accumulatedTime = 0f;

    public AISnake(SnakeView viewValue, SnakeSettings snakeSettings) : base(viewValue, snakeSettings)
    {
        _settinngs = snakeSettings;

        StartMove();
    }


    private void StartMove()
    {
        CalculateDirection();

        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (IsDead.Value || view == null) return;

            Move();
            Rotate(_currentDirrection);
            _accumulatedTime += Time.deltaTime;

            if (_accumulatedTime >= _updateDirrectionTime)
            {
                CalculateDirection();
                _accumulatedTime = 0f;
            }
            
        });
    }

    private void CalculateDirection()
    {
        _currentDirrection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _updateDirrectionTime = Random.Range(3f, 8f);
    }

}
