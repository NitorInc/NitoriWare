using UnityEngine;

/// <summary>
/// Changes z index
/// </summary>
public class BeachBallZSwitcher : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BeachBallBallLauncher launcher;

    private bool switched;
    public bool lockZSwitch = false;
    private float ballThrowSpeed = 1f;

    [Header("Z index switch threshold")]
    //When y value of velocity goes past this value object 
    //switches its z index (only if haven't switched before)
    public float velocityThreshold = 0.2f;

    void Start()
    {
        ballThrowSpeed = GameObject.Find("Ball")
            .GetComponent<BeachBallBallLauncher>().ThrowMultiplier;
        rigidBody = GetComponent<Rigidbody2D>();
        launcher = GetComponent<BeachBallBallLauncher>();
    }

    void Update()
    {
        if (!switched && launcher.Launched && rigidBody.velocity.y < -velocityThreshold * ballThrowSpeed
            * (1 / Time.timeScale))
            Switch();
    }

    public void Switch()
    {
        if (lockZSwitch)
            return;
        transform.position += new Vector3(0, 0, 2);
        switched = true;
    }
    public void Revert()
    {
        lockZSwitch = true;
        transform.position -= 2 * new Vector3(0, 0, 2);
    }
}
