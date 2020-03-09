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


	void Start ()
    {
		
	}
	
	void Update ()
    {
		//TODO handle when to fire each bulletPrefab based on how much time has passed
	}

    // Call this when firing a bullet in Update()
    void fireBullet(BulletSpawn bulletSpawn)
    {
        //TODO fire the bullet
    }
}
