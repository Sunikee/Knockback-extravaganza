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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems.ParticleSystems
{
    public class ParticleSystem: IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager,
            SceneManager sceneManager)
        {
            if (gametime == null)
                throw new ArgumentNullException("Gametime is null!");

            var particleComponents = componentManager.GetComponents<ParticleComponent>();
            if (particleComponents != null)
            {
                foreach (KeyValuePair<Entity, IComponent> component in particleComponents)
                {
                    ParticleComponent particleComponent = (ParticleComponent) component.Value;

                    Random random = particleComponent.Random;

                    foreach (ParticleComponent.ParticleSystemSettings smoke in particleComponent.SmokeParticlesList)
                    {
                        if (smoke.IsActive)
                        {
                            UpdateSmokePlume(smoke);
                        }
                    }
                    if (particleComponent.ActivateExplosion)
                        UpdateExplosions(gametime, particleComponent);
                    if (particleComponent.ActivateFire)
                        UpdateFire(gametime, particleComponent);

                    UpdateProjectiles(gametime, particleComponent);

                    UpdateAllParticleSystems(gametime, particleComponent);
                }
            }
        }

        public void UpdateAllParticleSystems(GameTime gametime, ParticleComponent particleComponent)
        {
                foreach (ParticleComponent.ParticleProjectile projectile in particleComponent.Projectiles)
                {
                    UpdateParticleSystem(gametime, projectile.ExplosionParticles);
                    UpdateParticleSystem(gametime, projectile.ExplosionSmokeParticles);
                    UpdateParticleSystem(gametime, projectile.TrailEmitter.ParticleSystem);
                }

                    UpdateParticleSystem(gametime, particleComponent.ExplosionParticleSystemSettings);
                    UpdateParticleSystem(gametime, particleComponent.ExplosionSmokeParticleSystemSettings);
               
                    UpdateParticleSystem(gametime,particleComponent.FireParticleSystemSettings);

                foreach(ParticleComponent.ParticleSystemSettings smoke in particleComponent.SmokeParticlesList)
                    UpdateParticleSystem(gametime, smoke);
        }

        public void UpdateParticleSystem(GameTime gametime, ParticleComponent.ParticleSystemSettings particleSystem)
        {
            particleSystem.CurrentTime += (float) gametime.ElapsedGameTime.TotalSeconds;

            RetireActiveParticles(particleSystem);
            FreeRetiredParticles(particleSystem);

            if (particleSystem.FirstActiveParticle == particleSystem.FirstFreeParticle)
                particleSystem.CurrentTime = 0;

            if (particleSystem.FirstRetiredParticle == particleSystem.FirstActiveParticle)
            {
                particleSystem.DrawCounter = 0;
            }
        }

        public void RetireActiveParticles(ParticleComponent.ParticleSystemSettings particleSystem)
        {
            float particleDuration = (float) particleSystem.ParticleSettings.Duration.TotalSeconds;

            while (particleSystem.FirstActiveParticle != particleSystem.FirstNewParticle)
            {
                float particleAge = particleSystem.CurrentTime -
                                    particleSystem.Particles[particleSystem.FirstActiveParticle*4].Time;

                if (particleAge < particleDuration)
                    break;

                particleSystem.Particles[particleSystem.FirstActiveParticle*4].Time = particleSystem.DrawCounter;

                particleSystem.FirstActiveParticle++;

                if (particleSystem.FirstActiveParticle >= particleSystem.ParticleSettings.MaxParticles)
                    particleSystem.FirstActiveParticle = 0;
            }
        }

        public void FreeRetiredParticles(ParticleComponent.ParticleSystemSettings particleSystem)
        {
            while (particleSystem.FirstRetiredParticle != particleSystem.FirstActiveParticle)
            {
                int age = particleSystem.DrawCounter -
                          (int) particleSystem.Particles[particleSystem.FirstRetiredParticle*4].Time;

                if (age < 3)
                    break;

                particleSystem.FirstRetiredParticle++;

                if (particleSystem.FirstRetiredParticle >= particleSystem.ParticleSettings.MaxParticles)
                    particleSystem.FirstRetiredParticle = 0;
            }
        }
        public void UpdateExplosion(GameTime gametime, ParticleComponent particleComponent)
        {
            particleComponent.TimeToNextProjectile -= gametime.ElapsedGameTime;

            if (particleComponent.TimeToNextProjectile <= TimeSpan.Zero)
            {
                particleComponent.Projectiles.Add(new ParticleComponent.ParticleProjectile(particleComponent.ExplosionParticleSystemSettings, particleComponent.ExplosionSmokeParticleSystemSettings,
                                                                                           particleComponent.ProjectileParticleSystemSettings));

                particleComponent.TimeToNextProjectile +=TimeSpan.FromSeconds(1);
            }
        }

        public void UpdateExplosions(GameTime gametime, ParticleComponent particleComponent)
        {
            particleComponent.TimeToNextProjectile -= gametime.ElapsedGameTime;

            if (particleComponent.TimeToNextProjectile <= TimeSpan.Zero)
            {
                particleComponent.Projectiles.Add(new ParticleComponent.ParticleProjectile(particleComponent.ExplosionParticleSystemSettings, 
                                                  particleComponent.ExplosionSmokeParticleSystemSettings, particleComponent.ProjectileParticleSystemSettings));

                particleComponent.TimeToNextProjectile += TimeSpan.FromSeconds(1);
            }
        }
        public void UpdateSmokePlume(ParticleComponent.ParticleSystemSettings smoke)
        {
            AddParticle(smoke.ParticleSettings.Position, Vector3.Zero, smoke, smoke.ParticleSettings);
        }

        public void UpdateProjectiles(GameTime gametime, ParticleComponent particleComponent)
        {
            int i = 0;

            while (i < particleComponent.Projectiles.Count)
            {
                if(!UpdateProjectile(gametime, particleComponent, i))
                    particleComponent.Projectiles.RemoveAt(i);
                else
                {
                    i++;
                }
            }
        }

        public void UpdateFire(GameTime gametime, ParticleComponent particleComponent)
        {
            const int fireParticlesPerFrame = 20;
                for (int i = 0; i < fireParticlesPerFrame; i++)
                {
                    AddParticle(RandomPointCircle(particleComponent), Vector3.Zero, particleComponent.FireParticleSystemSettings,particleComponent.FireParticleSystemSettings.ParticleSettings);
                }
                AddParticle(RandomPointCircle(particleComponent), Vector3.Zero, particleComponent.SmokePlumeParticleSystemSettings, particleComponent.SmokePlumeParticleSystemSettings.ParticleSettings);

        }

        public void AddParticle(Vector3 position, Vector3 velocity, ParticleComponent.ParticleSystemSettings particleSystem, ParticleComponent.ParticleSettings particleSettings)
        {
            int nextFreeParticle = particleSystem.FirstFreeParticle + 1;

            if (nextFreeParticle >= particleSettings.MaxParticles)
                nextFreeParticle = 0;

            if (nextFreeParticle == particleSystem.FirstRetiredParticle)
                return;

            velocity *= particleSettings.EmitterVelocitySensitivity;

            float horizontalVelocity = MathHelper.Lerp(particleSettings.MinHorizontalVelocity,
                                                       particleSettings.MaxHorizontalVelocity,
                                                       (float) particleSystem.Random.NextDouble());

            double horizontalAngle = particleSystem.Random.NextDouble()*MathHelper.TwoPi;

            velocity.X += horizontalVelocity*(float) Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity*(float) Math.Sin(horizontalAngle);
            velocity.Y += MathHelper.Lerp(particleSettings.MinVerticalVelocity,
                                          particleSettings.MaxVerticalVelocity,
                                          (float) particleSystem.Random.NextDouble());

            Color randomValues = new Color((byte) particleSystem.Random.Next(255),
                                           (byte) particleSystem.Random.Next(255),
                                           (byte) particleSystem.Random.Next(255),
                                           (byte) particleSystem.Random.Next(255));

            for (int i = 0; i < 4; i++)
            {
                particleSystem.Particles[particleSystem.FirstFreeParticle*4 + i].Position = position;
                particleSystem.Particles[particleSystem.FirstFreeParticle*4 + i].Velocity = velocity;
                particleSystem.Particles[particleSystem.FirstFreeParticle * 4 + i].Random = randomValues;
                particleSystem.Particles[particleSystem.FirstFreeParticle * 4 + i].Time = particleSystem.CurrentTime;
            }

            particleSystem.FirstFreeParticle = nextFreeParticle;
        }

        public Vector3 RandomPointCircle(ParticleComponent particleComponent)
        {
            const float radius = 30;
            const float height = 40;

            double angle = particleComponent.Random.NextDouble()*Math.PI*2;

            float x = (float) Math.Cos(angle);
            float y = (float) Math.Sin(angle);

            return new Vector3(x*radius, y*radius+height, 0);
        }

        public bool UpdateProjectile(GameTime gametime, ParticleComponent particleComponent, int index)
        {
            float elapsedTime = (float) gametime.ElapsedGameTime.TotalSeconds;

            particleComponent.Projectiles[index].Position += particleComponent.Projectiles[index].Velocity*elapsedTime;
            particleComponent.Projectiles[index].Velocity = new Vector3(particleComponent.Projectiles[index].Velocity.X, particleComponent.Projectiles[index].Velocity.Y - elapsedTime * particleComponent.Projectiles[index].Gravity, particleComponent.Projectiles[index].Velocity.Z);

            particleComponent.Projectiles[index].Age += elapsedTime;

            UpdateEmitter(gametime, particleComponent.Projectiles[index].Position, particleComponent, index);

            if (particleComponent.Projectiles[index].Age > particleComponent.Projectiles[index].ProjectileLifespan)
            {
                for (int i = 0; i < particleComponent.Projectiles[index].NumExplosionParticles; i++)
                {
                    AddParticle(particleComponent.Projectiles[index].Position, particleComponent.Projectiles[index].Velocity, particleComponent.Projectiles[index].ExplosionParticles, particleComponent.Projectiles[index].ExplosionParticles.ParticleSettings);
                }

                for (int i = 0; i < particleComponent.Projectiles[index].NumExplosionSmokeParticles; i++)
                {
                    AddParticle(particleComponent.Projectiles[index].Position, particleComponent.Projectiles[index].Velocity, particleComponent.Projectiles[index].ExplosionSmokeParticles, particleComponent.Projectiles[index].ExplosionSmokeParticles.ParticleSettings);
                }
                return false;
            }
            return true;
        }

        public void UpdateEmitter(GameTime gametime, Vector3 newPosition, ParticleComponent particleComponent, int index)
        {
            if(gametime == null)
                throw new ArgumentNullException("Gametime null. This is not good!");

            float elapsedTime = (float) gametime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > 0)
            {
                Vector3 velocity = (newPosition - particleComponent.Projectiles[index].TrailEmitter.PreviousPosition);

                float timeToSpend = particleComponent.Projectiles[index].TrailEmitter.TimeLeftOver + elapsedTime;

                float currentTime = -particleComponent.Projectiles[index].TrailEmitter.TimeLeftOver;

                while (timeToSpend > particleComponent.Projectiles[index].TrailEmitter.TimeBetweenParticles)
                {
                    currentTime += particleComponent.Projectiles[index].TrailEmitter.TimeBetweenParticles;
                    timeToSpend -= particleComponent.Projectiles[index].TrailEmitter.TimeBetweenParticles;

                    float mu = currentTime/elapsedTime;

                    Vector3 position = Vector3.Lerp(particleComponent.Projectiles[index].TrailEmitter.PreviousPosition, newPosition, mu);

                    AddParticle(position, velocity, particleComponent.Projectiles[index].TrailEmitter.ParticleSystem, particleComponent.Projectiles[index].TrailEmitter.ParticleSystem.ParticleSettings);
                }

                particleComponent.Projectiles[index].TrailEmitter.TimeLeftOver = timeToSpend;
            }

            particleComponent.Projectiles[index].TrailEmitter.PreviousPosition = newPosition;
        }
    }
}
