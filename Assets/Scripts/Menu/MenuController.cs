using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private bool dontEnableCursor;
#pragma warning restore 0649

	void Start()
	{
        if (!dontEnableCursor)
            Cursor.visible = true;
	}
	
	void Update()
	{
		
	}
}
