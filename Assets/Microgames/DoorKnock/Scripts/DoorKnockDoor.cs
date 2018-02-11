using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockDoor : MonoBehaviour {
    
    [SerializeField]
    private Collider2D clickCollider;
	
    [SerializeField]
    private bool teleportOnClick;

    [SerializeField]
    private int clicksToWin;

    private float screenWidth;
    private float screenHeight;
    
    // Use this for initialization
	void Start() {
        // Get the screen dimensions
        screenHeight = Camera.main.orthographicSize;    
        screenWidth = screenHeight * Screen.width / Screen.height;
 	}
	
	// Update is called once per frame
	void Update() {
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
            OnClick(); 
	}
    
    // When the object is clicked
    void OnClick() {
        print("Clicked.");
        clicksToWin--;
        if (clicksToWin <= 0){
            // We win
            MicrogameController.instance.setVictory(victory: true, final: true);
        }

        if (teleportOnClick)
            Teleport();
    }
    
    // Move to a random location
    void Teleport() {
        print(screenWidth);
        float newx = Random.Range(-screenWidth, screenWidth) / 2;
        float newy = Random.Range(-screenHeight, screenHeight) / 2;
        transform.position = new Vector2(newx, newy);
    }
}
