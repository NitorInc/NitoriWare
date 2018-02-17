using UnityEngine;

/// <summary>
/// Changes z index
/// </summary>
public class BeachBallZSwitcher : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BeachBallBallLauncher launcher;

    private bool switched;

    [Header("Z index switch treshold")]
    //When y value of velocity goes past this value object 
    //switches its z index (only if haven't switched before)
    public float velocityTreshold = 0.2f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        launcher = GetComponent<BeachBallBallLauncher>();
    }

    void Update()
    {
        if (!switched && launcher.Launched && rigidBody.velocity.y < -velocityTreshold
            * (1 / Time.timeScale))
            Switch();
    }

    public void Switch()
    {
        transform.position += new Vector3(0, 0, 2);
        switched = true;
    }
    public void Revert()
    {
        transform.position -= 2 * new Vector3(0, 0, 2);
    }
}
