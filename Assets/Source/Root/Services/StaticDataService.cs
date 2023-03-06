using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonsterStaticDataPath = "StaticData/Monsters";
    private const string LevelStaticDataPath = "StaticData/Levels";
    private const string WindowStaticDataPath = "StaticData/UI/WindowStaticData";

    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;

    public void LoadMonsters()
    {
        _monsters = Resources
            .LoadAll<MonsterStaticData>(MonsterStaticDataPath)
            .ToDictionary(x => x.MonsterTypeId, x => x);

        _levels = Resources
            .LoadAll<LevelStaticData>(LevelStaticDataPath)
            .ToDictionary(x => x.LevelKey, x => x);
        
        _windowConfigs = Resources
            .Load<WindowStaticData>(WindowStaticDataPath)
            .Configs
            .ToDictionary(x => x.WindowId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
         _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;

    public LevelStaticData ForLevel(string sceneKey) => 
        _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

    public WindowConfig ForWindow(WindowId windowId) =>
        _windowConfigs.TryGetValue(windowId, out WindowConfig staticData) ? staticData : null;
}
