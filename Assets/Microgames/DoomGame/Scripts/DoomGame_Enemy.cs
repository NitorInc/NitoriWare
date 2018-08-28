using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Enemy : MonoBehaviour
{
    
    DoomGame_Player player;

    [SerializeField]
    Vector3 targetPosition = Vector3.zero;
    [SerializeField]
    float moveSpeed = 5, damageDelay = 0.5f, distanceToHit = 5;
    [SerializeField]
    int hp = 2;
    SpriteRenderer rend;
    [SerializeField]
    AudioClip[] hurtAudio;
    [SerializeField]
    AudioClip deathAudio;
    [SerializeField]
    new ParticleSystem particleSystem;

    Vector3 direction;
    AudioSource audioSource;
    Transform mainCamera;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main.transform;
        player = mainCamera.GetComponent<DoomGame_Player>();
        player.enemies.Add(this);
    }

    void Update()
    {
        targetPosition = new Vector3(mainCamera.position.x, transform.position.y, mainCamera.position.z);

        direction = Vector3.Normalize(transform.position - targetPosition);

        if(Vector3.Distance(transform.position, targetPosition) < distanceToHit)
            DamagePlayer();
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPosition - direction * 5, moveSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        transform.rotation = mainCamera.rotation;
    }

    float delay = 0;
    void DamagePlayer()
    {
        if(delay > damageDelay)
        {
            player.Kill();
            delay = 0;
        }
        delay += Time.deltaTime;
    }

    public void DamageSelf()
    {
        hp--;
        if(hp <= 0)
            Kill();
        else
            Hurt();
    }

    void Kill()
    {
        audioSource.clip = deathAudio;
        audioSource.Play();
        particleSystem.transform.SetParent(null);
        particleSystem.Emit(30);
        Destroy(particleSystem.gameObject, 2);
        Destroy(gameObject, deathAudio.length);
        Destroy(GetComponent<Collider>());
        rend.enabled = false;
        player.enemies.Remove(this);
        Destroy(this);

        CheckVictory();
    }

    void CheckVictory()
    {
        if(player.enemies.Count == 0)
            MicrogameController.instance.setVictory(true);

    }

    void Hurt()
    {
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
