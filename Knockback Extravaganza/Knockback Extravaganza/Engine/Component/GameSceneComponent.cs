using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {

    public enum ScreenState {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public class GameSceneComponent : IComponent  {
        public bool IsPopup { get; protected set; }

        public TimeSpan TransitionOntime { get; protected set; }

        public TimeSpan TransitionOffTime { get; protected set; }

        public float TransitionPosition { get; protected set; }

        public byte TransitionAlpha { get; }

        public ScreenState ScreenState { get; protected set; }

        public bool IsExiting { get; protected internal set; }

        public bool IsActive { get; }

        public SceneManager SceneManager { get; }

        public PlayerIndex? ControllingPlayer { get; internal set; }

        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();
    }
}
