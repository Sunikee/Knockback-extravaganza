using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using Game.Source.Components.AI;

namespace Game.Source.Systems.AI.AIStates
{
    public class AIStop : IAiStates
    {
        public void Run(TransformComponent playerTransC, TransformComponent aiTransC, MovementComponent moveC)
        {
            moveC.Velocity = moveC.Velocity * 0.1f;
        }
    }
}