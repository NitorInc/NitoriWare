using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct KyoukoEchoNoisePair
{
    [SerializeField]
    public Sprite front;
    [SerializeField]
    public Sprite back;
}

public class KyoukoEchoNoise : MonoBehaviour
{
    
    [SerializeField]
    float speed = 10F;
    [SerializeField]
    float rotationTime = 0.5F;

    [Header("Timings")]
    [SerializeField]
    float hitDelay;

    [SerializeField]
    SpriteRenderer front;
    [SerializeField]
    SpriteRenderer back;

    Rigidbody2D rigidBody;
    Animator animator;

    bool canEcho = true;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetNoise(KyoukoEchoNoisePair noisePair)
    {
        front.sprite = noisePair.front;
        back.sprite = noisePair.back;
    }

    public void Fire(float targetX, float upperBoundY, float lowerBoundY, float delay)
    {
        float targetY = UnityEngine.Random.Range(lowerBoundY, upperBoundY);
        Vector2 target = new Vector2(targetX, targetY);

        // Set velocity
        Vector2 direction = ((Vector2)transform.position - target).normalized;
        StartCoroutine(SetDirection(direction, delay));

        // Rotate
        StartCoroutine(Rotate(direction, false));
    }

    public bool CanEcho()
    {
        return canEcho;
    }

    public void Echo(float hitLocation)
    {
        if (canEcho)
        {
            // Prevent further interactions
            canEcho = false;
            
            // Bounce
            Vector2 direction = new Vector2(1, -hitLocation).normalized;
            rigidBody.velocity = rigidBody.velocity * 0.5F;
            StartCoroutine(SetDirection(direction, hitDelay));

            // Rotate
            StartCoroutine(Rotate(direction, true, rotationTime));

            // Animate
            AnimateEcho();
        }
    }

    IEnumerator SetDirection(Vector2 direction, float delay)
    {
        yield return new WaitForSeconds(delay);

        rigidBody.velocity = -direction * speed;
    }

    IEnumerator Rotate(Vector2 direction, bool flip, float duration = 0)
    {
        if (!flip)
            direction = -direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion start = transform.rotation;
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);

        if (duration == 0)
        {
            transform.rotation = target;
        }
        else
        {
            for (float elapsed = 0; elapsed < 1; elapsed += Time.deltaTime / duration)
            {
                transform.rotation = Quaternion.Lerp(start, target, elapsed);
                yield return null;
            }
        }
    }

    void AnimateEcho()
    {
        animator.Play("Reflect");
    }

}
