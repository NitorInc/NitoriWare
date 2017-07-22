using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableByTarget : MonoBehaviour
    {
    #pragma warning disable 0649
    [SerializeField]
    private RuntimePlatform[] targetPlatforms;
    [SerializeField]
    private bool disableOnTargetPlatforms;

	void Start ()
    {
        RuntimePlatform platform = Application.platform;
        for (int i = 0; i < targetPlatforms.Length; i++)
        {
            if (targetPlatforms[i] == platform)
            {
                gameObject.SetActive(!disableOnTargetPlatforms);
                return;
            }
        }
	}
}
