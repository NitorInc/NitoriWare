using UnityEngine;
using UnityEngine.Events;

public class HecShapesSlottable : MonoBehaviour
{

    public enum Shape { none=0, circle=1, square=2, triangle=3 }
    
    [Header("How quickly the planet snaps into the holder")]
    [SerializeField]
    float snapSpeed = 10;
    [SerializeField]
    private AudioClip snapClip;
    
    public HecShapesHolder snapTarget;
    public bool snap;
    
    MouseGrabbable grabbable;
    HecShapesCelestialBody celestialBody;
    AudioSource[] audioSources;
    Vibrate vibration;

    void Start()
    {
        grabbable = GetComponent<MouseGrabbable>();
        celestialBody = GetComponentInChildren<HecShapesCelestialBody>();
        audioSources = GetComponents<AudioSource>();
        vibration = GetComponent<Vibrate>();

        if (grabbable)
        {
            var grabEvent = new UnityEvent();
            grabEvent.AddListener(OnGrab);
            grabbable.onGrab = grabEvent;

            var releaseEvent = new UnityEvent();
            releaseEvent.AddListener(OnRelease);
            grabbable.onRelease = releaseEvent;
        }
    }

    void Update()
    {
        if (snap)
        {
            float distance = Time.deltaTime * snapSpeed;
            if (snapTarget != null)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    snapTarget.SnapPosition,
                    distance);

                if ((Vector2)transform.position == snapTarget.SnapPosition)
                {
                    transform.SetParent(snapTarget.transform);
                    snapTarget.ShapeInSlot = celestialBody.shape;
                    snap = false;
                    MicrogameController.instance.playSFX(snapClip, AudioHelper.getAudioPan(transform.position.x));
                }
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        AcquireTarget(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<HecShapesHolder>())
            snapTarget = null;
    }

    void AcquireTarget(Collider2D collision)
    {
        HecShapesHolder holder = collision.GetComponentInParent<HecShapesHolder>();
        if (holder && !holder.Filled)
            snapTarget = holder;
    }
    
    public void OnGrab()
    {
        snap = false;
        if (snapTarget && snapTarget.ShapeInSlot == celestialBody.shape)
            snapTarget.ShapeInSlot = Shape.none;

        audioSources[0].Play();
        celestialBody.Enlarge();

        vibration.vibrateOn = false;
    }

    public void OnRelease()
    {
        if (snapTarget && snapTarget.SlotShape == celestialBody.shape)
        {
            snap = true;
        }
        else
        {
            vibration.vibrateOn = true;
        }

        audioSources[1].Play();
        celestialBody.ResetSize();
    }

}
