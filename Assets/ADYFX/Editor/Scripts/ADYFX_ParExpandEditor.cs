using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ADYFX_ParExpandEditor
{
    public ParticleSystem par;
    public ParticleSystem.MainModule main;
    public ParticleSystem.EmissionModule emission;
    public ParticleSystem.ShapeModule shape;
    public ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
    public ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime;
    public ParticleSystem.InheritVelocityModule inheritVelocity;
    public ParticleSystem.ForceOverLifetimeModule forceOverLifetime;
    public ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
    public ParticleSystem.ColorBySpeedModule colorBySpeed;
    public ParticleSystem.SizeOverLifetimeModule sizeOverLifetime;
    public ParticleSystem.SizeBySpeedModule sizeBySpeed;
    public ParticleSystem.RotationOverLifetimeModule rotationOverLifetime;
    public ParticleSystem.RotationBySpeedModule rotationBySpeed;
    public ParticleSystem.ExternalForcesModule externalForces;
    public ParticleSystem.NoiseModule noise;
    public ParticleSystem.CollisionModule collision;
    public ParticleSystem.TriggerModule trigger;
    public ParticleSystem.SubEmittersModule sub;
    public ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;
    public ParticleSystem.LightsModule lights;
    public ParticleSystem.TrailModule trail;
    public ParticleSystem.CustomDataModule custom;
    public ParticleSystemRenderer particleSystemRenderer;
    public void SetPar() 
    {

    }
    public void GetPar(ParticleSystem particle) 
    {
        par = particle;
        main = par.main;
        emission = par.emission;
        shape = par.shape;
        velocityOverLifetime = par.velocityOverLifetime;
        limitVelocityOverLifetime = par.limitVelocityOverLifetime;
        inheritVelocity = par.inheritVelocity;
        forceOverLifetime = par.forceOverLifetime;
        colorOverLifetime = par.colorOverLifetime;
        colorBySpeed = par.colorBySpeed;
        sizeOverLifetime = par.sizeOverLifetime;
        sizeBySpeed = par.sizeBySpeed;
        rotationOverLifetime = par.rotationOverLifetime;
        rotationBySpeed = par.rotationBySpeed;
        externalForces = par.externalForces;
        noise = par.noise;
        collision = par.collision;
        trigger = par.trigger;
        sub = par.subEmitters;
        textureSheetAnimation = par.textureSheetAnimation;
        lights = par.lights;
        trail = par.trails;
        custom = par.customData;
        particleSystemRenderer = par.gameObject.GetComponent<ParticleSystemRenderer>();
    }
}
