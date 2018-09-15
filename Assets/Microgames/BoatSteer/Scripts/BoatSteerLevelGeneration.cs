using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerLevelGeneration : MonoBehaviour {
	[SerializeField]
	private float fieldLength = 60f;

	[SerializeField]
	private float fieldWidth = 80f;

	[SerializeField]
	private float offset = 10f;

	[SerializeField]
	private float obstacleRadius = 10f;

	[SerializeField]
	private int obstacleK = 15;

	[SerializeField]
	private GameObject staticObstacleTemplate;

	// Use this for initialization
	void Start () {
		BoatSteerObstacleGenerator generator = new BoatSteerObstacleGenerator(fieldWidth, fieldLength, obstacleRadius, obstacleK);
		generator.GenerateObstacleField();
		foreach (Vector2 point in generator.GetPoints()) {
			GameObject obstacle = Instantiate(staticObstacleTemplate);
			obstacle.transform.localPosition = new Vector3(point.x - fieldWidth/2f, 0f, point.y + offset);
			obstacle.SetActive(true);
		}

	}
}
