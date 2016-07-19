using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECS_Engine.Engine.Scenes;

namespace ECS_Engine.Engine.Systems {
    /// <summary>
    /// Acts a split for the different render systems.
    /// </summary>
    public class RenderSystem : IRenderSystem {

        ModelRenderSystem modelRender = new ModelRenderSystem();
        SpriteRenderSystem spriteRender = new SpriteRenderSystem();
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManager) {

            modelRender.Render(gameTime, graphicsDevice, componentManager, sceneManager);

            spriteRender.Render(gameTime, graphicsDevice, componentManager, sceneManager);
        }
    }
}

