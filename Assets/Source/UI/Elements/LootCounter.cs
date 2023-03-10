using UnityEngine;
using TMPro;
using Source.Data;

public class LootCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter;

    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
        _worldData = worldData;

       _worldData.LootData.Changed += UpdateCounter;
        
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        _counter.text = $"{_worldData.LootData.Collected}";
    }
}
