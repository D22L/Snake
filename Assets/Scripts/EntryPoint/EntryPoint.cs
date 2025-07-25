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
    private MainSnake _mainSnake;
    private MainUI _mainUI;
    private InGameWIndow _inGame;

    private async void Start()
    {
        
        _foodSpawnSystem = new FoodSpawnSystem(_earthCollider, _foodSpawnSettings);
        
        _foodSpawnSystem.SpawnFoodInRandomPosition();

        await _inputSystem.Init();

        _snakeManager = new SnakeManager(_aiViewPfb, _levelConfig, _snakeView, _snakeSettings, _inputSystem, _foodSpawnSystem);

        _mainSnake = _snakeManager.InitMainSnake();
        
        _snakeManager.SpawnAI(_ground).Forget();

        _snakeManager.onEndGame += SnakeManager_onEndGame;

        _mainUI = FindObjectOfType<MainUI>();
        if (_mainUI != null)
        {
            _inGame = _mainUI.GetWindow<InGameWIndow>();
        }
        // _cameraController.Init(mainSnake);
    }

    private void Update()
    {
        if(_inGame != null) _inGame.SetScore(_mainSnake.TailParts.Count);
    }

    private void SnakeManager_onEndGame(int arg0)
    {

        
        if (_mainUI != null)
        {
            var winWindow = _mainUI.ShowWindow<InWinWindow>();
            ShowDetail(winWindow, arg0, _mainSnake.TailParts.Count,0.54f).Forget();
        }
    }
    private async UniTaskVoid ShowDetail(InWinWindow winWindow, int starsCount, int score, float progress)
    {
        winWindow.SetTarget(null); // TODO
        await winWindow.ShowStars(starsCount);
        winWindow.StartScoring(score, progress);
    }

    private void OnDestroy()
    {
        _snakeManager.onEndGame -= SnakeManager_onEndGame;
    }
}
