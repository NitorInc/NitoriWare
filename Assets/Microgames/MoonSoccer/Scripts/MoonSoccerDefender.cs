using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerDefender : MonoBehaviour {

    // Each object has 3 layout structs, and one will be picked at random at the start
    [Header("Movement Layouts")]
    [SerializeField]
    private float position1;
    [SerializeField]
    private float position2;
    [SerializeField]
    private float position3;
		
	private MoonSoccerBall ballScript;
    
	
	
    void Start () {
        // Get what the chosen layout is
        int layout = GameObject.Find("LayoutPicker").GetComponent<MoonSoccerLayoutPick>().layout;
        switch (layout)
        {
            case 0:
				transform.position = new Vector3(transform.position.x, position1, transform.position.z);
                break;
            case 1:
				transform.position = new Vector3(transform.position.x, position2, transform.position.z);
                break;
            case 2:
				transform.position = new Vector3(transform.position.x, position3, transform.position.z);
                break;
        }
	
	}
	
	
	
	
    // Check for collision againt the character which causes the minigame to fail and a ball animation to play out
    void OnTriggerEnter2D(Collider2D col) 
    {
		
        if (col.gameObject.name == "Ball" && !MicrogameController.instance.getVictoryDetermined())
        {
			ballScript = col.gameObject.GetComponent<MoonSoccerBall>();
            // When the ball hits Kaguya it is destroyed
            if (gameObject.name == "Kaguya")
            { 
                Destroy(col.gameObject);
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball anyone else
            } else { 
                ballScript.moveSpeed = -3;
                col.gameObject.GetComponentInChildren<Animator>().Play("MoonSoccerBallBounce");
                MicrogameController.instance.setVictory(victory: false, final: true);
			}
        }
    }
}

