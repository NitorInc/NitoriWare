using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DarkRoomObstacleEnable : MonoBehaviour
{
    [SerializeField]
    private float maxXDistance = 11f;
    
	void Start ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
                Destroy(child.gameObject);
        }
    }
	
	void Update ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var shouldEnable = Mathf.Abs(MainCameraSingleton.instance.transform.position.x - child.position.x) <= maxXDistance;
            if (shouldEnable != child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(shouldEnable);
        }
	}
}
