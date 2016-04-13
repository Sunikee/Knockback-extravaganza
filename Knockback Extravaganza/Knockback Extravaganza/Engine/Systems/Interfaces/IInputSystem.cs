using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems.Interfaces {
    public interface IInputSystem : ISystem{
        void Update(GameTime gameTime, ComponentManager componentManager);
    }
}
