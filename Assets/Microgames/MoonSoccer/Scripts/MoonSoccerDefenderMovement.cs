using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerDefenderMovement : MonoBehaviour {
    
    // Contain all the information relating to the object's movement
    [System.Serializable]
    public struct Layout 
    {
        public float moveSpeed;
        public float startVerticalPosition;
        public bool startsDownward;
    }
    
    // Each object has 3 layout structs, and one will be picked at random at the start
    [Header("Movement Layouts")]
    [SerializeField]
    public Layout layout1;
    [SerializeField]
    public Layout layout2;
    [SerializeField]
    public Layout layout3;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("Horizontal Movement Length")]
    [SerializeField]
    private float xMovement = 1f;
    
    // The change in scale that happens when going all the way up the screen, in percentage
    [Header("Scale Change From Perspective")]
    [SerializeField]
    private float scaleChange = 5f;
    
    
    // The upper, lower and middle bounds of the vertical movement 
    // Which are used depends on the values of the VerticalMovementRange enum
    [Header("Vertical Movement Range")]
    [SerializeField]
    private float TopY = 1.5f;
    private float BottomY = -2.9f;
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The starting x value of the object transform
    private float startX = 0f;
    
    private bool downward;
    
    private Layout chosenLayout;
    
    // The scale of the sprite at the very start
    private Vector3 startScale;
    
    // Initialization
    void Start () {
        // Get what the chosen layout is
        int layout = GameObject.Find("LayoutPicker").GetComponent<MoonSoccerLayoutPick>().layout;
        switch (layout)
        {
            case 0:
                chosenLayout = layout1;
                break;
            case 1:
                chosenLayout = layout2;
                break;
            case 2:
                chosenLayout = layout3;
                break;
        }
		transform.position = new Vector3(transform.position.x, chosenLayout.startVerticalPosition, transform.position.z);
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
        startScale = transform.localScale;
        downward = chosenLayout.startsDownward;
    }

    
	// Update the object's position by moving it vertically according to moveSpeed. X value is updated based on the y position
	void Update () {
        if (MicrogameController.instance.getVictoryDetermined() != true) {
            float x = transform.position.x;
            float y = transform.position.y;
            if (downward == true)
            {
                if (transform.position.y >= BottomY)
                    y = transform.position.y - chosenLayout.moveSpeed * Time.deltaTime;
                else
                    downward = false;
            }
            else
            {
                if (transform.position.y <= TopY)
                    y = transform.position.y + chosenLayout.moveSpeed * Time.deltaTime;
                else
                    downward = true;
            }
            // Horizontal Movement based on how high they are on screen
            x = -((y - BottomY) / moveDistance) * xMovement;
            transform.position = new Vector3(startX + x, y, transform.position.z);
            // Scale the character's size based on how high they are on screen
            float vDistance = 1 - ((y - BottomY) / moveDistance);
            transform.localScale = new Vector3((startScale.x / 100f) * (100-scaleChange + vDistance*scaleChange), 
                                               (startScale.y / 100f) * (100-scaleChange + vDistance*scaleChange), 
                                               startScale.z);
        }
	}
}