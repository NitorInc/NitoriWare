using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMaskBGTiler : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float moveSpeed, wrapAt, wrapTo;
#pragma warning restore 0649

	void Update()
    {
        float y = transform.localPosition.y;
        y += moveSpeed * Time.deltaTime;
        if (y >= wrapAt)
            y = wrapTo;

        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
