using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutController : MonoBehaviour {

    [Header("How fast the knife moves")]
    [SerializeField]
    private float speed = 1f;

    [Header("Minimum x position the knife can reach")]
    [SerializeField]
    private float minX = -5f;

    [Header("Maximum x position the knife can reach")]
    [SerializeField]
    private float maxX = 5f;

    [Header("Number of cuts needed")]
    [SerializeField]
    private int cutsNeeded = 1;

    [Header("Put distance of each mask from center here:")]
    [SerializeField]
    private float[] distance;

    [Header("Put your meat here:")]
    [SerializeField]
    private GameObject meatHolder;

    [SerializeField]
    private AudioClip missClip;


    [Header("Starting speed of meat getting separated:")]
    [SerializeField]
    private float separationSpeed = 0.5f;

    [Header("Deceleration rate of meat getting separated:")]
    [SerializeField]
    private float separationDeceleration = 0.2f;

    [SerializeField]
    private float cuttingMoveSpeedMult = .5f;


    private Collider2D knifeCollider;
    private int cutCount = 0;
    private Collider2D currentTrigger = null;
    private GameObject currentLine;
    private GameObject currentFish;
    public bool isCutting = false;
    public GameObject knifeChild;
    public GameObject xChild;
    public GameObject[] masks;
    public GameObject[] lines;
    public GameObject[] sides;


    // Use this for initialization
    void Start ()
    {
        transform.localPosition = new Vector2(Random.Range(minX, maxX), transform.position.y);

        
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures that all lines have decent space away from each other
        if (lines.Length > 0)
        {
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (Mathf.Abs(lines[i].transform.position.x - lines[i + 1].transform.position.x) < 0.9f)
                {
                    if (lines[i].transform.position.x - 0.5f > minX && lines[i + 1].transform.position.x > lines[i].transform.position.x)
                    {
                        lines[i].transform.position = new Vector3(lines[i].transform.position.x - 0.5f, lines[i].transform.position.y, lines[i].transform.position.z);
                    }
                    else if (lines[i].transform.position.x + 0.5f < maxX && lines[i + 1].transform.position.x < lines[i].transform.position.x)
                    {
                        lines[i].transform.position = new Vector3(lines[i].transform.position.x + 0.5f, lines[i].transform.position.y, lines[i].transform.position.z);
                    }
                    else
                    {
                        lines[i].transform.position = new Vector2(Random.Range(minX, maxX), lines[i].transform.position.y);
                    }
                }
            }
            //If on level three, check if the first and last line of the array have space from each other
            if (lines.Length == 3)
            {
                if (Mathf.Abs(lines[0].transform.position.x - lines[2].transform.position.x) < 0.9f)
                {
                    if (lines[0].transform.position.x - 0.5f > minX && lines[2].transform.position.x > lines[0].transform.position.x)
                    {
                        lines[0].transform.position = new Vector3(lines[0].transform.position.x - 0.5f, lines[0].transform.position.y, lines[0].transform.position.z);
                    }
                    else if (lines[0].transform.position.x + 0.5f < maxX && lines[2].transform.position.x < lines[0].transform.position.x)
                    {
                        lines[0].transform.position = new Vector3(lines[0].transform.position.x + 0.5f, lines[0].transform.position.y, lines[0].transform.position.z);
                    }
                    else
                    {
                        lines[0].transform.position = new Vector2(Random.Range(minX, maxX), lines[0].transform.position.y);
                    }
                }
            }
        }

        // Knife will stop moving while animations are playing
        if (knifeChild.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FoodCutKnife")
            || xChild.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FoodCutX"))
        {
        }
        else
        {
            isCutting = false;
        }

        // Handle Movement
        transform.Translate(Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed * (isCutting ? cuttingMoveSpeedMult : 1f), 0f, 0f);

        // Restrict Movement
        if (transform.position.x <= minX)
        {
            transform.localPosition = new Vector2(minX, transform.position.y);
        }
        else if (transform.position.x >= maxX)
        {
            transform.localPosition = new Vector2(maxX, transform.position.y);
        }

        // Checks to see if there is a dotted line to cut
        if (Input.GetKeyDown(KeyCode.Space) && currentTrigger != null)
        {
            //Play animation and sound
            isCutting = true;
            knifeChild.GetComponent<Animator>().SetTrigger("Cut");
            gameObject.GetComponent<AudioSource>().Play();

            //Create the right side of the meat and mask it
            GameObject rightMeat = (GameObject)Instantiate(meatHolder);
            rightMeat.GetComponent<FoodCutFishScript>().mask = rightMeat.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            rightMeat.transform.position = currentFish.transform.position;
            Transform maskPos = rightMeat.GetComponent<FoodCutFishScript>().mask.transform;
            maskPos.position = new Vector2(xChild.transform.position.x, maskPos.position.y);
            
            //Check if the right end of this meat has already been cut
            float nearestLine = 9999;
            for (int i = 0; i < sides.Length; i++)
            {
                float dist = Mathf.Abs(sides[i].transform.position.x - xChild.transform.position.x);
                if (dist < nearestLine && dist != 0 && sides[i].transform.position.x > currentLine.transform.position.x && sides[i].GetComponent<SpriteRenderer>().enabled == false)
                {
                    nearestLine = dist;
                    //Debug.Log(sides[i]);
                    //Debug.Log(nearestLine);
                }
            }
            //If a cut on the right end has been detected, maintain the mask so that the right end still appears to be cut
            if (nearestLine < 9999)
            {
                //0.2f is temporary, because sometimes the masking increases to the right by 0.1 to 0.2... I'm unsure how to solve this issue
                maskPos.localScale = new Vector2(nearestLine - 0.2f, maskPos.localScale.y);
            }

            //Because the meat has been cut, readjust the collision boxes for the right side
            rightMeat.GetComponent<BoxCollider2D>().size = new Vector2(maskPos.GetChild(0).gameObject.transform.lossyScale.x, maskPos.GetChild(0).gameObject.transform.lossyScale.y);
            rightMeat.GetComponent<BoxCollider2D>().offset = new Vector2(maskPos.GetChild(0).gameObject.transform.position.x - rightMeat.transform.position.x, maskPos.GetChild(0).gameObject.transform.position.y - rightMeat.transform.position.y);

            //Play animation of the right meat getting separated
            rightMeat.GetComponent<FoodCutFishScript>().speed = separationSpeed / (cutCount + 1);
            rightMeat.GetComponent<FoodCutFishScript>().deceleration = separationDeceleration / (cutCount + 1);

            //Create the left side of the meat and mask it
            GameObject leftMeat = (GameObject)Instantiate(meatHolder);
            leftMeat.GetComponent<FoodCutFishScript>().mask = leftMeat.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            leftMeat.transform.position = currentFish.transform.position;
            maskPos = leftMeat.GetComponent<FoodCutFishScript>().mask.transform;
            maskPos.position = new Vector2(xChild.transform.position.x, maskPos.position.y);
            maskPos.localScale = new Vector2(-maskPos.localScale.x, maskPos.localScale.y);

            //Check if the left end of this meat has already been cut
            nearestLine = 9999;
            for (int i = 0; i < sides.Length; i++)
            {
                float dist = Mathf.Abs(sides[i].transform.position.x - xChild.transform.position.x);
                if (dist < nearestLine && dist != 0 && sides[i].transform.position.x < currentLine.transform.position.x && sides[i].GetComponent<SpriteRenderer>().enabled == false)
                {
                    nearestLine = dist;
                }
            }

            //If a cut on the left end has been detected, maintain the mask so that the left end still appears to be cut
            if (nearestLine < 9999)
            {
                maskPos.localScale = new Vector2(-(nearestLine - 0.2f), maskPos.localScale.y);
            }

            //Because the meat has been cut, readjust the collision boxes for the left side
            leftMeat.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(maskPos.GetChild(0).gameObject.transform.lossyScale.x), maskPos.GetChild(0).gameObject.transform.lossyScale.y);
            leftMeat.GetComponent<BoxCollider2D>().offset = new Vector2(maskPos.GetChild(0).gameObject.transform.position.x - leftMeat.transform.position.x, maskPos.GetChild(0).gameObject.transform.position.y - leftMeat.transform.position.y);

            //Play animation of the left meat getting separated
            leftMeat.GetComponent<FoodCutFishScript>().speed = -separationSpeed / (cutCount + 1);
            leftMeat.GetComponent<FoodCutFishScript>().deceleration = separationDeceleration / (cutCount + 1);

            //Create side indicators to determine where the cut happened and follow both ends of the meat as they move and separate
            sides[cutCount * 2].transform.position = new Vector3(xChild.transform.position.x, sides[cutCount * 2].transform.position.y, sides[cutCount * 2].transform.position.z);
            sides[cutCount * 2].gameObject.GetComponent<SpriteRenderer>().enabled = false;
            sides[cutCount * 2].gameObject.GetComponent<FoodCutSideScript>().speed = separationSpeed / (cutCount + 1);
            sides[cutCount * 2].gameObject.GetComponent<FoodCutSideScript>().deceleration = separationDeceleration;
            sides[(cutCount * 2) + 1].transform.position = new Vector3(xChild.transform.position.x, sides[cutCount * 2].transform.position.y, sides[cutCount * 2].transform.position.z);
            sides[(cutCount * 2) + 1].gameObject.GetComponent<SpriteRenderer>().enabled = false;
            sides[(cutCount * 2) + 1].gameObject.GetComponent<FoodCutSideScript>().speed = -separationSpeed / (cutCount + 1);
            sides[(cutCount * 2) + 1].gameObject.GetComponent<FoodCutSideScript>().deceleration = separationDeceleration;

            //Increase the count for the cut
            cutCount++;
            //Debug.Log("Object Cut");

            //Make the dotted lines disappear
            currentTrigger.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            currentTrigger = null;

            //We have separated the meat into two parts, so delete the whole part
            Destroy(currentFish);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isCutting == false)
        {
            isCutting = true;
            xChild.GetComponent<Animator>().Play("FoodCutX");
            MicrogameController.instance.playSFX(missClip);
        }

        // Player wins if they cut the proper amount of lines
        if (cutCount == cutsNeeded)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<FoodCutDetection>().line && other.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            currentTrigger = other;
            currentLine = other.gameObject;
            //Debug.Log(currentTrigger);
        }
        else if (other.gameObject.GetComponent<FoodCutDetection>().fish)
        {
            currentFish = other.gameObject;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<FoodCutDetection>().line && other.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            currentTrigger = other;
            currentLine = other.gameObject;
        }
        else if (other.gameObject.GetComponent<FoodCutDetection>().fish)
        {
            currentFish = other.gameObject;
            //Debug.Log(currentFish);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentTrigger = null;
        //Debug.Log(currentTrigger);
    }
}
