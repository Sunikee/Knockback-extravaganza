using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using Game.Source.Components.AI;
using Microsoft.Xna.Framework;

namespace Game.Source.Systems.AI.AIStates
{
    public class AIStop : IAiStates
    {
        public void Run(TransformComponent playerTransC, TransformComponent aiTransC, MovementComponent moveC)
        {
            moveC.Velocity = moveC.Velocity = Vector3.Zero;
        }
    }
}