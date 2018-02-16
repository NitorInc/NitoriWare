using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookTargetSpawn : MonoBehaviour
{
    [SerializeField]
    private NitoriLookMovement playerMovement;
    [Header("Parameters for what position the target will spawn")]
    [Header("Spawning is via a random angle from the player at a random distance")]
    [SerializeField]
    private Vector2 floorSpawnAngleBounds;
    [SerializeField]
    private float floorSpawnAngleOffset;
    [SerializeField]
    private Vector2 floorSpawnDistanceBounds;
    [SerializeField]
    private bool relativeToPlayer = true;

    [Header("Random spawn height, independent of floor spawn")]
    [SerializeField]
    private Vector2 heightSpawnBounds;

    [Header("Components to reset when repositioned")]
    [SerializeField]
    private SineWave sineWave;

    void Start ()
    {
        float spawnAngle = floorSpawnAngleOffset + MathHelper.randomRangeFromVector(floorSpawnAngleBounds);
        if (relativeToPlayer)
            spawnAngle += -(playerMovement.transform.eulerAngles.y);
        spawnAngle = spawnAngle % 360;
        Vector2 floorSpawnVector = MathHelper.getVector2FromAngle(
            spawnAngle, MathHelper.randomRangeFromVector(floorSpawnDistanceBounds));
        transform.position = new Vector3(floorSpawnVector.x, MathHelper.randomRangeFromVector(heightSpawnBounds),floorSpawnVector.y);

        if (sineWave != null)
            sineWave.resetStartPosition();
	}
	
	void Update ()
    {
		
	}
}
