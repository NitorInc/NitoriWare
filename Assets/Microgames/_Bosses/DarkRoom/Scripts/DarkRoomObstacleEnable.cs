using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomObstacleEnable : MonoBehaviour
{
    [SerializeField]
    private float maxXDistance = 11f;
    
	void Start ()
    {
		
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
