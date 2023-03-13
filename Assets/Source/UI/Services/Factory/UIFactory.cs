using Root.Services.PersistentProgress;
using Source.Root.AssetManagement;
using System.Threading.Tasks;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private const string UIRootPath = "UIRoot";

    private readonly IAssets _assets;

    private Transform _uiRoot;
    private IStaticDataService _staticData;
    private IPersistentProgressService _progressService;
    private IAdsService _adsService;

    public UIFactory(IAssets assets,
        IStaticDataService staticData,
        IPersistentProgressService progressService, 
        IAdsService adsService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _adsService = adsService;
    }

    public void CreateShop()
    {
        WindowConfig config = _staticData.ForWindow(WindowId.Shop);

        ShopWindow shopWindow = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;

        shopWindow.Construct(_adsService , _progressService);
    }

    public async Task CreateUIRoot()
    {
        GameObject root = await _assets.Instantiate(UIRootPath);
        _uiRoot = root.transform;
    }
}