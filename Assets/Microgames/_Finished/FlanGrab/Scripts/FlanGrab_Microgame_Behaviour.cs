using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_Microgame_Behaviour : MonoBehaviour {

	public static FlanGrab_Microgame_Behaviour instance;

    public GameObject meteorPrefab;
    public float leftLimit;
    public float rightLimit;

    public int meteorQuantity;
	public float createTime = 1f;
    public float timeBetweenCreation;
	public Animator flanimator, victoryAnimator;
    private float timeCounter;
    private int meteorCreationCounter = 0;
    private int meteorDestructionCounter = 0;
    private bool rightCreationPosition;

    public CameraShake hitShake, loseShake;

	//which meteor (0 or 1) will be the one coming from the left
	private int leftMeteor;

    // Use this for initialization
    void Start () {
		instance = this;
        meteorDestructionCounter = 0;           // Delete Later
        meteorCreationCounter = 0;              // Delete Later
		timeCounter = createTime;
		leftMeteor = Random.Range(0, 2);
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

        var meteorBounds = meteorInstance.GetComponent<CircleCollider2D>().bounds;
        //var meteorScript = meteorPrefab.GetComponent<FlanGrab_Meteor_BehaviourScript>();
        //var waveMovementIsOn = meteorScript.waveMovementIsOn;
        //var waveAmplitude = meteorScript.amplitude;
        //var centerPoint = meteorBounds.center.x;

        //Conditionally assign xPosition based on whether this is the left meteor
        var xPosition = (leftMeteor == meteorCreationCounter) ? Random.Range(A, 0f) : Random.Range(0f, B);

        meteorInstance.transform.position = new Vector3(xPosition, 10, 0);
        meteorInstance.SetActive(true);
    }

    public void increaseDestructionCounter()
    {
        meteorDestructionCounter += 1;
        if (meteorDestructionCounter == meteorQuantity)
        {
			MicrogameController.instance.setVictory(true, true);
			FlanGrab_Microgame_Behaviour.instance.flanimator.Play("Victory");
			victoryAnimator.enabled = true;
        }
    }

}
