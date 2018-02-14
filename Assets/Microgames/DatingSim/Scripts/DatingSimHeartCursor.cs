using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.DatingSim {
    public class DatingSimHeartCursor : MonoBehaviour {

        Animator anim;
        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            DatingSimOptionController.OnLosing += PlayOnFail;
        }

        void PlayOnFail() {
            if (gameObject.activeInHierarchy)
                anim.Play("Break", 0, 0.0f);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}