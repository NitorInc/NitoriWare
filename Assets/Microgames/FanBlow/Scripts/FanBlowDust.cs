using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowDust : MonoBehaviour
{
    [SerializeField]
    private Vibrate vibrate;
    [SerializeField]
    private ParticleSystem deathParticles;
    [SerializeField]
    private SpriteRenderer dustRenderer;

    [SerializeField]
    private float collisionRegisterThreshold;
    [SerializeField]
    private Vector2 scaleHealthBounds;
    [SerializeField]
    private Vector2 baseScaleRandomRange;
    [SerializeField]
    private float damagePerSpeed = .1f;
    public float DamagePerSpeed { get { return damagePerSpeed; } set { damagePerSpeed = value; } }
    [SerializeField]
    private float minDamageFanSpeed = 5f;
    [SerializeField]
    private float damageDistanceDropOffRate = .5f;
    public float DamageDistanceDropOffRate{ get { return damageDistanceDropOffRate; } set { damageDistanceDropOffRate = value; } }
    [SerializeField]
    private float blowAwayFanSpeedMult;

    [SerializeField]
    private FanBlowFanMovement fan;
    public FanBlowFanMovement Fan { get { return fan; } set { fan = value; } }
    float baseScale;
    float health;
    Vector3 blowVelocity;

    private State state;
    public enum State
    {
        Float,
        BlowAway
    }
    
	void Start ()
    {
        baseScale = MathHelper.randomRangeFromVector(baseScaleRandomRange);
        health = 1f;
        updateScale();
        dustRenderer.sortingOrder = Random.Range(-32767, 32767);
    }
	
	void Update ()
    {
        switch (state)
        {
            case (State.Float):
                var distanceFromFan = ((Vector2)(fan.transform.position - transform.position)).magnitude;
                if (distanceFromFan <= collisionRegisterThreshold)
                {
                    if (fan.CurrentSpeed >= minDamageFanSpeed)
                    {
                        var damageMult = 1f - (damageDistanceDropOffRate * distanceFromFan);
                        if (damageMult < 0f)
                            return;

                        health -= fan.CurrentSpeed * Time.deltaTime * damagePerSpeed * damageMult;
                        if (health <= 0f)
                            kill();
                        else
                            updateScale();
                    }
                }
                break;
            case (State.BlowAway):
                transform.position += blowVelocity * Time.deltaTime;
                break;
        }

    }

    void updateScale()
    {
        //dustRenderer.transform.localScale = Vector3.one * baseScale * Mathf.Lerp(scaleHealthBounds.x, scaleHealthBounds.y, health);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    void kill()
    {
        transform.parent = null;
        var fromFan = (Vector2)(transform.position - fan.transform.position);
        deathParticles.transform.localEulerAngles = Vector3.forward * fromFan.getAngle();
        blowVelocity = fan.CurrentVelocity * blowAwayFanSpeedMult;
        deathParticles.Play();
        dustRenderer.enabled = false;
        state = State.BlowAway;
    }
}
