﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowDustSpawner : MonoBehaviour
{
    [SerializeField]
    private ObjectPool dustPool;
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
        for (int i = 0; i < spawnCount; i++)
        {
            createDust(new Vector3(MathHelper.randomRangeFromVector(spawnXRandomRange),
                MathHelper.randomRangeFromVector(spawnYRandomRange),
                transform.position.z + (.001f * i)));
        }
	}

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            enabled = false;
            MicrogameController.instance.setVictory(true);
        }
    }

    FanBlowDust createDust(Vector3 position)
    {
        var dustObject = dustPool.getObjectFromPool();
        dustObject.transform.position = position;
        dustObject.transform.parent = transform;
        var dustComponent = dustObject.GetComponent<FanBlowDust>();
        dustComponent.Fan = fan;
        dustComponent.DamagePerSpeed *= levelDustDamageMult;
        dustComponent.DamageDistanceDropOffRate *= levelDustDistanceDropoffMult;
        return dustComponent;
    }
}
