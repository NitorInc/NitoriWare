using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_Cursor : MonoBehaviour {

    [SerializeField]
    private Sprite closedsprite;

    [SerializeField]
    private Sprite opensprite;

    [SerializeField]
    private AudioClip snipClip;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        spriteRenderer.sprite = Input.GetMouseButton(0) ? closedsprite : opensprite;
        if (Input.GetMouseButtonDown(0))
            MicrogameController.instance.playSFX(snipClip,
                AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x));

        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            spriteRenderer.sprite = closedsprite;
            return;
        }
    }
}
