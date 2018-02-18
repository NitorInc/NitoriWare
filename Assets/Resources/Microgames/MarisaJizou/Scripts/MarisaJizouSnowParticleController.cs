using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {
    public class MarisaJizouSnowParticleController : MonoBehaviour {

        bool isLeft = true;
        int leftHash;
        int rightHash;
        Animator anim;
        ParticleSystem ps;
        // Use this for initialization
        void Start() {
            leftHash = Animator.StringToHash("ToLeft");
            rightHash = Animator.StringToHash("ToRight");
            anim = GetComponent<Animator>();
            ps = GetComponentInChildren<ParticleSystem>();
            //MarisaJizouMarisaController.onTurning += Turn;
            MarisaJizouController.onVictory += Stop;
        }

        // Update is called once per frame
        void Update() {

        }

        void Turn() {
            anim.Play(isLeft ? rightHash : leftHash, 0);
            isLeft = !isLeft;
        }

        void Stop() {
            ps.Stop();
        }
    }
}