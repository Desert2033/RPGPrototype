using Source.Data;
using Source.Root.Services;

public interface ISaveLoadService : IService 
{
    public void SaveProgress();

    public PlayerProgress LoadProgress();
}
