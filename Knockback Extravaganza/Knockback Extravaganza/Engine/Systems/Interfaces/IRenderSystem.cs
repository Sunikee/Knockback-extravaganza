using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems.Interfaces {
    interface IRenderSystem {
        void Render(GraphicsDevice graphicsDevice);
    }
}
