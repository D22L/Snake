using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
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
    private SaveSystem _saveSystem;
    private Loader _loader;

    private async void Start()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        _loader = FindObjectOfType<Loader>();

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
    }

    private void Update()
    {
        if (_inGame != null)
        {
            _inGame.SetScore(_mainSnake.TailParts.Count);
        }
    }

    private void SnakeManager_onEndGame(int arg0)
    {
        _inputSystem.Disable();
        _mainSnake.Stop();

        if (_mainUI != null)
        {
            var winWindow = _mainUI.ShowWindow<InWinWindow>();
            winWindow.LoadMenuButton.onClick.AddListener(()=> {
                _loader.LoadMenu();
               var menu = _mainUI.ShowWindow<MainMenuUI>();
                menu.StarText.text = _saveSystem != null ? _saveSystem.saveData.CountStars.ToString() : "0";
            });

            float progress = 0f;
            float oldProgress = 0f;
            int savedFoodCount = 0;

            if (_saveSystem != null)
            {
                oldProgress = _saveSystem.saveData.LastProgress;
                progress = _saveSystem.saveData.LastProgress;
                savedFoodCount = _saveSystem.saveData.LeftoverFood;
                _saveSystem.saveData.LeftoverFood = 0;
            }

            
            progress += (float)(savedFoodCount + _mainSnake.TailParts.Count) / (float)_levelConfig.CountFoodForOpenNextLevel;
           
            int leftoverFood = 0;
            if (progress >= 1f)
            {
                progress = 1f;
                leftoverFood = (int)((progress - 1f) * _levelConfig.CountFoodForOpenNextLevel);
                

                if (_loader != null)
                {
                    if (!_loader.isMaxLevel())
                    {
                        _loader.LevelUp();
                    }
                }
            }

            if (_saveSystem != null)
            {
                _saveSystem.saveData.CountStars += arg0;
                _saveSystem.saveData.LeftoverFood += leftoverFood;
                if (progress >= 1f)
                {
                    _saveSystem.saveData.LastProgress = 0f;
                }
                else
                {
                    _saveSystem.saveData.LastProgress = progress;
                }

                _saveSystem.Save();
            }

            ShowDetail(winWindow, arg0, _mainSnake.TailParts.Count, progress, oldProgress).Forget();
        }
    }

    private async UniTaskVoid ShowDetail(InWinWindow winWindow, int starsCount, int score, float progress, float oldProgress)
    {
        winWindow.SetProgressAndScore(score, oldProgress);
        if (_loader != null)
        {
            if (_loader.isMaxLevel())
            {
                winWindow.SetTarget(null);
                await winWindow.ShowStars(starsCount);
            }
            else 
            {
                winWindow.SetTarget(_levelConfig.NextLevelIcon);
                await winWindow.ShowStars(starsCount);
                winWindow.StartScoring(score, progress);
            }
        }
        else
        {
            winWindow.SetTarget(_levelConfig.NextLevelIcon);
            await winWindow.ShowStars(starsCount);
            winWindow.StartScoring(score, progress);
        }
    }

    private void OnDestroy()
    {
        _snakeManager.onEndGame -= SnakeManager_onEndGame;
    }
}
