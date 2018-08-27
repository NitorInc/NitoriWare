using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetStringDrawer : MonoBehaviour {

	[SerializeField]
	private GameObject line;

	private PuppetStringCollider start;
	private PuppetStringCollider goal;

	private Vector2 mousePosition;
	private bool drawOn = false;
	private bool end = false;

	// Use this for initialization
	void Start () {
		GameObject temp;

		temp = GameObject.Find("Start");
		//Debug.Log("Start: " + temp.GetComponent<Transform>().position);
		start = temp.GetComponent<PuppetStringCollider>();

		temp = GameObject.Find("Goal");
		//Debug.Log("Goal: " + temp.GetComponent<Transform>().position);
		goal = temp.GetComponent<PuppetStringCollider>();

	}
	
	// Update is called once per frame
	private void Update () {
		if(!end)
		{
			if(Input.GetMouseButtonDown(0) && !drawOn)
			{
				if(checkPosition(start))
				{
					drawOn = true;

					//Create draw object
					mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Instantiate(line, mousePosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
				}
			}
			else if(drawOn)
			{
				//Ends draw at current position
				//Maybe let them "retry"?
				if(Input.GetMouseButtonUp(0))
				{
					drawOn = false;
					end = true;
				}

				if(checkPosition(goal))
				{
					drawOn = false;
					end = true;
					MicrogameController.instance.setVictory(true, true);
				}
			}
		}
	}


	//Checks whether the mouse location is in some collision box
	private bool checkPosition(PuppetStringCollider obj)
	{
		//collision box logic for mouse position
		//return true;

		return obj.isMouseOver();
	}
}