using Root.Services.PersistentProgress;
using Source.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class WindowBase : MonoBehaviour
{
    [SerializeField] protected Button _closeButton;
    
    protected IPersistentProgressService _progressService;
    protected PlayerProgress Progress => _progressService.Progress;

    public void Construct(IPersistentProgressService progressService) => 
        _progressService = progressService;

    private void Awake() => 
        OnAwake();

    private void Start()
    {
        Init();
        SubscribeUpdates();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    internal void Construct(object progressService)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnAwake() => 
        _closeButton.onClick.AddListener(() => Destroy(gameObject));

    protected virtual void Init()
    {

    }

    protected virtual void SubscribeUpdates()
    {

    }

    protected virtual void Cleanup()
    {

    }
}
