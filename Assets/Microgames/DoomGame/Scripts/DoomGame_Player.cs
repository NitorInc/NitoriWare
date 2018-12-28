using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Player : MonoBehaviour {
    [SerializeField]
    bool useAmmo = true;
    [HideInInspector]
    public List<DoomGame_Enemy> enemies = new List<DoomGame_Enemy> ();
    [SerializeField]
    DoomGame_UI ui;
    [SerializeField]
    Animator gunAnimator;
    Transform mainCamera;
    new AudioSource audio;
    [SerializeField]
    AudioClip shootSound, deadSound;
    [SerializeField]
    public Material screen;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public float dead_lerp = 0;
    float smooth_gun = 0;
    int bullets = 6;
    Vector3 startPosition;
    [HideInInspector]
    public float shake = 0;

    void Start () {
        startPosition = transform.position;
        mainCamera = Camera.main.transform;
        audio = GetComponent<AudioSource> ();
    }

    void Update () {
        transform.position = startPosition + Random.insideUnitSphere * shake;
        shake -= Time.deltaTime * 3;
        if (shake <= 0)
            shake = 0;
        float mX = Input.GetAxis ("Mouse X");
        transform.Rotate (Vector3.up, mX);
        gunAnimator.transform.localPosition = Vector3.Lerp (
            gunAnimator.transform.localPosition,
            new Vector3 (Mathf.Clamp (-mX / 50, -0.2f, 0.2f), -0.3f, 0.5f),
            Time.deltaTime * 5);
        mainCamera.transform.position = transform.position;
        mainCamera.transform.rotation = transform.rotation;

        if (Input.GetMouseButtonDown (0))
            Shoot ();
    }

    void Shoot () {
        if (useAmmo && bullets <= 0 || dead)
            return;
        if (useAmmo)
            bullets--;
        ui.Shoot ();
        ui.UpdateAmmo (bullets);
        audio.PlayOneShot (shootSound);
        gunAnimator.Play ("doom_gun");
        gunAnimator.SetTrigger ("shoot");
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer ("MicrogameLayer1")))
            hit.collider.GetComponent<DoomGame_Enemy> ().DamageSelf ();
    }

    public void AddBullets (int value) {
        bullets += value;
        if (bullets > 6) bullets = 6;
        ui.UpdateAmmo (bullets);
    }

    public void Kill () {
        if (!dead) {
            dead = true;
            ui.Die ();
            MicrogameController.instance.setVictory (false, true);
            AudioSource.PlayClipAtPoint (deadSound, transform.position);
        }
    }

}