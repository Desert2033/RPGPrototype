using UnityEngine;
using Source.Logic;
using Source.Root;
using Root.Services.PersistentProgress;
using UnityEngine.SceneManagement;

public class LoadLevelState : IPayloadedState<string>
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine stateMachine,
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

    private void OnLoaded()
    {
        InitUIRoot();
        InitGameWorld();
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

    private void InitUIRoot() => 
        _uiFactory.CreateUIRoot();

    private void InitGameWorld()
    {
        LevelStaticData levelData = InitLevelStaticData();

        GameObject hero = InitHero(levelData.InitialHeroPosition);

        InitSpawners(levelData);
        InitHud(hero);
        CameraFollow(hero);
    }

    private LevelStaticData InitLevelStaticData() => 
        _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private GameObject InitHero(Vector3 at) =>
        _gameFactory.CreateHero(at: at);

    private void InitSpawners(LevelStaticData levelData)
    {
        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        {
            _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
        }
    }

    private void InitHud(GameObject hero)
    {
        GameObject hud = _gameFactory.CreateHud();

        hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero) => 
        Camera.main.GetComponent<CameraFollow>().Follow(hero);

    public void Enter(string sceneName)
    {
        _curtain.Show();

        _gameFactory.Cleanup();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
        _curtain.Hide();
}