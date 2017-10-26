using UnityEngine;

public class KyoukoEchoMissZone : MonoBehaviour
{

    KyoukoEchoKyouko kyouko;

    void Start()
    {
        this.kyouko = FindObjectOfType<KyoukoEchoKyouko>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        KyoukoEchoNoise noise = other.GetComponent<KyoukoEchoNoise>();
        if (noise && noise.CanEcho())
        {
            this.kyouko.Miss();
        }
    }

}
