using UnityEngine;

/// <summary>
/// Observes ball params. Switches z index and triggers win if requirements are met
/// </summary>
public class BeachBallCollisionObserver : MonoBehaviour
{
    [SerializeField]
    private AudioClip victoryClip;

    protected Collider2D innerArea;

    protected Collider2D ballCollider;
    protected Rigidbody2D ballPhysics;

    protected bool fired = false;
    public bool Fired
    {
        get
        {
            return fired;
        }
        set
        {}
    }

    protected virtual void Start()
    {
        innerArea = GetComponent<Collider2D>();

        var ballGo = GameObject.Find("Ball");
        ballCollider = ballGo.GetComponent<CircleCollider2D>();
        ballPhysics = ballGo.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (!fired && ballPhysics.velocity.y < 0 && other == ballCollider
            && !MicrogameController.instance.getVictoryDetermined())
        {
            fired = true;
            other.gameObject.GetComponent<BeachBallZSwitcher>().Switch();

            MicrogameController.instance.setVictory(victory: true, final: true);
            MicrogameController.instance.playSFX(victoryClip);
        }
    }
}
