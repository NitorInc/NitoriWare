using UnityEngine;
using UnityEngine.Events;

public class HecShapesSlottable : MonoBehaviour
{

    public enum Shape { circle=1, square=2, triangle=3 }

    public bool correct = false;

    [Header("How quickly the planet snaps into the holder")]
    [SerializeField]
    float snapSpeed = 10;
    
    Transform snapTarget;
    MouseGrabbable grabbable;

    void Start()
    {
        this.grabbable = GetComponent<MouseGrabbable>();

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
        if (this.snapTarget != null)
        {
            float distance = Time.deltaTime * this.snapSpeed;
            this.transform.position = Vector2.MoveTowards(
                this.transform.position,
                this.snapTarget.position,
                distance);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HecShapesHolder holder = collision.GetComponentInParent<HecShapesHolder>();
        if (holder)
            this.snapTarget = collision.transform;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<HecShapesHolder>())
            this.snapTarget = null;
    }
    
    public void OnGrab()
    {
        
    }

    public void OnRelease()
    {
        if (this.snapTarget)
        {
            // Send message to Holder
            var holder = this.snapTarget.GetComponentInParent<HecShapesHolder>();
            holder.SendMessage("FillSlot", this.correct);
        }
    }

}
