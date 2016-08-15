using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using Microsoft.Xna.Framework;

namespace Game.Source.Systems.AI.AIStates
{
    public class AICharge : IAiStates
    {
        public void Run(TransformComponent playerTransC, TransformComponent aiTransC, MovementComponent moveC)
        {
            var diff = playerTransC.Position - aiTransC.Position;
            diff.Normalize();
            moveC.Velocity += diff * new Vector3(2, 0, 2);
        }
    }
}