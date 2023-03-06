using Root.Services.PersistentProgress;
using TMPro;
using UnityEngine;

public class ShopWindow : WindowBase
{
    [SerializeField] private TextMeshProUGUI _skullText;
    [SerializeField] private RewardAdItem _rewardAdItem;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
        base.Construct(progressService);

        _rewardAdItem.Construct(adsService, progressService);
    }

    protected override void Init()
    {
        _rewardAdItem.Init();

        RefreshText();
    }

    protected override void SubscribeUpdates() => 
        Progress.WorldData.LootData.Changed += RefreshText;

    protected override void Cleanup()
    {
        base.Cleanup();
        Progress.WorldData.LootData.Changed -= RefreshText;
    }

    private void RefreshText() => 
        _skullText.text = Progress.WorldData.LootData.Collected.ToString();
}