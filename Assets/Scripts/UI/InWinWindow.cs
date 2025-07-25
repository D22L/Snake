using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InWinWindow : ABaseUiWindow
{
    [SerializeField] private List<GameObject> _stars;
    [SerializeField] private TextMeshProUGUI _foodCounter;
    [SerializeField] private Image _targetNextLevel;
    [SerializeField] private Image _targetNextLevelProgress;
    [SerializeField] private Button _loadMenuButton;

    private void OnEnable()
    {
        _stars.ForEach(x=>x.SetActive(false));
        _loadMenuButton.onClick.AddListener(LoadMenu);
    }

    private void OnDisable()
    {
        _loadMenuButton.onClick.RemoveListener(LoadMenu);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void SetTarget(Sprite sprite) => _targetNextLevel.sprite = sprite;

    public async UniTask ShowStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _stars[i].transform.localScale = Vector3.zero;
            _stars[i].SetActive(true);
            await _stars[i].transform.DOScale(Vector3.one,1f).AsyncWaitForCompletion();
        }
    }

    public void StartScoring(int score, float targetProgress)
    {
        _foodCounter.text = score.ToString();
        _targetNextLevelProgress.DOFillAmount(targetProgress,2f);
        DOTween.To(() => score, x => score = x, 0, 2f)
                .OnUpdate(() =>
                {
                    _foodCounter.text = score.ToString();
                });
        
    }
}
