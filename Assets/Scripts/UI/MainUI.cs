using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private List<ABaseUiWindow> _windows;

    private ABaseUiWindow _currentWindow;

    public ABaseUiWindow CurrentWindow => _currentWindow;

    public T GetWindow<T>() where T : ABaseUiWindow
    {
        return (T)_windows.Find(x => x is T);
    }
    public T ShowWindow<T>() where T: ABaseUiWindow
    {
        _currentWindow?.Hide();
        _currentWindow = _windows.Find(x => x is T);
        _currentWindow?.Show();
        return (T)_currentWindow;
    }

}
