using Root.Services.PersistentProgress;
using Source.Data;
using Source.Root;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISavedProgress
{
    private IGameFactory _gameFactory;
    private EnemyDeath _enemyDeath;
    private bool _slain;

    public MonsterTypeId MonsterTypeId;

    public string Id { get; set; }

    public void Construct(IGameFactory gameFactory) => 
        _gameFactory = gameFactory;

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.KillData.ClearedSpawners.Contains(Id))
            _slain = true;
        else
            Spawn();
    }

    private async void Spawn()
    {
        var monster = await _gameFactory.CreateMonster(MonsterTypeId, transform);
        _enemyDeath = monster.GetComponent<EnemyDeath>();
        _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
        if(_enemyDeath != null)
            _enemyDeath.Happened -= Slay;
        
        _slain = true;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        if (_slain)
            progress.KillData.ClearedSpawners.Add(Id);
    }
}
