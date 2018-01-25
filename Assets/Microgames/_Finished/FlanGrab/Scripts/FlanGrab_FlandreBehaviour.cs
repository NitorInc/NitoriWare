﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_FlandreBehaviour : MonoBehaviour
{

  private GameObject rightArmObject;

  [SerializeField]
  private float bodyRotationMult;

  // Use this for initialization
  void Start() => rightArmObject = transform.Find("Right_Arm").gameObject;

  // Update is called once per frame
  void Update()
  {
    if (!MicrogameController.instance.getVictoryDetermined())
    {
      rotateRightArm();
    }
  }

  void rotateRightArm()
  {
    var mouseOnScreen = CameraHelper.getCursorPosition();
    var positionOnScreen = rightArmObject.transform.position;
    float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
    if (-90 <= angle && angle <= 90)
    {
      rightArmObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
      transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * bodyRotationMult));
    }
  }

  float AngleBetweenTwoPoints(Vector3 a, Vector3 b) => Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
}
