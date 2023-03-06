using Source.Root.Services;
using UnityEngine;

namespace Source.Root.AssetManagement
{
    public interface IAssets : IService
    {
        public GameObject Instantiate(string path);

        public GameObject Instantiate(string path, Vector3 at);
    }
}