using UnityEngine.Purchasing;
using System;
using Root.Services.PersistentProgress;
using System.Collections.Generic;
using System.Linq;

public class IAPService : IIAPService
{
    private readonly IAPProvider _iapProvider;
    private readonly IPersistentProgressService _progressService;

    public bool IsInitialized => _iapProvider.IsInitialized;

    public event Action Initialized;

    public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
    {
        _iapProvider = iapProvider;
        _progressService = progressService;
    }

    public void Init()
    {
        _iapProvider.Init(this);
        _iapProvider.Initialized += () => Initialized?.Invoke();
    }

    private IEnumerable<ProductDescription> ProductDescriptions()
    {
        PurchaseData purchaseData = _progressService.Progress.PurchaseData;

        foreach (string productId in _iapProvider.Products.Keys)
        {
            ProductConfig config = _iapProvider.Configs[productId];
            Product product = _iapProvider.Products[productId];

            BoughtIAP boughtIAP = purchaseData.BoughtIAPs.Find(x => x.IdIAP == productId);

            if (IsProductBoughtOut(config, boughtIAP))
                continue;

            yield return new ProductDescription
            {
                Id = productId,
                Config = config,
                Product = product,
                AvailablePurchaseLeft = boughtIAP != null
                ? config.MaxPurchaseCount - boughtIAP.Count
                : config.MaxPurchaseCount,
            };
        }
    }

    private bool IsProductBoughtOut(ProductConfig config, BoughtIAP boughtIAP) =>
        boughtIAP != null && boughtIAP.Count >= config.MaxPurchaseCount;

    public List<ProductDescription> Products() =>
        ProductDescriptions().ToList();

    public void StartPurchase(string productId) =>
        _iapProvider.StartPurchase(productId);

    public PurchaseProcessingResult ProcessPurchase(Product purchaseProduct)
    {
        ProductConfig productConfig = _iapProvider.Configs[purchaseProduct.definition.id];

        switch (productConfig.ItemType)
        {
            case ItemType.Skulls:
                _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);
                _progressService.Progress.PurchaseData.AddPurchase(purchaseProduct.definition.id);
                break;
        }

        return PurchaseProcessingResult.Complete;
    }
}
