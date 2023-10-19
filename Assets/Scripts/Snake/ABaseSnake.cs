using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ABaseSnake
{
    protected SnakeView view;
    protected List<SnakeTailView> tail = new List<SnakeTailView>();
    protected FoodCollector foodCollector;
    protected TailFactory _tailFactory;

    private readonly float _rayDistance = 2f;
    private readonly float _maxMinHitDistance = 1f;
    private readonly float _posDistance = 0.5f;
    private readonly int _earthMask = 3;
    private RaycastHit _hit;
    private GameObject _pointer;
    private Vector3 _lastCorrectPos;
    private Quaternion _lastCorrectRot;
    public RaycastHit HitToPlace => _hit;
 
    public ABaseSnake(SnakeView viewValue)
    {
        view = viewValue;
        viewValue.StartTailParts.ForEach(x=>x.transform.parent = null);
        tail.AddRange(viewValue.StartTailParts);
        _tailFactory = new TailFactory();

        InitCollector();
        _pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _pointer.transform.localScale = Vector3.one * 0.2f;
        _pointer.transform.parent = view.transform;
        _pointer.transform.localPosition = view.Head.localPosition;
        _pointer.gameObject.SetActive(false);
    }

    private void InitCollector()
    {
        foodCollector = new FoodCollector(view.FoodCollectCollider);
        foodCollector.OnCollect.Subscribe(_ => {
            var tailPart = _tailFactory.Create(view.TailPfb);            
            var pos = tail[tail.Count - 1].transform.position - tail[tail.Count - 1].transform.forward * view.SnakeSettings.DistanceBetweenTail;
            tailPart.transform.position = pos;
            tailPart.transform.forward = tail[tail.Count - 1].transform.forward;
            tail.Add(tailPart);
        }).AddTo(view);
    }

    protected virtual void Move()
    {
        var targetPos = view.transform.position + view.Head.forward;

        Ray ray = new Ray(view.transform.position, -view.Head.up * _rayDistance);

        if (Physics.Raycast(ray, out _hit, _earthMask))
        {
            if (_hit.distance > _maxMinHitDistance || _hit.distance < _maxMinHitDistance)
            {
                view.transform.position = _hit.point + (view.transform.position - _hit.point).normalized * _posDistance;
                RotateHeadToPlane(_hit.normal);
                _lastCorrectPos = view.transform.position;
                _lastCorrectRot = view.transform.rotation;
            }
        }
        else
        {
            targetPos = _lastCorrectPos;
            view.transform.rotation = _lastCorrectRot;
            view.transform.up = HitToPlace.normal;
        }

        view.transform.position = Vector3.Lerp(view.transform.position, targetPos, Time.deltaTime * view.SnakeSettings.Speed);
        MoveTail();
    }

    private void MoveTail()
    {
        for (int i = 0; i < tail.Count; i++)
        {
            Vector3 target = new Vector3();
            Quaternion rotation = new Quaternion();
            var currentPos = tail[i].transform.position;
            var currentRot = tail[i].transform.rotation;
            if (i == 0)
            {
                target = view.Head.position;
                var targetRotation = Quaternion.LookRotation(view.Head.position - currentPos);
                rotation = targetRotation;
            }
            else
            {                
                target = tail[i - 1].transform.position;
                var targetRotation = Quaternion.LookRotation(target - currentPos);
                rotation = targetRotation;
            }

            if ((currentPos - target).magnitude > view.SnakeSettings.DistanceBetweenTail)
            {
                tail[i].transform.position = Vector3.MoveTowards(currentPos, target, Time.deltaTime * view.SnakeSettings.Speed);
                tail[i].transform.rotation = Quaternion.Lerp(currentRot, rotation, Time.deltaTime * view.SnakeSettings.HeadRotationSpeed);
            }

            
        }
    }

    protected virtual void Rotate(Vector3 direction = new())
    {
        var pointerPos = _pointer.transform.localPosition;
        pointerPos += new Vector3(direction.x,0, direction.y);
        pointerPos = Vector3.ClampMagnitude(pointerPos, 2f);
        pointerPos.y = 0f;
        _pointer.transform.localPosition = pointerPos;

        var targetRotation = Quaternion.FromToRotation(view.Head.forward, _pointer.transform.position - view.Head.position) * view.Head.rotation;

        //        var targetRotation = Quaternion.FromToRotation(view.Head.forward, new Vector3(direction.x, view.Head.forward.y, direction.y)) * view.Head.rotation;       
        view.Head.rotation = Quaternion.Slerp(view.Head.rotation, targetRotation, Time.deltaTime * view.SnakeSettings.HeadRotationSpeed);        
    }

    protected virtual void RotateHeadToPlane(Vector3 normal)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(view.transform.up, normal) * view.transform.rotation;
        view.transform.rotation = Quaternion.Slerp(view.transform.rotation, targetRotation, Time.deltaTime * view.SnakeSettings.HeadRotationSpeedToPLane);       
    }
}
