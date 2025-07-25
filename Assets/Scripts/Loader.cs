using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private MainUI _mainUI;

    public bool IsInited { get; private set; }
    private SaveSystem _saveSystem;
    private readonly int _maxCountLevelScene = 5;
    private void Awake()
    {
        var loaders = FindObjectsOfType<Loader>();
        if (loaders.Length > 1 && !this.IsInited)
        {
            Destroy(this.gameObject);
            return;
        }

        _saveSystem = new SaveSystem();

        //LoadNextLevel();
        DontDestroyOnLoad(this.gameObject);

        IsInited = true;
        _mainUI.ShowWindow<MainMenuUI>();
    }
    private void OnEnable()
    {
        var menuUI =_mainUI.ShowWindow<MainMenuUI>();
        menuUI.PlayButton.onClick.AddListener(Play);
    }

    private void Play()
    {
        LoadNextLevel();
        _mainUI.ShowWindow<InGameWIndow>();
    }



    public void LevelUp()
    {
        _saveSystem.saveData.Level++;
        _saveSystem.Save();
    }

    public void LoadNextLevel()
    {        
           var level = _saveSystem.saveData.Level;
        var sceneIndex = level % (_maxCountLevelScene+1);
        sceneIndex = Mathf.Clamp(sceneIndex,1,_maxCountLevelScene);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
