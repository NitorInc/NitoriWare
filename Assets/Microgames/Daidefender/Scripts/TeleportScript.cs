using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour {

    [SerializeField]
    private GameObject Up;

    [SerializeField]
    private GameObject Down;

    [SerializeField]
    private GameObject Left;

    [SerializeField]
    private GameObject Right;

    // Update is called once per frame
    void Update()
    {
        //Appear up
        if (Input.GetKey("w"))
        {
            Debug.Log("Active");
            Up.gameObject.SetActive(true);
            Down.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
            Left.gameObject.SetActive(false);
        }
        else
        {
            Up.gameObject.SetActive(false);
        }

        //Appear Down
        if (Input.GetKey("s"))
        {
            Debug.Log("Active");
            Down.gameObject.SetActive(true);
            Up.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
            Left.gameObject.SetActive(false);
        }
        else
        {
            Down.gameObject.SetActive(false);
        }

        //Appear Right
        if (Input.GetKey("d"))
        {
            Debug.Log("Active");
            Right.gameObject.SetActive(true);
            Up.gameObject.SetActive(false);
            Down.gameObject.SetActive(false);
            Left.gameObject.SetActive(false);
        }
        else
        {
            Right.gameObject.SetActive(false);
        }

        //Appear Left
        if (Input.GetKey("a"))
        {
            Debug.Log("Active");
            Left.gameObject.SetActive(true);
            Up.gameObject.SetActive(false);
            Down.gameObject.SetActive(false);
            Right.gameObject.SetActive(false);
        }
        else
        {
            Left.gameObject.SetActive(false);
        }

    } 

}
