using UnityEngine;

public class ReimuDodgeBulletSpawn : MonoBehaviour
{
    [Header("Speed range")]
    [SerializeField]
    private float bulletSpeedMin;
    [SerializeField]
    private float bulletSpeedMax;

    [Header("Spawn delay range")]
    [SerializeField]
    private float bulletDelayMin;
    [SerializeField]
    private float bulletDelayMax;

    [Header("Spawn radius range")]
    [SerializeField]
    private float spawnRadiusMin;
    [SerializeField]
    private float spawnRadiusMax;

    [Header("Bullet count range")]
    [SerializeField]
    private int bulletCountMin;
    [SerializeField]
    private int bulletCountMax;

    [Header("Bullet prefab")]
    [SerializeField]
    public GameObject bullet;

    [Header("Scaled up bullet prefab")]
    [SerializeField]
    public GameObject greaterBullet;

    [Header("Scaled up bullet spawn chance")]
    [SerializeField]
    public float spawnChance;

    void Start()
    {
        SpawnBullets();
    }

    void Update()
    {
    }

    private void SpawnBullets()
    {
        if (bullet == null)
        {
            Debug.Log("Bullet prefab not provided");
            return;
        }
        Vector2 center = transform.position;
        int bulletCount = Random.Range(bulletCountMin, bulletCountMax);
        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 pos = RandomCircle();
            GameObject instance = Instantiate(bullet, pos, Quaternion.identity);
            ReimuDodgeBullet bulletComponent = instance.GetComponent<ReimuDodgeBullet>();
            bulletComponent.speed = Random.Range(bulletSpeedMin, bulletSpeedMax);
            bulletComponent.delay = Random.Range(bulletDelayMin, bulletDelayMax);
            if (Random.Range(0f, 1f) < spawnChance)
            {
                instance.transform.localScale = 2 * instance.transform.localScale;
            }
        }

    }

    private Vector2 RandomCircle()
    {
        float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);
        float ang = Random.value * 360;
        return new Vector2(transform.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad),
            transform.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
    }
}
