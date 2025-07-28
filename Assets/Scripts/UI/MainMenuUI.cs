using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : ABaseUiWindow
{
    [field:SerializeField] public Button PlayButton { get; private set; }
    [field: SerializeField] public TextMeshProUGUI StarText { get; private set; }
}
