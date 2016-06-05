using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems.ParticleSystems
{
    public class ParticleRenderSystem : IRenderSystem
    {

        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager)
        {
            var particleComponents = componentManager.GetComponents<ParticleComponent>();
            var cameraComponents = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cameraComponents.First().Value;

            foreach (KeyValuePair<Entity, IComponent> component in particleComponents)
            {
                ParticleComponent particleComponent = (ParticleComponent)component.Value;

                if (particleComponents != null)
                {
                    SetCamera(camera.View, camera.Projection, particleComponent);
                    DrawAllParticleSystems(gameTime, graphicsDevice, particleComponent);

                }
            }
        }

        public void SetCamera(Matrix view, Matrix projection, ParticleComponent particleComponent)
        {
            foreach (ParticleComponent.ParticleProjectile projectile in particleComponent.Projectiles)
            {
                projectile.ExplosionParticles.EffectViewParameter.SetValue(view);
                projectile.ExplosionParticles.EffectProjectionParameter.SetValue(projection);

                projectile.ExplosionSmokeParticles.EffectViewParameter.SetValue(view);
                projectile.ExplosionSmokeParticles.EffectProjectionParameter.SetValue(projection);

                projectile.TrailEmitter.ParticleSystem.EffectViewParameter.SetValue(view);
                projectile.TrailEmitter.ParticleSystem.EffectProjectionParameter.SetValue(projection);
            }
 
                particleComponent.ExplosionParticleSystemSettings.EffectViewParameter.SetValue(view);
                particleComponent.ExplosionParticleSystemSettings.EffectProjectionParameter.SetValue(projection);

                particleComponent.ExplosionSmokeParticleSystemSettings.EffectViewParameter.SetValue(view);
                particleComponent.ExplosionSmokeParticleSystemSettings.EffectProjectionParameter.SetValue(projection);

                particleComponent.FireParticleSystemSettings.EffectViewParameter.SetValue(view);
                particleComponent.FireParticleSystemSettings.EffectProjectionParameter.SetValue(projection);


                particleComponent.SmokePlumeParticleSystemSettings.EffectViewParameter.SetValue(view);
                particleComponent.SmokePlumeParticleSystemSettings.EffectProjectionParameter.SetValue(projection);

        }

        public void DrawAllParticleSystems(GameTime gametime, GraphicsDevice graphicsDevice, ParticleComponent particleComponent)
        {

            foreach (ParticleComponent.ParticleProjectile projectile in particleComponent.Projectiles)
            {
                DrawParticleSystem(gametime, graphicsDevice, projectile.ExplosionParticles);
                DrawParticleSystem(gametime, graphicsDevice, projectile.ExplosionSmokeParticles);
                DrawParticleSystem(gametime, graphicsDevice, projectile.TrailEmitter.ParticleSystem);
            }

            DrawParticleSystem(gametime, graphicsDevice, particleComponent.ExplosionParticleSystemSettings);
            DrawParticleSystem(gametime, graphicsDevice, particleComponent.ExplosionSmokeParticleSystemSettings);


            DrawParticleSystem(gametime, graphicsDevice, particleComponent.FireParticleSystemSettings);


            DrawParticleSystem(gametime, graphicsDevice, particleComponent.SmokePlumeParticleSystemSettings);
        }

        public void DrawParticleSystem(GameTime gametime, GraphicsDevice graphicsDevice, ParticleComponent.ParticleSystemSettings particleSystem)
        {
            if (particleSystem.VertexBuffer.IsContentLost)
            {
                particleSystem.VertexBuffer.SetData(particleSystem.Particles);
            }

            if (particleSystem.FirstNewParticle != particleSystem.FirstFreeParticle)
            {
                AddNewParticlesToVertexBuffer(particleSystem);
            }

            if (particleSystem.FirstActiveParticle != particleSystem.FirstFreeParticle)
            {
                graphicsDevice.BlendState = particleSystem.ParticleSettings.BlendState;
                graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

                particleSystem.EffectViewportScaleParameter.SetValue(new Vector2(0.5f / graphicsDevice.Viewport.AspectRatio, -0.5f));

                particleSystem.EffectTimeParameter.SetValue(particleSystem.CurrentTime);

                graphicsDevice.SetVertexBuffer(particleSystem.VertexBuffer);
                graphicsDevice.Indices = particleSystem.IndexBuffer;

                foreach (EffectPass pass in particleSystem.ParticleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (particleSystem.FirstActiveParticle < particleSystem.FirstFreeParticle)
                    {

                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     particleSystem.FirstActiveParticle * 4, (particleSystem.FirstFreeParticle - particleSystem.FirstActiveParticle) * 4,
                                                     particleSystem.FirstActiveParticle * 6, (particleSystem.FirstFreeParticle - particleSystem.FirstActiveParticle) * 2);
                    }
                    else
                    {

                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     particleSystem.FirstActiveParticle * 4, (particleSystem.ParticleSettings.MaxParticles - particleSystem.FirstActiveParticle) * 4,
                                                     particleSystem.FirstActiveParticle * 6, (particleSystem.ParticleSettings.MaxParticles - particleSystem.FirstActiveParticle) * 2);

                        if (particleSystem.FirstFreeParticle > 0)
                        {
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         0, particleSystem.FirstFreeParticle * 4,
                                                         0, particleSystem.FirstFreeParticle * 2);
                        }
                    }
                }

                graphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            particleSystem.DrawCounter++;
        }

        public void AddNewParticlesToVertexBuffer(ParticleComponent.ParticleSystemSettings particleSystem)
        {
            int stride = ParticleComponent.ParticleVertex.SizeInBytes;

            if (particleSystem.FirstNewParticle < particleSystem.FirstFreeParticle)
            {

                particleSystem.VertexBuffer.SetData(particleSystem.FirstNewParticle * stride * 4, particleSystem.Particles,
                                      particleSystem.FirstNewParticle * 4,
                                     (particleSystem.FirstFreeParticle - particleSystem.FirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                particleSystem.VertexBuffer.SetData(particleSystem.FirstNewParticle * stride * 4, particleSystem.Particles,
                                      particleSystem.FirstNewParticle * 4,
                                     (particleSystem.ParticleSettings.MaxParticles - particleSystem.FirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (particleSystem.FirstFreeParticle > 0)
                {
                    particleSystem.VertexBuffer.SetData(0, particleSystem.Particles,
                                         0, particleSystem.FirstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            particleSystem.FirstNewParticle = particleSystem.FirstFreeParticle;
        }
    }
}
