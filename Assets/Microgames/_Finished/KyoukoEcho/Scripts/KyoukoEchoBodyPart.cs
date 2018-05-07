using UnityEngine;

public class KyoukoEchoBodyPart : MonoBehaviour
{
    
    KyoukoEchoKyouko kyouko;
    Bounds bodyBounds;
    
    void Start()
    {
        kyouko = GetComponentInParent<KyoukoEchoKyouko>();
        bodyBounds = kyouko.GetComponent<Collider2D>().bounds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        KyoukoEchoNoise noise = other.GetComponent<KyoukoEchoNoise>();
        if (noise && noise.CanEcho() && kyouko.WillEcho)
        {
            noise.Echo(CalculateBodyHitLocationY(noise.transform.position.y));

            kyouko.Hit(name);
        }
    }

    // Get a hit location relative to the main body
    public float CalculateBodyHitLocationY(float positionY)
    {
        float locationOffset = positionY - kyouko.transform.position.y;
        return locationOffset / bodyBounds.size.y;
    }

}
