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
        public List<ParticleSystemSettings> SmokeParticlesList { get; set; } = new List<ParticleSystemSettings>();
        public ParticleProjectile ParticleProjectileSettings { get; set; }
        public List<ParticleProjectile> Projectiles { get; set; } = new List<ParticleProjectile>();
        public Random Random { get; set; } = new Random();
        public bool ActivateSmoke { get; set; } = false;
        public bool ActivateExplosion { get; set; } = false;
        public bool ActivateFire { get; set; } = false;
        public TimeSpan TimeToNextProjectile { get; set; }

        public ParticleComponent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            TimeToNextProjectile = TimeSpan.Zero;
            SmokePlumeParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "smoke", "smoke");
            FireParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "fire", "fire");
            ExplosionParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "explosion", "explosion");
            ExplosionSmokeParticleSystemSettings = new ParticleSystemSettings(contentManager, graphicsDevice, "explosionsmoke", "smoke");

        }

        /*
        The emitter is a helper class for objects that wants to leave a trail of particles behind when moving.
        The emitter has it's own ParticleSystem and keeps track of the moving objects previous position and it's velocity.
        */
        public class ParticleEmitter
        {
            
            public ParticleSystemSettings ParticleSystem { get; set; }
            public float TimeBetweenParticles { get; set; }
            public Vector3 PreviousPosition { get; set; }
            public float TimeLeftOver { get; set; }
            //Constructor for ParticleEmitter, setting up its particlesystem, 
            //the time between particles and sets the starting position of the object that particles are produces for.
            public ParticleEmitter(ParticleSystemSettings particleSystem, float particlesPerSecond, Vector3 initialPosition)
            {
                ParticleSystem = particleSystem;
                TimeBetweenParticles = 1.0f / particlesPerSecond;
                PreviousPosition = initialPosition;
            }
        }

        /*
        The ParticleSystemSettings is what holds on to the particles and helps display them through the system
        */
        public class ParticleSystemSettings
        {
            //Setting up variables needed for the particlesystem,
            //ParticleSettings are what is used to control the appearance of the particle system
            public ParticleSettings ParticleSettings;
            public ContentManager Content;

            //Variables used to set the effect that should be used for the particle system
            //The particle effect is used to copute the animation in the vertex shader.
            public Effect ParticleEffect;
            public EffectParameter EffectViewParameter;
            public EffectParameter EffectProjectionParameter;
            public EffectParameter EffectViewportScaleParameter;
            public EffectParameter EffectTimeParameter;
            //Array of particles
            public ParticleVertex[] Particles;

            //vertextbuffer for holding onto particles in the same way as the particles array,
            //but it is copied so that the GPU can access it.
            public DynamicVertexBuffer VertexBuffer;
            
            //IndexBuffer turn four vertices into particle quads
            public IndexBuffer IndexBuffer;

            //Variables for keeping track of which particles to draw and which ones to retire back into the free list.
            public int FirstActiveParticle { get; set; }
            public int FirstNewParticle { get; set; }
            public int FirstFreeParticle { get; set; }
            public int FirstRetiredParticle { get; set; }

            public float CurrentTime { get; set; }
            
            //How many times draw has been called. Used to keep track of particles to retire.
            public int DrawCounter { get; set; }
            public Random Random { get; set; }

            //A bool to check if the system is active and should be updated and drawn.
            public bool IsActive { get; set; } = true;
            public ParticleSystemSettings(ContentManager contentManager, GraphicsDevice graphicsDevice,
                                          string typeOfParticleSystem, string textureName)
            {

                this.Content = contentManager;
                
                //Sets up the particle settings
                ParticleSettings = new ParticleSettings(typeOfParticleSystem, textureName);

                Random = new Random();

                //Allocate the particle array and sets it's corners, since the corners won't change.
                Particles = new ParticleVertex[ParticleSettings.MaxParticles * 4];
                
                for (int i = 0; i < ParticleSettings.MaxParticles; i++)
                {
                    Particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                    Particles[i * 4 + 1].Corner = new Vector2(1, -1);
                    Particles[i * 4 + 2].Corner = new Vector2(1, 1);
                    Particles[i * 4 + 3].Corner = new Vector2(-1, 1);
                }

                //Sets up the parameters of the effect
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

                //Creates the dynamic vertex buffer
                VertexBuffer = new DynamicVertexBuffer(graphicsDevice, ParticleVertex.VertexDeclaration,
                                                       ParticleSettings.MaxParticles * 4, BufferUsage.WriteOnly);


                //Create and sets the index buffer
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
        //Class for combining several particle systems and composite an effect from them.
        //With the help of a particle emitter it leaves a trail of particles behind. 
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
            public Vector3 Position { get; set; } = Vector3.Zero;
            public Vector3 Gravity { get; set; }
            public float EmitterVelocitySensitivity { get; set; }
            public BlendState BlendState { get; set; }


//INIT for all types of particlesettings
            public ParticleSettings(string typeOfParticleSystem, string textureName)
            {
                typeOfParticleSystem = typeOfParticleSystem.ToLower();
                InitializeStandardValues();

                if (typeOfParticleSystem == "explosion")
                {
                    InitializeExplosion(textureName);
                }
                else if (typeOfParticleSystem == "explosionsmoke")
                {
                    InitializeExplosionSmoke(textureName);    
                }
                else if (typeOfParticleSystem == "fire")
                {
                    InitializeFire(textureName);
                }
                else if (typeOfParticleSystem == "projecttrail")
                {
                    InitializeProjectileTrail(textureName);
                }
                else if (typeOfParticleSystem == "smoke")
                {
                    InitializeSmokePlume(textureName);
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
            public void InitializeExplosion(string textureName)
            {
                TextureName = textureName;

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

            public void InitializeExplosionSmoke(string textureName)
            {
                TextureName = textureName;

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

            public void InitializeFire(string textureName)
            {

                TextureName = textureName;

                MaxParticles = 300;

                Duration = TimeSpan.FromSeconds(5);

                DurationRandomness = 1;

                MinHorizontalVelocity = 0;
                MaxHorizontalVelocity = -15;

                MinVerticalVelocity = -20;
                MaxVerticalVelocity = 20;

                Gravity = new Vector3(0, -15, 0);

                MinColor = new Color(255, 255, 255, 10);
                MaxColor = new Color(255, 255, 255, 40);

                MinStartSize = 15;
                MaxStartSize = 40;

                MinEndSize = 65;
                MaxEndSize = 90;

                BlendState = BlendState.Additive;
            }

            public void InitializeProjectileTrail(string textureName)
            {
                TextureName = textureName;

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

            public void InitializeSmokePlume(string textureName)
            {
                TextureName = textureName;

                MaxParticles = 50;

                Duration = TimeSpan.FromSeconds(10);

                MinHorizontalVelocity = 0.1f;
                MaxHorizontalVelocity = 1;

                MinVerticalVelocity = -25;
                MaxVerticalVelocity = 25;

                Gravity = new Vector3(-20, 5, 0);

                EndVelocity = 0.75f;

                MinRotateSpeed = -1;
                MaxRotateSpeed = 1;

                MinStartSize = 15;
                MaxStartSize = 35;

                MinEndSize = 70;
                MaxEndSize = 150;
            }
        }
    }
}

