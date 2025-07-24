using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    [SerializeField] private float _distance;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _interpolateSpeed;
    [SerializeField] private float _angularSpeed;

    private MainSnake _mainSnake;
    public void Init(MainSnake mainSnake)
    {
        _mainSnake = mainSnake;
    }

    private void Update()
    {
        if (_mainSnake == null) return;

        var targetpos = Vector3.MoveTowards(transform.position, _mainSnake.View.Head.up * _distance, Time.deltaTime * _interpolateSpeed);
        transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.deltaTime * _movementSpeed);
        var rotation = Quaternion.FromToRotation(transform.forward, _mainSnake.View.transform.position - transform.position) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _angularSpeed);
    }
}
