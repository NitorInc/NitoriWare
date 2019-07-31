using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerTimeline : MonoBehaviour {

    [SerializeField]
    private int RequiredFiles = 3;

    public int Files = 0;

	void Update () {
		if (Files >= RequiredFiles)
        {
            GameObject.Find("BG").SendMessage("Upload");
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "MicrogameTag1")
            {
                Files++;
                hit.collider.gameObject.tag = "MicrogameTag2";
            }
        }
    }

}
