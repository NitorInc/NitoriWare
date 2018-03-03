using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareVictimBehavior : MonoBehaviour
{

    public Transform victimTransform;

    public Animator rigAnimator;
    public Vibrate vibrate;

    private KogasaScareKogasaBehaviour.State state;


    // Use this for initialization
    void Start ()
    {
        state = KogasaScareKogasaBehaviour.State.Default;

        //victimTransform = GetComponent<Transform>();
        //xspawn = Random.Range(minvictimspawnx, maxvictimspawnx);
        //Vector2 pos = new Vector2(xspawn, -0.55f);
        //victimTransform.position = pos;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void scare(bool successful, int direction)
    {
        vibrate.vibrateOn = true;
        state = successful ? KogasaScareKogasaBehaviour.State.Victory : KogasaScareKogasaBehaviour.State.Loss;

        rigAnimator.SetTrigger("scare");
        rigAnimator.SetInteger("state", (int)state);
        rigAnimator.SetInteger("direction", direction);

        SendMessage("onScare", successful, SendMessageOptions.DontRequireReceiver);
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
