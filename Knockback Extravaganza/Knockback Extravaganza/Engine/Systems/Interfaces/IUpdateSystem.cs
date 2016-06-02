using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Managers;

namespace ECS_Engine.Engine.Systems.Interfaces {
    public interface IUpdateSystem : ISystem{
        void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager);
    }
}
