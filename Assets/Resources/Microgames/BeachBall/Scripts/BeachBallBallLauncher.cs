using UnityEngine;

public class BeachBallBallLauncher : MonoBehaviour
{
    public float ThrowForce = 500f;
    public Vector2 ThrowDirection = new Vector2(0, 1);

    private bool launched = false;
    private Rigidbody2D physicsModel;
    private BeachBallScaler scaleMultiplier;

    private BoxCollider2D ballStandCollider;

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
            scaleMultiplier.Started = true;
            physicsModel.AddForce(ThrowDirection.normalized * ThrowForce);
            ballStandCollider.isTrigger = true; 
        }
    }
}
