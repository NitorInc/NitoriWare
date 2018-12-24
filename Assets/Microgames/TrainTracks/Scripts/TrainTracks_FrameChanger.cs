using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_FrameChanger : MonoBehaviour {

    [SerializeField]
    private Sprite[] spriteArray;

    [SerializeField]
    private float framesPerSprite = 1;

    private int frame = 0;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<SpriteRenderer>().sprite = spriteArray[Mathf.FloorToInt(frame/framesPerSprite)];
        frame++;
        if (Mathf.FloorToInt(frame / framesPerSprite) >= spriteArray.Length) {
            frame = 0;
        }
	}
}
