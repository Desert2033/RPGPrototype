using Source.Root.Services;

public interface IAdsService : IService 
{
    public void Init();

    public bool IsInitialized { get; }
    public string RewardedId { get; }
}