using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerChase_CirnoCollisionCheck : MonoBehaviour {
    public FlowerChase_GameMaster GameMasterScript;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MicrogameTag1")
        {
            GameMasterScript.HitBlock();
        }

    }
}
