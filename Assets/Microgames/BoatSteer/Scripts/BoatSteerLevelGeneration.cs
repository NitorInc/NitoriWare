using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerLevelGeneration : MonoBehaviour {
	[SerializeField]
	private BoatSteerBoat boat;

	// Level generation parameters
	[SerializeField]
	private float fieldStartSeconds = 1.5f;

	[SerializeField]
	private float fieldEndSeconds = 4.15f;

	[SerializeField]
	private float obstacleRadius = 10f;

	[SerializeField]
	private int obstacleK = 15;

	[SerializeField]
	private GameObject staticObstacleTemplate;

	[SerializeField]
	private Sprite[] obstacleSprites;

	[SerializeField]
	private Sprite[] uniqueSprites;

	[SerializeField]
	private float uniqueProbability;

	// Use this for initialization
	void Start () {
		// Apply boat settings to actual boat.
		float speed = boat.speed;
		float maxLateralFactor = boat.maxLateralSpeedFactor;

		// Calculate obstacle field dimensions from boat settings

		float fieldMercyLength = fieldStartSeconds * speed;
		float fieldLength = (fieldEndSeconds - fieldStartSeconds) * speed;
		float fieldWidth = 2f * maxLateralFactor * speed * fieldEndSeconds;

		BoatSteerObstacleGenerator generator = new BoatSteerObstacleGenerator(fieldWidth, fieldLength, obstacleRadius, obstacleK);
		generator.GenerateObstacleField();

		int obstacleSpriteIndex = 0;

		uniqueSprites.Shuffle();
		int uniqueSpriteIndex = 0;

		foreach (Vector2 point in generator.GetPoints()) {
			// Horizontally center the point
			Vector2 obstacleLocation = point;
			obstacleLocation.x -= fieldWidth/2f;
			// Shift the point out far enough that it can't be hit for fieldStartSeconds
			obstacleLocation.y += fieldMercyLength;

			// Actually instantiate the obstacle
			GameObject obstacle = Instantiate(staticObstacleTemplate);
			SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
			if (uniqueSpriteIndex < uniqueSprites.Length && Random.value < uniqueProbability) {
				renderer.sprite = uniqueSprites[uniqueSpriteIndex];
				uniqueSpriteIndex++;
			} else {
				renderer.sprite = obstacleSprites[obstacleSpriteIndex];
				obstacleSpriteIndex = (obstacleSpriteIndex + 1) % obstacleSprites.Length;
			}
			obstacle.transform.localPosition = new Vector3(obstacleLocation.x, 0f, obstacleLocation.y);
			obstacle.SetActive(true);
		}
	}
}
