using UnityEngine;

/// <summary>
/// Observes ball params. Launches the ball backwards and triggers loss if requirements are met
/// </summary>
public class BeachBallBallLauncher : MonoBehaviour
{
    //[Header("Deprecated physics launch properties")]
    //public float ThrowForce = 500f;

    [Header("Launch equation arg (affects gravity scale and throw force)")]
    public float ThrowMultiplier = 8f;

    [Header("Launch equation arg (affects height)")]
    public float ThrowConstant = 442f;

    public AudioClip launchSound;

    public Vector2 ThrowDirection = new Vector2(0, 1);

    private Rigidbody2D physicsModel;
    private BeachBallScaler scaleMultiplier;

    private BoxCollider2D ballStandCollider;
    private Animation sealAnimation;

    private bool launched = false;
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
        sealAnimation = GameObject.Find("Seal").GetComponent<Animation>();
    }

    void Update()
    {
        if (!launched && Input.GetKeyDown(KeyCode.Space))
        {
            launched = true;
            //start scaling
            scaleMultiplier.Started = true;
            //animate the seal
            sealAnimation.Play();

            //throw the ball using physics
            //ThrowForce = ThrowConstant * Sqrt(ThrowMultiplier) (obtained using power curve fitting)
            physicsModel.gravityScale = ThrowMultiplier;
            physicsModel.AddForce(ThrowDirection.normalized *
                (float)System.Math.Sqrt(ThrowMultiplier) * ThrowConstant);

            MicrogameController.instance.playSFX(launchSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(transform.position.x));
        }
    }
}
