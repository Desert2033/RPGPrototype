using Source.Root.Services;

public interface IStaticDataService : IService
{
    public MonsterStaticData ForMonster(MonsterTypeId typeId);
    public void LoadMonsters();
    public LevelStaticData ForLevel(string sceneKey);
    public WindowConfig ForWindow(WindowId windowId);
}
