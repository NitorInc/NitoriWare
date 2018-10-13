using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaServeCustomer : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnXRange;

    private MystiaServeCustomerData.Customer data;
    public MystiaServeCustomerData.Customer Data
    {
        get { return data; }
        set { data = value; }
    }

	void Awake ()
    {
        var x = MathHelper.randomRangeFromVector(spawnXRange);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}
}
