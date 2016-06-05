using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Component
{
    public class ParticleComponent : IComponent
    {
        public ParticleSystemSettings ExplosionParticleSystemSettings { get; set; }
        public ParticleSystemSettings ExplosionSmokeParticleSystemSettings { get; set; }
        public ParticleSystemSettings FireParticleSystemSettings { get; set; }
        public ParticleSystemSettings ProjectileParticleSystemSettings { get; set; }
        public ParticleSystemSettings SmokePlumeParticleSystemSettings { get; set; }
        public ParticleProjectile ParticleProjectileSettings { get; set; }
        public List<ParticleProjectile> Projectiles { get; set; } = new List<ParticleProjectile>();
        public Random Random { get; set; } = new Random();
        public bool ActivateFire { get; set; } = false;
        public bool ActivateSmoke { get; set; } = false;
        public bool ActivateExplosion { get; set; } = false;
        public TimeSpan TimeToNextProjectile { get; set; }
        public ParticleComponent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            TimeToNextProjectile = TimeSpan.Zero;
            ExplosionSmokeParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "explosionsmoke");
            ExplosionParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice,"explosion");
            FireParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "fire");
            ProjectileParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "projecttrail");
            SmokePlumeParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "smokeplume");
            ParticleProjectileSettings = new ParticleProjectile(ExplosionParticleSystemSettings, ExplosionSmokeParticleSystemSettings,
                                                                ProjectileParticleSystemSettings);
        }
        public class ParticleEmitter
        {
            public ParticleSystemSettings ParticleSystem { get; set; }
            public float TimeBetweenParticles { get; set; }
            public Vector3 PreviousPosition { get; set; }
            public float TimeLeftOver { get; set; }

            public ParticleEmitter(ParticleSystemSettings particleSystem, float particlesPerSecond, Vector3 initialPosition)
            {
                ParticleSystem = particleSystem;
                TimeBetweenParticles = 1.0f / particlesPerSecond;
                PreviousPosition = initialPosition;
            }
        }

        public class ParticleSystemSettings
        {
            public ParticleSettings ParticleSettings;
            public ContentManager Content;

            public Effect ParticleEffect;

            public EffectParameter EffectViewParameter;
            public EffectParameter EffectProjectionParameter;
            public EffectParameter EffectViewportScaleParameter;
            public EffectParameter EffectTimeParameter;

            public ParticleVertex[] Particles;

            public DynamicVertexBuffer VertexBuffer;

            public IndexBuffer IndexBuffer;

            public int FirstActiveParticle { get; set; }
            public int FirstNewParticle { get; set; }
            public int FirstFreeParticle { get; set; }
            public int FirstRetiredParticle { get; set; }

            public float CurrentTime { get; set; }

            public int DrawCounter { get; set; }

            public Random Random { get; set; } 

            public ParticleSystemSettings(ContentManager contentManager, GraphicsDevice graphicsDevice, 
                                          string typeOfParticleSystem)
            {

                this.Content = contentManager;
                
                ParticleSettings = new ParticleSettings(typeOfParticleSystem);

                Random = new Random();

                Particles = new ParticleVertex[ParticleSettings.MaxParticles * 4];
                
                for (int i = 0; i < ParticleSettings.MaxParticles; i++)
                {
                    Particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                    Particles[i * 4 + 1].Corner = new Vector2(1, -1);
                    Particles[i * 4 + 2].Corner = new Vector2(1, 1);
                    Particles[i * 4 + 3].Corner = new Vector2(-1, 1);
                }

                Effect effect = Content.Load<Effect>("ParticleEffect");

                ParticleEffect = effect.Clone();

                EffectParameterCollection parameters = ParticleEffect.Parameters;

                EffectViewParameter = parameters["View"];
                EffectProjectionParameter = parameters["Projection"];
                EffectViewportScaleParameter = parameters["ViewportScale"];
                EffectTimeParameter = parameters["CurrentTime"];

                parameters["Duration"].SetValue((float)ParticleSettings.Duration.TotalSeconds);
                parameters["DurationRandomness"].SetValue(ParticleSettings.DurationRandomness);
                parameters["Gravity"].SetValue(ParticleSettings.Gravity);
                parameters["EndVelocity"].SetValue(ParticleSettings.EndVelocity);
                parameters["MinColor"].SetValue(ParticleSettings.MinColor.ToVector4());
                parameters["MaxColor"].SetValue(ParticleSettings.MaxColor.ToVector4());

                parameters["RotateSpeed"].SetValue(
                    new Vector2(ParticleSettings.MinRotateSpeed, ParticleSettings.MaxRotateSpeed));

                parameters["StartSize"].SetValue(
                    new Vector2(ParticleSettings.MinStartSize, ParticleSettings.MaxStartSize));

                parameters["EndSize"].SetValue(
                    new Vector2(ParticleSettings.MinEndSize, ParticleSettings.MaxEndSize));

                Texture2D texture = Content.Load<Texture2D>(ParticleSettings.TextureName);

                parameters["Texture"].SetValue(texture);

                VertexBuffer = new DynamicVertexBuffer(graphicsDevice, ParticleVertex.VertexDeclaration,
                                                       ParticleSettings.MaxParticles * 4, BufferUsage.WriteOnly);


                ushort[] indices = new ushort[ParticleSettings.MaxParticles * 6];

                for (int i = 0; i < ParticleSettings.MaxParticles; i++)
                {
                    indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                    indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                    indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                    indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                    indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                    indices[i * 6 + 5] = (ushort)(i * 4 + 3);
                }

                IndexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

                IndexBuffer.SetData(indices);


            }
        }

        public class ParticleProjectile
        {
            public ParticleSystemSettings ExplosionParticles;
            public ParticleSystemSettings ExplosionSmokeParticles;
            public ParticleEmitter TrailEmitter;
            public float TrailParticlesPerSecond { get; set; }
            public int NumExplosionParticles { get; set; }
            public int NumExplosionSmokeParticles { get; set; }
            public float ProjectileLifespan { get; set; }
            public float SidewaysVelocityRange { get; set; }
            public float VerticalVelocityRange { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Velocity { get; set; }
            public float Age { get; set; }
            public Random Random { get; set; }
            public float Gravity { get; set; }
            public ParticleProjectile(ParticleSystemSettings explosionParticles, ParticleSystemSettings explosionSmokeParticles,
                                      ParticleSystemSettings projectileTrailParticles)
            {
                ExplosionParticles = explosionParticles;
                ExplosionSmokeParticles = explosionSmokeParticles;
                TrailParticlesPerSecond = 200;
                NumExplosionParticles = 30;
                NumExplosionSmokeParticles = 50;
                ProjectileLifespan = 1.5f;
                SidewaysVelocityRange = 60;
                VerticalVelocityRange = 40;
                Gravity = 15;
                Age = 0;
                Random = new Random();

                Position = new Vector3(0,0,0);

                Velocity = new Vector3((float)(Random.NextDouble() - 0.5) * SidewaysVelocityRange,
                    (float)(Random.NextDouble() + 0.5) * SidewaysVelocityRange,
                    (float)(Random.NextDouble() - 0.5) * SidewaysVelocityRange);

                TrailEmitter = new ParticleEmitter(projectileTrailParticles, TrailParticlesPerSecond, Position);
            }
        }

        public struct ParticleVertex
        {
            public Vector3 Position { get; set; }
            public Vector2 Corner { get; set; }
            public Vector3 Velocity { get; set; }
            public Color Random { get; set; }
            public float Time { get; set; }
            public const int SizeInBytes = 40;

            public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3,
                                                                         VertexElementUsage.Position, 0), new VertexElement(12,
                                                                         VertexElementFormat.Vector2, VertexElementUsage.Normal, 0),
                                                                         new VertexElement(20, VertexElementFormat.Vector3,
                                                                         VertexElementUsage.Normal, 1), new VertexElement(32, VertexElementFormat.Color,
                                                                         VertexElementUsage.Color, 0), new VertexElement(36, VertexElementFormat.Single,
                                                                         VertexElementUsage.TextureCoordinate, 0));
        }

        public class ParticleSettings
        {
            public string TextureName { get; set; }
            public int MaxParticles { get; set; }
            public TimeSpan Duration { get; set; }
            public float DurationRandomness { get; set; }
            public float MinHorizontalVelocity { get; set; }
            public float MaxHorizontalVelocity { get; set; }
            public float MinVerticalVelocity { get; set; }
            public float MaxVerticalVelocity { get; set; }
            public float EndVelocity { get; set; }
            public Color MinColor { get; set; }
            public Color MaxColor { get; set; }
            public float MinRotateSpeed { get; set; }
            public float MaxRotateSpeed { get; set; }
            public float MinStartSize { get; set; }
            public float MaxStartSize { get; set; }
            public float MinEndSize { get; set; }
            public float MaxEndSize { get; set; }

            public Vector3 Gravity { get; set; }
            public float EmitterVelocitySensitivity { get; set; }
            public BlendState BlendState { get; set; }


//INIT for all types of particlesettings
            public ParticleSettings(string typeOfParticleSystem)
            {
                typeOfParticleSystem = typeOfParticleSystem.ToLower();
                InitializeStandardValues();

                if (typeOfParticleSystem == "explosion")
                {
                    InitializeExplosion();
                }
                else if (typeOfParticleSystem == "explosionsmoke")
                {
                    InitializeExplosionSmoke();    
                }
                else if (typeOfParticleSystem == "fire")
                {
                    InitializeFire();
                }
                else if (typeOfParticleSystem == "projecttrail")
                {
                    InitializeProjectileTrail();
                }
                else if (typeOfParticleSystem == "smokeplume")
                {
                    InitializeSmokePlume();
                }
            }

            public void InitializeStandardValues()
            {
                TextureName = null;

                MaxParticles = 100;

                Duration = TimeSpan.FromSeconds(1);

                DurationRandomness = 0;

                EmitterVelocitySensitivity = 1;

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = 0;

                MinVerticalVelocity = 0;
                MaxVerticalVelocity = 0;

                Gravity = Vector3.Zero;

                EndVelocity = 1;

                MinColor = Color.White;
                MaxColor = Color.White;

                MinRotateSpeed = 0;
                MaxRotateSpeed = 0;

                MinStartSize = 100;
                MaxStartSize = 100;

                MinEndSize = 100;
                MaxEndSize = 100;

                BlendState = BlendState.NonPremultiplied;
            }
            public void InitializeExplosion()
            {
                TextureName = "explosion";

                MaxParticles = 100;

                Duration = TimeSpan.FromSeconds(2);
                DurationRandomness = 1;

                MinHorizontalVelocity = 20;
                MaxHorizontalVelocity = 30;

                MinVerticalVelocity = -20;
                MaxVerticalVelocity = 20;

                EndVelocity = 0;

                MinColor = Color.DarkGray;
                MaxColor = Color.Gray;

                MinRotateSpeed = -1;
                MaxRotateSpeed = 1;

                MinStartSize = 7;
                MaxStartSize = 7;

                MinEndSize = 70;
                MaxEndSize = 140;

                BlendState = BlendState.Additive;
            }

            public void InitializeExplosionSmoke()
            {
                TextureName = "smoke";

                MaxParticles = 200;

                Duration = TimeSpan.FromSeconds(4);

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = 50;

                MinVerticalVelocity = -10;
                MaxVerticalVelocity = 50;

                Gravity = new Vector3(0, -20, 0);

                EndVelocity = 0;

                MinColor = Color.LightGray;
                MaxColor = Color.White;

                MinRotateSpeed = -2;
                MaxRotateSpeed = 2;

                MinStartSize = 7;
                MaxStartSize = 7;

                MinEndSize = 70;
                MaxEndSize = 140;
            }

            public void InitializeFire()
            {

                TextureName = "fire";

                MaxParticles = 2400;

                Duration = TimeSpan.FromSeconds(2);

                DurationRandomness = 1;

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = 15;

                MinVerticalVelocity = -10;
                MaxVerticalVelocity = 10;

                Gravity = new Vector3(0, 15, 0);

                MinColor = new Color(255, 255, 255, 10);
                MaxColor = new Color(255, 255, 255, 40);

                MinStartSize = 5;
                MaxStartSize = 10;

                MinEndSize = 10;
                MaxEndSize = 40;

                BlendState = BlendState.Additive;
            }

            public void InitializeProjectileTrail()
            {
                TextureName = "smoke";

                MaxParticles = 1000;

                Duration = TimeSpan.FromSeconds(3);

                DurationRandomness = 1.5f;

                EmitterVelocitySensitivity = 0.1f;

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = 1;

                MinVerticalVelocity = -1;
                MaxVerticalVelocity = 1;

                MinColor = new Color(64, 96, 128, 255);
                MaxColor = new Color(255, 255, 255, 128);

                MinRotateSpeed = -4;
                MaxRotateSpeed = 4;

                MinStartSize = 1;
                MaxStartSize = 3;

                MinEndSize = 4;
                MaxEndSize = 11;
            }

            public void InitializeSmokePlume()
            {
                TextureName = "smoke";

                MaxParticles = 400;

                Duration = TimeSpan.FromSeconds(10);

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = 15;

                MinVerticalVelocity = 10;
                MaxVerticalVelocity = 20;

                Gravity = new Vector3(-20, 0, 0);

                EndVelocity = 0.75f;

                MinRotateSpeed = -1;
                MaxRotateSpeed = 1;

                MinStartSize = 40;
                MaxStartSize = 70;

                MinEndSize = 150;
                MaxEndSize = 250;
            }
        }
    }
}

