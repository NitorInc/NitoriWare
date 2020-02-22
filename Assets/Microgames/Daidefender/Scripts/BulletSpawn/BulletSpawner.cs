using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //To be mount on main camera
    public GameObject bulletPrefab;

    [SerializeField]
    private float respawnTime = 1.0f;

    private Vector2 screenBounds;

    private Vector2[] SpawnPos = new Vector2[0];

    // Use this for initialization
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(bulletWave());
        /*SpawnPos[0] = new Vector2(0,screenBounds.y * -2);
        SpawnPos[1] = new Vector2(0,screenBounds.y * 2);
        SpawnPos[2] = new Vector2(screenBounds.x * 2,0);
        SpawnPos[3] = new Vector2(screenBounds.x * -2,0); */
    }

    

   

    private void spawnBullets()
    {
        GameObject a = Instantiate(bulletPrefab) as GameObject;
        //set a randomizer 
        //a.transform.position = new Vector2(screenBounds.x * -2, 0); //spawn on the left
        // a.transform.position = new Vector2(screenBounds.x * 2, 0);//spawn from right
        //a.transform.position = new Vector2(0, screenBounds.y * 2); //spawn from above
        a.transform.position = new Vector2(0, screenBounds.y * -2);// spawn from below
        /*int SpawnPoint = 0;
        SpawnPoint = Random.Range(0, 3); 
        a.transform.position = SpawnPos[1]; */
    }
    IEnumerator bulletWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnBullets();
        }
    }
}

//Hey how about having a line where you can type wasd and let the script process to set the bullet spawn. A sequence spawner.