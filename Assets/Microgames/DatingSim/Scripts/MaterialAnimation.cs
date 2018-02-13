using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimation : MonoBehaviour {

    Renderer rnd;
    public Vector2 speed;
    Vector2 offset;
	// Use this for initialization
	void Start () {
        rnd = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        offset += speed * Time.deltaTime;
        rnd.material.SetTextureOffset("_MainTex", offset);
	}
}
