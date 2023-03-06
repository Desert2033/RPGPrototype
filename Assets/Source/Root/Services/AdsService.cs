using UnityEngine.Advertisements;
using UnityEngine;

public class AdsService : IUnityAdsInitializationListener, IAdsService
{
    private const string AndroidGameId = "5186748";
    private const string IOSGameId = "5186749";
    private const string RewardedAndroid = "Rewarded_Android";
    private const string RewardedIOS = "Rewarded_iOS";

    private bool _testMode = true;
    private string _gameId;
    private string _rewardedId;

    public bool IsInitialized => Advertisement.isInitialized;
    public string RewardedId => _rewardedId;


    public void Init()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                _gameId = AndroidGameId;

                _rewardedId = RewardedAndroid;
                break;
            case RuntimePlatform.IPhonePlayer:
                _gameId = IOSGameId;

                _rewardedId = RewardedIOS;
                break;
            case RuntimePlatform.WindowsEditor:
                _gameId = IOSGameId;

                _rewardedId = RewardedIOS;
                break;
            default:
                Debug.LogError("Unsupported platform for ads");
                break;
        }

        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete() =>
        Debug.Log("Ads complete");

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
        Debug.LogError($"Initialization Failed: {message}");
}