using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //The direction of the character
    float xspeed = 1;
    float yspeed = 1;

    public Collider2D colliderPerson;
    public GameObject colliderWallTop;
    public GameObject colliderWallBottom;
    public GameObject colliderWallLeft;
    public GameObject colliderWallRight;

    void Start ()
    {
        float randomx = Random.Range(0f, 10f);
        float randomy = Random.Range(0f, 10f);
        xspeed = Random.Range(2f, 4f);
        yspeed = Random.Range(2f, 4f);
        if(randomx >= 5)
        {
            xspeed *= -1;
        }
        if (randomy >= 5)
        {
            yspeed *= -1;
        }
    }

    // Update is called once per frame
    void Update () {
        float newxvalue = transform.position.x + (xspeed * Time.deltaTime);
        float newyvalue = transform.position.y + (yspeed * Time.deltaTime);
        transform.position = new Vector3(newxvalue, newyvalue, transform.position.z);
        if (colliderPerson.IsTouching(colliderWallTop.GetComponent<Collider2D>()) || transform.position.y >= 4)
        {
            yspeed *= -1;
        }
        if (colliderPerson.IsTouching(colliderWallRight.GetComponent<Collider2D>()) || transform.position.x >= 6.3)
        {
            xspeed *= -1;
        }
        if (colliderPerson.IsTouching(colliderWallBottom.GetComponent<Collider2D>()) || transform.position.y <= -4.3)
        {
            yspeed *= -1;
        }
        if (colliderPerson.IsTouching(colliderWallLeft.GetComponent<Collider2D>()) || transform.position.x <= -6.3)
        {
            xspeed *= -1;
        }
    }
}
