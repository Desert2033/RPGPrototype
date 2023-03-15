using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Source.Root.AssetManagement;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Button _buyItemButton;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private TextMeshProUGUI _availableItemsText;
    [SerializeField] private Image _icon;

    private ProductDescription _productDescription;
    private IIAPService _iapService;
    private IAssets _assets;

    public void Construct(IIAPService iapService, IAssets assets, ProductDescription productDescription)
    {
        _productDescription = productDescription;
        _iapService = iapService;
        _assets = assets;
    }

    public async void Init()
    {
        _buyItemButton.onClick.AddListener(OnBuyItemClick);

        _priceText.text = _productDescription.Config.Price;
        _quantityText.text = _productDescription.Config.Quantity.ToString();
        _availableItemsText.text = _productDescription.AvailablePurchaseLeft.ToString();

        _icon.sprite = await _assets.Load<Sprite>(_productDescription.Config.Icon);
    }

    private void OnBuyItemClick() => 
        _iapService.StartPurchase(_productDescription.Id);
}