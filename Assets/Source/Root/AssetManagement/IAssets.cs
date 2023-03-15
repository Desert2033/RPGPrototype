using Source.Root.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Root.AssetManagement
{
    public interface IAssets : IService
    {
        public void CleanUpHandles();
        public void Init();
        
        public Task<GameObject> Instantiate(string path);
        public Task<GameObject> Instantiate(string path, Vector3 at);
        public Task<GameObject> Instantiate(string address, Transform parent);
        
        public Task<T> Load<T>(AssetReferenceGameObject prefabReference) where T : class;
        public Task<T> Load<T>(string address) where T : class;
    }
}