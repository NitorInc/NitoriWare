using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerTimeline : MonoBehaviour {

    [SerializeField]
    private int RequiredFiles = 3;

    public int Files = 0;
    public Collider2D currentCollider;

	void Update () {

        if (!Input.GetMouseButton(0) && currentCollider != null)
        {
            Files++;
            currentCollider.gameObject.tag = "MicrogameTag2";
            currentCollider.gameObject.GetComponent<Animation>().Play();
            currentCollider = null;
        }

        if (Files >= RequiredFiles)
        {
            GameObject.Find("BG").SendMessage("Upload");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) => collide(collision, true);

    private void OnTriggerStay2D(Collider2D collision) => collide(collision, true);

    private void OnTriggerExit2D(Collider2D collision) => collide(collision, false);

    void collide(Collider2D collision, bool isColliding)
    {
        if (collision.gameObject.tag == "MicrogameTag1")
            currentCollider = isColliding ? collision : null;
    }
}
