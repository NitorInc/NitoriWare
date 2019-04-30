using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutDotLine : MonoBehaviour
{

    [Header("Minimum x position of dotted line")]
    [SerializeField]
    private float minX = -3f;

    [Header("Maximum x position of dotted line")]
    [SerializeField]
    private float maxX = 3f;

    private int lineNum;
    // Use this for initialization
    void Start()
    {
        transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
        if(gameObject.tag == "MicrogameTag1")
        {
            lineNum = 1;
        } else if(gameObject.tag == "MicrogameTag2")
        {
            lineNum = 2;
        } else if(gameObject.tag == "MicrogameTag3")
        {
            lineNum = 3;
        }
    }

    private void Update()
    {   
        switch (lineNum)
        {
            case 1:
                if (GameObject.FindGameObjectWithTag("MicrogameTag2") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag2").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                if (GameObject.FindGameObjectWithTag("MicrogameTag3") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag3").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                break;
            case 2:
                if (GameObject.FindGameObjectWithTag("MicrogameTag1") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag1").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                if (GameObject.FindGameObjectWithTag("MicrogameTag3") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag3").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                break;
            case 3:
                if (GameObject.FindGameObjectWithTag("MicrogameTag1") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag1").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                if (GameObject.FindGameObjectWithTag("MicrogameTag2") != null && gameObject.GetComponent<Collider2D>().bounds.Intersects(GameObject.FindGameObjectWithTag("MicrogameTag2").GetComponent<Collider2D>().bounds))
                {
                    Debug.Log("Line Moved");
                    transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                }
                break;
            default:
                break;

        }
    }
}