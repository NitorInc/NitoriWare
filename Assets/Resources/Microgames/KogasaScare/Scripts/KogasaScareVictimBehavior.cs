using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareVictimBehavior : MonoBehaviour {

    public Transform victimTransform;
    public float minvictimspawnx; //-4.64
    public float maxvictimspawnx; // 4.64
    public float xspawn;

    public Animator rigAnimator;
    public Vibrate vibrate;
    

    // Use this for initialization
    void Start ()
    {
        transform.position = new Vector3(Random.Range(minvictimspawnx, maxvictimspawnx), transform.position.y, transform.position.z);

        //victimTransform = GetComponent<Transform>();
        //xspawn = Random.Range(minvictimspawnx, maxvictimspawnx);
        //Vector2 pos = new Vector2(xspawn, -0.55f);
        //victimTransform.position = pos;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void scare()
    {
        vibrate.vibrateOn = true;
        rigAnimator.SetTrigger("scare");
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    collide(other);
    //}

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    collide(other);
    //}

    //void collide(Collider2D other)
    //{
    //    var kogasaBehavior = other.GetComponent<KogasaScareKogasaBehaviour>();
    //    //if ( == true && other.name == "KogasaObject")
    //    //{
    //    //    destroy(gameobject);
    //    //}
    //}

}
