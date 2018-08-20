using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Player : MonoBehaviour
{
    DoomGame_EnemySingleton enemySingleton;
    [SerializeField]
    Animator gunAnimator;
    Camera mainCamera;
    new AudioSource audio;
    [SerializeField]
    AudioClip shootSound;

    void Start()
    {
        mainCamera = Camera.main;
        enemySingleton = mainCamera.GetComponent<DoomGame_EnemySingleton>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
        if(Input.GetMouseButtonDown(0))
            Shoot();
        CheckEnemies();
    }

    void Shoot()
    {
        audio.PlayOneShot(shootSound);
        gunAnimator.Play("doom_gun");
        gunAnimator.SetTrigger("shoot");
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("MicrogameLayer1")))
            hit.collider.GetComponent<DoomGame_Enemy>().DamageSelf();
    }

    void CheckEnemies()
    {
        DoomGame_UI.rightArrow = DoomGame_UI.leftArrow = false;
        for(int i = 0; i < enemySingleton.enemies.Count; i++)
        {
            Vector3 vec = mainCamera.WorldToViewportPoint(
                enemySingleton.enemies[i].transform.position);
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
    }

    public void Kill()
    {
        MicrogameController.instance.setVictory(false, true);
    }

    /*
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        blit.SetFloat("_Amount", bloodfx -= Time.deltaTime);
        if(bloodfx < 0)
            bloodfx = 0;
        Graphics.Blit(source, destination, blit);
    }
    */
}
