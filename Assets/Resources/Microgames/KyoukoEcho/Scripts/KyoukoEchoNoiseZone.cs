using UnityEngine;

public class KyoukoEchoNoiseZone : MonoBehaviour
{

    [SerializeField]
    KyoukoEchoNoise noise;

    Collider2D zone;
    KyoukoEchoKyouko kyouko;

	[SerializeField]
    int noiseCount = 3;
	[SerializeField]
    float delay = 1;

    void Start()
    {
        this.zone = GetComponent<Collider2D>();
        this.kyouko = FindObjectOfType<KyoukoEchoKyouko>();

        for (int i = 0; i < this.noiseCount; i++)
        {
            MakeNoise(this.delay * i);
            this.kyouko.IncrementEchoCount();
        }
    }
    
    void MakeNoise(float delay)
    {
        // Calculate random start position
        Bounds bounds = this.zone.bounds;
        Vector2 noisePosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));

        KyoukoEchoNoise noise = Instantiate(this.noise, noisePosition, this.transform.rotation);
        noise.Fire(this.kyouko.transform.position.x, this.kyouko.UpperBoundY, this.kyouko.LowerBoundY, delay);
    }

}
