using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashHitEffectController : MonoBehaviour
{

    [Header("Adjust timing fields for ranked hits")]
    [SerializeField]
    private float goodThreshold = .05f;
    [SerializeField]
    private float normalThreshold = .1f;

    [Header("Particle effect fields")]
    [SerializeField]
    private GameObject goodParticles;
    [SerializeField]
    private GameObject normalParticles;
    [SerializeField]
    private GameObject badParticles;
    [SerializeField]
    private float normalArc;
    [SerializeField]
    private float badArc;
    [SerializeField]
    private Vector3 positionOffset;

    enum HitLevel
    {
        Good,
        Normal,
        Bad
    }

    HitLevel getHitLevel(float AbsOffset)
    {
        if (AbsOffset < goodThreshold)
            return HitLevel.Good;
        else if (AbsOffset < normalThreshold)
            return HitLevel.Normal;
        else
            return HitLevel.Bad;
    }

    void onSlash(YoumuSlashTarget.SlashData data)
    {
        var typeData = data.target.TypeData;

        //set Variables
        var AbsOffset = Mathf.Abs(data.timeOffset);
        var hitLevel = getHitLevel(AbsOffset);
        var particlePrefab = getParticlePrefab(hitLevel);
        
        //Create particles
        var newObject = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        newObject.transform.parent = transform.root;
        newObject.transform.position += positionOffset;
        newObject.transform.localScale = new Vector3(
            Mathf.Abs(newObject.transform.localScale.x),
            Mathf.Abs(newObject.transform.localScale.y),
            Mathf.Abs(newObject.transform.localScale.z));

        //Particle arcing for non-good hits
        if (hitLevel != HitLevel.Good)
        {
            var particles = newObject.GetComponent<ParticleSystem>();
            var particlesShapeModule = particles.shape;
            var arc = hitLevel == HitLevel.Bad ? badArc : normalArc;
            particlesShapeModule.arc = arc;
            var rotation = (180f - arc) / 2f;
            if (data.timeOffset > 0f)
                rotation += 180f;
            newObject.transform.eulerAngles = Vector3.forward * rotation;
        }

        //Play audio

        YoumuSlashSoundEffectPlayer.instance.play(data.target.TypeData.HitBaseSoundEffect, data.target.HitDirection);
        if (hitLevel != HitLevel.Bad)
            YoumuSlashSoundEffectPlayer.instance.play(data.target.TypeData.HitNormalSoundEffect, data.target.HitDirection);
        else
            YoumuSlashSoundEffectPlayer.instance.play(data.target.TypeData.HitBarelySoundEffect, data.target.HitDirection);
    }

    GameObject getParticlePrefab(HitLevel hitLevel)
    {
        switch (hitLevel)
        {
            case (HitLevel.Good):
                return goodParticles;
            case (HitLevel.Normal):
                return normalParticles;
            case (HitLevel.Bad):
                return badParticles;
            default:
                return null;
        }
    }
}