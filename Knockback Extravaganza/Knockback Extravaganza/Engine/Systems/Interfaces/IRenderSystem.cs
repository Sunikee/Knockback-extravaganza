﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Systems.Interfaces {
    public interface IRenderSystem : ISystem{
        void Render(GameTime gameTime, GraphicsDeviceManager graphicsDevice);
    }
}
