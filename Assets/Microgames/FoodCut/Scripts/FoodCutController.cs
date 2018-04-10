using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutController : MonoBehaviour {

    [Header("How fast the knife moves")]
    [SerializeField]
    private float speed = 1f;

    [Header("Minimum x position the knife can reach")]
    [SerializeField]
    private float minX = -5f;

    [Header("Maximum x position the knife can reach")]
    [SerializeField]
    private float maxX = 5f;

    [Header("Number of cuts needed")]
    [SerializeField]
    private int cutsNeeded = 1;

    private Collider2D knifeCollider;
    private int cutCount = 0;
    private Collider2D currentTrigger = null;


    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Movement
        transform.Translate(Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed, 0f, 0f);

        // Restrict Movement
        if (transform.position.x <= minX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
        else if (transform.position.x >= maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentTrigger != null)
        {
            cutCount++;
            Debug.Log("Object Cut");
            Destroy(currentTrigger.gameObject);
            currentTrigger = null;
        }

        if (cutCount == cutsNeeded)
            MicrogameController.instance.setVictory(victory: true, final: true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentTrigger = other;
        Debug.Log(currentTrigger);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        currentTrigger = other;
        Debug.Log(currentTrigger);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentTrigger = null;
        Debug.Log(currentTrigger);
    }
}
