using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouCutController : MonoBehaviour {
     
//    [SerializeField]
//    private AudioClip[] soundTracks;
    
    [SerializeField]
    private int furballCount = 0;

    [SerializeField]
    private KagCutCharacter character;
    [SerializeField]
    private GameObject furballPrefab;
    [SerializeField] 
    private RazorController razor;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private AudioClip victoryClip;

    [SerializeField]
    private float furspeed = 0.01f;
    [SerializeField]
    private float furDistance = 1.2f;
    [SerializeField]
    private Vector2 furAngleRange;
    [SerializeField]
    private Vector2 furAngleRangeRight;

    [SerializeField]
    private float particleRate = 0.2f;
    
    public bool shouldExplode = false;

    private GameObject[] furballs;

    [SerializeField]
    private Color hairColor;

    // Use this for initialization
	void Start () {
		//Dog dog = (MicrogameController.instance.getTraits() as KagCutTraits).dog;
        furballs = new GameObject[furballCount];
        // Set the sprites
        //AudioClip music = soundTracks[randChar];
        
        //character.GetComponent<SpriteRenderer>().sprite = charSprite; 
        
        GameObject furball = Instantiate(furballPrefab);
        GameObject spriteObj = furball.transform.Find("Sprite").gameObject;
        GameObject furExplo = furball.transform.Find("FurExplosion").gameObject;
        spriteObj.GetComponent<SpriteRenderer>().color = hairColor;
        
        ParticleSystem.MainModule partMod = furball.GetComponent<ParticleSystem>().main;
        ParticleSystem.MinMaxGradient partColor = new ParticleSystem.MinMaxGradient(hairColor); 
        partMod.startColor = partColor;
        partMod = furExplo.GetComponent<ParticleSystem>().main;
        partMod.startColor = partColor;


        bool flipHairAngle = MathHelper.randomBool();
        for (int i=0; i<furballCount; i++){
            //angle = angles[i];
            var angle =  MathHelper.randomRangeFromVector(flipHairAngle ? furAngleRange : furAngleRangeRight) * Mathf.Deg2Rad;
            flipHairAngle = !flipHairAngle;
            float x = razor.transform.position.x + furDistance * Mathf.Cos(angle);
            float y = razor.transform.position.y + furDistance * Mathf.Sin(angle);
            GameObject newFur = Instantiate(furball, new Vector3(x, y, 0), Quaternion.identity);
            newFur.transform.position += Vector3.forward * (float)i * .01f;
            FurBallController s = newFur.GetComponent<FurBallController>();
            s.speed = furspeed;
            s.particleRate = particleRate;
            s.shouldExplode = shouldExplode;
            newFur.GetComponent<Animator>().SetFloat("offset", Random.Range(0f, 1f));
            furballs[i] = newFur;
        }
        furball.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		bool ballsLeft = false;
        foreach (GameObject ball in furballs){
            if (ball.tag != "Finish"){
                ballsLeft = true;
                break;
            }
        }
        if (!ballsLeft) {
            character.GetComponent<Animator>().SetTrigger("Win");
            background.GetComponent<Animator>().SetTrigger("Win");
            MicrogameController.instance.setVictory(victory: true, final: true);
            MicrogameController.instance.playSFX(victoryClip);
            enabled = false;
        }
	}
}
