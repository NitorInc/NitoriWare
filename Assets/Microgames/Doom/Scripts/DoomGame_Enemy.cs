using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Enemy : MonoBehaviour
{

    public static List<DoomGame_Enemy> enemies = new List<DoomGame_Enemy>();

    [SerializeField]
    Vector3 targetPosition = Vector3.zero;
    [SerializeField]
    float moveSpeed = 5, damageDelay = 0.5f, distanceToHit = 5;
    [SerializeField]
    int damageAmount = 30, hp = 2;
    SpriteRenderer rend;
    [SerializeField]
    AudioClip[] hurtAudio;
    [SerializeField]
    AudioClip deathAudio;

    [SerializeField]
    Sprite[] movementSprites;
    [SerializeField]
    float animationDelay = 0.1f;
    [SerializeField]
    new ParticleSystem particleSystem;

    Vector3 direction;
    AudioSource audioSource;

    void Start()
    {
        enemies.Clear();
        enemies.Add(this);
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        targetPosition = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);

        direction = transform.position - targetPosition;
        direction.Normalize();

        if(Vector3.Distance(transform.position, targetPosition) < distanceToHit)
            DamagePlayer();
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPosition - direction * 5, moveSpeed * Time.deltaTime);

        transform.rotation = Camera.main.transform.rotation;

        Animate();
    }

    float anim;
    int animationFrame = 0;
    void Animate()
    {
        anim += Time.deltaTime;
        if(anim > animationDelay)
        {
            animationFrame++;
            if(animationFrame >= movementSprites.Length)
                animationFrame = 0;
            rend.sprite = movementSprites[animationFrame];
            anim = 0;
        }
    }

    float delay = Mathf.Infinity;
    void DamagePlayer()
    {
        if(delay > damageDelay)
        {
            DoomGame_Player.hp -= damageAmount;
            DoomGame_Player.bloodfx = 0.3f;
            delay = 0;
        }
        delay += Time.deltaTime;
    }

    public void DamageSelf()
    {
        hp--;
        if(hp <= 0)
        {
            audioSource.clip = deathAudio;
            audioSource.Play();
            particleSystem.transform.SetParent(null);
            particleSystem.Emit(1);
            Destroy(particleSystem.gameObject, particleSystem.startLifetime);
            Destroy(gameObject, deathAudio.length);
            Destroy(GetComponent<Collider>());
            rend.enabled = false;
            enemies.Remove(this);
            Destroy(this);
            return;
        }
        StartCoroutine(Knockback(direction * 5));
        StartCoroutine(DamageColor());
        audioSource.pitch = Random.value * 0.2f + 0.9f;
        audioSource.clip = hurtAudio[Random.Range(0, hurtAudio.Length)];
        audioSource.Play();
    }

    IEnumerator DamageColor()
    {
        float f = 0;
        while(f < 1)
        {
            rend.color = Color.Lerp(Color.red, Color.white, f);
            f += Time.deltaTime * 5;
            yield return null;
        }
        rend.color = Color.white;
    }

    IEnumerator Knockback(Vector3 force)
    {
        while(force.sqrMagnitude > 0.01f)
        {
            transform.position += force * Time.deltaTime * 10;
            force = Vector3.MoveTowards(force, Vector3.zero, Time.deltaTime * 50);
            yield return null;
        }
    }

}
