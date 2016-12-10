using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChenQuestionBehavior : MonoBehaviour {

    public Animator questionAnimator;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        questionAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        collide(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        collide(other);
    }

    void collide(Collider2D other)
    {
        if (other.name == "ChenHonk")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            questionAnimator.Play("QuestionAnimation");
        }
    }
}
