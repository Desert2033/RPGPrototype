using Source.Data;
using Source.Root.Services;

namespace Root.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}