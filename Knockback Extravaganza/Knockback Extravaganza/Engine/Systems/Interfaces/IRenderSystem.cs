using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems.Interfaces {
    /// <summary>
    /// Interface for the Render systems
    /// </summary>
    public interface IRenderSystem : ISystem{
        void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManagers);

    }
}
