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
        this.kyouko.Miss();
    }

}
