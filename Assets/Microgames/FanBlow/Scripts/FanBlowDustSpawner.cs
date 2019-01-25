using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowDustSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnXRandomRange;
    [SerializeField]
    private Vector2 spawnYRandomRange;
    [SerializeField]
    private int spawnCount;
    [SerializeField]
    private GameObject dustPrefab;
    [SerializeField]
    private FanBlowFanMovement fan;
    
	void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            createDust(new Vector3(MathHelper.randomRangeFromVector(spawnXRandomRange),
                MathHelper.randomRangeFromVector(spawnYRandomRange),
                transform.position.z + (.001f * i)));
        }
	}
	
    FanBlowDust createDust(Vector3 position)
    {
        var dustObject = Instantiate(dustPrefab, position, Quaternion.identity);
        var dustComponent = dustObject.GetComponent<FanBlowDust>();
        dustComponent.Fan = fan;
        return dustComponent;
    }
}
