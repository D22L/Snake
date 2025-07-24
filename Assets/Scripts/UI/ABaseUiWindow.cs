using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseUiWindow : MonoBehaviour
{
    [field: SerializeField] public eUIWindowType windowType { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
