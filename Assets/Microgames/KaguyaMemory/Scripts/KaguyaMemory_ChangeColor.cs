using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ChangeColor : MonoBehaviour {

    public Vector3 RGB;
    public float alpha = 1;

    public void InitiateChange()
    {
        GetComponent<SpriteRenderer>().color = new Color(RGB.x, RGB.y, RGB.z, alpha);
        if (alpha == 0)
        {
            Destroy(gameObject);
        }
    }
}
