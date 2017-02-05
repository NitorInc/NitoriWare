using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_Microgame_Behaviour : MonoBehaviour {

    public GameObject meteorPrefab;
    public float leftLimit;
    public float rightLimit;

    public int meteorQuantity;
    public float timeBetweenCreation;
    private float timeCounter = 0;
    [SerializeField]                            // Delete Later
    private int meteorCreationCounter = 0;
    [SerializeField]                            // Delete Later
    private int meteorDestructionCounter = 0;
    private bool rightCreationPosition;

    // Use this for initialization
    void Start () {     
        meteorDestructionCounter = 0;           // Delete Later
        meteorCreationCounter = 0;              // Delete Later
        timeCounter = 0.5f;
    }

    // Update is called once per frame
    void Update () {

        if (MicrogameController.instance.getVictory())
        {
            if (meteorCreationCounter < meteorQuantity)
            {
                if (timeCounter > 0)
                {
                    timeCounter -= Time.deltaTime;
                }

                else
                {
                    generateMeteor(leftLimit, rightLimit);
                    meteorCreationCounter += 1;
                    timeCounter = timeBetweenCreation;
                }
            }
        }
       
    }

    // Generate a meteor between the A and B horizontal position
    void generateMeteor(float A, float B)
    {
        var meteorInstance = Instantiate(meteorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        meteorInstance.SetActive(false);
        var meteorScript = meteorPrefab.GetComponent<FlanGrab_Meteor_BehaviourScript>();
        var waveMovementIsOn = meteorScript.waveMovementIsOn;
        var waveAmplitude = meteorScript.amplitude;
        var meteorBounds = meteorInstance.GetComponent<CircleCollider2D>().bounds;
        var centerPoint = meteorBounds.center.x;
        var xPosition = -1f;                                        // Default value for initialization
        switch (waveMovementIsOn)
        {
            case true:
                xPosition = Random.Range(A + centerPoint + waveAmplitude, B - centerPoint - waveAmplitude);
                break;

            case false:
                xPosition = Random.Range(A + centerPoint, B - centerPoint);
                break;
        }

        meteorInstance.transform.position = new Vector3(xPosition, 10, 0);
        meteorInstance.SetActive(true);
    }

    
}
