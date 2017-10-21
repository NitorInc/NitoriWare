using UnityEngine;

public class KyoukoEchoBodyPart : MonoBehaviour
{
    
    KyoukoEchoKyouko kyouko;
    Bounds bodyBounds;

    void Start()
    {
        this.kyouko = GetComponentInParent<KyoukoEchoKyouko>();
        this.bodyBounds = this.kyouko.GetComponent<Collider2D>().bounds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.kyouko.Hit(this.name);
    }

    // Get a hit location relative to the main body
    public float CalculateBodyHitLocationY(float positionY)
    {
        float locationOffset = positionY - this.kyouko.transform.position.y;
        return locationOffset / this.bodyBounds.size.y;
    }

}
