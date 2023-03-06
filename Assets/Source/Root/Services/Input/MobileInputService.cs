using UnityEngine;

namespace Assets.Source.Services
{
    public class MobileInputService : InputService
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}