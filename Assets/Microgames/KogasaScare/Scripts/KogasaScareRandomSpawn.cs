using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareRandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float minSpawnX;
    [SerializeField]
    private float maxSpawnX;
    [SerializeField]
    private float minKogasaDistance;
    [SerializeField]
    private KogasaScareKogasaBehaviour kogasa;


    void Start()
    {
        int tries = 100;
        do
        {
            transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), transform.position.y, transform.position.z);

            tries--;
            if (tries < 0)
            {
                print("Too many placement tries!");
                break;
            }
        }
        while (Mathf.Abs(transform.position.x - kogasa.transform.position.x) < minKogasaDistance);

        SendMessage("onSpawnSet", options: SendMessageOptions.DontRequireReceiver);
    }
}
