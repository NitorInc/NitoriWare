using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_WeakPoint
    : MonoBehaviour
{

    public static int weakpointCount = 0;

    [SerializeField]
    private bool flipped;

    // Use this for initialization
    void Start() {
        weakpointCount = 2; //TODO: Figure out nice way to increment this without breaking R debugging
        float startx = Random.Range(-2f, 2f);
        float yfactor = 0.47f;
        if (flipped) {
            yfactor *= -1;
        }
        Vector3 position = new Vector3(startx, startx * yfactor, 0);
        transform.localPosition = position;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnMouseDown() {
        weakpointCount--;
        if (weakpointCount == 0) {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
        Destroy(transform.parent.gameObject);
    }
}
