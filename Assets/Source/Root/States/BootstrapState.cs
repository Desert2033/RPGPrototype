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

        _services.RegisterSingle<IGameStateMachine>(_stateMachine);
        _services.RegisterSingle<IRandomService>(new RandomService());
        _services.RegisterSingle<IInputService>(InputService());
        _services.RegisterSingle<IAssets>(new AssetProvider());
        _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

        _services.RegisterSingle<IUIFactory>(new UIFactory(
            _services.Single<IAssets>(),
            _services.Single<IStaticDataService>(),
            _services.Single<IPersistentProgressService>(),
            _services.Single<IAdsService>()));

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

    private void RegisterAdsService()
    {
        IAdsService adsSevice = new AdsService();
        adsSevice.Init();
        _services.RegisterSingle<IAdsService>(adsSevice);
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.LoadMonsters();
        _services.RegisterSingle<IStaticDataService>(staticData);
    }

    public void Enter() => 
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

    public void Exit()
    {
    }

    public static IInputService InputService()
    {
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }
}
