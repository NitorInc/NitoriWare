using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_Victim : MonoBehaviour {

    int frame = 0;
    int windupFrames = 30;
    int lungeFrames = 10;
    int resetFrames = 5;
    int idleFrames = 15;
    int totalFrames;

    int victoryFrame = 0;
    float parabolaValue = -2;
    int jumpFrames = 18;
    bool jumping = false;

    float scale = 1;
    Vector3 scaleVector = new Vector3(1, 1, 1);

    float rotation = 0;
    Vector3 rotationVector = new Vector3(1, 1, 1);

    float initialxpos;
    float initialypos;
    float xoffset = 0;
    float yoffset = 0;
    float angle = 90;
    float radianAngle = 0;
    Vector3 positionVector = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
        totalFrames = windupFrames + lungeFrames + resetFrames + idleFrames;
        initialxpos = transform.parent.transform.position.x;
        initialypos = transform.parent.transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        //On the victim, this is pretty much exclusively used for animation purposes
        frame++;
        if (!MicrogameController.instance.getVictory())
        {
            if (frame % totalFrames <= windupFrames)
            {
                scale -= 0.15f / windupFrames;
            }
            else if (frame % totalFrames <= (windupFrames + lungeFrames))
            {
                scale += 0.3f / lungeFrames;
            }
            else if (frame % totalFrames <= (windupFrames + lungeFrames + resetFrames))
            {
                scale -= 0.15f / resetFrames;
            }
            else
            {
                scale = 1;
            }
        } else {
            if (victoryFrame == 0)
            {
                jumping = true;
            }
            victoryFrame++;

            if (victoryFrame <= jumpFrames) {
                rotation = (-70f / jumpFrames);
                angle += rotation;
                radianAngle = (angle / 180) * Mathf.PI;
                xoffset += Mathf.Cos(radianAngle) / (jumpFrames / 11.33f);//(3.5f / jumpFrames);
                yoffset += Mathf.Sin(radianAngle) / (jumpFrames / 3);//(2f / jumpFrames);
            } else {
                rotation = 0;
            }

            if (scale < 1.15f && jumping) {
                scale += 0.3f / lungeFrames;
            } else if (scale > 1f) {
                jumping = false;
                scale -= 0.15f / resetFrames;
            } else {
                scale = 1f;
            }


        }
        scaleVector = new Vector3(-1, scale, 1);
        transform.parent.transform.localScale = scaleVector;
        rotationVector = new Vector3(0, 0, rotation);
        transform.parent.transform.Rotate(rotationVector);
        positionVector = new Vector3(initialxpos + xoffset, initialypos + yoffset, 0);
        transform.parent.transform.position = positionVector;
    }
}
