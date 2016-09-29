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
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace Game.Source.Systems.Network {
    public class NetworkHandelerSystem : IUpdateSystem {

        private const int ip = 0;
        private const int tag = 1;
        private const int id = 2;
        private const int comp = 3;

        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            var networkData = componentManager.GetComponents<NetworkDataComponent>().First().Value as NetworkDataComponent;

            if (networkData != null) {
                foreach(var kvp in networkData.NetMessages) {
                    var key = kvp.Key.Split(',');
                    var value = kvp.Value;

                    if (value.Comp == null && key.Count() == 4) {
                        if(key[tag].ToLower() == "player") {
                            var e = componentManager.MakeEntity();
                            e.Tag = "player";
                            value.Comp = new NetworkRecieveTransformComponent();
                            if(value.Msg.PositionInBytes == 49)
                                value.Comp.UnpackMessage(value.Msg);
                            
                            MakePlayer(componentManager, e);

                            componentManager.AddComponent(e, value.Comp as IComponent);
                        }
                    }
                    else {
                        if (value.Msg.PositionInBytes == 49)
                            value.Comp.UnpackMessage(value.Msg);
                    }
                }
            }
        }

        void MakePlayer(ComponentManager componentManager, Entity e) {
            // Player
            ModelComponent player1 = new ModelComponent();

            while (player1.Model == null) {
                var models = componentManager.GetComponents<ModelComponent>().Values.Cast<ModelComponent>();
                var model = models.First(x => componentManager.GetEntity(x).Tag.ToLower() == "player");

                player1.Model = model.Model;
            }
            //player1.Texture = model.Texture;

            ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

            TransformComponent tc1 = new TransformComponent() {
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            PhysicsComponent pc1 = new PhysicsComponent {
                InAir = false,
                GravityStrength = 4,
                Mass = 150
            };

            MovementComponent moveC1 = new MovementComponent {
                Acceleration = 2f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model,
                tc1.GetWorld(tc1.UpdateBuffer));

            var player1C = new PlayerComponent { knockBackResistance = 100 };

            componentManager.AddComponent(e, moveC1);
            componentManager.AddComponent(e, pc1);
            componentManager.AddComponent(e, tc1);
            componentManager.AddComponent(e, actColl);
            componentManager.AddComponent(e, t1);
            componentManager.AddComponent(e, player1);
            componentManager.AddComponent(e, player1C);
        }
    }
}
