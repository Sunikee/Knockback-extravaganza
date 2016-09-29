using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Systems
{
    public class ScoreTimeSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            //Calculate the timer
            var scoreTimeComponents = componentManager.GetComponents<ScoreTimeComponent>();
            var scoreTimeComponent = (ScoreTimeComponent)scoreTimeComponents.First().Value;
            scoreTimeComponent.ElapsedTime = (float)scoreTimeComponent.stopWatch.Elapsed.TotalSeconds;
            scoreTimeComponent.Score += (decimal)scoreTimeComponent.ElapsedTime;
            scoreTimeComponent.TotalScore = (int)Decimal.Round(scoreTimeComponent.Score, 2);

            scoreTimeComponent.stopWatch.Reset();
            scoreTimeComponent.stopWatch.Start();



        }
    }
}
