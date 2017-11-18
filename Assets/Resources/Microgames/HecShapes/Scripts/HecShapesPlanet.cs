using UnityEngine;

public class HecShapesPlanet : MonoBehaviour
{

    [Header("How quickly the planet snaps into the holder")]
    [SerializeField]
    float snapSpeed;

    Transform snapTarget;

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
        if (collision.GetComponentInParent<HecShapesHolder>())
            this.snapTarget = collision.transform;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<HecShapesHolder>())
            this.snapTarget = null;
    }

    public void OnGrab()
    {
        print("Grab");
    }

    public void OnRelease()
    {
        print("Release");
    }

}
