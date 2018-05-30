using UnityEngine;

public class KyoukoEchoMissZone : MonoBehaviour
{

    KyoukoEchoKyouko kyouko;

    void Start()
    {
        kyouko = FindObjectOfType<KyoukoEchoKyouko>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        KyoukoEchoNoise noise = other.GetComponent<KyoukoEchoNoise>();
        if (noise && noise.CanEcho())
        {
            kyouko.Miss();
        }
    }

}
