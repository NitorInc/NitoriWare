using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchPlayerTorchController : MonoBehaviour {

        // Use this for initialization
        void Start() {
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update() {
            var point = MainCameraSingleton.instance.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0.0f;
            transform.position = point;
            MicrogameController.instance.getSFXSource().panStereo = AudioHelper.getAudioPan(transform.position.x);
        }
    }
}
