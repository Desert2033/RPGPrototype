using Root.Services.PersistentProgress;
using Source.Root.AssetManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Root
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _random;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject _heroGameObject;

        public GameFactory(
            IAssets assetProvider,
            IStaticDataService staticData,
            IRandomService random,
            IPersistentProgressService progressService, 
            IWindowService windowService)
        {
            _assetProvider = assetProvider;

            _staticData = staticData;

            _random = random;

            _progressService = progressService;

            _windowService = windowService;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
        
        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
       
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public GameObject CreateHero(GameObject at)
        {
            _heroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

            return _heroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);

            hud.GetComponentInChildren<LootCounter>().
                Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindow in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindow.Construct(_windowService);
            }

            return hud;
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = GameObject.Instantiate(
                    monsterData.Prefab, 
                    parent.position, 
                    Quaternion.identity, 
                    parent);
            
            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);

            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            monster.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform);
            monster.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
            lootSpawner.Construct(this, _random);

            var attack = monster.GetComponent<Attack>();
            attack.Construct(_heroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            return monster;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }
        
        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this);

            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }
    }
}