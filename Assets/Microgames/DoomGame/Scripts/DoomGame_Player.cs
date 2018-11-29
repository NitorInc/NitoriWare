using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Player : MonoBehaviour
{
    [HideInInspector]
    public List<DoomGame_Enemy> enemies = new List<DoomGame_Enemy>();
    [SerializeField]
    DoomGame_UI ui;
    [SerializeField]
    Animator gunAnimator;
    //Camera mainCamera;
    new AudioSource audio;
    [SerializeField]
    AudioClip shootSound;
    [SerializeField]
    Material screen;
    bool dead = false;
    float dead_lerp = 0;
    float smooth_gun = 0;
    int bullets = 6;

    void Start()
    {
        //mainCamera = Camera.main;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        float mX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mX);
        gunAnimator.transform.localPosition = Vector3.Lerp(
            gunAnimator.transform.localPosition,
            new Vector3(Mathf.Clamp(-mX / 50, -0.2f, 0.2f), -0.3f, 0.5f),
            Time.deltaTime * 5);

        if(Input.GetMouseButtonDown(0))
            Shoot();
        //CheckEnemies();
    }

    void Shoot()
    {
        if(bullets <= 0)
            return;
        bullets--;
        ui.Shoot();
        ui.UpdateAmmo(bullets);
        audio.PlayOneShot(shootSound);
        gunAnimator.Play("doom_gun");
        gunAnimator.SetTrigger("shoot");
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("MicrogameLayer1")))
            hit.collider.GetComponent<DoomGame_Enemy>().DamageSelf();
    }
    /*
    void CheckEnemies()
    {
        DoomGame_UI.rightArrow = DoomGame_UI.leftArrow = false;
        for(int i = 0; i < enemies.Count; i++)
        {
            Vector3 vec = mainCamera.WorldToViewportPoint(
                enemies[i].transform.position);
            if(vec.z < mainCamera.nearClipPlane)
            {
                if(vec.x > 0.5f)
                    DoomGame_UI.leftArrow = true;
                else
                    DoomGame_UI.rightArrow = true;
            }
            else
            {
                if(vec.x < 0.25f)
                    DoomGame_UI.leftArrow = true;
                if(vec.x > 0.75f)
                    DoomGame_UI.rightArrow = true;
            }
        }
    }*/

    public void AddBullets(int value)
    {
        bullets += value;
        if(bullets > 6) bullets = 6;
        ui.UpdateAmmo(bullets);
    }

    public void Kill()
    {
        dead = true;
        ui.Die();
        MicrogameController.instance.setVictory(false, true);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(dead)
            dead_lerp = Mathf.Clamp(dead_lerp + Time.deltaTime * 2, 0, 1);
        screen.SetFloat("_Amount", dead_lerp);
        Graphics.Blit(source, destination, screen);
    }
}
