using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerUpload : MonoBehaviour {

    [SerializeField]
    private bool CanUpload = false;

    [SerializeField]
    private bool IsDiffOne = false;

    [SerializeField]
    private bool IsFirst = false;

    public void Upload () {
        CanUpload = true;
        if (!IsFirst)
        {
            GetComponent<Animator>().SetBool("Glow", true);
        }
        if (IsDiffOne)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            GameObject.Find("BlackScreen").GetComponent<Animator>().SetBool("Expand", true);
        }
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
