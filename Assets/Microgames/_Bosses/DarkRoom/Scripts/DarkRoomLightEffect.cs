using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomLightEffect : MonoBehaviour
{
    public static Transform lampTransformSingleton;
    public static Transform cursorTransformSingleton;

    [SerializeField]
    private float radiusBoostMult = -.4f;
    [Header("Singleton values only necessary in one instance")]
    [SerializeField]
    private Transform lampTransform;
    [SerializeField]
    private Transform cursorTransform;

    private Renderer rend;

	void Start()
    {
        rend = GetComponent<Renderer>();
        if (lampTransform != null)
            lampTransformSingleton = lampTransform;
        if (cursorTransform != null)
            cursorTransformSingleton = cursorTransform;
    }
	
	void LateUpdate()
    {
        updateValues();
    }

    void updateValues()
    {
        var material = rend.material;
        material.SetVector("_LampPos", lampTransformSingleton.position);
        material.SetVector("_CursorPos", cursorTransformSingleton.position);
        material.SetFloat("_LampAnim", DarkRoomEffectAnimationController.instance.lampBoost);
        material.SetFloat("_CursorAnim", DarkRoomEffectAnimationController.instance.cursorBoost);

        var radBoost = DarkRoomEffectAnimationController.instance.radiusBoost * radiusBoostMult;
        radBoost = Mathf.Max(radBoost, -material.GetFloat("_FadeEnd"));
        material.SetFloat("_LampRadiusBoost", radBoost);
    }
}
