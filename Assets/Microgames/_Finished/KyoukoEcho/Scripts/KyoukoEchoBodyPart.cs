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
        KyoukoEchoNoise noise = other.GetComponent<KyoukoEchoNoise>();
        if (noise && noise.CanEcho() && this.kyouko.WillEcho)
        {
            noise.Echo(CalculateBodyHitLocationY(noise.transform.position.y));

            this.kyouko.Hit(this.name);
        }
    }

    // Get a hit location relative to the main body
    public float CalculateBodyHitLocationY(float positionY)
    {
        float locationOffset = positionY - this.kyouko.transform.position.y;
        return locationOffset / this.bodyBounds.size.y;
    }

}
