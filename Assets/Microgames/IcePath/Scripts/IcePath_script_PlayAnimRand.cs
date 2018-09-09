using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_PlayAnimRand : MonoBehaviour {

    Animator anim;

    void Start() {
        // Set
        anim = GetComponent<Animator>();
        anim.Play("Undulate", 0, Random.Range(0, 10)/ 10);
    }

    int rand(float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }

}
