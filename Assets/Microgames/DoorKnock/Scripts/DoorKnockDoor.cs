using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockDoor : MonoBehaviour {
    
    [SerializeField]
    private BoxCollider2D clickCollider;
	
    [SerializeField]
    private AudioClip knockSound;

    [SerializeField]
    private AudioClip openSound;
    
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
    private Transform rigTransform;
    private Vector2 origScale; 
    private Vector2 direction;  
    private bool win = false;

    // Use this for initialization
	void Start() {
        // Get the screen dimensions
        screenHeight = Camera.main.orthographicSize;    
        screenWidth = screenHeight * Screen.width / Screen.height;
        
        // save the scale
        rigTransform = transform.Find("Rig");
        origScale = rigTransform.localScale;
        
        // Randomize starting position and movement direction
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
            if (Mathf.Abs(transform.position.x) + clickCollider.size.x/4 > screenWidth){
                direction.x *= -1;
            }
            if (Mathf.Abs(transform.position.y) + clickCollider.size.y/4 > screenHeight){
                direction.y *= -1;
            }
        }
	}
    
    // When the object is clicked
    void OnClick() {
        if (!win) {
            clicksToWin--;
            if (clicksToWin <= 0){
                // We win
                win = true;
                MicrogameController.instance.setVictory(victory: true, final: true);
                WinAnimation();
            }
            else if (teleportOnClick){
                Teleport();
            }
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
            NewDirection();
        }
        MicrogameController.instance.playSFX(
            knockSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x)
        );
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
        MicrogameController.instance.playSFX(
            openSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x)
        );
        StartCoroutine(OpenDoors());
    }

    // Door opening animation
    IEnumerator OpenDoors(){
        int speed = 10;
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
        for (float i = 1.0f; i <= 11.0f; i+=0.7f){
            rigTransform.localScale = origScale * i/10;
            yield return new WaitForFixedUpdate();
        }
        rigTransform.localScale = origScale;
        yield return null;
    }
}
