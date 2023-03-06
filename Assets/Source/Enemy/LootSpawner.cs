using Source.Root;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private EnemyDeath _enemyDeath;
    private IGameFactory _gameFactory;
    private IRandomService _random;
    private int _lootMin;
    private int _lootMax;

    public void Construct(IGameFactory gameFactory, IRandomService random)
    {
        _gameFactory = gameFactory;

        _random = random;
    }

    private void Start()
    {
        _enemyDeath.Happened += SpawnLoot;
    }

    private void SpawnLoot()
    {
        LootPiece loot = _gameFactory.CreateLoot();
        loot.transform.position = transform.position;

        Loot lootItem = GenerateLoot();

        loot.Init(lootItem);
    }

    private Loot GenerateLoot()
    {
        return new Loot()
        {
            Value = _random.Next(_lootMin, _lootMax)
        };
    }

    public void SetLoot(int min, int max)
    {
        _lootMin = min;
        _lootMax = max;
    }
}
