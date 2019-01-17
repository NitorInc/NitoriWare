using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeController : MonoBehaviour {
	// Private Stuff
	List<GameObject> knifeList;
	List<GameObject> knifeTargetsList;

    // Public Stuff
    public GameObject parallaxController;
	public GameObject knifePrefab;
	public GameObject knifeTargetPrefab;
    public GameObject inversionController;
    public GameObject whiteoutController;
	public int numKnives = 14;
	public float spawnDistance = 10.0f;
	public int knivesRemoved = 4;
	public float timeUntilStrike = 3.0f;
    public float knifeFreezeTime = 0.5f;
    public float knifeStopHeight = 3.0f;
    public float blackOutAValue = 255.0f;
    public float blackOutSpeed = 2.0f;
    public float parallaxMaxSpeed = 1.0f;

    // Only applies if tiltedKnives enabled
    public bool tiltedKnives = true;
	public bool tiltedKnivesRandomAngle = true;

    public bool horizontalMovementKnives = false;
    public float horizontalKnifeSpeed = 1f;
	public float tiltedKnivesAngle = 0;
	public int tiltedKnivesNumZeroTilt = 4;
    public enum KnifeDirections {
		MINUS_ANGLE,
		POSITIVE_ANGLES,
		NUM_DIRECTIONS
	}

    bool knifeMoveRight;
    int currentState;

    // Todo: how to get enum from KnifeDodgeKnife.cs
    enum KnifeState
    {
        FLYING_IN,
        STOP_AND_ROTATE,
        MOVING_TO_GROUND,
    }

    // Use this for initialization
    void Start () {
        currentState = -1;

        knifeMoveRight = (Random.value > 0.5f);
        SpawnTargets ();
		CreateSafeZone ();
		SpawnKnives ();

    }

	void SpawnTargets() {
		knifeTargetsList = new List<GameObject> ();
		Vector3 offset = new Vector3(-numKnives / 2.0f + 0.5f, -1.0f / 2.0f  + 1.5f, 0.0f);

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
            knife.transform.position += new Vector3(0, knife.GetComponent<KnifeDodgeKnife>().knifeSpeed * knifeFreezeTime,0);

            knifeList.Add(knife);

            foreach (GameObject k in knifeList) {
				Physics2D.IgnoreCollision (knife.GetComponent<BoxCollider2D>(), k.GetComponent<BoxCollider2D>());
			}
		}

        for (int i = 0; i < knifeList.Count; i++)
        {
            knifeList[i].GetComponent<KnifeDodgeKnife>().SetTilted(tiltedKnives);
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
					Vector3 pos = knifeList[i].GetComponent<Transform>().position + lDirection /*- new Vector3(0.0f, knifeStopHeight, 0.0f)*/;
					knifeList[i].GetComponent<KnifeDodgeKnife>().SetFacing(pos);
				} 
			}
		}
	}

	// Deletes targets to create a safe zone.
	void CreateSafeZone() {
		int startingIndex = Random.Range (0,knifeTargetsList.Count - knivesRemoved);

        if (horizontalMovementKnives)
        {
            int offset = 2;
            startingIndex /= 2;
            if (!knifeMoveRight)
            {
                startingIndex += (knifeTargetsList.Count / 2);
                startingIndex -= offset;
            }
            else
                startingIndex += offset;
        }

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

    void playKnifeSound()
    {
        GetComponents<AudioSource>()[0].Play();
    }

	void Update() {
        for (int i = 0; i < knifeList.Count; i++)
        {
            bool allLanded = true;
            allLanded &= !knifeList[i].GetComponent<Rigidbody2D>().simulated;
            if (allLanded) MicrogameController.instance.setVictory(true, true);

            float parallaxSpeed = parallaxController.GetComponent<KnifeDodgeParallaxBackground>().GetSpeed();
            inversionController.GetComponent<KnifeDodgeInversionController>().fadeSpeed = blackOutSpeed;
            if (knifeList[i].transform.position.y > knifeStopHeight)
            {
                parallaxController.GetComponent<KnifeDodgeParallaxBackground>().SetSpeed(Mathf.Lerp(parallaxSpeed, parallaxMaxSpeed, Time.deltaTime));
                if (currentState != (int)KnifeState.FLYING_IN)
                {
                    Invoke("playKnifeSound", StageController.beatLength * 1.5f);
                }
                currentState = (int)KnifeState.FLYING_IN;
                knifeList[i].GetComponent<KnifeDodgeKnife>().SetState(currentState);
                inversionController.GetComponent<KnifeDodgeInversionController>().invertFilterAlpha = 0;
            }
            else if (timeUntilStrike < 0.0f)
            {
                parallaxController.GetComponent<KnifeDodgeParallaxBackground>().SetSpeed(Mathf.Lerp(parallaxSpeed, parallaxMaxSpeed, Time.deltaTime));
                if (currentState != (int)KnifeState.MOVING_TO_GROUND)
                {
                    GetComponents<AudioSource>()[1].Play();
                }
                currentState = (int)KnifeState.MOVING_TO_GROUND;
                knifeList[i].GetComponent<KnifeDodgeKnife>().SetState(currentState);
                inversionController.GetComponent<KnifeDodgeInversionController>().invertFilterAlpha = 0;
            }
            else
            {
                parallaxController.GetComponent<KnifeDodgeParallaxBackground>().SetSpeed(Mathf.Lerp(parallaxSpeed, 0, Time.deltaTime));
                if (currentState != (int)KnifeState.STOP_AND_ROTATE)
                {
                    GetComponents<AudioSource>()[2].Play();
                }

                if (horizontalMovementKnives)
                {
                    if (knifeMoveRight)
                    {
                        knifeList[i].transform.position += new Vector3(horizontalKnifeSpeed, 0, 0) * Time.deltaTime;
                    } else
                    {
                        knifeList[i].transform.position -= new Vector3(horizontalKnifeSpeed, 0, 0) * Time.deltaTime;
                    }
                }
                currentState = (int)KnifeState.STOP_AND_ROTATE;
                knifeList[i].GetComponent<KnifeDodgeKnife>().SetState(currentState);
                whiteoutController.GetComponent<KnifeDodgeWhiteOutController>().DoFlash();
                inversionController.GetComponent<KnifeDodgeInversionController>().invertFilterAlpha = blackOutAValue;
            }
        }

        knifeFreezeTime -= Time.deltaTime;
        timeUntilStrike -= Time.deltaTime;
    }
}
