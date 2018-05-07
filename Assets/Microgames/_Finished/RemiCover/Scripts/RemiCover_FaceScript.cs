using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_FaceScript : MonoBehaviour {

    private Animator animator;
    private int minFace = 0;
    private int maxFace = 2;
    public float faceSwapTimer;
    private float minSwapTimer = 0.25f;
    private float maxSwapTimer = 1;
    

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (animator)
        {
            if (faceSwapTimer <= 0)
            {
                var value = Random.Range(minFace, maxFace + 1);
                animator.SetInteger("SelectedFace", value);
                faceSwapTimer = Random.Range(minSwapTimer, maxSwapTimer);
            }

            else
            {
                faceSwapTimer -= Time.deltaTime;
            }

        }
	}

}
