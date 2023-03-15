using UnityEngine;
using UnityEngine.Purchasing;
using Source.Data;
using System.Collections.Generic;
using System;
using System.Linq;

public class IAPProvider : IStoreListener
{
    private const string IAPConfigsPath = "IAP/products";

    private IAPService _iapService;
    private IStoreController _controller;
    private IExtensionProvider _extensions;

    public bool IsInitialized => _controller != null && _extensions != null;

    public Dictionary<string, ProductConfig> Configs { get; private set; }
    public Dictionary<string, Product> Products { get; private set; }

    public event Action Initialized;

    public void Init(IAPService iapService)
    {
        _iapService = iapService;

        Configs = new Dictionary<string, ProductConfig>();
        Products = new Dictionary<string, Product>();

        Load();

        ConfigurationBuilder builder = ConfigurationBuilder
            .Instance(StandardPurchasingModule.Instance());

        foreach (ProductConfig productConfig in Configs.Values)
        {
            builder.AddProduct(productConfig.Id, productConfig.ProductType);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private void Load()
    {
        Configs =
            Resources
            .Load<TextAsset>(IAPConfigsPath)
            .text
            .ToDeserialized<ProductConfigWrapper>()
            .Configs
            .ToDictionary(x => x.Id, x => x);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _controller = controller;
        _extensions = extensions;

        foreach (Product product in _controller.products.all)
        {
            Products.Add(product.definition.id, product);
        }

        Initialized?.Invoke();

        Debug.Log("UnityPurchasing initialization success");
    }

    public void OnInitializeFailed(InitializationFailureReason error) =>
        throw new Exception($"UnityPurchasing initialization failed: {error}");

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        throw new Exception($"Product{product.definition.id} purchase failed: {failureReason} transaction id {product.transactionID}");

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"UnityPurchasing ProcessPurchase success product id: {purchaseEvent.purchasedProduct.definition.id}");

        return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
    }

    public void StartPurchase(string productId) =>
        _controller.InitiatePurchase(productId);
}
