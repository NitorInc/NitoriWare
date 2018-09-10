using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingPartyScript : MonoBehaviour
{
    private float ScaleLose = 0.1f;
    public GameObject ObjectLiquide1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space key was pressed");
            ObjectLiquide1 = GameObject.Find("/Liquide1");
            SpriteRenderer spriteRenderer = ObjectLiquide1.GetComponent<SpriteRenderer>();
            spriteRenderer.gameObject.SetActive(false);
        }
    }
}
