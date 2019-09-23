using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockDoor : MonoBehaviour {
    
    [SerializeField]
    private AudioClip knockSound;

    [SerializeField]
    private AudioClip missSound;

    [SerializeField]
    private AudioClip openSound;

    [SerializeField]
    private float knockVolume = .5f;
    
    [SerializeField]
    private float missVolume = .5f;

    [SerializeField]
    private float openVolume = .5f;

    [SerializeField]
    private bool teleportOnClick;

    [SerializeField]
    private bool shouldMove;

    [SerializeField]
    private int clicksToWin;

    [SerializeField]
    private int speed;

    [SerializeField]
    private float minTeleportDistance = 2f;

    [SerializeField]
    private float moveAwayFromCursorRange = 90f;

    private float screenWidth;
    private float screenHeight;
    private Vector2 direction;  
    private bool win = false;
    private Animator animator;
    private BoxCollider2D collider;

    // Use this for initialization
    void Start() {
        // Get the screen dimensions
        screenHeight = MainCameraSingleton.instance.orthographicSize;    
        screenWidth = screenHeight * 4f / 3f;
        
        animator = GetComponentInChildren<Animator>(); 
        collider = GetComponent<BoxCollider2D>();
        // Randomize starting position and movement direction
        //NewDirection();
        Teleport(false);
    }
	
    // Update is called once per frame
    void Update() {
        if (shouldMove && direction != null && !win){
            // Add the direction we're moving in to our position
            Vector2 newPosition = (Vector2)transform.position + (direction*Time.deltaTime);
            transform.position = newPosition;
            // bounce if on edge
            if (Mathf.Abs(transform.position.x) + collider.size.x/4 > screenWidth
                && Mathf.Sign(transform.position.x) == Mathf.Sign(direction.x)){
                direction.x *= -1;
            }
            if (Mathf.Abs(transform.position.y) + collider.size.y/4 > screenHeight
                && Mathf.Sign(transform.position.y) == Mathf.Sign(direction.y))
            {
                direction.y *= -1;
            }
        }
    }

    public void MissKnock(float xPosition)
    {
        MicrogameController.instance.playSFX(
            missSound, volume: missVolume,
            panStereo: AudioHelper.getAudioPan(xPosition));
    }
    
    // When the object is clicked
    void OnClick() {
        if (!win) {
            clicksToWin--;
            if (clicksToWin <= 0){
                // We win
                win = true;
                MicrogameController.instance.setVictory(victory: true, final: true);
                Win();
            }
            else if (teleportOnClick){
                Teleport();
            }
            
            MicrogameController.instance.playSFX(
                knockSound, volume: knockVolume,
                panStereo: AudioHelper.getAudioPan(transform.position.x)
            );
            //NewDirection();
        }
    }
    
    // Move to a random location
    void Teleport(bool animate=true) {

        var newPosition = transform.position;
        var oldPosition = transform.position;
        int tries = 100;
        for (int i = 0; i < tries || (newPosition - oldPosition).magnitude <= minTeleportDistance; i++)
        {
            newPosition = new Vector3(
                Random.Range(-screenWidth, screenWidth) / 2,
                Random.Range(-screenHeight, screenHeight) / 2,
                newPosition.z);
            if (i >= tries)
                Debug.Log("Too many tries!");
        }

        transform.position = newPosition;

        NewDirection();

        if (animate) animator.SetTrigger("Clicked");
    }
    
    // Set a different direction
    void NewDirection() {
        var mouseAngle = ((Vector2)(CameraHelper.getCursorPosition() - transform.position)).getAngle();
        var newAngle = mouseAngle + Random.Range(-moveAwayFromCursorRange, moveAwayFromCursorRange) + 180f;
        float angle = newAngle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
    }

    // Winning animation
    void Win(){
        MicrogameController.instance.playSFX(
            openSound, volume: openVolume,
            panStereo: AudioHelper.getAudioPan(transform.position.x)
        );
        animator.SetBool("Win", true);
    }
}
