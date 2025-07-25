using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameWIndow : ABaseUiWindow
{
    [SerializeField] private TextMeshProUGUI _score;

    public void SetScore(int score)
    {
        _score.text = score.ToString();
    }

}
