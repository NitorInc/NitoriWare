using System.Collections;
using UnityEngine;

public class KyoukoEchoNoise : MonoBehaviour
{
    
    [SerializeField]
    float speed = 10F;

    [Header("Timings")]
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
    }

    public void Fire(float targetX, float upperBoundY, float lowerBoundY, float delay)
    {
        float targetY = Random.Range(lowerBoundY, upperBoundY);
        Vector2 target = new Vector2(targetX, targetY);

        // Set velocity
        Vector2 direction = ((Vector2)this.transform.position - target).normalized;
        StartCoroutine(SetDirection(direction, delay));

        // Rotate
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public bool CanEcho()
    {
        return this.canEcho;
    }

    public void Echo(float hitLocation)
    {
        if (this.canEcho)
        {
            // Prevent further interactions
            this.canEcho = false;
            
            // Bounce
            Vector2 direction = new Vector2(1, -hitLocation).normalized;
            this.rigidBody.velocity = this.rigidBody.velocity * 0.5F;
            StartCoroutine(SetDirection(direction, this.hitDelay));

            // Rotate
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Animate
            AnimateEcho();
        }
    }

    IEnumerator SetDirection(Vector2 direction, float delay)
    {
        yield return new WaitForSeconds(delay);

        this.rigidBody.velocity = -direction * this.speed;
    }

    void AnimateEcho()
    {
        this.animator.Play("Reflect");
    }

}
