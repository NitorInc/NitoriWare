using UnityEngine;

public class KyoukoEchoNoiseZone : MonoBehaviour
{

    [SerializeField]
    KyoukoEchoNoise noise;
    [SerializeField]
    KyoukoEchoNoisePair[] noisePairs;

    Collider2D zone;
    KyoukoEchoKyouko kyouko;

	[SerializeField]
    int noiseCount = 3;
	[SerializeField]
    float delay = 1;

    void Start()
    {
        zone = GetComponent<Collider2D>();
        kyouko = FindObjectOfType<KyoukoEchoKyouko>();

        for (int i = 0; i < noiseCount; i++)
        {
            MakeNoise(delay * i);
            kyouko.IncrementEchoCount();
        }
    }
    
    void MakeNoise(float delay)
    {
        // Calculate random start position
        Bounds bounds = zone.bounds;
        Vector2 noisePosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));

        KyoukoEchoNoisePair noisePair = noisePairs[Random.Range(0, noisePairs.Length)];
        
        KyoukoEchoNoise noise = Instantiate(this.noise, noisePosition, transform.rotation);
        noise.SetNoise(noisePair);
        noise.Fire(kyouko.transform.position.x, kyouko.UpperBoundY, kyouko.LowerBoundY, delay);
    }

}
