using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFoodHandler : MonoBehaviour
{
    [SerializeField] private FoodSpawnSettings _foodSpawnSettings;
    [SerializeField] private MeshCollider _earthCollider;

    private FoodSpawnSystem _foodSpawnSystem;

    public FoodSpawnSystem foodSpawn => _foodSpawnSystem;
    private void OnEnable()
    {
        _foodSpawnSystem = new FoodSpawnSystem(_earthCollider, _foodSpawnSettings);

        _foodSpawnSystem.SpawnFoodInRandomPosition();
    }
}
