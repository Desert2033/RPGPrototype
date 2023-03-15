using Root.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Source.Root.AssetManagement;

public class ShopWindow : WindowBase
{
    [SerializeField] private TextMeshProUGUI _skullText;
    [SerializeField] private RewardAdItem _rewardAdItem;
    [SerializeField] private ShopItemsContainer _shopItemsContainer;

    public void Construct(IAdsService adsService, 
        IPersistentProgressService progressService, 
        IIAPService iapService,
        IAssets assets)
    {
        base.Construct(progressService);

        _rewardAdItem.Construct(adsService, progressService);
        _shopItemsContainer.Construct(iapService, progressService, assets);
    }

    protected override void Init()
    {
        _rewardAdItem.Init();

        _shopItemsContainer.Init();

        RefreshText();
    }

    protected override void SubscribeUpdates()
    {
        Progress.WorldData.LootData.Changed += RefreshText;

        _shopItemsContainer.Subscribe();
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        _shopItemsContainer.Cleanup();

        Progress.WorldData.LootData.Changed -= RefreshText;
    }

    private void RefreshText() => 
        _skullText.text = Progress.WorldData.LootData.Collected.ToString();
}
