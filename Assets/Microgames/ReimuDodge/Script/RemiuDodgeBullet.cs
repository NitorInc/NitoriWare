using UnityEngine;

public class RemiuDodgeBullet : MonoBehaviour {

    // Use this for initialization
    // A Unity in-editor variable
    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    // Stores the direction of travel for the bullet
    private Vector2 trajectory;

    [Header("How fast the bullet goes")]
    [SerializeField]

    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;
    // Use this for initialization
    void Start()
    {
        // Calculate a trajectory towards the target
        Invoke("SetTrajectory", delay);
    }

    // Update is called once per frame
    void Update ()
    {
        if (trajectory != null)
        {
            // Move bullet based on trajectory speed and time
            Vector2 newPos = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);

            transform.position = newPos;
        }
     
	}
    
    //this method gets the difference between the player and the bullet.
    void SetTrajectory()
    {
        trajectory = (target.transform.position - transform.position).normalized;
    }
}
