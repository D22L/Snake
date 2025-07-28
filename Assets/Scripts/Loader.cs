using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private MainUI _mainUI;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private List<LevelConfig> _levels;

    public bool IsInited { get; private set; }
    
    private readonly int _maxCountLevelScene = 5;
    private void Awake()
    {
        var loaders = FindObjectsOfType<Loader>();
        if (loaders.Length > 1 && !this.IsInited)
        {
            Destroy(this.gameObject);
            return;
        }

        _saveSystem.Load();

   
        DontDestroyOnLoad(this.gameObject);

        IsInited = true;
        _mainUI.ShowWindow<MainMenuUI>();
    }

    private void OnEnable()
    {
        var menuUI =_mainUI.ShowWindow<MainMenuUI>();
        menuUI.PlayButton.onClick.AddListener(Play);
        menuUI.StarText.text = _saveSystem.saveData.CountStars.ToString();
    }

    private void Play()
    {
        int levelIndex = _saveSystem.saveData.OpenedLevel % _levels.Count;        
        var sceneName = _levels[levelIndex].SceneName;
        LoadNextLevel(sceneName);
        _mainUI.ShowWindow<InGameWIndow>();
        
    }

    public bool isMaxLevel()
    {
        return _saveSystem.saveData.OpenedLevel >= _levels.Count;
    }
    public void LevelUp()
    {
        _saveSystem.saveData.OpenedLevel++;
        _saveSystem.Save();
    }

    public void LoadNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
