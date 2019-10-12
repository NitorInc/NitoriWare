using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerUpload : MonoBehaviour {

    [SerializeField]
    private bool CanUpload = false;

	public void Upload () {
        CanUpload = true;
        GetComponent<Animator>().SetBool("Glow", true);
	}

    private void OnMouseDown()
    {
        if (CanUpload)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            GameObject.Find("BlackScreen").GetComponent<Animator>().SetBool("Expand", true);
        }
    }
}
