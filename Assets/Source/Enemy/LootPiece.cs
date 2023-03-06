using Source.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LootPiece : MonoBehaviour
{
    [SerializeField] private GameObject _skull;
    [SerializeField] private GameObject _pickupFxPrefab;
    [SerializeField] private GameObject PickupPopup; 
    [SerializeField] private TextMeshPro _lootText; 

    private Loot _loot;
    private WorldData _worldData;
    private bool _picked;

    public void Construct(WorldData worldData)
    {
        _worldData = worldData;
    }

    public void Init(Loot loot)
    {
        _loot = loot;
    }

    private void OnTriggerEnter(Collider other) => 
        Pickup();

    private void Pickup()
    {
        if (_picked)
            return;

        _picked = true;

        UpdateWorldData();
        HideSkull();
        PlayPickupFx();
        ShowText();

        StartCoroutine(StartDestroyTimer());
    }

    private void UpdateWorldData() => 
        _worldData.LootData.Collect(_loot);

    private void HideSkull() => 
        _skull.SetActive(false);

    private IEnumerator StartDestroyTimer()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    private void PlayPickupFx() => 
        Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);

    private void ShowText()
    {
        _lootText.text = $"{_loot.Value}";
        PickupPopup.SetActive(true);
    }
}
