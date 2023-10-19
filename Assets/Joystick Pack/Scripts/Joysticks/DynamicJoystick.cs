using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick, IJoystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    public IReactiveCommand<Vector2> OnDragAction { get; private set; }

    [SerializeField] private float moveThreshold = 1;

    protected override void Start()
    {
       

        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        OnDragAction.Execute(Direction);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {        

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {           

        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }

    public async UniTask Init()
    {
        OnDragAction = new ReactiveCommand<Vector2>();
        
        await UniTask.CompletedTask;
    }
}