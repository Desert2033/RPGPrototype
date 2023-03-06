using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;
using Root.Services.PersistentProgress;

public class RewardAdItem : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button _showAdButton;
    [SerializeField] private GameObject[] _adActiveObjects;
    [SerializeField] private GameObject[] _adInactiveObjects;

    private IAdsService _adsService;
    private IPersistentProgressService _progressService;
    private int _loot = 10;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
        _adsService = adsService;
        _progressService = progressService;
    }

    public void Init()
    {
        LoadAd();

        _showAdButton.onClick.AddListener(ShowAd);
    }

    private void LoadAd() => 
        Advertisement.Load(_adsService.RewardedId, this);

    private void ShowAd() => 
        Advertisement.Show(_adsService.RewardedId, this);

    private void SetAdInActiveObjects(bool active)
    {
        foreach (GameObject adInactiveObject in _adInactiveObjects)
            adInactiveObject.SetActive(active);
    }

    private void SetAdActiveObjects(bool active)
    {
        foreach (GameObject adActiveObject in _adActiveObjects)
            adActiveObject.SetActive(active);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        SetAdActiveObjects(true);

        SetAdInActiveObjects(false);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Load error: {message}");
        
        SetAdActiveObjects(false);

        SetAdInActiveObjects(true);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        _progressService.Progress.WorldData.LootData.Add(_loot);

        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show error: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

}
