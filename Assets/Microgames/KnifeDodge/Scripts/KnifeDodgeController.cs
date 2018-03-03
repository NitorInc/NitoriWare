using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeController : MonoBehaviour {
	// Private Stuff
	List<GameObject> knifeList;
	List<GameObject> knifeTargetsList;

	// Public Stuff
	public GameObject knifePrefab;
	public GameObject knifeTargetPrefab;
	public int numKnives = 14;
	public float spawnDistance = 10.0f;
	public int knivesRemoved = 4;
	public float timeUntilStrike = 3.0f;
	public bool tiltedKnives = true;
	public bool tiltedKnivesRandomAngle = true;
	public float tiltedKnivesAngle = 0;
	public int tiltedKnivesNumZeroTilt = 4;
	public float knifeStopHeight = 2.0f;
	public float knifeFreezeTime = 1.0f;
	public float knifeUnfreezeTime = 1.0f;

	public enum KnifeDirections {
		MINUS_ANGLE,
		POSITIVE_ANGLES,
		NUM_DIRECTIONS
	}

    // Todo: how to get enum from KnifeDodgeKnife.cs
    enum KnifeState
    {
        FLYING_IN,
        STOP_AND_ROTATE,
        MOVING_TO_GROUND,
    }

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

			if (tiltedKnivesRandomAngle) {
				// Set a random position on the ground instead of a fixed one

				// A really hacky way to shuffle
				knifeTargetsList.Sort ((a, b) => 1 - 2 * Random.Range (0, 1));
				for (int i = 0; i < knifeList.Count; i++) {
					Vector3 pos = knifeTargetsList [i].transform.position;
					knifeList[i].GetComponent<KnifeDodgeKnife>().SetFacing(pos);
				} 
			} else {
				// Set a fixed one.
				// A really hacky way to shuffle
				knifeList.Sort ((a, b) => 1 - 2 * Random.Range (0, 1));

				for (int i = 0; i < knifeList.Count; i++) {
					int directionChoice = (int) Random.Range(0, (int)KnifeDirections.NUM_DIRECTIONS);
					float angle = 180;

					switch (directionChoice) {
					case (int)KnifeDirections.MINUS_ANGLE:
						angle = 360 - tiltedKnivesAngle;
						break;
					case (int)KnifeDirections.POSITIVE_ANGLES:
						angle = tiltedKnivesAngle;
						break;
					}

					if (i < tiltedKnivesNumZeroTilt) {
						angle = 0;
					}

					Vector3 lDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.down;
					Vector3 pos = knifeList[i].GetComponent<Transform>().position + lDirection;
					knifeList[i].GetComponent<KnifeDodgeKnife>().SetFacing(pos);
				} 
			}
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
        for (int i = 0; i < knifeList.Count; i++)
        {
            if (timeUntilStrike < 0.0f)
            {
            
                    knifeList[i].GetComponent<KnifeDodgeKnife>().SetState((int) KnifeState.MOVING_TO_GROUND);
            
            } 
            else if (knifeList[i].transform.position.y > knifeStopHeight)
            {
                    knifeList[i].GetComponent<KnifeDodgeKnife>().SetState((int)KnifeState.FLYING_IN);
            }
            else
            {
                    knifeList[i].GetComponent<KnifeDodgeKnife>().SetState((int)KnifeState.STOP_AND_ROTATE);

            }
        }
    }
}
