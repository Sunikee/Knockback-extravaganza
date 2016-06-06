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
    public class RenderSystem : IRenderSystem {

        ModelRenderSystem modelRender = new ModelRenderSystem();
        SpriteRender spriteRender = new SpriteRender();
        void IRenderSystem.Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager) {
            Scene currScene = sceneManager.GetCurrentScene();

            if (currScene.Name == "singlePlayerScene")
                modelRender.RenderModels(gameTime, graphicsDevice, componentManager, sceneManager);
            else
                spriteRender.RenderSprites(gameTime, graphicsDevice, componentManager, sceneManager);
        }
    }
}

