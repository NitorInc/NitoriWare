using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCreditsString : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private CanvasTextLimitSize limitSize;
    [SerializeField]
    private CanvasTextOutline outline;
    [SerializeField]
    private float offscreenThreshold = 10f;
#pragma warning restore 0649

    void Update()
	{
		if (!CameraHelper.isObjectOffscreen(transform, offscreenThreshold))
        {
            limitSize.enabled = true;
            outline.enabled = true;
            enabled = false;
        }
	}
}
