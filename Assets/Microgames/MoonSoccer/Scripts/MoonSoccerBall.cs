using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBall : MonoBehaviour {
    
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;

    
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf)
        {
            // Move to the right at the set movement speed
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
    
	}
    
    // Make this object active and set it's starting position
    public void activate (Vector2 position) 
    {
        transform.position = position;
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        gameObject.SetActive(true);
    }
    
    // Collision with other gameobjects
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "GoalZone")
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
        else
        {
            Destroy(gameObject);
            col.gameObject.catchBall();
            MicrogameController.instance.setVictory(victory: false, final: true);
            // todo: check what object was touched and change their sprite to reflect that
        }
    }
}
