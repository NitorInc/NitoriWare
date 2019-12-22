using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private float obstacleScaleMult = 1f;

	[SerializeField]
	private GameObject staticObstacleTemplate;

	[SerializeField]
	private Sprite[] obstacleSprites;

	[SerializeField]
	private Sprite[] uniqueSprites;

	[SerializeField]
	private float uniqueProbability;

    private List<GameObject> obstacles;

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
        obstacles = new List<GameObject>();

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
            obstacle.transform.localScale *= obstacleScaleMult;
            obstacle.SetActive(true);
            obstacles.Add(obstacle);
        }

        // Find the closest obstacle to the center of the obstacle spread and put murasa in front of it,
        // ensuring she doesn't start with a clear path
        var minPos = new Vector3(
            obstacles.Min(a => a.transform.position.x),
            0f,
            obstacles.Min(a => a.transform.position.z));
        var maxPos = new Vector3(
            obstacles.Max(a => a.transform.position.x),
            0f,
            obstacles.Max(a => a.transform.position.z));
        var midPos = minPos + ((maxPos - minPos) / 2f);
        var closestObstaclePos = obstacles
            .OrderBy(a => Mathf.Abs((a.transform.position - midPos).magnitude))
            .FirstOrDefault()
            .transform.position;
        boat.transform.position = new Vector3(closestObstaclePos.x, boat.transform.position.y, boat.transform.position.z);
	}
}
