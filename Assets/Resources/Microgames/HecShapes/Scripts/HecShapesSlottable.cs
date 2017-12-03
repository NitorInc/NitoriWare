using UnityEngine;

public class HecShapesSlottable : MonoBehaviour
{

    public enum Shape { circle=1, square=2, triangle=3 }

    public bool correct = false;

    [Header("How quickly the planet snaps into the holder")]
    [SerializeField]
    float snapSpeed = 10;
    
    HecShapesHolder snapTarget;
    
    void Update()
    {
        if (this.snapTarget != null)
        {
            float distance = Time.deltaTime * this.snapSpeed;
            this.transform.position = Vector2.MoveTowards(
                this.transform.position,
                this.snapTarget.transform.position,
                distance);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HecShapesHolder holder = collision.GetComponentInParent<HecShapesHolder>();
        if (holder)
            this.snapTarget = holder;
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
        if (snapTarget)
        {
            // Send message to Holder
            snapTarget.SendMessage("FillSlot", this.correct);
        }
    }

}
