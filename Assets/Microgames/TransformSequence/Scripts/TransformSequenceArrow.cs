using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSequenceArrow : MonoBehaviour {

    public float speed, scale, damping, failTime;
    public KeyCode key;
    private SpriteRenderer sr;
    private bool success = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update () {
        transform.position += transform.up * speed;
        transform.localScale *= 1+scale;
        speed *= damping;
        scale *= damping;

        if (speed < 0.1 && !success)
        {
            sr.color = Color.blue;
            failTime -= Time.deltaTime;
            if (failTime < 0)
            {
                sr.color = Color.red;
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
            else if (Input.GetKeyDown(key))
            {
                sr.color = Color.green;
                success = true;
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
