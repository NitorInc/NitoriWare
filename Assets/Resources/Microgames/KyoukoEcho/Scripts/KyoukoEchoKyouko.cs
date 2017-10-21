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

    // Use this for initialization
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
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
