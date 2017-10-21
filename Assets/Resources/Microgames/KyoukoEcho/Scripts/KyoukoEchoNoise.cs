using System.Collections;
using UnityEngine;

public class KyoukoEchoNoise : MonoBehaviour
{
    
    [SerializeField]
    float speed = 10F;

    [Header("Timings")]
    [SerializeField]
    float startDelay;
    [SerializeField]
    float hitDelay;
    
    Rigidbody2D rigidBody;
    Animator animator;

    bool canEcho = true;

    // Use this for initialization
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        // Calculate random start position
        Bounds bounds = this.transform.parent.GetComponent<Collider2D>().bounds;
        Vector2 startPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
        // Set start position
        this.transform.position = startPosition;

        // Pick a target which will intersect with Kyouko's area of effect
        KyoukoEchoKyouko kyouko = FindObjectOfType<KyoukoEchoKyouko>();
        float targetY = Random.Range(kyouko.BoundBottom, kyouko.BoundTop);
        Vector2 target = new Vector2(kyouko.transform.position.x, targetY);

        // Set velocity
        Vector2 direction = ((Vector2)this.transform.position - target).normalized;
        StartCoroutine(SetDirection(direction, this.startDelay));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger on player
        if (this.canEcho && other.CompareTag("Player"))
        {
            // Modify new direction based on what part of Kyouko was hit
            KyoukoEchoBodyPart bodyPart = other.GetComponent<KyoukoEchoBodyPart>();
            float hitLocation = bodyPart.CalculateBodyHitLocationY(this.transform.position.y);

            // Bounce
            Vector2 direction = new Vector2(1, -hitLocation).normalized;
            this.rigidBody.velocity = this.rigidBody.velocity * 0.5F;
            StartCoroutine(SetDirection(direction, this.hitDelay));

            // Animate
            Echo();
        }
    }

    IEnumerator SetDirection(Vector2 direction, float delay)
    {
        yield return new WaitForSeconds(delay);

        this.rigidBody.velocity = -direction * this.speed;
    }

    void Echo()
    {
        this.animator.Play("Reflect");

        // Prevent further interactions
        this.canEcho = false;
    }

}
