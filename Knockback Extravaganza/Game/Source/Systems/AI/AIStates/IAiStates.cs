using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;

namespace Game.Source.Systems.AI.AIStates
{
    public interface IAiStates
    {
        void Run(TransformComponent playerTransC, TransformComponent aiTransC, MovementComponent moveC);
    }
}