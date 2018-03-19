using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockDoor : MonoBehaviour {
    
    [SerializeField]
    private Collider2D clickCollider;
	
    [SerializeField]
    private bool teleportOnClick;

    [SerializeField]
    private bool shouldMove;

    [SerializeField]
    private int clicksToWin;

    [SerializeField]
    private int speed;

    private float screenWidth;
    private float screenHeight;
    private Vector2 direction; 
    private bool win = false;

    // Use this for initialization
	void Start() {
        // Get the screen dimensions
        screenHeight = Camera.main.orthographicSize;    
        screenWidth = screenHeight * Screen.width / Screen.height;
        NewDirection();
 	    Teleport();
    }
	
	// Update is called once per frame
	void Update() {
        // Test if sprite is clicked
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider)) {
            OnClick(); 
        }
        if (shouldMove && direction != null && !win){
            // Add the direction we're moving in to our position
            Vector2 newPosition = (Vector2)transform.position + (direction*Time.deltaTime);
            transform.position = newPosition;
            // bounce if on edge
            if (Mathf.Abs(transform.position.x) > screenWidth){
                direction.x *= -1;
            }
            if (Mathf.Abs(transform.position.y) > screenHeight){
                direction.y *= -1;
            }
        }
	}
    
    // When the object is clicked
    void OnClick() {
        clicksToWin--;
        if (clicksToWin <= 0 && !win){
            // We win
            win = true;
            MicrogameController.instance.setVictory(victory: true, final: true);
            WinAnimation();
        }
        // Don't teleport if we've won
        else if (teleportOnClick && !win){
            Teleport();
        }
        NewDirection();
    }
    
    // Move to a random location
    void Teleport() {
        float newx = Random.Range(-screenWidth, screenWidth) / 2;
        float newy = Random.Range(-screenHeight, screenHeight) / 2;
        transform.position = new Vector2(newx, newy);
        StartCoroutine(Appear());
    }
    
    // Set a different direction
    void NewDirection() {
        float angle = Random.Range(0.0f, 2*Mathf.PI);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
    }

    // Winning animation
    void WinAnimation(){
        StartCoroutine(OpenDoors());
    }

    // Door opening animation
    IEnumerator OpenDoors(){
        int speed = 10;
        Transform rigTransform = transform.Find("Rig").transform;
        Transform doorL = rigTransform.Find("DoorPanelL").transform;
        Transform doorR = rigTransform.Find("DoorPanelR").transform;
        for (int i = 0; i < 180/speed; i++){
            doorL.Rotate(new Vector3(0, speed, 0));
            doorR.Rotate(new Vector3(0, -speed, 0));
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    // popping back animation
    IEnumerator Appear(){
        Transform rigTransform = transform.Find("Rig").transform;
        Vector2 origScale = rigTransform.localScale;
        for (float i = 1.0f; i <= 11.0f; i+=0.7f){
            rigTransform.localScale = origScale * i/10;
            yield return new WaitForFixedUpdate();
        }
        rigTransform.localScale = origScale;
        yield return null;
    }
}
