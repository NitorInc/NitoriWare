using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouCutController : MonoBehaviour {
    
    [SerializeField]
    private KagCutCharacter[] characterPrefabs; 

    [SerializeField]
    private int furballCount = 0;
    
    [SerializeField] 
    private RazorController razor;
    
    [SerializeField]
    private float furspeed = 0.01f;

    private float furDistance = 1.2f;
    private GameObject[] furballs;
	
    // Use this for initialization
	void Start () {
        furballs = new GameObject[furballCount];

        int randChar = Random.Range(0, characterPrefabs.Length);
	    KagCutCharacter character = Instantiate(characterPrefabs[randChar]);
        
        GameObject furball = character.transform.Find("FurBall").gameObject;
        for (int i=0; i<furballCount; i++){
            float angle = Random.Range(0, -Mathf.PI);
            float x = razor.transform.position.x + furDistance * Mathf.Cos(angle);
            float y = razor.transform.position.y + furDistance * Mathf.Sin(angle);
            GameObject newFur = Instantiate(furball, new Vector3(x, y, 0), Quaternion.identity);
            FurBallController s = newFur.GetComponent<FurBallController>();
            s.speed = furspeed;
            furballs[i] = newFur;
        }
        furball.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		bool ballsLeft = false;
        foreach (GameObject ball in furballs){
            if (ball != null){
                ballsLeft = true;
                break;
            }
        }
        if (!ballsLeft) {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
	}
}
