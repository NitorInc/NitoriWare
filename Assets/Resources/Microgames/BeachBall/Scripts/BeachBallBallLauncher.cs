using UnityEngine;

/// <summary>
/// Observes ball params. Launches the ball backwards and triggers loss if requirements are met
/// </summary>
public class BeachBallBallLauncher : MonoBehaviour
{
    public float ThrowForce = 500f;
    public Vector2 ThrowDirection = new Vector2(0, 1);

    private bool launched = false;
    private Rigidbody2D physicsModel;
    private BeachBallScaler scaleMultiplier;

    private BoxCollider2D ballStandCollider;

    public bool Launched
    {
        get
        {
            return launched;
        }
        set
        { }
    }

    void Start()
    {
        physicsModel = GetComponent<Rigidbody2D>();
        scaleMultiplier = GetComponent<BeachBallScaler>();
        ballStandCollider = GameObject.Find("BallStand").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!launched && Input.GetKeyDown(KeyCode.Space))
        {
            launched = true;
            //start scaling
            scaleMultiplier.Started = true;
            //throw the ball
            physicsModel.AddForce(ThrowDirection.normalized * ThrowForce);
            //set triggerMode to prevent collisions when the ball falls
            ballStandCollider.isTrigger = true; 
        }
    }
}
