using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SnakeView _snakeView;
    [SerializeField] private FoodSpawnSettings _foodSpawnSettings;
    [SerializeField] private MeshCollider _earthCollider;
    [SerializeField] private CameraController _cameraController;
    
    [Inject] private IJoystick _inputSystem;

    private FoodSpawnSystem _foodSpawnSystem;

    private async void Start()
    {
        _foodSpawnSystem = new FoodSpawnSystem(_earthCollider, _foodSpawnSettings);
        _foodSpawnSystem.SpawnFoodInRandomPosition();

        await _inputSystem.Init();

        MainSnake mainSnake = new MainSnake(_snakeView, _inputSystem);
        
        _cameraController.Init(mainSnake);
    }



}
