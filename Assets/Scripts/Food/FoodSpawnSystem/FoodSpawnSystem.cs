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

    public void SpawnFoodInPositions(List<Vector3> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var newFood = _spawnedFood.Find(x=>x.IsCollected);
            if (newFood == null)
            {
                newFood = _foodFactory.Create(_settings.FoodViewPfb);
            }
            newFood.SetActive(true);
            newFood.Init(SetRandomPosition);
            SetPosition(newFood, positions[i]);
            _spawnedFood.Add(newFood);
        }
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
    private void SetPosition(IFood food, Vector3 position)
    {
        var newPosition = GetRandomPosition();
        food.FoodView.transform.position = position;
        food.FoodView.transform.up = (position - _earthCollider.transform.position).normalized;
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
