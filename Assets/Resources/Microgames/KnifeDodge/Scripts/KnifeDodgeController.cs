using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeController : MonoBehaviour {
	List<GameObject> knifeList;
	List<GameObject> knifeTargetsList;
	public GameObject knifePrefab;
	public GameObject reimuPrefab;
	public GameObject knifeTargetPrefab;
	public int numKnives = 14;
	public float spawnDistance = 10.0f;
	public int knivesRemoved = 4;
	public float timeUntilStrike = 3.0f;
	public bool tiltedKnives;
	// Use this for initialization
	void Start () {
		SpawnTargets ();
		CreateSafeZone ();
		SpawnKnives ();
	}

	void SpawnTargets() {
		knifeTargetsList = new List<GameObject> ();
		Vector3 offset = new Vector3(-numKnives / 2.0f + 0.5f, -1.0f / 2.0f  + 0.5f, 0.0f);

		for (int j = 0; j < numKnives; j++) {
			GameObject target = Instantiate(knifeTargetPrefab, new Vector3(j, -5.0f, 0.0f) + offset, Quaternion.identity);
			knifeTargetsList.Add(target);
		}
	}

	// Spawns several knives above the player.
	void SpawnKnives() {
		
		knifeList = new List<GameObject> ();
		for (int i = 0; i < knifeTargetsList.Count; i++) {
			Vector3 loc = knifeTargetsList [i].transform.position + new Vector3 (0,spawnDistance,0);
			GameObject knife = Instantiate (knifePrefab, loc, Quaternion.identity);
			knifeList.Add(knife);

			foreach (GameObject k in knifeList) {
				Physics2D.IgnoreCollision (knife.GetComponent<BoxCollider2D>(), k.GetComponent<BoxCollider2D>());
			}
		}			

		if (tiltedKnives) {
			knifeTargetsList.Sort ((a, b) => 1 - 2 * Random.Range (0, 1));
		}

		//Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		for (int i = 0; i < knifeList.Count; i++) {
			Vector3 pos = knifeTargetsList [i].transform.position;
				//GetClosestTarget (knifeList [i].transform.position).transform.position;
			knifeList[i].GetComponent<KnifeDodgeKnife>().SetFacing(pos);
		} 
	}

	// Deletes targets to create a safe zone.
	void CreateSafeZone() {
		int startingIndex = Random.Range (0,knifeTargetsList.Count - knivesRemoved);
		for (int i = startingIndex; i < startingIndex + knivesRemoved; i++) {
			knifeTargetsList.RemoveAt (startingIndex);
		}
	}
		
	GameObject GetClosestTarget(Vector3 knifeVector) {
		GameObject closest = knifeTargetsList [0];
		foreach (GameObject target in knifeTargetsList) {
			if (Vector3.Distance (target.transform.position, knifeVector) < Vector3.Distance (closest.transform.position, knifeVector)) {
				closest = target;
			}
		}
		return closest;
	}

	void Update() {
		timeUntilStrike -= Time.deltaTime;
		if (timeUntilStrike <= 0) {
			foreach (GameObject knife in knifeList) {
				knife.GetComponent<KnifeDodgeKnife> ().isMoving = true;
			}
		}

	}
}
