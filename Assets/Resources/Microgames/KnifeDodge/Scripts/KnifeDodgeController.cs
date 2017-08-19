using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeController : MonoBehaviour {
	List<GameObject> knifeList;
	public GameObject knifePrefab;
	public GameObject reimuPrefab;


	// Use this for initialization
	void Start () {
		SpawnKnives ();
		CreateSafeZone ();
	}

	// Spawns several knives above the player.
	void SpawnKnives() {
		knifeList = new List<GameObject> ();
		Instantiate (knifePrefab, GetRandomLocation(), Quaternion.identity);

	}

	Vector3 GetRandomLocation() {
		Vector2 dir = Random.insideUnitCircle;
		Vector3 position = Vector3.zero;
		//make it appear on the top/bottom
		return new Vector3(dir.x * Camera.main.orthographicSize * Camera.main.aspect, 
			Mathf.Sign(dir.y) * Camera.main.orthographicSize);
		
	}

	// Deletes knives to create a safe zone.
	void CreateSafeZone() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
