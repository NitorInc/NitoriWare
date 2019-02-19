using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Enemy : MonoBehaviour {

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
    [SerializeField]
    Vector3[] path;
    Coroutine hurtCr;

    int pid;
    Vector3 direction;
    AudioSource audioSource;

    public bool startDeactivated;

    void Start () {
        if (path == null) path = new Vector3[0];
        rend = GetComponent<SpriteRenderer> ();
        audioSource = GetComponent<AudioSource> ();
        player = GameObject.FindObjectOfType<DoomGame_Player> ();
        player.enemies.Add (this);
        if (startDeactivated)
            gameObject.SetActive (false);
    }

    void Update () {
        if (pid < path.Length) {
            if (transform.position.x == path[pid].x && transform.position.z == path[pid].z)
                pid++;
            if (pid < path.Length)
                targetPosition = new Vector3 (path[pid].x, transform.position.y, path[pid].z);
            else
                targetPosition = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - direction * distanceToHit;
        } else {
            targetPosition = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - direction * distanceToHit;
        }

        direction = Vector3.Normalize (transform.position - targetPosition);

        if (Vector3.Distance (transform.position, player.transform.position) < distanceToHit + 0.01f)
            DamagePlayer ();
        else
            transform.position = Vector3.MoveTowards (transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void LateUpdate () {
        transform.rotation = player.transform.rotation;
    }

    float delay = 0;
    void DamagePlayer () {
        if (delay > damageDelay) {
            player.Kill ();
            delay = 0;
        }
        delay += Time.deltaTime;
    }

    public void DamageSelf () {
        particleSystem.Emit (20);
        hp--;
        if (hp <= 0)
            Kill ();
        else
            Hurt ();
    }

    void Kill () {
        audioSource.clip = deathAudio;
        audioSource.Play ();
        particleSystem.transform.SetParent (null);
        float dist = Vector3.Distance (player.transform.position, transform.position);
        particleSystem.startSpeed *= 1 + 0.05f * dist;
        particleSystem.Emit (50 + (int) dist);
        Destroy (particleSystem.gameObject, 2);
        Destroy (GetComponent<Collider> ());
        Destroy (gameObject, deathAudio.length);
        if (hurtCr != null)
            StopCoroutine (hurtCr);
        player.StartCoroutine (DeathAnimation ());
        player.enemies.Remove (this);
        player.AddBullets (6);
        player.shake += 0.6f;
        Destroy (this);

        CheckVictory ();
    }

    IEnumerator DeathAnimation () {
        float t = 1;
        while (t > 0) {
            rend.color = new Color (1, 0, 0, t);
            t -= Time.deltaTime * 3;
            rend.transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
    }

    void CheckVictory () {
        if (player.enemies.Count == 0)
            MicrogameController.instance.setVictory (true);
    }

    void Hurt () {
        StartCoroutine (Knockback (direction * 5));
        if (hurtCr != null)
            StopCoroutine (hurtCr);
        hurtCr = StartCoroutine (DamageColor ());
        audioSource.pitch = Random.value * 0.2f + 0.9f;
        audioSource.clip = hurtAudio[Random.Range (0, hurtAudio.Length)];
        audioSource.Play ();
    }

    IEnumerator DamageColor () {
        float f = 0;
        while (f < 1) {
            rend.color = Color.Lerp (Color.red, Color.white, f);
            f += Time.deltaTime * 5;
            yield return null;
        }
        rend.color = Color.white;
    }

    IEnumerator Knockback (Vector3 force) {
        while (force.sqrMagnitude > 0.01f) {
            transform.position += force * Time.deltaTime * 10;
            force = Vector3.MoveTowards (force, Vector3.zero, Time.deltaTime * 50);
            yield return null;
        }
    }

}