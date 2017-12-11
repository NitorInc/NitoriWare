using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_WriggleScript : MonoBehaviour {

    public GameObject myHitbox;

    

    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<BugSwat_InstantDestroy>() != null)
        {
            MicrogameController.instance.setVictory(true, true);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
