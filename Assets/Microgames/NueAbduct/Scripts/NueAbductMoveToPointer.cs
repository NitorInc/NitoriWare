using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.NueAbduct {

    public class NueAbductMoveToPointer : MonoBehaviour {

        public Transform target;
        public float speed = 2.0f;

        void Update() {
            if (MicrogameController.instance.getVictoryDetermined())
                return;

            var targetPos = CameraHelper.getCursorPosition();
            float step = speed * Time.deltaTime;
            target.transform.position = Vector2.MoveTowards(target.transform.position, targetPos, step);
        }
    }
}
