using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimMaterialAnimation : MonoBehaviour {

    Renderer rnd;
    public Vector2 speed;
    float yMult = 1f;
    Vector2 offset;
    Vector2 scale;

    public void setMaterialYScaleMult(float yMult) => this.yMult = yMult;

    // Use this for initialization
    void Start () {
        rnd = GetComponent<Renderer>();
        scale = rnd.material.GetTextureScale("_MainTex");
        rnd.material.SetTextureScale("_MainTex", getMultipliedScale());
    }
	
	// Update is called once per frame
	void Update () {
        offset += speed * Time.deltaTime;
        rnd.material.SetTextureOffset("_MainTex", offset);
        rnd.material.SetTextureScale("_MainTex", getMultipliedScale());
    }

    Vector2 getMultipliedScale() => new Vector2(scale.x, scale.y * yMult);

}
