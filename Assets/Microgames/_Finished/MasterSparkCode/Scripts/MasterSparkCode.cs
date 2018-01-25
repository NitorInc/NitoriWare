﻿using UnityEngine;
using System.Collections.Generic;

public class MasterSparkCode : MonoBehaviour
{

  // Game Controller's properties
  public MasterSparkCodeCommandSequence InputSequence;
  public GameObject MarisaObject;
  public GameObject EnemyObject;
  public Animator MarisaController;
  public Animator CameraController;
  public GameObject MasterSparkSuccess;
  public GameObject MasterSparkFailure;
  public List<AudioSource> AudioSources;

  // Update is called once per frame
  void Update()
  {
    if (!(MicrogameController.instance.getVictoryDetermined()))
    {
      if (Input.GetKeyDown(KeyCode.UpArrow))
        ProcessKey(MasterSparkCodeCommandType.Up);
      else if (Input.GetKeyDown(KeyCode.DownArrow))
        ProcessKey(MasterSparkCodeCommandType.Down);
      else if (Input.GetKeyDown(KeyCode.LeftArrow))
        ProcessKey(MasterSparkCodeCommandType.Left);
      else if (Input.GetKeyDown(KeyCode.RightArrow))
        ProcessKey(MasterSparkCodeCommandType.Right);
      else if (Input.GetKeyDown(KeyCode.Space))
        ProcessKey(MasterSparkCodeCommandType.Action);
    }
  }

  void ProcessKey(MasterSparkCodeCommandType c)
  {
    if (InputSequence.IsCorrectInput(c))
    {
      InputSequence.DequeueCommand();
      if (InputSequence.IsEmpty())
        SetVictory();
    }
    else
      SetFailure();
  }

  void SetVictory()
  {
    MicrogameController.instance.setVictory(true, true);
    CameraController.SetTrigger("stateVictory");
    MarisaController.SetTrigger("stateVictory");
  }

  void SetFailure()
  {
    MicrogameController.instance.setVictory(false, true);
    MarisaController.SetTrigger("stateFailure");
  }
}
