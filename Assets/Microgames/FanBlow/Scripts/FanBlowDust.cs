using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowDust : MonoBehaviour
{
    [SerializeField]
    private Vibrate vibrate;

    [SerializeField]
    private Vector2 scaleHealthBounds;
    [SerializeField]
    private Vector2 baseScaleRandomRange;
    [SerializeField]
    private float damagePerSpeed = .1f;
    [SerializeField]
    private float minDamageFanSpeed = 5f;
    [SerializeField]
    private float damageDistanceDropOffRate = .5f;

    [SerializeField]
    private FanBlowFanMovement fan;
    public FanBlowFanMovement Fan { get { return fan; } set { fan = value; } }
    float baseScale;
    float health;
    
	void Start ()
    {
        baseScale = MathHelper.randomRangeFromVector(baseScaleRandomRange);
        health = 1f;
        updateScale();
    }
	
	void Update ()
    {

	}

    void updateScale()
    {
        transform.localScale = Vector3.one * baseScale * Mathf.Lerp(scaleHealthBounds.x, scaleHealthBounds.y, health);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals(fan.tag))
        {
            if (fan.CurrentSpeed >= minDamageFanSpeed)
            {
                var distance = ((Vector2)(fan.transform.position - transform.position)).magnitude;
                var damageMult = 1f - (damageDistanceDropOffRate * distance);
                if (damageMult < 0f)
                    return;

                health -= fan.CurrentSpeed * Time.deltaTime * damagePerSpeed * damageMult;
                if (health <= 0f)
                    Destroy(gameObject);
                else
                    updateScale();
            }
        }
    }
}
