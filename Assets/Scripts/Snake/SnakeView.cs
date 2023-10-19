using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [field: SerializeField] public Transform Head { get; private set; }
    [field: SerializeField] public Collider FoodCollectCollider { get; private set; }
    [field: SerializeField] public List<SnakeTailView> StartTailParts { get; private set; }
    [field: SerializeField] public SnakeSettings SnakeSettings { get; private set; }
    [field: SerializeField] public SnakeTailView TailPfb { get; private set; }
}
