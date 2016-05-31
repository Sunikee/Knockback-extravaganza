using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Managers {
    public class SceneManager {

        Dictionary<Type, Dictionary<Entity, IComponent>> componentList = new Dictionary<Type, Dictionary<Entity, IComponent>>();
        Dictionary<Type, Dictionary<Entity, IComponent>> activeComponents = new Dictionary<Type, Dictionary<Entity, IComponent>>();
        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();
        List<IRenderSystem> systemsToRender = new List<IRenderSystem>();
        List<IUpdateSystem> systemsToUpdate = new List<IUpdateSystem>();
    }
}
