using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private List<ABaseUiWindow> _windows;

    private ABaseUiWindow _currentWindow;
    public T ShowWindow<T>() where T: ABaseUiWindow
    {
        _currentWindow?.Hide();
        _currentWindow = _windows.Find(x => x is T);
        return (T)_currentWindow;
    }

}
