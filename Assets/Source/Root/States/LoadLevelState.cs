using UnityEngine;
using Source.Logic;
using Source.Root;
using Root.Services.PersistentProgress;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadLevelState : IPayloadedState<string>
{
    private readonly IGameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(IGameStateMachine stateMachine,
        SceneLoader sceneLoader,
        LoadingCurtain curtain,
        IGameFactory gameFactory,
        IPersistentProgressService progressService,
        IStaticDataService staticData, 
        IUIFactory uiFactory)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _progressService = progressService;
        _staticData = staticData;
        _uiFactory = uiFactory;
    }

    private async void OnLoaded()
    {
        await InitUIRoot();
        await InitGameWorld();
        InformProgressReaders();

        _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        {
            progressReader.LoadProgress(_progressService.Progress);
        }
    }

    private async Task InitUIRoot() => 
        await _uiFactory.CreateUIRoot();

    private async Task InitGameWorld()
    {
        LevelStaticData levelData = InitLevelStaticData();

        GameObject hero = await InitHero(levelData.InitialHeroPosition);

        await InitSpawners(levelData);
        await InitHud(hero);

        CameraFollow(hero);
    }

    private LevelStaticData InitLevelStaticData() => 
        _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private async Task<GameObject> InitHero(Vector3 at) =>
        await _gameFactory.CreateHero(at: at);

    private async Task InitSpawners(LevelStaticData levelData)
    {
        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        {
            await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
        }
    }

    private async Task InitHud(GameObject hero)
    {
        GameObject hud = await _gameFactory.CreateHud();

        hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero) => 
        Camera.main.GetComponent<CameraFollow>().Follow(hero);

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _gameFactory.Cleanup();
        _gameFactory.WarmUp();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
        _curtain.Hide();
}