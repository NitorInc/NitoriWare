using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsRandomLimbCreator : MonoBehaviour {

	[SerializeField]
	private List<Transform> availablePositions = null;
	[SerializeField]
	private List<ZombiePartsLimbTilt> zombiePartLimbs = null;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < zombiePartLimbs.Count; i++) {
			int randomPositionId = (int)Random.Range(0f, availablePositions.Count);
			zombiePartLimbs[i].gameObject.transform.position = availablePositions[randomPositionId].transform.position;
			zombiePartLimbs[i].gameObject.transform.eulerAngles = availablePositions[randomPositionId].transform.eulerAngles;

			zombiePartLimbs[i].SetInitialRotationAngle(availablePositions[randomPositionId].transform.eulerAngles.z);

			availablePositions.RemoveAt(randomPositionId);
		}
	}

}
