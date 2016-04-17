﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine
{
    class MovementComponent
    {
        public Vector3 Velocity {get; set;}
        public float Gravity { get; set; }
        public float Friction { get; set; }

    }
}
