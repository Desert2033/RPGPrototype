using Source.Root.Services;
using System.Threading.Tasks;

public interface IUIFactory : IService
{
    public void CreateShop();
    public Task CreateUIRoot();
}
