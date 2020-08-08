using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiadefenderBulletController3 : MonoBehaviour
{

    [SerializeField] //Prefab bullets for each direction
    private GameObject TopBullet, BottomBullet, LeftBullet, RightBullet;


    public int TotalBullet;

    public int BulletFired = 0;

    public int SpawnLocation;

    //spawn prefab every 2 seconds
    public float SpawnRate = 2f;

    public float NextSpawn = 0f;

    private bool Firing = true;

    private void Start()
    {
        Vector2 TopBullet = new Vector2(8, 0);

        Vector2 BottomBullet = new Vector2(-8, 0);

        Vector2 LeftBullet = new Vector2(0, -8);

        Vector2 RightBullet = new Vector2(0, 8);

    }

    
    // Update is called once per frame
    void Update()
    {

        if (Time.time > NextSpawn) //Endless loop of firing bullets
        //if (TotalBullet > BulletFired) //Shoot only a number of bullet but unable to set spawn timer
        {
            SpawnLocation = Random.Range(1, 5);
            Debug.Log(SpawnLocation);

            //Instantiate a prefab depending on value
            switch (SpawnLocation)
            {
                case 1:
                    Instantiate(TopBullet, TopBullet.transform.position, Quaternion.Euler(0, 0, 270));
                    break;
                case 2:
                    Instantiate(BottomBullet, BottomBullet.transform.position, Quaternion.Euler(0, 0, 90));
                    break;
                case 3:
                    Instantiate(LeftBullet, LeftBullet.transform.position, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(RightBullet, RightBullet.transform.position, Quaternion.Euler(0, 0, 180));
                    break;
            }

            //Set delay for next bullet
            NextSpawn = Time.time + SpawnRate;



            //Counting bullets
            //BulletFired++;
            Debug.Log("BulletFIred");
        }


    }
}