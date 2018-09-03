using UnityEngine;

/// <summary>
/// Observes ball params. Launches the ball backwards and triggers loss if requirements are met
/// </summary>
public class BeachBallBallLauncher : MonoBehaviour
{
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

    public BeachBallHoopParamsRandomizer hoopAnimationControl;

    public bool Launched { get; private set; }

    void Start()
    {
        physicsModel = GetComponent<Rigidbody2D>();
        scaleMultiplier = GetComponent<BeachBallScaler>();
        sealAnimation = GameObject.Find("Seal").GetComponent<Animation>();
        hoopAnimationControl = FindObjectOfType<BeachBallHoopParamsRandomizer>();
    }

    void Update()
    {
        if (!Launched && Input.GetKeyDown(KeyCode.Space))
        {
            Launched = true;
            //start scaling
            scaleMultiplier.Started = true;
            //animate the seal
            sealAnimation.Play();

            //throw the ball using physics
            physicsModel.gravityScale = ThrowMultiplier;
            physicsModel.AddForce(ThrowDirection.normalized *
                (float)System.Math.Sqrt(ThrowMultiplier) * ThrowConstant);

            hoopAnimationControl.onToss();

            MicrogameController.instance.playSFX(launchSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(transform.position.x));
        }
    }
}
