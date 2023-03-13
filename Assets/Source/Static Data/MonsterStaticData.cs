using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
public class MonsterStaticData : ScriptableObject
{
    public MonsterTypeId MonsterTypeId;

    public int MinLoot;
    public int MaxLoot;

    [Range(1, 100)]
    public int Hp;
    
    [Range(1f, 30f)]
    public float Damage;

    [Range(0.5f, 1f)]
    public float EffectiveDistance = 0.666f;
    
    [Range(0.5f, 1f)]
    public float Cleavage;

    [Range(1f, 100f)]
    public float MoveSpeed;
    
    public AssetReferenceGameObject PrefabReference;
}
