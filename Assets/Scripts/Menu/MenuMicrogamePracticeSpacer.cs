using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MenuMicrogamePracticeSpacer : MonoBehaviour
{
    [SerializeField]
    private int rowSize;
    [SerializeField]
    private float spacing;
    [SerializeField]
    private Vector3 topRight;
    
	void Start ()
    {
        if (Application.isPlaying)
            enabled = false;
	}

	void Update ()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (child.name.ToLower().Contains("microgame")
                || child.name.ToLower().Contains("boss"))
            {
                child.transform.position = topRight
                    + (Vector3.right * (i % rowSize) * spacing)
                    + (Vector3.down * (i / rowSize) * spacing);
            }
        }
	}
}
