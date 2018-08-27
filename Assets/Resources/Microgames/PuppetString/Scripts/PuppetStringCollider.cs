using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetStringCollider : MonoBehaviour {

	bool mouseOver;

	// Use this for initialization
	void Start () {
		mouseOver = false;
	}
	
	void OnMouseOver()
	{
		mouseOver = true;
	}

	void OnMouseExit()
	{
		mouseOver = false;
	}

	public bool isMouseOver()
	{
		return mouseOver;
	}
}
