using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Network;
using ECS_Engine.Engine.Component;
using Game.Source.Components;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Component.Network;

namespace Game.Source.Systems.Network {
    public class NetworkHandelerSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            var networkData = componentManager.GetComponents<NetworkDataComponent>().First().Value as NetworkDataComponent;

            if(networkData != null) {
                foreach(var data in networkData.NetMessages) {
                    if(data.Value[0] == null) {
                        data.Value[0] = componentManager.MakeEntity();
                        var playerEntity1 = data.Value[0] as Entity;
                        // Player
                        ModelComponent player1 = new ModelComponent();
                        //player1.Model = sceneManager.c.Load<Model>("Player");
                        //ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

                        TransformComponent tc1 = new TransformComponent() {
                            Position = new Vector3(0, 10, 0),
                            Rotation = new Vector3(0, 0, 0),
                            Scale = Vector3.One
                        };

                        PhysicsComponent pc1 = new PhysicsComponent {
                            InAir = true,
                            GravityStrength = 4,
                            Mass = 150
                        };

                        MovementComponent moveC1 = new MovementComponent {
                            Acceleration = 2f,
                            Speed = 0,
                            Velocity = Vector3.Zero,
                            AirTime = 0f
                        };

                        NetworkRecieveTransformComponent netTrans = new NetworkRecieveTransformComponent();

                        ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model,
                            tc1.GetWorld(tc1.UpdateBuffer));

                        var player1C = new PlayerComponent { knockBackResistance = 100 };

                        componentManager.AddComponent(playerEntity1, netTrans);
                        componentManager.AddComponent(playerEntity1, moveC1);
                        componentManager.AddComponent(playerEntity1, pc1);
                        componentManager.AddComponent(playerEntity1, tc1);
                        componentManager.AddComponent(playerEntity1, actColl);
                        //componentManager.AddComponent(playerEntity1, t1);
                        componentManager.AddComponent(playerEntity1, player1);
                        componentManager.AddComponent(playerEntity1, player1C);
                    }
                    else {

                    }
                }
            }
        }
    }
}
