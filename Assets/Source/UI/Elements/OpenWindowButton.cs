using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindowButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private IWindowService _windowService;

    public WindowId WindowId;

    public void Construct(IWindowService windowService) => 
        _windowService = windowService;

    private void Awake() => 
        _button.onClick.AddListener(Open);

    private void Open() => 
        _windowService.Open(WindowId);

    internal void Construct(object windowService)
    {
        throw new NotImplementedException();
    }
}
