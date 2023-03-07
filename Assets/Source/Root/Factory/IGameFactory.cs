using Root.Services.PersistentProgress;
using Source.Root.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Root
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }
        public void Cleanup();
        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        public GameObject CreateHud();
        public GameObject CreateHero(Vector3 at);
        public GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        public LootPiece CreateLoot();
    }
}