using UnityEngine;

[System.Serializable]
public class FoodSpawnSettings
{
    [field: SerializeField] public int CountInStart { get; private set; }
    [field: SerializeField] public FoodView FoodViewPfb { get; private set; }
}
