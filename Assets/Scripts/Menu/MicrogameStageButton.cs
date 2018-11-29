using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MicrogameStageButton : SceneButton
{

#pragma warning disable 0649
    [SerializeField]
    private bool getIdFromName;
    [SerializeField]
    private string microgameId;
#pragma warning restore 0649

    void Start()
    {
        if (getIdFromName)
            microgameId = name;
    }

    void Update()
    {
        if (!Application.isPlaying && getIdFromName)
            microgameId = name;
    }

    public override void press()
    {
        MicrogameStage.microgameId = microgameId;
        base.press();
    }
}
