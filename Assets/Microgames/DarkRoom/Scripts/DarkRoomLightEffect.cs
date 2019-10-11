using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomLightEffect : MonoBehaviour
{
    private static Transform lampTransformSingleton;

    [Header("Singleton value only necessary in one instance")]
    [SerializeField]
    private Transform lampTransform;

    private Material material;

	void Start()
    {
        material = GetComponent<Renderer>().material;
        if (lampTransform != null)
            lampTransformSingleton = lampTransform;
	}
	
	void LateUpdate()
    {
        updateValues();
    }

    void updateValues()
    {
        material.SetVector("_LampPos", lampTransformSingleton.position);
        material.SetVector("_CursorPos", CameraHelper.getCursorPosition());
    }
}
