using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	//Future site of code that will do something (global settings and such)

	public static GameController instance;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(transform.gameObject);
		instance = this;


		Cursor.visible = false;
		Camera[] cameras = GameObject.FindObjectsOfType<Camera>();

		foreach (Camera camera in cameras)
		{
			camera.aspect = 4f / 3f;
		}


	}

	void Start ()
	{

	}
	
	void Update ()
	{
	
	}
}
