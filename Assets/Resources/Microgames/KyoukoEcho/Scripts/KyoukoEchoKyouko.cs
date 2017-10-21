using System.Collections.Generic;
using UnityEngine;

public class KyoukoEchoKyouko : MonoBehaviour
{

    [SerializeField]
    float speed = 10F;

    [SerializeField]
    float boundTop;
    [SerializeField]
    float boundBottom;

    Rigidbody2D rigidBody;
    Animator animator;
    
    // Use this for initialization
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow))
            velocity.y = speed;
        else if (Input.GetKey(KeyCode.DownArrow))
            velocity.y = -speed;

        rigidBody.velocity = velocity;

        Vector2 currentPosition = this.transform.position;
        if (currentPosition.y > boundTop)
            currentPosition.y = boundTop;
        else if (currentPosition.y < boundBottom)
            currentPosition.y = boundBottom;
        
        transform.position = currentPosition;
    }

    public void Hit(string partName)
    {
        // Body parts share names with animations
        this.animator.SetTrigger(partName);
    }

    public void Miss()
    {
        this.animator.SetTrigger("Miss");
    }

    public float BoundTop
    {
        get
        {
            return this.boundTop;
        }
    }

    public float BoundBottom
    {
        get
        {
            return this.boundBottom;
        }
    }

}
