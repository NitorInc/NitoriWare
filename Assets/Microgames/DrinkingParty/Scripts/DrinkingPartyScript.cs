using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingPartyScript : MonoBehaviour
{
    public GameObject ObjectLiquide1;
    public float scale = 2.0f;

    // Use this for initialization
    void Start()
    {
        ObjectLiquide1 = GameObject.Find("/Liquide1");
        //ObjectLiquide1 = GameObject.Find("/Cara_1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space key was pressed");
            ObjectLiquide1.transform.localScale = new Vector3(0.09395978f, 0.20f,1f);
            //SpriteRenderer spriteRenderer = ObjectLiquide1.GetComponent<SpriteRenderer>();
            //spriteRenderer.gameObject.SetActive(false);
        }
    }
}

//ObjectLiquide1.transform.Translate(1, 1, 1); Use for translate the sprite