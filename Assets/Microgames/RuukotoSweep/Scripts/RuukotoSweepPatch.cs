using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweepPatch : MonoBehaviour
{
    public RuukotoSweep_Movement playerMovement;
    public ParticleSystem leafParticles;
    public int spawnCountMin, spawnCountMax;
    public Vector2 xSpawnBounds, ySpawnBounds;
    public float minLeafSeparation;
    public AudioClip sweepClip;
    public Vector2 sweepPitchRange;
    public float interruptSweepVolume = .5f;
    
	void Start ()
    {
        leafParticles.Emit(Random.Range(spawnCountMin, spawnCountMax));
        var leafModule = leafParticles.main;
        leafModule.simulationSpeed = 0f;

        setStartPosition();
	}

    void setStartPosition()
    {
        do
        {
            transform.position = new Vector3(MathHelper.randomRangeFromVector(xSpawnBounds), MathHelper.randomRangeFromVector(ySpawnBounds), transform.position.z);
        }
        while (isTooClose());
    }

    bool isTooClose()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform childTransform = transform.parent.GetChild(i);
            if (childTransform != transform && ((Vector2)(childTransform.position - transform.position)).magnitude < minLeafSeparation)
                return true;
        }
        return ((Vector2)(transform.position - playerMovement.transform.position)).magnitude < 2f;
    }
	
	void Update ()
    {
	}

    void collide(Collider2D other)
    {
        //Patch is hit
        if (transform.parent != null && other.name.Contains("Player"))
        {
            var leafModule = leafParticles.main;
            leafModule.simulationSpeed = 1f;
            MicrogameController.instance.playSFX(sweepClip, pitchMult: MathHelper.randomRangeFromVector(sweepPitchRange),
                volume: (MicrogameController.instance.getSFXSource().isPlaying ? interruptSweepVolume : 1f),
                panStereo: AudioHelper.getAudioPan(transform.position.x / 2f));

            Transform leafParent = transform.parent;
            transform.parent = null;
            if (leafParent.childCount == 0)
            {
                playerMovement.victory();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collide(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        collide(other);
    }
}
