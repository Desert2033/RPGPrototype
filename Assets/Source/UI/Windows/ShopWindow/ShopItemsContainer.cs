using Root.Services.PersistentProgress;
using Source.Root.AssetManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShopItemsContainer : MonoBehaviour
{
    private const string ShopItemPath = "ShopItem";

    [SerializeField] private GameObject[] _shopUnavailableObjects;
    [SerializeField] private Transform _parent;

    private IIAPService _iapService;
    private IPersistentProgressService _progressService;
    private IAssets _assets;
    private List<GameObject> _shopItems = new List<GameObject>();

    public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssets assets)
    {
        _iapService = iapService;
        _progressService = progressService;
        _assets = assets;
    }

    public void Init() => 
        RefreshAvailableItems();

    private async void RefreshAvailableItems()
    {
        UpdateUnavailableObjects();

        if (!_iapService.IsInitialized)
            return;

        ClearShopItems();

        await FillShopItems();
    }

    private void ClearShopItems()
    {
        foreach (GameObject shopItem in _shopItems)
        {
            Destroy(shopItem);
        }
    }

    private async Task FillShopItems()
    {
        foreach (ProductDescription productDescription in _iapService.Products())
        {
            GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, _parent);
            ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

            shopItem.Construct(_iapService, _assets, productDescription);

            shopItem.Init();

            _shopItems.Add(shopItemObject);
        }
    }

    private void UpdateUnavailableObjects()
    {
        foreach (GameObject shopObject in _shopUnavailableObjects)
        {
            shopObject.SetActive(!_iapService.IsInitialized);
        }
    }

    public void Subscribe()
    {
        _iapService.Initialized += RefreshAvailableItems;
        _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
    }

    public void Cleanup()
    {
        _iapService.Initialized -= RefreshAvailableItems;
        _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
    }
}
