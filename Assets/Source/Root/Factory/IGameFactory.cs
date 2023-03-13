using Root.Services.PersistentProgress;
using Source.Root.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Source.Root
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }
        public void Cleanup();
        public Task<GameObject> CreateHud();
        public Task<GameObject> CreateHero(Vector3 at);
        public Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        public Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        public Task WarmUp();
        public Task<LootPiece> CreateLoot();
    }
}