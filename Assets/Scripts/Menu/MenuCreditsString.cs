using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCreditsString : MonoBehaviour
{

#pragma warning disable 0649
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
            if (limitSize != null)
                limitSize.enabled = true;
            if (outline != null)
                outline.enabled = true;
            enabled = false;
        }
	}
}
