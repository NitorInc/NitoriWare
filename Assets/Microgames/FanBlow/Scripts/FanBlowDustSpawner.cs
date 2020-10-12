using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowDustSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform dustParent;
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
    [SerializeField]
    private float levelDustDamageMult = 1f;
    [SerializeField]
    private float levelDustDistanceDropoffMult = 1f;

    void Start()
    {
        for (int i = 0; i < dustParent.childCount; i++)
        {
            var dustObject = dustParent.GetChild(i).GetComponent<FanBlowDust>();
            var position = new Vector3(MathHelper.randomRangeFromVector(spawnXRandomRange),
                MathHelper.randomRangeFromVector(spawnYRandomRange),
                transform.position.z - (.003f * i));
            dustObject.transform.position = position;
            var dustComponent = dustObject.GetComponent<FanBlowDust>();
            dustComponent.Fan = fan;
            dustComponent.DamagePerSpeed *= levelDustDamageMult;
            dustComponent.DamageDistanceDropOffRate *= levelDustDistanceDropoffMult;
        }
	}

    private void Update()
    {
        if (dustParent.childCount <= 0)
        {
            enabled = false;
            MicrogameController.instance.setVictory(true);
        }
    }
}
