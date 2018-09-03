using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBall : MonoBehaviour {
    
    // A Unity in-editor variable
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;

    
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
    
	}
    
    
    public void activate (Vector2 position) 
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "GoalZone")
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
    }
}
