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
#pragma warning restore 0649

    private bool activated;

	void Update()
	{
		if (!activated && !CameraHelper.isObjectOffscreen(transform, 2f))
        {
            limitSize.enabled = true;
            outline.enabled = true;
            activated = true;
        }
	}
}
