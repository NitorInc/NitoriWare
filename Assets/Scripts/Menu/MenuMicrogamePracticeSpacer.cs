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

    [SerializeField]
    private bool setActiveOnly;
    
	void Start ()
    {
	}

	void Update ()
    {
        int j = 0;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (!Application.isPlaying && !setActiveOnly)
            {
                if (child.name.ToLower().Contains("microgame")
                    || child.name.ToLower().Contains("boss"))
                {
                    child.transform.position = topRight
                        + (Vector3.right * (j % rowSize) * spacing)
                        + (Vector3.down * (j / rowSize) * spacing);
                    j++;
                }
            }
            else if (!child.name.Contains("(0)") && Application.isPlaying)
                child.gameObject.SetActive(transform.localScale.x > .011f);
        }
	}
}
