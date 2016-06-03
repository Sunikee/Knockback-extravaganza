using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Component
{
    public class PhysicsComponent : IComponent 
    {
        public float Gravity = 9.82f;
        public Vector3 Velocity { get; set; } = Vector3.Zero;
        public float GravityStrength { get; set; }
        public float Friction { get; set; }
        public float Mass { get; set; }
        public float ElapsedTime { get; set; }
        public bool InAir { get; set; }
       
    }
}
