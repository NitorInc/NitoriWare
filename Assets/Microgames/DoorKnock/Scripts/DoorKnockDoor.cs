using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockDoor : MonoBehaviour {
    
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

    [SerializeField]
    private GameObject fist;

    private float screenWidth;
    private float screenHeight;
    private Vector2 direction;  
    private bool win = false;
    private Animator animator;
    private BoxCollider2D collider;
    private bool intersecting = false;

    // Use this for initialization
    void Start() {
        // Get the screen dimensions
        screenHeight = Camera.main.orthographicSize;    
        screenWidth = screenHeight * Screen.width / Screen.height;
        
        animator = GetComponentInChildren<Animator>(); 
        collider = GetComponent<BoxCollider2D>();
        // Randomize starting position and movement direction
        NewDirection();
        Teleport(false);
    }
	
    // Update is called once per frame
    void Update() {
        // Test if sprite is clicked
        if (Input.GetMouseButtonDown(0) && intersecting) {
            OnClick(); 
        }
        if (shouldMove && direction != null && !win){
            // Add the direction we're moving in to our position
            Vector2 newPosition = (Vector2)transform.position + (direction*Time.deltaTime);
            transform.position = newPosition;
            // bounce if on edge
            if (Mathf.Abs(transform.position.x) + collider.size.x/4 > screenWidth){
                direction.x *= -1;
            }
            if (Mathf.Abs(transform.position.y) + collider.size.y/4 > screenHeight){
                direction.y *= -1;
            }
        }
    }
    //OnTriggerStay2D doesn't work as well
    void OnTriggerEnter2D(Collider2D other){
        intersecting = true;
    }
    void OnTriggerExit2D(Collider2D other){
        intersecting = false;
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
            ParticleSystem particleSystem = fist.GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
            
            MicrogameController.instance.playSFX(
                knockSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(transform.position.x)
            );
            //NewDirection();
        }
    }
    
    // Move to a random location
    void Teleport(bool animate=true) {
        float newx = Random.Range(-screenWidth, screenWidth) / 2;
        float newy = Random.Range(-screenHeight, screenHeight) / 2;
        transform.position = new Vector2(newx, newy);
        if (animate) animator.SetTrigger("Clicked");
    }
    
    // Set a different direction
    void NewDirection() {
        float angle = Random.Range(0.0f, 2*Mathf.PI);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
    }

    // Winning animation
    void Win(){
        MicrogameController.instance.playSFX(
            openSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x)
        );
        animator.SetBool("Win", true);
    }
}
