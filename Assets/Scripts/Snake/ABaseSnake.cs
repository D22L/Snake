using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public abstract class ABaseSnake: ISnake
{
    protected SnakeView view;
    protected List<SnakeTailView> tailParts = new List<SnakeTailView>();
    protected FoodCollector foodCollector;
    protected TailFactory _tailFactory;
    protected SnakeSettings settings;    

    private readonly float _rayDistance = 2f;
    private readonly float _maxMinHitDistance = 1f;
    private readonly float _posDistance = 0.5f;
    private readonly int _earthMask = 3;
    private RaycastHit _hit;
    private GameObject _pointer;
    private Vector3 _lastCorrectPos;
    private Quaternion _lastCorrectRot;
    private RaycastHit _previuosHit;

    public SnakeView View => view;
    public ReactiveProperty<bool> IsDead { get; private set; } = new ReactiveProperty<bool>();
    public IReadOnlyList<SnakeTailView> TailParts => tailParts;

    public ABaseSnake(SnakeView viewValue, SnakeSettings settings)
    {
        view = viewValue;
        _lastCorrectPos = view.transform.position;
        _lastCorrectRot = view.transform.rotation;

        viewValue.StartTailParts.ForEach(x=>x.transform.parent = null);
        tailParts.AddRange(viewValue.StartTailParts);
        _tailFactory = new TailFactory();
        this.settings = settings;
        
        SetSkinColor();

        InitCollector();
        InitHeadCollider();
        _pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _pointer.transform.localScale = Vector3.one * 0.2f;
        _pointer.transform.parent = view.transform;
        _pointer.transform.localPosition = view.Head.localPosition;
        _pointer.gameObject.SetActive(false);
    }

    private void SetSkinColor()
    {
        var renderes = view.SkinView.StartRenderer;
        
        for (int i = 0; i < renderes.Length; i++)
        {
            var mats = renderes[i].materials;
            for (int m = 0; m < mats.Length; m++)
            {
                mats[m].SetColor("_Color",settings.SkinColor);
            }
            
        }

        view.SkinView.HeadRender.materials[view.SkinView.HeadBaseVaterialIndex].SetColor("_Color", settings.SkinColor);
    }

    private void InitHeadCollider()
    {
        view.HeadCollider.OnTriggerEnterAsObservable().Subscribe(collider => {
            if (collider.gameObject.TryGetComponent(out SnakeTailView tail)) 
            {
                if (tailParts.Contains(tail))
                {
                    return;
                }
                else
                {
                    Die();
                }
            }

        }).AddTo(view);
    }

    private void InitCollector()
    {
        foodCollector = new FoodCollector(view.FoodCollectCollider);
        foodCollector.OnCollect.Subscribe(_ => {
            var tailPart = _tailFactory.Create(view.TailPfb);
            tailPart.Render.materials[0].SetColor("_Color",settings.SkinColor);
            var pos = tailParts[tailParts.Count - 1].transform.position - tailParts[tailParts.Count - 1].transform.forward * settings.DistanceBetweenTail;
            tailPart.transform.position = pos;
            tailPart.transform.forward = tailParts[tailParts.Count - 1].transform.forward;
            tailParts.Add(tailPart);
        }).AddTo(view);
    }

    protected virtual void Move()
    {
        var targetPos = view.transform.position + view.Head.forward;

        Ray ray = new Ray(targetPos, -view.Head.up * _rayDistance);

        if (Physics.Raycast(ray, out _hit, _earthMask))
        {
            if (_hit.distance > _maxMinHitDistance || _hit.distance < _maxMinHitDistance)
            {
                targetPos = _hit.point + _hit.normal * _posDistance;
                RotateHeadToPlane(_hit.normal);
                _lastCorrectPos = view.transform.position;
                _lastCorrectRot = view.transform.rotation;
                _previuosHit = _hit;                
            }
        }
        else
        {
            targetPos = _lastCorrectPos;
            view.transform.rotation = _lastCorrectRot;
            view.transform.up = _previuosHit.normal;
            
        }

        view.transform.position = Vector3.Lerp(view.transform.position, targetPos, Time.deltaTime * settings.Speed);
        MoveTail();
    }

    private void MoveTail()
    {
        for (int i = 0; i < tailParts.Count; i++)
        {
            Vector3 target = new Vector3();
            Quaternion rotation = new Quaternion();
            var currentPos = tailParts[i].transform.position;
            var currentRot = tailParts[i].transform.rotation;
            if (i == 0)
            {
                target = view.Head.position;
                var targetRotation = Quaternion.LookRotation(view.Head.position - currentPos);
                rotation = targetRotation;
            }
            else
            {                
                target = tailParts[i - 1].transform.position;
                var targetRotation = Quaternion.LookRotation(target - currentPos);
                rotation = targetRotation;
            }

            if ((currentPos - target).magnitude > settings.DistanceBetweenTail)
            {
                tailParts[i].transform.position = Vector3.MoveTowards(currentPos, target, Time.deltaTime * settings.Speed * 1.5f);
                tailParts[i].transform.rotation = Quaternion.Lerp(currentRot, rotation, Time.deltaTime * settings.HeadRotationSpeed);
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
        view.Head.rotation = Quaternion.Slerp(view.Head.rotation, targetRotation, Time.deltaTime * settings.HeadRotationSpeed);        
    }

    protected virtual void RotateHeadToPlane(Vector3 normal)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(view.transform.up, normal) * view.transform.rotation;
        view.transform.rotation = Quaternion.Slerp(view.transform.rotation, targetRotation, Time.deltaTime * settings.HeadRotationSpeedToPLane);       
    }

    protected virtual void Die()
    {
        if (IsDead.Value) return;

        Debug.Log("Die");
        IsDead.Value = true;
        view.gameObject.SetActive(false);
        tailParts.ForEach(x => x.gameObject.SetActive(false));
    }
}
