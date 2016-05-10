using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Component
{
    public class AnimationComponent : IComponent
    {
        float MeshRotation { get; set; }
    }
}
