using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{
	//"A Unity in-editor variable"
	[Header("Target")] //Adds a header to the script within the Unity editor
	[SerializeField] //Serialized fields can be accessed within the editor
	private GameObject target;
	
	[Header("Bullet speed")]
	[SerializeField]
	private float speed = 1f;
	
	[Header("Firing delay (in seconds)")]
	[SerializeField]
	private float delay = 1f;
	
	//Stores the direction the bullet is traveling in
	private Vector2 trajectory;
	
	// Use this for initialization
	void Start()
	{
		//Invoke the setTrajectory method after the delay
		Invoke("SetTrajectory", delay);
	}
	
	// Update is called once per frame
	void Update()
	{
		//Only start moving after the trajectory is set
		if (trajectory != null)
		{
			//Move the bullet a certain distance based on trajectory speed and time
			Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
			transform.position = newPosition;
		}
	}
	void SetTrajectory()
	{
		//Calculate a trajectory towards Reimu
		trajectory = (target.transform.position - transform.position).normalized; //what does any of this mean? :thinking:
	}
}