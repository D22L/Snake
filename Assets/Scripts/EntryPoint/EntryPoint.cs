using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SnakeView _snakeView;
    [SerializeField] private SnakeSettings _snakeSettings;
    [SerializeField] private FoodSpawnSettings _foodSpawnSettings;
    [SerializeField] private MeshCollider _earthCollider;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private MeshFilter _ground;

    [Space]
    [SerializeField] private SnakeView _aiViewPfb;
    [SerializeField] private LevelConfig _levelConfig;

    [Inject] private IJoystick _inputSystem;

    private SnakeManager _snakeManager;
    private FoodSpawnSystem _foodSpawnSystem;    


    private async void Start()
    {
        
        _foodSpawnSystem = new FoodSpawnSystem(_earthCollider, _foodSpawnSettings);
        
        _foodSpawnSystem.SpawnFoodInRandomPosition();

        await _inputSystem.Init();

        _snakeManager = new SnakeManager(_aiViewPfb, _levelConfig, _snakeView, _snakeSettings, _inputSystem, _foodSpawnSystem);
        
        var mainSnake = _snakeManager.InitMainSnake();
        
        _snakeManager.SpawnAI(_ground).Forget();

       // _cameraController.Init(mainSnake);
    }




}
