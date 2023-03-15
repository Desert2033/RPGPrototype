using Assets.Source.Services;
using Root.Services.PersistentProgress;
using Source.Root;
using Source.Root.AssetManagement;
using Source.Root.Services;
using UnityEngine;

public class BootstrapState : IState
{
    private const string Initial = "Initial";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    private AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
        _stateMachine = stateMachine;

        _sceneLoader = sceneLoader;

        _services = services;
        
        RegisterServices();
    }

    private void EnterLoadLevel() => 
        _stateMachine.Enter<LoadProgressState>();

    private void RegisterServices()
    {
        RegisterAdsService();
        RegisterStaticData();
        RegisterAssetProvider();

        _services.RegisterSingle<IGameStateMachine>(_stateMachine);
        _services.RegisterSingle<IRandomService>(new RandomService());
        _services.RegisterSingle<IInputService>(InputService());
        _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

        RegisterIAPService(new IAPProvider(), _services.Single<IPersistentProgressService>());

        _services.RegisterSingle<IUIFactory>(new UIFactory(
            _services.Single<IAssets>(),
            _services.Single<IStaticDataService>(),
            _services.Single<IPersistentProgressService>(),
            _services.Single<IAdsService>(),
            _services.Single<IIAPService>()
            ));

        _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

        _services.RegisterSingle<IGameFactory>(new GameFactory(
            _services.Single<IAssets>(),
            _services.Single<IStaticDataService>(),
            _services.Single<IRandomService>(),
            _services.Single<IPersistentProgressService>(),
            _services.Single<IWindowService>()));

        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
    }

    private void RegisterAssetProvider()
    {
        AssetProvider assetProvider = new AssetProvider();

        assetProvider.Init();

        _services.RegisterSingle<IAssets>(assetProvider);
    }

    private void RegisterAdsService()
    {
        IAdsService adsSevice = new AdsService();
        adsSevice.Init();
        _services.RegisterSingle<IAdsService>(adsSevice);
    }

    private void RegisterIAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
    {
        IIAPService iapSevice = new IAPService(iapProvider, progressService);
        iapSevice.Init();
        _services.RegisterSingle<IIAPService>(iapSevice);
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.LoadMonsters();
        _services.RegisterSingle<IStaticDataService>(staticData);
    }

    private IInputService InputService()
    {
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }

    public void Enter() => 
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

    public void Exit()
    {
    }
}
