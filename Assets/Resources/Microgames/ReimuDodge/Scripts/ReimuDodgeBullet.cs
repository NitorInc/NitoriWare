using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{
    [Header("How fast the bullet goes")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    private Vector2 trajectory = new Vector2(0, 0);
    private GameObject player;

    void Start()
    {
        Invoke("SetTrajectory", delay);
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + (trajectory * Time.deltaTime);
    }

    void SetTrajectory()
    {
        GameObject player = GameObject.Find("Player");
        trajectory = (player.transform.position - transform.position).normalized;
    }
}
