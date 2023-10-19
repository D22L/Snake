using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawnSystem
{
    private FoodSpawnSettings _settings;
    private MeshCollider _earthCollider;
    private List<IFood> _spawnedFood;
    private FoodFactory _foodFactory;        
    public FoodSpawnSystem(MeshCollider collider, FoodSpawnSettings settings)
    {        
        _earthCollider = collider;
        _settings = settings;
        _spawnedFood = new List<IFood>();
        _foodFactory = new FoodFactory();
        
    }

    public void SpawnFoodInRandomPosition()
    {
        for (int i = 0; i < _settings.CountInStart; i++)
        {
            var newFood = _foodFactory.Create(_settings.FoodViewPfb);
            newFood.Init(SetRandomPosition);
            SetRandomPosition(newFood);
            _spawnedFood.Add(newFood);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 newPosition = new Vector3();
        bool foundRandomPoint = false;
       
        do
        {            
            foundRandomPoint = GetRandomPointOnColliderSurface(out newPosition);
        } while (!foundRandomPoint);

        return newPosition;
    }
    private void SetRandomPosition(IFood food)
    {
        var newPosition = GetRandomPosition();
        food.FoodView.transform.position = newPosition;
        food.FoodView.transform.up = (newPosition - _earthCollider.transform.position).normalized;
    }

    private bool GetRandomPointOnColliderSurface(out Vector3 pointSurface)
    {
        Vector3 pointOnSurface = Vector3.zero;
        RaycastHit hit;

        Vector3 point = Random.insideUnitSphere * 1000;
        bool pointFound = false;        
        if (Physics.Raycast(point, _earthCollider.transform.position - point, out hit, Mathf.Infinity))
        {
            pointOnSurface = hit.point;
            pointFound = true;
        }
    
        pointSurface = pointOnSurface;
        return pointFound;
    }

}
