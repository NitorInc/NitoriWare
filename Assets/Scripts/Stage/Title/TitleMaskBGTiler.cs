using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMaskBGTiler : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float moveSpeed, wrapDistance;
#pragma warning restore 0649

	void Start()
	{

	}
	
	void Update()
    {
        float y = transform.localPosition.y;
        y += moveSpeed * Time.deltaTime;
        if (y >= wrapDistance)
            y -= wrapDistance * 2f;

        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
