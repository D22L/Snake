using UnityEngine;

[CreateAssetMenu(fileName = "SnakeSettings", menuName = "Configs/SnakeSettings")]
public class SnakeSettings : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }    
    [field: SerializeField] public int StartTailLenght { get; private set; }
    [field: SerializeField] public float HeadRotationSpeed { get; private set; }
    [field: SerializeField] public float HeadRotationSpeedToPLane { get; private set; }
    [field: SerializeField] public float DistanceBetweenTail { get; private set; }
    
}
