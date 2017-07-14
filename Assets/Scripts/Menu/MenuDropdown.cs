using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Dropdown dropdown;
#pragma warning restore 0649

    public enum Type
    {
        Explicit,
        Language,
        Resolution
    }


	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

    public void setValue(int value)
    {

    }
}
