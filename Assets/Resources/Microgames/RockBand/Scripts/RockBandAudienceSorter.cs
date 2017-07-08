using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandAudienceSorter : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float leftX, xPerAudience, xFudge;
    [SerializeField]
    private Vector2 yBounds;
#pragma warning restore 0649

	void Awake()
	{
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            childTransform.position =
                new Vector3(leftX + (xPerAudience * (float)i) + Random.Range(-xFudge, xFudge),
                MathHelper.randomRangeFromVector(yBounds),
                childTransform.position.z);
        }
	}
}
