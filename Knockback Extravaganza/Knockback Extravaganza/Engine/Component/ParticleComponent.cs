﻿using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{

    public class ParticleEmitter : IComponent
    {

        public ParticleSettings ParticleSettings { get; set; }
        public ParticleProjectile Projectile { get; set; }
        public float TimeBetweenParticles { get; set; }
        public Vector3 PreviousPosition { get; set; }
        public float TimeLeftOver { get; set; }
        public ParticleVertex ParticleVertex { get; set; }
        public float ParticlesPerSecond { get; set; }
    }
    public class ParticleSettings
    {
        public string TextureName { get; set; }
        public int MaxParticles { get; set; }
        public TimeSpan Duration { get; set; }
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
        public ParticleType Type { get; set; }
        public Vector3 Gravity { get; set; }
    }
    public class ParticleProjectile
    {
        public float TrailParticlesPerSecond { get; set; }
        public int NumExplosionParticles { get; set; }
        public int NumExplosionSmokeParticles { get; set; }
        public float ProjectileLifeSpan { get; set; }
        public float SidewaysVelocityRange { get; set; }
        public float VerticalVelocityRange { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Age { get; set; }
        public Random Random { get; set; }
        public float Gravity { get; set; }
    }
    public class ParticleVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 Corner { get; set; }
        public Vector3 Velocity { get; set; }
        public Color Random { get; set; }
        public float Time { get; set; }
        public int SizeInBytes { get; set; }
    }
    public enum ParticleType
    {
        Smoke,
        Explosion,
        RingOfFire
    }
}