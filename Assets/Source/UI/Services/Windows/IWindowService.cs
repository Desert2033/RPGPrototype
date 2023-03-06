using Source.Root.Services;

public interface IWindowService : IService
{
    public void Open(WindowId windowId);
}