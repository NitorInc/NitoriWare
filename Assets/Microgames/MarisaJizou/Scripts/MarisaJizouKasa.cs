using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouKasa : MonoBehaviour {

        int colCount = 0;

        void OnCollisionEnter2D(Collision2D collision) {
            if (colCount == 0) {
                var jizou = collision.gameObject.GetComponent<MarisaJizouJizou>();
                if (jizou != null) {
                    jizou.HatLanding();
                }
                colCount++;
                Destroy(gameObject);
            }
        }
    }

}
