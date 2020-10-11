using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiDefenderBulletController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private BulletSpawn[] bulletSpawns; // This is where you'll put your four (or however many are supposed to fire) bullets in the Inspector

    [System.Serializable]
    public class BulletSpawn
    {
        public float fireTime; // Time the shot fires (X seconds after the scene starts)
        public float speed; // Speed of the bullet

    }
    public int TotalBullet;


    private class spawner
    {

        public Vector2 TopBullet = new Vector2(8, 0);

        private Vector2 BottomBullet = new Vector2(-8, 0);

        private Vector2 LeftBullet = new Vector2(0, -8);

        private Vector2 RightBullet = new Vector2(0, 8);

        
    }



    void Start ()
    {

        


        //determine the number of bullets fired
         for(int i = 0; i<TotalBullet; i++) {
            //
            StartCoroutine("fireBullet");
            Vector2 position = new Vector2(8, 0); //(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            Instantiate(bulletPrefab, position, Quaternion.identity); //only works on void start
            //yield return new WaitForSeconds(1f);

           
        }
        //if total bullet is not = 0, bullet prefab ++ and bullet -1


        // dir = new Vector2(Random.Range(5,-5), (5,-5));
 
    }

	
	void Update ()
    {
        //TODO handle when to fire each bulletPrefab based on how much time has passed
        //https://docs.unity3d.com/Manual/Coroutines.html
        //https://www.youtube.com/watch?v=_4xNz23Wlfo
        //if microgame start, run docheck for firebullet after disntance travelled
        

    }

    // Call this when firing a bullet in Update()
    void fireBullet(BulletSpawn bulletSpawn)
    {
        //TODO fire the bullet

      
    }
  
}
