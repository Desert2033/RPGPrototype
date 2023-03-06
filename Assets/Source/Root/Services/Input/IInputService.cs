using Source.Root.Services;
using UnityEngine;

namespace Assets.Source.Services
{
    public interface IInputService : IService
    {
        public Vector2 Axis { get; }

        public bool IsAttackButtonUp();
    }
}