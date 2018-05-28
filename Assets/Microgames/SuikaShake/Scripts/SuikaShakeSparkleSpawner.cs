using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeSparkleSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnStartTime = .05f;
    [SerializeField]
    private float spawnFrequency = .25f;
    [SerializeField]
    private GameObject sparklePrefab;
    [SerializeField]
    private SuikaShakeSpriteFinder spriteCollection;
    [SerializeField]
    private BoxCollider2D spawnCollider;
    
	void Update ()
    {

        InvokeRepeating("createSparkle", spawnStartTime, spawnFrequency);
        enabled = false;
    }

    void createSparkle()
    {
        var newSparkle = Instantiate(sparklePrefab, transform);

        newSparkle.transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1f : 1f, 1f, 1f);
        newSparkle.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
        
        var chosenSprite = spriteCollection.sprites[Random.Range(0, spriteCollection.sprites.Length)];
        var spriteRenderers = newSparkle.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sparkleRenderer in spriteRenderers)
        {
            sparkleRenderer.sprite = chosenSprite;
        }

        float xOffset = spawnCollider.bounds.extents.x;
        float yOffset = spawnCollider.bounds.extents.y;
        newSparkle.transform.position += (Vector3)spawnCollider.offset + new Vector3(Random.Range(-xOffset, xOffset), Random.Range(-yOffset, yOffset), 0f);
    }
}
