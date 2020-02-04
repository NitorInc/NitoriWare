using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoomGame_Player : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 1f;
    [SerializeField]
    private float turnAroundSensitivty = 5f;
    [SerializeField]
    private float turnAroundStopEnemiesAngle = 30f;
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
    AudioClip shootSound, deadSound, emptyClipSound;
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

    private bool isTurningAround;
    public void setTurningAround() => isTurningAround = true;

    void Start ()
    {
        startPosition = transform.position;
        mainCamera = MainCameraSingleton.instance.transform;
        audio = GetComponent<AudioSource> ();
    }

    void Update ()
    {
        transform.position = startPosition + Random.insideUnitSphere * shake;
        shake -= Time.deltaTime * 3;
        if (shake <= 0)
            shake = 0;
        var sensitivity = mouseSensitivity;
        if (isTurningAround)
        {
            if (getEnemyAverateAngle() < turnAroundStopEnemiesAngle)
                isTurningAround = false;
            else
                sensitivity = turnAroundSensitivty;
        }
        float mX = Input.GetAxis("Mouse X") * sensitivity;
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

    void Shoot ()
    {
        if (useAmmo && bullets <= 0 || dead)
        {
            audio.PlayOneShot (emptyClipSound, 0.8f);
            return;
        }
        if (useAmmo)
            bullets--;
        ui.Shoot ();
        ui.UpdateAmmo (bullets);
        audio.PlayOneShot (shootSound, 0.6f);
        gunAnimator.Play ("doom_gun");
        gunAnimator.SetTrigger ("shoot");
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer ("MicrogameLayer1")))
            hit.collider.GetComponent<DoomGame_Enemy> ().DamageSelf ();
    }

    float getEnemyAverateAngle()
    {
        if (!enemies.Any())
            return 0f;
        var enemyAngleDiffs = new List<float>();
        var playerAngle = mainCamera.transform.eulerAngles.y;
        var playerPos = new Vector2(transform.position.x, transform.position.z);
        foreach (var enemy in enemies)
        {
            if (enemy.isActiveAndEnabled)
            {
                var enemyPos = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
                var enemyAngle = (enemyPos - playerPos).getAngle() - 90f;
                enemyAngleDiffs.Add(Mathf.Abs(MathHelper.getAngleDifference(playerAngle, enemyAngle)));
            }
        }
        return enemyAngleDiffs.Min();
        //if (Mathf.Abs(average) < minSensitivityAddAngle)
        //    return 0f;
        //return Mathf.Abs(average) * sensitivityAngleAdd;
    }

    public void AddBullets (int value)
    {
        bullets += value;
        if (bullets > 6) bullets = 6;
        ui.UpdateAmmo (bullets);
    }

    public void Kill ()
    {
        if (!dead)
        {
            dead = true;
            ui.Die ();
            MicrogameController.instance.setVictory (false, true);
            MicrogameController.instance.playSFX(deadSound);
            //AudioSource.PlayClipAtPoint (deadSound, transform.position);
        }
    }

}