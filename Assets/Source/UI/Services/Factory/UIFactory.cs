﻿using Root.Services.PersistentProgress;
using Source.Root.AssetManagement;
using System.Threading.Tasks;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private const string UIRootPath = "UIRoot";

    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IAdsService _adsService;
    private readonly IIAPService _iapService;

    private Transform _uiRoot;

    public UIFactory(IAssets assets,
        IStaticDataService staticData,
        IPersistentProgressService progressService,
        IAdsService adsService, 
        IIAPService iapService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _adsService = adsService;
        _iapService = iapService;
    }

    public void CreateShop()
    {
        WindowConfig config = _staticData.ForWindow(WindowId.Shop);

        ShopWindow shopWindow = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;

        shopWindow.Construct(_adsService , _progressService, _iapService, _assets);
    }

    public async Task CreateUIRoot()
    {
        GameObject root = await _assets.Instantiate(UIRootPath);
        _uiRoot = root.transform;
    }
}