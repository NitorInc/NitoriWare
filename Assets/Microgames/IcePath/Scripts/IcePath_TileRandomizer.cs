using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_TileRandomizer : MonoBehaviour {

    [Header("Sprites")]
    [SerializeField] private Sprite a;
    [SerializeField] private Sprite b;
    private Sprite[] sprite = new Sprite[2];

    [Header("Shadow")]
    [SerializeField] private GameObject shadow;

    [Header("Randomizer")]
    [SerializeField] private float distMin;
    [SerializeField] private float distMax;
    [SerializeField] private float scaleMin;
    [SerializeField] private float scaleMax;

    void Start () {
        sprite[0] = a;
        sprite[1] = b;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite[rand(0, 1)];
        
        // Randomize setup

        float randomDist = Random.Range(distMin, distMax);
        float randomScale = Random.Range(scaleMin, scaleMax);
        float randomRotRad = Random.Range(0f, 2 * Mathf.PI);
        float randomRotDeg = Random.Range(0f, 360f);

        transform.position += randomDist * new Vector3(Mathf.Cos(randomRotRad), Mathf.Cos(randomRotRad), 0);
        transform.eulerAngles = new Vector3(0, 0, randomRotDeg);
        transform.localScale = new Vector3(randomScale, randomScale, 1);

        // Shade child

        GameObject child = Instantiate(shadow, transform.position + new Vector3(0.1f, -0.1f, 0f), Quaternion.identity);

        child.transform.eulerAngles = transform.eulerAngles;
        child.transform.localScale = transform.localScale;

    }

    int rand(float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }

}
