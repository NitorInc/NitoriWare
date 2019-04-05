using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckWrigglyJointsUpperArms : MonoBehaviour {

    

    public Transform tail;
    private float angle;
  
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private float spaghetti, anglemultiplier;

//rotates arm things based on vector to connected arm things
    // Update is called once per frame
    void Update () {
        Vector2 toBody = (Vector2)(body.transform.position - transform.position).normalized;
        angle = MathHelper.trueMod(toBody.getAngle(), 360f);
        tail.transform.rotation = Quaternion.Euler(0f, 0f, angle * anglemultiplier + spaghetti);
    }
   
}
