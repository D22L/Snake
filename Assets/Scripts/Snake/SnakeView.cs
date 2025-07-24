using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [field: SerializeField] public Transform Head { get; private set; }
    [field: SerializeField] public Collider FoodCollectCollider { get; private set; }
    [field: SerializeField] public Collider HeadCollider { get; private set; }
    [field: SerializeField] public List<SnakeTailView> StartTailParts { get; private set; }    
    [field: SerializeField] public SnakeTailView TailPfb { get; private set; }
    [field: SerializeField] public SnakeSkinView SkinView { get; private set; }
}
