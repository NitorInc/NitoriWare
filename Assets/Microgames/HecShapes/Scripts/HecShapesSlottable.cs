using UnityEngine;
using UnityEngine.Events;

public class HecShapesSlottable : MonoBehaviour
{

    public enum Shape { none=0, circle=1, square=2, triangle=3 }
    
    [Header("How quickly the planet snaps into the holder")]
    [SerializeField]
    float snapSpeed = 10;
    
    public HecShapesHolder snapTarget;
    public bool snap;

    public AudioClip pickupSound;

    MouseGrabbable grabbable;
    HecShapesCelestialBody celestialBody;

    void Start()
    {
        this.grabbable = GetComponent<MouseGrabbable>();
        this.celestialBody = GetComponentInChildren<HecShapesCelestialBody>();

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
        if (this.snap)
        {
            float distance = Time.deltaTime * this.snapSpeed;
            this.transform.position = Vector2.MoveTowards(
                this.transform.position,
                this.snapTarget.SnapPosition,
                distance);
            
            if ((Vector2)this.transform.position == this.snapTarget.SnapPosition)
            {
                this.snapTarget.ShapeInSlot = this.celestialBody.shape;
                this.snap = false;
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
            this.snapTarget = null;
    }

    void AcquireTarget(Collider2D collision)
    {
        HecShapesHolder holder = collision.GetComponentInParent<HecShapesHolder>();
        if (holder && !holder.Filled)
            this.snapTarget = holder;
    }
    
    public void OnGrab()
    {
        this.snap = false;
        if (this.snapTarget && this.snapTarget.ShapeInSlot == this.celestialBody.shape)
            this.snapTarget.ShapeInSlot = Shape.none;

        MicrogameController.instance.playSFX(this.pickupSound, pitchMult: 1F, volume: 0.4F);
    }

    public void OnRelease()
    {
        if (this.snapTarget && this.snapTarget.SlotShape == this.celestialBody.shape)
            this.snap = true;

        MicrogameController.instance.playSFX(this.pickupSound, pitchMult: 1.2F, volume: 0.4F);
    }

}
